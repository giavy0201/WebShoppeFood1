using DAL.Entities;
using DAL.ModelsRequest;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Unidecode.NET;

namespace DAL.Non_Repository.CartRepo
{

    public class CartRepository : ICartRepository
    {
        private readonly IRepository<Cart> _cartRepositoty;
        private readonly IRepository<DetailCart> _detailCartRepositoty;
        private readonly DataContext _dataContext;

        public CartRepository(IRepository<Cart> cartRepositoty, IRepository<DetailCart> detailCartRepositoty, DataContext dataContext)
        {
            _cartRepositoty = cartRepositoty;
            _detailCartRepositoty = detailCartRepositoty;
            _dataContext = dataContext;
        }
        public async Task<Cart> AddCart(int StoreID, string CustomerID, string DeliveryAddress, int Phone, int Status)
        {
            var cart = new Cart
            {
                StoreId = StoreID,
                CustomerId = CustomerID,
                TimeOrder = DateTime.Now,
                Delivery = DeliveryAddress,
                DeliveryNoDiacritic = DeliveryAddress.Unidecode(),
                PhoneNumber = Phone,
                Status = Status
               
                //PaymentMethod =PaymentMethod
            };
            _cartRepositoty.Insert(cart);
            _cartRepositoty.Save();
            return cart;
        }

        public async Task<DetailCart> AddDetailCart(int FoodID, int Quantity, double Price, int CartID)
        {
            var detailCart = new DetailCart
            {
                FoodId = FoodID,
                Quantity = Quantity,
                Price = Price,
                CartID = CartID
            };
            _detailCartRepositoty.Insert(detailCart);
            _detailCartRepositoty.Save();
            return detailCart;
        }

        public async Task<double> GetCartTotalToday(int StoreID)
        {
            var today = DateTime.Now.Date;
            var cart = _dataContext.Carts;
            var detailcart = _dataContext.DetailCarts;
            var result = (from x in cart
                          join y in detailcart on x.Id equals y.CartID
                          where x.StoreId == StoreID && x.TimeOrder.HasValue && x.TimeOrder.Value.Date == today && x.Status == ValueOrder.Done
                          select y.Price * y.Quantity)
                          .Sum();
            return result;
        }
        public async Task<List<Cart>> GetCartsByStatus(int status)
        {
            // Lấy danh sách Cart có Status bằng 1
            var carts = _dataContext.Carts
                 .Where(cart => cart.Status == status)
                 .Include(carts => carts.DetailCarts)
                 .ThenInclude(carts => carts.Food)
                 .ToList();

            return carts;
        }
        public async Task<List<Cart>> GetCartsByStatusDone(int status, string shipper)
        {
            // Lấy danh sách Cart có Status bằng 1
            var carts = _dataContext.Carts
                .Where(cart => cart.Status == status && cart.ShipperId == shipper)
                .Include(carts => carts.DetailCarts)
                .ThenInclude(carts => carts.Food)
                .ToList();

            return carts;
        }
        public async Task<List<Cart>> GetCartsByShipperConfirmedStatus(string shipper)
        {
            return await _dataContext.Carts
                .Where(c => c.Status == ValueOrder.ShipperConfirmed && c.ShipperId == shipper) // Trạng thái shipper xác nhận
                .Include(c => c.Customer) // Bao gồm thông tin shipper
                .Include(c => c.DetailCarts) // Bao gồm chi tiết giỏ hàng
                .ThenInclude(dc => dc.Food) // Bao gồm món ăn
                .ToListAsync();
        }

        // Lấy thông tin shipper từ CartId
        public async Task<Customer> GetShipperInfoByCartId(int cartId)
        {
            var cart = await _dataContext.Carts
                .Include(c => c.Customer) // Bao gồm thông tin shipper
                .FirstOrDefaultAsync(c => c.Id == cartId); // Lấy theo cartId

            if (cart == null)
                throw new Exception("Đơn hàng không tồn tại.");

            return cart.Customer;
        }

        // Cập nhật trạng thái đơn hàng khi shipper nhận đơn
        public async Task AcceptOrder(int cartId, string userId)
        {
            var cart = await _dataContext.Carts
         .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                throw new Exception("Đơn hàng không tồn tại.");

            if (cart.Status != ValueOrder.Order) // Ensure the order is in status 1 (Order Confirmed)
                throw new Exception("Trạng thái đơn hàng không hợp lệ để shipper xác nhận.");

            var user = await _dataContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("Người dùng không tồn tại.");

            if (user.Role != "Shipper")
                throw new Exception("Người dùng này không phải shipper.");

            cart.Status = ValueOrder.ShipperConfirmed; // Set status to Shipper Confirmed (4)
            cart.ShipperId = userId;
            _dataContext.Carts.Update(cart);
            await _dataContext.SaveChangesAsync();
        }
        public async Task CompleteOrder(int cartId, string userId)
        {
            var cart = await _dataContext.Carts
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                throw new Exception("Đơn hàng không tồn tại.");

            if (cart.Status != ValueOrder.ShipperConfirmed) // Đảm bảo trạng thái là Shipper Confirmed (4)
                throw new Exception("Trạng thái đơn hàng không hợp lệ để hoàn thành.");

            var user = await _dataContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("Người dùng không tồn tại.");

            //if (user.Role != "Shipper")
            //    throw new Exception("Người dùng này không phải shipper.");

            cart.Status = ValueOrder.Done; // Chuyển trạng thái thành Done (3)

            _dataContext.Carts.Update(cart);
            await _dataContext.SaveChangesAsync();
        }


        public async Task<Cart> UpdateStatusOrder(int CartID, int Status)
        {
            var cart = _cartRepositoty.GetById(CartID);
            if (cart == null)
            {
                return null;
            }

            switch (Status)
            {
                case ValueOrder.Order:
                case ValueOrder.Cancel:
                case ValueOrder.Done:
                    cart.Status = Status;
                    break;
                default:
                    return null;
            }
            _cartRepositoty.Update(cart);
            _cartRepositoty.Save();
            return cart;
        }

        public async Task<List<Cart>> ListCartByStore(SearchCartReq Request)
        {
            if (!string.IsNullOrEmpty(Request.Delivery))
            {
                Request.Delivery = Request.Delivery.Unidecode().ToLower();
            }
            var cart = _dataContext.Carts.Where(x =>
                 x.StoreId == Request.StoreID &&
                 (Request.CartID == null || x.Id == Request.CartID) &&
                 (Request.DayStart == null || x.TimeOrder >= Request.DayStart) &&
                 (Request.DayEnd == null || x.TimeOrder <= Request.DayEnd) &&
                 (Request.Delivery == null || x.DeliveryNoDiacritic.ToLower().Contains(Request.Delivery)) &&
                 (Request.Phone == null || x.PhoneNumber == Request.Phone) &&
                 (Request.StatusID == null || x.Status == Request.StatusID)
                ).ToList();
            cart = cart.OrderByDescending(x => x.TimeOrder).ToList();
            return cart;
        }

        public async Task<Cart> SelectCartById(int CartID)
        {
            var cart = _dataContext.Carts.Include(x => x.DetailCarts).ThenInclude(dc => dc.Food).FirstOrDefault(x => x.Id == CartID);
            return cart;
        }

        //public async Task<List<Cart>> GetOrdersByStoreAndMonth(int StoreID, int month)
        //{
        //    var orders = await _dataContext.Carts
        //.Where(x => x.StoreId == StoreID && x.TimeOrder.HasValue && x.TimeOrder.Value.Month == month)
        //.Include(x => x.DetailCarts) // Bao gồm chi tiết đơn hàng
        //.ToListAsync();

        //    return orders;
        //}
        public async Task<double> GetCartTotalForMonth(int storeID, int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var cart = _dataContext.Carts;
            var detailcart = _dataContext.DetailCarts;

            var result = (from x in cart
                          join y in detailcart on x.Id equals y.CartID
                          where x.StoreId == storeID &&
                                x.TimeOrder.HasValue &&
                                x.TimeOrder.Value.Date >= startDate &&
                                x.TimeOrder.Value.Date <= endDate &&
                                x.Status == ValueOrder.Done
                          select y.Price * y.Quantity)
                          .Sum();

            return result;
        }
        public async Task<List<object>> GetMonthlyRevenue(int storeID, int year)
        {
            var result = await (from cart in _dataContext.Carts
                                join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                where cart.StoreId == storeID &&
                                      cart.TimeOrder.HasValue &&
                                      cart.TimeOrder.Value.Year == year &&
                                      cart.Status == ValueOrder.Done
                                group detail by cart.TimeOrder.Value.Month into g
                                select new
                                {
                                    Month = g.Key,
                                    TotalRevenue = g.Sum(detail => detail.Price * detail.Quantity)
                                }).ToListAsync();

            return result.Cast<object>().ToList();
        }

        public async Task<List<DetailCart>> GetDetailCartsForDate(int storeID, DateTime date)
        {
            var detailCarts = await _dataContext.DetailCarts
             .Include(dc => dc.Cart) // Bao gồm thông tin giỏ hàng liên quan
                .Where(dc => dc.Cart.StoreId == storeID &&
                     dc.Cart.TimeOrder.HasValue &&
                     dc.Cart.TimeOrder.Value.Date == date.Date &&
                     dc.Cart.Status == ValueOrder.Done)
            .ToListAsync();

            return detailCarts;
        }
        public async Task<List<object>> GetRevenueForLast7Days(int storeID)
        {
            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-6);

            var result = await (from cart in _dataContext.Carts
                                join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                where cart.StoreId == storeID &&
                                      cart.TimeOrder.HasValue &&
                                      cart.TimeOrder.Value.Date >= sevenDaysAgo &&
                                      cart.TimeOrder.Value.Date <= today &&
                                      cart.Status == ValueOrder.Done
                                group detail by cart.TimeOrder.Value.Date into g
                                select new
                                {
                                    Date = g.Key,
                                    TotalRevenue = g.Sum(detail => detail.Price * detail.Quantity)
                                }).ToListAsync();

            return result.Cast<object>().ToList();
        }
        public async Task<List<object>> GetRevenueForLast8Weeks(int storeID)
        {
            var today = DateTime.Today;
            var eightWeeksAgo = today.AddDays(-7 * 8);

            var result = await (from cart in _dataContext.Carts
                                join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                where cart.StoreId == storeID &&
                                      cart.TimeOrder.HasValue &&
                                      cart.TimeOrder.Value.Date >= eightWeeksAgo &&
                                      cart.TimeOrder.Value.Date <= today &&
                                      cart.Status == ValueOrder.Done
                                group detail by CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(cart.TimeOrder.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday) into g
                                select new
                                {
                                    WeekNumber = g.Key,
                                    TotalRevenue = g.Sum(detail => detail.Price * detail.Quantity)
                                }).ToListAsync();

            return result.Cast<object>().ToList();
        }
        public async Task<List<object>> GetRevenueForLast12Months(int storeID)
        {
            var today = DateTime.Today;
            var twelveMonthsAgo = today.AddMonths(-12);

            var result = await (from cart in _dataContext.Carts
                                join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                where cart.StoreId == storeID &&
                                      cart.TimeOrder.HasValue &&
                                      cart.TimeOrder.Value.Date >= twelveMonthsAgo &&
                                      cart.TimeOrder.Value.Date <= today &&
                                      cart.Status == ValueOrder.Done
                                group detail by cart.TimeOrder.Value.Month into g
                                select new
                                {
                                    Month = g.Key,
                                    TotalRevenue = g.Sum(detail => detail.Price * detail.Quantity)
                                }).ToListAsync();

            return result.Cast<object>().ToList();
        }
        //public async Task<List<object>> GetRevenueForStoreId(int storeId,DateTime startDate, DateTime endDate)
        //{
        //    var storeRevenueList = new List<object>();

        //    // Get all store IDs
        //    var storeIds = await _dataContext.Stores.Select(store => store.Id).ToListAsync();

        //    foreach (var storeId in storeIds)
        //    {
        //        var revenueForStore = await (from cart in _dataContext.Carts
        //                                     join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
        //                                     where cart.StoreId == storeId
        //                                           && cart.Status == ValueOrder.Done
        //                                           && cart.TimeOrder >= startDate
        //                                           && cart.TimeOrder < endDate
        //                                     select detail.Price * detail.Quantity)
        //                                     .ToListAsync(); // Retrieve the list of revenue values

        //        var totalRevenue = revenueForStore.Sum(); // Calculate the sum of the revenues

        //        storeRevenueList.Add(new
        //        {
        //            StoreId = storeId,
        //            TotalRevenue = revenueForStore
        //        });
        //    }

        //    return storeRevenueList;
        //}
        public async Task<double> GetRevenueForStoreId(int storeId, DateTime startDate, DateTime endDate)
        {
            var storeRevenueList = new List<object>();

            // Get revenue for the specific store
            var revenueForStore = await (from cart in _dataContext.Carts
                                         join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                         where cart.StoreId == storeId
                                               && cart.Status == ValueOrder.Done
                                               && cart.TimeOrder >= startDate
                                               && cart.TimeOrder < endDate
                                         select detail.Price * detail.Quantity)
                                         .SumAsync();

            // Add the revenue data to the list


            return revenueForStore;
        }





        public async Task<List<int>> GetCartIdsByEmail(string email)
        {
            var cartIds = await _dataContext.Carts
                                            .Where(c => c.Customer.Email == email)
                                            .Select(c => c.Id)
                                            .ToListAsync();
            return cartIds;
        }

        //public async Task UpdatePaymentStatus(int cartId, string paymentStatus, string paymentMethod, DateTime? paymentTime)
        //{

        //    if (cartId <= 0)
        //        throw new ArgumentException("Invalid cart ID.", nameof(cartId));

        //    if (string.IsNullOrWhiteSpace(paymentStatus))
        //        throw new ArgumentException("Payment status is required.", nameof(paymentStatus));

        //    var cart = await _dataContext.Set<Cart>().FirstOrDefaultAsync(c => c.Id == cartId);
        //    if (cart == null)
        //        throw new Exception($"Cart with ID {cartId} not found.");

        //    cart.PaymentStatus = paymentStatus;
        //    cart.PaymentMethod = paymentMethod;
        //    cart.PaymentTime = paymentTime;

        //    _dataContext.Set<Cart>().Update(cart);
        //    await _dataContext.SaveChangesAsync();
        //}
        public async Task UpdateMomoStatus(int cartId, string MomoTransactionId,string MomoStatus)
        {

            if (cartId <= 0)
                throw new ArgumentException("Invalid cart ID.", nameof(cartId));

           

            var cart = await _dataContext.Set<Cart>().FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
                throw new Exception($"Cart with ID {cartId} not found.");

            cart.MomoStatus = MomoStatus;
            cart.MomoTransactionId = MomoTransactionId;
          

            _dataContext.Set<Cart>().Update(cart);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<Cart> GetCartForPayment(int cartId)
        {
            return await _dataContext.Set<Cart>()
                .Include(c => c.DetailCarts)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

    }
}
