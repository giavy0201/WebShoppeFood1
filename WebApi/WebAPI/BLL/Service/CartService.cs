using AutoMapper;
using BLL.Helpers;
using BLL.IService;
using BLL.Models.DTOs;
using BLL.Models.DTOs.Cart;
using BLL.Models.DTOs.Customer;
using BLL.Models.Request;
using DAL;
using DAL.Entities;
using DAL.ModelsRequest;
using DAL.Non_Repository;
using DAL.Non_Repository.CartRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Utils.Windows;
using System;
using System.Net;
using Microsoft.AspNetCore.Http; // Để sử dụng HttpContex
using static BLL.Models.Validate.EnumNumber;

namespace BLL.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;
        private readonly CartDtos _cartDtos;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartService(ICartRepository cartRepo, IMapper mapper, CartDtos cartDtos,IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
            _cartDtos = cartDtos;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }
        public async Task<List<CartSystem>> GetCartsByStatus(int status)
        {
            // Kiểm tra nếu status không hợp lệ (không nằm trong enum CartStatus)
            if (!Enum.IsDefined(typeof(CartStatus), status))
            {
                return null; // Trả về null nếu trạng thái không hợp lệ
            }

            // Gọi phương thức trong repository để lấy danh sách Cart
            var listCart = await _cartRepo.GetCartsByStatus(status);

            // Kiểm tra nếu danh sách trả về null
            if (listCart == null || !listCart.Any())
            {
                return null; // Không tìm thấy cart phù hợp
            }

            // Sử dụng AutoMapper để ánh xạ sang danh sách CartSystem
            var cartSystem = _mapper.Map<List<CartSystem>>(listCart);

            return cartSystem;
        }
        public async Task<List<CartSystem>> GetCartsByStatusDone(int status,string shipper)
        {
            // Kiểm tra nếu status không hợp lệ (không nằm trong enum CartStatus)
            if (!Enum.IsDefined(typeof(CartStatus), status))
            {
                return null; // Trả về null nếu trạng thái không hợp lệ
            }

            // Gọi phương thức trong repository để lấy danh sách Cart
            var listCart = await _cartRepo.GetCartsByStatusDone(status,shipper);

            // Kiểm tra nếu danh sách trả về null
            if (listCart == null || !listCart.Any())
            {
                return null; // Không tìm thấy cart phù hợp
            }

            // Sử dụng AutoMapper để ánh xạ sang danh sách CartSystem
            var cartSystem = _mapper.Map<List<CartSystem>>(listCart);

            return cartSystem;
        }
        public async Task<Cart> InsertCart(CartRequest reqCart)
        {
            try
            {
                var req = await _cartRepo.AddCart((int)reqCart.StoreId, reqCart.CustomerId, reqCart.Delivery, reqCart.PhoneNumber, 0);
                var cartID = req.Id;
                foreach (var detailcart in reqCart.Details)
                {
                    await _cartRepo.AddDetailCart(detailcart.FoodId, detailcart.Quantity, detailcart.Price, cartID);
                }
                return req;
            }
            catch
            {
                return null;
            }
        }

        public async Task<double> TotalMoneyToday(int StoreID)
        {
            var cart = await _cartRepo.GetCartTotalToday(StoreID);
            if (cart == null)
            {
                return 0;
            }
            else return cart;
        }

        public async Task<ModelApiRequest> UpdateCartOrder(SetStatusCartReq request)
        {
            var req = new ModelApiRequest();
            var cart = await _cartRepo.UpdateStatusOrder(request.CartID, request.Status);
            if (cart == null)
            {
                return null;
            }
            req.ID = cart.Id;
            return req;
        }

        public async Task<List<CartSystem>> ListCartByStore(SearchCartRequest model)
        {
            if (model.StatusID != null && (!Enum.IsDefined(typeof(CartStatus), model.StatusID)))
            {
                return null;
            }
            var req = _mapper.Map<SearchCartReq>(model);
            var listCart = await _cartRepo.ListCartByStore(req);
            var cartSystem = _mapper.Map<List<CartSystem>>(listCart);
            return cartSystem;
        }

        public async Task<CartSystem> CartByID(int CartID)
        {
            var cart = await _cartRepo.SelectCartById(CartID);
            var cartSystem = _mapper.Map<CartSystem>(cart);
            return cartSystem;
        }

        

        public async Task<double> TotalMoneyForMonth(int storeID, int month, int year)
        {
            double totalMoney = 0;

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                // Lấy danh sách chi tiết giỏ hàng cho mỗi ngày trong tháng
                var dailyDetailCarts = await _cartRepo.GetDetailCartsForDate(storeID, new DateTime(year, month, i));

                // Tính tổng doanh thu cho ngày đó và cộng vào tổng doanh thu của tháng
                var dailyMoney = dailyDetailCarts.Sum(dc => dc.Price * dc.Quantity);
                totalMoney += dailyMoney;
            }

            return totalMoney;
        }
        public async Task<List<double>> TotalMoneyForYear(int storeID, int year)
        {
            var monthlyRevenues = new List<double>();

            for (int month = 1; month <= 12; month++)
            {
                double totalMoney = 0;

                for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
                {
                    // Lấy danh sách chi tiết giỏ hàng cho mỗi ngày trong tháng
                    var dailyDetailCarts = await _cartRepo.GetDetailCartsForDate(storeID, new DateTime(year, month, day));

                    // Tính tổng doanh thu cho ngày đó và cộng vào tổng doanh thu của tháng
                    var dailyMoney = dailyDetailCarts.Sum(dc => dc.Price * dc.Quantity);
                    totalMoney += dailyMoney;
                }

                monthlyRevenues.Add(totalMoney);
            }

            return monthlyRevenues;
        }
        public async Task<List<double>> TotalMoneyForLast7Days(int storeID)
        {
            var revenueList = new List<double>();
            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-6);

            // Loop through the last 7 days
            for (var date = sevenDaysAgo; date <= today; date = date.AddDays(1))
            {
                // Get the list of detailed cart items for each day
                var dailyDetailCarts = await _cartRepo.GetDetailCartsForDate(storeID, date);

                // Calculate the daily revenue and add it to the list
                var dailyMoney = dailyDetailCarts.Sum(dc => dc.Price * dc.Quantity);
                revenueList.Add(dailyMoney);
            }

            return revenueList;
        }
        public async Task<List<double>> TotalMoneyForLast8Weeks(int storeID)
        {
            var revenueList = new List<double>();
            var today = DateTime.Today;
            var eightWeeksAgo = today.AddDays(-7 * 8); // 8 weeks ago

            // Loop through the last 8 weeks
            for (var weekStartDate = eightWeeksAgo; weekStartDate <= today; weekStartDate = weekStartDate.AddDays(7))
            {
                var weekEndDate = weekStartDate.AddDays(6); // End of the week (7 days)

                double totalWeekRevenue = 0;

                // Get the list of detailed cart items for each day in the week
                for (var date = weekStartDate; date <= weekEndDate && date <= today; date = date.AddDays(1))
                {
                    var dailyDetailCarts = await _cartRepo.GetDetailCartsForDate(storeID, date);

                    // Calculate the daily revenue and add it to the weekly total revenue
                    var dailyMoney = dailyDetailCarts.Sum(dc => dc.Price * dc.Quantity);
                    totalWeekRevenue += dailyMoney;
                }

                revenueList.Add(totalWeekRevenue);
            }

            return revenueList;
        }
        public async Task<List<double>> TotalMoneyForLast12Months(int storeID)
        {
            var revenueList = new List<double>();
            var today = DateTime.Today;
            var twelveMonthsAgo = today.AddMonths(-12); // 12 months ago

            // Loop through the last 12 months
            for (var monthDate = twelveMonthsAgo; monthDate <= today; monthDate = monthDate.AddMonths(1))
            {
                double totalMonthRevenue = 0;

                // Loop through each day of the month
                for (int day = 1; day <= DateTime.DaysInMonth(monthDate.Year, monthDate.Month); day++)
                {
                    var dailyDetailCarts = await _cartRepo.GetDetailCartsForDate(storeID, new DateTime(monthDate.Year, monthDate.Month, day));

                    // Calculate the daily revenue and add it to the monthly total revenue
                    var dailyMoney = dailyDetailCarts.Sum(dc => dc.Price * dc.Quantity);
                    totalMonthRevenue += dailyMoney;
                }

                revenueList.Add(totalMonthRevenue);
            }

            return revenueList;
        }




        public async Task<List<int>> GetCartIdByEmail(string email)
        {
            var carts = await _cartRepo.GetCartIdsByEmail(email);
            if (carts == null || !carts.Any())
            {
                // Nếu không tìm thấy giỏ hàng cho email đã cho, bạn có thể trả về null hoặc danh sách rỗng
                return new List<int>(); // hoặc return null;
            }

            return carts;
        }

        public async Task<double> GetRevenueForStoreId(int storeId, DateTime startDate, DateTime endDate)
        {
            return await _cartRepo.GetRevenueForStoreId(storeId, startDate, endDate);   
        }

        public async Task<List<CartSystem>> GetCartsByShipperConfirmedStatus(string shipper)
        {
            var carts = await _cartRepo.GetCartsByShipperConfirmedStatus(shipper);

            var cartSystems = carts.Select(cart => new CartSystem
            {
                Id = cart.Id,
                StoreId = cart.StoreId,
                CustomerId = cart.CustomerId,
                Delivery = cart.Delivery,
                PhoneNumber = cart.PhoneNumber,
                ShipperId = cart.ShipperId,
                TimeOrder = cart.TimeOrder,
                MomoStatus = cart.MomoStatus,
                Status = cart.Status,
                DetailCarts = cart.DetailCarts.Select(dc => new SelectCart
                {
                    Id = dc.Id,
                    FoodId = dc.FoodId,
                    Quantity = dc.Quantity,
                    Price = dc.Price,
                    CartID = dc.CartID,
                    FoodName = dc.Food.Name,
                }).ToList()
            }).ToList();

            return cartSystems;
        }

        public async Task<CustomerDtos> GetShipperInfoByCartId(int cartId)
        {
            var customer = await _cartRepo.GetShipperInfoByCartId(cartId);
            if (customer == null)
            {
                throw new Exception("Shipper không tồn tại hoặc không có thông tin đơn hàng.");
            }

            // Chuyển đổi từ Customer sang CustomerDtos
            var customerDtos = new CustomerDtos
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Birthday = customer.Birthday,
                PhoneNumber = customer.PhoneNumber,
                Gender = customer.Gender,
                Location = customer.Location,
                Image = customer.Image
            };

            return customerDtos;
        }

        public async Task AcceptOrder(int cartId, string userId)
        {
            await _cartRepo.AcceptOrder(cartId, userId);
        }

        public async Task CompleteOrder(int cartId, string userId)
        {
            await _cartRepo.CompleteOrder(cartId, userId);
        }

        public async Task<string> CreateVNPayPaymentUrl(int cartId, decimal amount,string returnUrl)
        {
            //        var cart = await _cartRepo.GetCartForPayment(cartId);
            //        if (cart == null) throw new Exception("Cart not found.");

            //        string tmnCode = _configuration["VNPay:TmnCode"];
            //        string hashSecret = _configuration["VNPay:HashSecret"];
            //        string vnpUrl = _configuration["VNPay:Url"];
            //        //string returnUrl = _configuration["VNPay:RetunrnUrl"];
            //        string vnpCreateDate = DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss");

            //        var vnpParams = new SortedList<string, string>()

            //        {
            //            { "vnp_Version", "2.1.0" },
            //        { "vnp_Command", "pay" },
            //        { "vnp_TmnCode", tmnCode },
            //        { "vnp_Amount", ((int)(amount * 100)).ToString() },
            //            //{"vnp_BankCode","VNBANK" },
            //        { "vnp_CreateDate", vnpCreateDate },
            //        { "vnp_CurrCode", "VND" },
            //        { "vnp_IpAddr", "127.0.0.1" }, // Lấy IP từ Utils
            //        { "vnp_Locale", "vn" },
            //        { "vnp_OrderInfo", WebUtility.UrlEncode($"Payment for cart {cartId}") },
            //            {"vnp_OrderType","other" },
            //        { "vnp_Returnurl", returnUrl },
            //        { "vnp_TxnRef", cartId.ToString() }
            //        };

            //        // Tạo query string không mã hóa để tính SecureHash
            //        //string rawQueryString = string.Join("&",
            //        //    vnpParams.Select(kv => $"{kv.Key}={kv.Value}"));
            //        string rawQueryString = string.Join("&",
            //vnpParams.OrderBy(kv => kv.Key) // Sắp xếp theo key
            //         .Select(kv => $"{kv.Key}={kv.Value}")); // Không mã hóa giá trị


            //        // Tạo SecureHash
            //        string secureHash = VNPayHelper.CreateSecureHash(hashSecret, rawQueryString);

            //        // Tạo query string đã mã hóa
            //        string encodedQueryString = string.Join("&",
            //            vnpParams.Select(kv => $"{kv.Key}={WebUtility.UrlEncode(kv.Value)}"));

            //        // Tạo URL cuối cùng
            //        return $"{vnpUrl}?{encodedQueryString}&vnp_SecureHash={secureHash}";
            // Lấy thông tin giỏ hàng
            // Validate amount
            if (amount <= 0)
            {
                throw new Exception("Invalid payment amount.");
            }

            // Validate configuration
            if (string.IsNullOrEmpty(_configuration["VNPay:TmnCode"]) ||
                string.IsNullOrEmpty(_configuration["VNPay:HashSecret"]) ||
                string.IsNullOrEmpty(_configuration["VNPay:Url"]) ||
                string.IsNullOrEmpty(returnUrl))
            {
                throw new Exception("VNPay configuration is incomplete. Please verify the settings.");
            }

            // Retrieve cart for payment
            var cart = await _cartRepo.GetCartForPayment(cartId);
            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }

            // Initialize VnPayLibrary
            var vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", _configuration["VNPay:TmnCode"]);
            vnPay.AddRequestData("vnp_Amount", ((int)(amount * 100)).ToString());
            vnPay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_contextAccessor.HttpContext) ?? "127.0.0.1");
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" +cartId);
            vnPay.AddRequestData("vnp_OrderType", "other");
            vnPay.AddRequestData("vnp_Returnurl", returnUrl);
            vnPay.AddRequestData("vnp_TxnRef", cartId.ToString());

            // Generate payment URL
            string vnpUrl = _configuration["VNPay:Url"];
            string vnpHashSecret = _configuration["VNPay:HashSecret"];

            try
            {
                var paymentUrl = vnPay.CreateRequestUrl(vnpUrl, vnpHashSecret);
                //_logger.LogInformation("Generated VNPay URL: {PaymentUrl}", paymentUrl);
                return paymentUrl;
            }
            catch (Exception ex)
            {
                //_logger.LogError("Error generating VNPay payment URL: {Error}", ex.Message);
                throw;
            }
        }


        //public async Task HandleVNPayCallbackAsync(int cartId, string responseCode)
        //{
        //    string paymentStatus = responseCode == "00" ? "Success" : "Failed";
        //    DateTime? paymentTime = responseCode == "00" ? DateTime.Now : (DateTime?)null;

        //    await _cartRepo.UpdatePaymentStatus(cartId, paymentStatus, "VNPay", paymentTime);
        //}

        //public async Task<List<object>> GetRevenueForEachStore(DateTime startDate, DateTime endDate)
        //{
        //   return await _cartRepo.GetRevenueForEachStore(startDate, endDate);
        //}
        //public async Task<List<CartSystem>> GetOrdersByStoreAndMonth(int storeId, int month)
        //{
        //    var orders = await _cartRepo.GetOrdersByStoreAndMonth(storeId, month);
        //    var cartSystems = _mapper.Map<List<CartSystem>>(orders);
        //    return cartSystems;
        //}

    }
}
