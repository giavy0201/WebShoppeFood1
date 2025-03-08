using BLL.IService;
using BLL.Model;
using BLL.Model.Address;
using BLL.Model.Cart;
using BLL.Model.Customer;
using BLL.Service;
using Microsoft.AspNetCore.Mvc;
using WebSystemStore.Models;
using X.PagedList;
using static NuGet.Packaging.PackagingConstants;

namespace WebSystemStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IStoreService _storeService;
        private readonly IAdminService _adminService;
        private readonly ICartService _cartService;
        public CartController(IHttpContextAccessor context, IStoreService storeService,IAdminService adminService, ICartService cartService)
        {
            _context = context;
            _storeService = storeService;
            _adminService = adminService;
            _cartService = cartService;
        }
        public async Task<IActionResult> ListOrderNow(ModelSelectCart req)
        {
            //if (_context.HttpContext.Session.GetInt32("storeID") == null)
            //{
            //    return RedirectToAction("Login", "Store");
            //}
            //var listcart = await _storeService.ListCartOrderTodayByStore(req);
            //if (listcart.Data == null)
            //{
            //    return View(ListEmpty.ListCart);
            //}
            //var customerDtosList = new List<CustomerDtos>();

            //foreach (var order in listcart.Data)
            //{
            //    // Gọi API để lấy thông tin khách hàng từ CustomerId của mỗi đơn hàng
            //    var customer = await _adminService.GetUserById(order.CustomerId);  // Nếu bạn không dùng ID, thay thế bằng thông tin khác

            //    if (customer != null)
            //    {
            //        customerDtosList.Add(customer);
            //    }
            //}
            //// Tạo ViewModel kết hợp đơn hàng và thông tin khách hàng
            //var viewModelList = listcart.Data
            //    .Select(order => new CustomerAndCart
            //    {
            //        CartDtos = new List<CartDtos> { order },
            //        // Ở đây không kiểm tra CustomerId mà chỉ gán customerDtos vào trực tiếp
            //        CustomerDtos = customerDtosList.FirstOrDefault() // Bạn có thể chọn khách hàng đầu tiên hoặc logic khác
            //    }).ToList();
            //return View(viewModelList);
            if (_context.HttpContext.Session.GetInt32("storeID") == null)
            {
                return RedirectToAction("Login", "Store");
            }

            // Lấy danh sách đơn hàng hôm nay của cửa hàng
            //var listcart = await _storeService.ListCartOrderTodayByStore(req);

            //if (listcart.Data == null || !listcart.Data.Any())
            //{
            //    return View(ListEmpty.ListCart); // Trả về View nếu không có đơn hàng
            //}
            var orderDetail = await _cartService.GetCartOrderStatus(0);
            //var customerId = orderDetail?.FirstOrDefault()?.CustomerId;
            //var customerDtos = await _cartService.GetUserById(customerId);

            if (orderDetail == null || !orderDetail.Any())
            {
                TempData["Message"] = "Không có đơn hàng nào đang chờ.";
                return View("NoOrders");
            }
            var customerIds = orderDetail.Select(o => o.CustomerId).Distinct().ToList();
            // Tạo danh sách chứa thông tin khách hàng tương ứng với mỗi đơn hàng
            var customerDtosList = new List<CustomerDtos>();

            foreach (var order in customerIds)
            {
                // Gọi API để lấy thông tin khách hàng từ CustomerId của mỗi đơn hàng
                var customer = await _adminService.GetUserById(order);

                if (customer != null)
                {
                    customerDtosList.Add(customer); // Lưu thông tin khách hàng vào danh sách
                }
            }

            // Tạo ViewModel kết hợp đơn hàng và thông tin khách hàng
            var customerAndCartList = orderDetail.Select(cart => new CustomerAndCart
            {
                CustomerDtos = customerDtosList.FirstOrDefault(c=>c.CustomerId == cart.CustomerId), // Lấy danh sách tất cả khách hàng có cùng CustomerId
                CartDtos = new List<CartDtos> { cart }
            }).ToList();

            // Pass the combined data to the view
            return View(customerAndCartList);
        }

        public async Task<IActionResult> SetStatusOrder(SetStatusCart setReq)
        {
            await _storeService.UpdateStatusOrder(setReq);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> DetailCartByID(int CartID)
        {
            if (_context.HttpContext.Session.GetInt32("storeID") == null)
            {
                return RedirectToAction("Login", "Store");
            }
            ViewBag.CodeCart = CartID;
            //var cart = await _storeService.SelectCartByID(CartID);
            //var totalMoney = cart.DetailCarts.Select(x=>x.TotalMoney).Sum();
            //ViewBag.TotalMoney = totalMoney;
            //return View(cart.DetailCarts);
            var cart = await _cartService.GetCartByID(CartID);
            //var totalMoney = cart.DetailCarts.Select(x => x.Price).Sum();
            // ViewBag.TotalMoney = totalMoney;
            var customer = await _adminService.GetUserById(cart.CustomerId);
            var shipper = await _adminService.GetUserById(cart.ShipperId);
            var viewModelList = new CartAndCustomerAndShipper
            {
                cartDtos = cart,
                customerDtos = customer,
                shipperDtos = shipper,

            };
            return View(viewModelList);
        }

        public async Task<IActionResult> ListCart(/*ModelSelectCart model,*/ int? page, int? cartIdSearch, int? statusSearch)
        {
            if (_context.HttpContext.Session.GetInt32("storeID") == null)
            {
                return RedirectToAction("Login", "Store");
            }
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNum = page ?? 1;
          

            var StoreID = _context.HttpContext.Session.GetInt32("storeID");
            //model.StoreID = (int)StoreID;
            //ViewBag.Status = model.StatusID;
            //ViewBag.DayStart = model.DayStart;
            //ViewBag.DayEnd = model.DayEnd;
            //ViewBag.CartID = model.CartID;
            //ViewBag.Delivery = model.Delivery;
            //ViewBag.Phone = model.Phone;
            //ViewBag.CustomerID = model.CustomerId;
            //var listcart = await _storeService.ListCartByStore(model);
            var orders = await _adminService.GetOrdersByStoreId((int)StoreID);
            //var customer = await _adminService.GetUserById(ViewBag.customerId);
            //var viewModel = new ModelSelectCartAndCustomer
            //{
            //    cartDtos = listcart,
            //    CustomerDtos = customer
            //};
            if (cartIdSearch.HasValue)
            {
                orders = orders.Where(o => o.Id == cartIdSearch.Value).ToList();
            }

            // Lọc theo Status
            if (statusSearch.HasValue)
            {
                orders = orders.Where(o => o.Status == statusSearch.Value).ToList();
            }

            // Lưu lại giá trị tìm kiếm để hiển thị lại trong view
            ViewBag.CartIdSearch = cartIdSearch;
            ViewBag.StatusSearch = statusSearch;
            return View(orders.ToPagedList(pageNum, pageSize));
        }

        public async Task<List<object>> GetTotalCart()
        {
            var model = new ModelSelectCart();
            var storeID = _context.HttpContext.Session.GetInt32("storeID");
            model.StoreID = (int)storeID;
            var listcart = await _storeService.ListCartByStore(model);

            List<object> data = new List<object>();
            var listCount = new List<int>();
            DateTime startDate = DateTime.Now.Date.AddDays(-6);

            List<DateTime> listDay = listcart.Where(x => x.TimeOrder >= startDate).Select(y => y.TimeOrder.Date).Distinct().ToList();

            foreach (var date in listDay)
            {
                int totalCart = listcart.Count(x => x.TimeOrder.Date == date);
                listCount.Add(totalCart);
            }
            data.Add(listDay);
            data.Add(listCount);

            return data;
        }
    }
}
