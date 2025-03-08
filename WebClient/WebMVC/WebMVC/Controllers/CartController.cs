
using BLL.IService;
using BLL.Model;
using BLL.Model.Cart;
using BLL.Model.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using WebMVC.Helper;
using WebMVC.Models.Payments;
using BLL.Model.ModelRequest;
using System.Net.WebSockets;
using static NuGet.Packaging.PackagingConstants;
using BLL.Model.ModelStoreDtos;
using Newtonsoft.Json;

namespace WebMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IStoreService _storeService;
        private readonly ICartService _cartService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(IHttpContextAccessor context, IStoreService storeService, ICartService cartService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _storeService = storeService;
            _cartService = cartService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        //private async Task<List<int>> GetOrCreateCartIds()
        //{
        //    var customerId = _context.HttpContext.Session.GetString("customerID");
        //    return await _cartService.GetOrCreateCartIdByCustomerId(customerId);
        //}

        private async Task<List<CartModel>> GetListCart()
        {
            var listCart = _context.HttpContext.Session.Get<List<CartModel>>("Cart");

            if (listCart == null)
            {
                listCart = new List<CartModel>();
                _context.HttpContext.Session.Set("Cart", listCart);
            }
            return listCart;
        }

        private double TotalMoney()
        {
            var listCart = _context.HttpContext.Session.Get<List<CartModel>>("Cart");
            return listCart.Sum(n => n.TotalMoney);
        }

        public async Task<IActionResult> AddToCart(int FoodID)
        {
            var listCart = await GetListCart();
            var cart = listCart.FirstOrDefault(item => item.FoodID == FoodID);

            if (cart == null)
            {
                var food = await _storeService.DetailFood(FoodID);
                if (food == null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }

                cart = new CartModel(FoodID, food.Name, food.PriceDiscount ?? 0, food.StoreID ?? 0);
                if (listCart.Any(x => x.StoreID != cart.StoreID))
                {
                    TempData["ErrorMessage"] = "Bạn không thể thêm sản phẩm từ cửa hàng khác vào giỏ hàng. Hãy xóa giỏ hàng hoặc đặt hàng.";
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                listCart.Add(cart);
            }
            else
            {
                cart.FoodQuantity++;
            }

            _context.HttpContext.Session.Set("Cart", listCart);
            _context.HttpContext.Session.SetString("TotalMoney", string.Format("{0:0,0}", TotalMoney()));
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> ListCart()
        {
            var listCart = await GetListCart();
            return PartialView("_listcart", listCart);
        }

        public async Task<IActionResult> RemoveFoodCart(int FoodID, string strUrl)
        {
            var listCart = await GetListCart();
            var food = listCart.SingleOrDefault(x => x.FoodID == FoodID);

            if (food != null)
            {
                listCart.RemoveAll(x => x.FoodID == FoodID);
                if (listCart.Count == 0)
                {
                    _context.HttpContext.Session.Remove("Cart");
                }
                else
                {
                    _context.HttpContext.Session.Set("Cart", listCart);
                }
                _context.HttpContext.Session.SetString("TotalMoney", string.Format("{0:0,0}", TotalMoney()));
            }
            return Redirect(strUrl);
        }

        public async Task<IActionResult> UpdateCart(int FoodID, int FoodQuantity)
        {
            var listCart = await GetListCart();
            var food = listCart.FirstOrDefault(x => x.FoodID == FoodID);

            if (food != null)
            {
                food.FoodQuantity = FoodQuantity;
                _context.HttpContext.Session.Set("Cart", listCart);
                _context.HttpContext.Session.SetString("TotalMoney", string.Format("{0:0,0}", TotalMoney()));
            }
            return Json(new { success = true });
        }

        public IActionResult RemoveCart(string strUrl)
        {
            _context.HttpContext.Session.Remove("Cart");
            return Redirect(strUrl);
        }

        public async Task<IActionResult> ViewOrder()
        {
            var listCart = await GetListCart();
            return View(listCart);
        }

        //  [HttpGet("/Cart")]
        //// [HttpPost("/Cart")]
        // public async Task<IActionResult> Cart()
        // {
        //     if (_context.HttpContext.Session.GetString("customer") == null)
        //     {
        //         return RedirectToAction("Index", "Home");
        //     }

        //     var listCart = await GetListCart();
        //     //var reqOrderJson = _context.HttpContext.Session.GetString("Order");
        //     //if (!string.IsNullOrEmpty(reqOrderJson))
        //     //{
        //     //    var reqOrder = JsonConvert.DeserializeObject<ReqOrder>(reqOrderJson);
        //     //    ViewData["Order"] = reqOrder;
        //     //}
        //     return View(listCart);
        // }

        // [HttpGet]
        // public IActionResult DatHang()
        // {
        //     if (_context.HttpContext.Session.GetString("customer") == null)
        //     {
        //         return RedirectToAction("Login", "Customer");
        //     }
        //     return RedirectToAction("Cart", "Cart");
        // }

        // [HttpPost]
        // //[HttpPost("/Cart")]
        // public async Task<IActionResult> Payment([FromBody] ReqOrder reqOrder)
        // {
        //     var listCart = await GetListCart();
        //     if (listCart.Count == 0)
        //     {
        //         return Json(new { StatusCode = 0, Message = "Giỏ hàng của bạn đang trống." });
        //     }
        //     //var paymentMethod = reqOrder.PaymentMethod;  // Lấy phương thức thanh toán từ đối tượng ReqOrder
        //     //if (string.IsNullOrEmpty(paymentMethod))
        //     //{
        //     //    return Json(new { StatusCode = 0, Message = "Vui lòng chọn phương thức thanh toán." });
        //     //}

        //     reqOrder.StoreId = listCart.Select(x => x.StoreID).FirstOrDefault();
        //     reqOrder.CustomerId = _context.HttpContext.Session.GetString("customerID");
        //     if (string.IsNullOrEmpty(reqOrder.CustomerId))
        //     {
        //         return Json(new { StatusCode = 0, Message = "Không tìm thấy thông tin khách hàng." });
        //     }
        //     reqOrder.Details = listCart.Select(food => new ReqDetailCart
        //     {
        //         FoodId = food.FoodID,
        //         Quantity = food.FoodQuantity,
        //         Price = food.FoodPrice
        //     }).ToList();
        //     if (!reqOrder.Details.Any())
        //     {
        //         return Json(new { StatusCode = 0, Message = "Chi tiết giỏ hàng không hợp lệ." });
        //     }
        //    _context.HttpContext.Session.SetString("Order", JsonConvert.SerializeObject(reqOrder));
        //     var req = await _storeService.OrderFood(reqOrder);
        //     if (req.StatusCode == 1)
        //     {
        //         _context.HttpContext.Session.Remove("Cart");
        //     }
        //     return Json(req);
        //     //    return Json(new
        //     //    {
        //     //        StatusCode = 1,
        //     //        Data = reqOrder,

        //     //});
        // }
        [HttpGet("/Cart")]
        public async Task<IActionResult> Cart()
        {
            if (_context.HttpContext.Session.GetString("customer") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var listCart = await GetListCart();
            return View(listCart);
        }

        [HttpGet]
        public IActionResult DatHang()
        {
            if (_context.HttpContext.Session.GetString("customer") == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            return RedirectToAction("Cart", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> DatHang([FromBody] ReqOrder reqOrder)
        {
            var listCart = await GetListCart();
            if (listCart.Count == 0)
            {
                return Json(new { StatusCode = 0, Message = "Giỏ hàng của bạn đang trống." });
            }

            reqOrder.StoreId = listCart.Select(x => x.StoreID).FirstOrDefault();
            reqOrder.CustomerId = _context.HttpContext.Session.GetString("customerID");
            reqOrder.Details = listCart.Select(food => new ReqDetailCart
            {
                FoodId = food.FoodID,
                Quantity = food.FoodQuantity,
                Price = food.FoodPrice
            }).ToList();

            var req = await _storeService.OrderFood(reqOrder);
            if (req.StatusCode == 1)
            {
              
                _context.HttpContext.Session.Remove("Cart");
            }
            return Json(req);
        }
        //[HttpPost]
        //public async Task<IActionResult> Payment([FromBody] ReqOrder paymentRequest)
        //{
        //    var orderJson = _context.HttpContext.Session.GetString("Order");
        //    if (string.IsNullOrEmpty(orderJson))
        //    {
        //        return Json(new { StatusCode = 0, Message = "Không tìm thấy đơn hàng." });
        //    }

        //    var reqOrder = JsonConvert.DeserializeObject<ReqOrder>(orderJson);

        //    // Kiểm tra thông tin thanh toán, ví dụ: phương thức thanh toán, số tiền, v.v.
        //    //if (paymentRequest == null || string.IsNullOrEmpty(paymentRequest.Pay))
        //    //{
        //    //    return Json(new { StatusCode = 0, Message = "Vui lòng chọn phương thức thanh toán." });
        //    //}

        //    // Tiến hành xử lý thanh toán ở đây (gọi API thanh toán, lưu trữ thông tin đơn hàng vào database, v.v.)

        //    // Giả sử thanh toán thành công
        //    _context.HttpContext.Session.Remove("Cart"); // Xóa giỏ hàng
        //    _context.HttpContext.Session.Remove("Order"); // Xóa thông tin đơn hàng

        //    return Json(new { StatusCode = 1, Message = "Thanh toán thành công." });
        //}
        //[HttpGet]
        //public async Task<IActionResult> Payment()
        //{
        //    var orderJson = _context.HttpContext.Session.GetString("Order");
        //    if (string.IsNullOrEmpty(orderJson))
        //    {
        //        return RedirectToAction("Cart"); // Nếu không có đơn hàng, quay lại giỏ hàng
        //    }

        //    var reqOrder = JsonConvert.DeserializeObject<ReqOrder>(orderJson);

        //    return View(reqOrder); // Hiển thị đơn hàng đã xác nhận
        //}
        //public IActionResult GetCartDetailsByCustomerId()
        //{
        //    return View();
        //}
        public async Task<IActionResult> History()
        {
            var customerEmail = _context.HttpContext.Session.GetString("customerEmail");
            if (string.IsNullOrEmpty(customerEmail))
            {
                return RedirectToAction("Login", "Customer");
            }

            var orders = await _cartService.GetOrderHistoryByEmail(customerEmail);
            return View(orders);
        }
        //public async Task<IActionResult> SetStatusOrder(SetStatusRequest setReq)
        //{
        //    await _cartService.UpdateStatusOrder(setReq);
        //    return Redirect(Request.Headers["Referer"].ToString());
        //}

        public async Task<IActionResult> DetailCartByID(int CartID)
        {
            //if (_context.HttpContext.Session.GetInt32("storeID") == null)
            //{
            //    return RedirectToAction("Login", "Store");
            //}
            ViewBag.CodeCart = CartID;
            var cart = await _cartService.GetOrderHistory(CartID);
            //var totalMoney = cart.DetailCarts.Select(x => x.Price).Sum();
            // ViewBag.TotalMoney = totalMoney;
            var store = await _storeService.DetailStore(cart.StoreId);
            var viewModelList = new DetailCartAndStore
            {
                Cart = cart,
                Store = store
                
             };
            return View(viewModelList);
            
        }
        // Phương thức xử lý đơn hàng đang chờ (status = 1)
        public async Task<IActionResult> OrderPending()
        {
            var customerEmail = _context.HttpContext.Session.GetString("customerEmail");
            var customerID = _context.HttpContext.Session.GetString("customerID");

            if (string.IsNullOrEmpty(customerEmail))
            {
                return RedirectToAction("Login", "Customer");
            }

            ViewBag.CustomerID = customerID;

            // Lấy danh sách đơn hàng có trạng thái là 1 (Đang chờ)
            var orderDetail = await _cartService.GetCartOrderStatus(1);
            //var customerId = orderDetail?.FirstOrDefault()?.CustomerId;
            //var customerDtos = await _cartService.GetUserById(customerId);

            if (orderDetail == null || !orderDetail.Any())
            {
                TempData["Message"] = "Không có đơn hàng nào đang chờ.";
                return View("NoOrders");
            }
            //var customerDtos = await _cartService.GetUserById(customerId);
            // Lấy danh sách CustomerId từ orderDetail
            var customerIds = orderDetail.Select(o => o.CustomerId).Distinct().ToList();

            // Lấy thông tin khách hàng
            var customerDtosList = new List<CustomerDtos1>();
            foreach (var id in customerIds)
            {
                var customer = await _cartService.GetUserById(id);
                if (customer != null)
                {
                    customerDtosList.Add(customer);
                }
            }

          
            var storeDtosList = new List<StoreDtos>();
            foreach (var order in orderDetail)
            {
                // Gọi API để lấy thông tin khách hàng từ CustomerId của mỗi đơn hàng
                var store = await _storeService.DetailStore(order.StoreId);  // Nếu bạn không dùng ID, thay thế bằng thông tin khác

                if (store != null)
                {
                    storeDtosList.Add(store);
                }
            }
            // Tạo ViewModel kết hợp đơn hàng và thông tin khách hàng
            var viewModelList = orderDetail
                .Select(order => new CustomerAndCart
                {
                    Carts = new List<CartDtos> { order },
                    // Ở đây không kiểm tra CustomerId mà chỉ gán customerDtos vào trực tiếp
                    CustomerDtos1 = customerDtosList.FirstOrDefault(c=> c.CustomerID == order.CustomerId), // Bạn có thể chọn khách hàng đầu tiên hoặc logic khác
                    StoreDtos = storeDtosList.FirstOrDefault(s=> s.Id == order.StoreId)
                }).ToList();

            // Truyền ViewModel vào View
            return View(viewModelList);
        }

        // Phương thức xử lý khi shipper xác nhận đơn hàng (status = 4)
        public async Task<IActionResult> AcceptOrder(int cartId, string userId)
        {
            var customerEmail = _context.HttpContext.Session.GetString("customerEmail");
            if (string.IsNullOrEmpty(customerEmail))
            {
                return RedirectToAction("Login", "Customer");
            }

            // Xác nhận đơn hàng
            bool result = await _cartService.AcceptOrder(cartId, userId);

            if (result)
            {
                TempData["Status"] = 4; // Cập nhật trạng thái sau khi shipper xác nhận

                // Điều hướng đến trang đơn hàng shipper đã xác nhận
                return RedirectToAction("OrderAccepted");
            }

            return RedirectToAction("OrderPending");
        }

        // Phương thức xử lý khi shipper hoàn thành đơn hàng (status = 3)
        public async Task<IActionResult> CompleteOrder(int cartId, string userId)
        {
            var customerEmail = _context.HttpContext.Session.GetString("customerEmail");
            if (string.IsNullOrEmpty(customerEmail))
            {
                return RedirectToAction("Login", "Customer");
            }

            // Hoàn thành giao hàng
            bool result = await _cartService.CompleteOrder(cartId, userId);

            if (result)
            {
                TempData["Status"] = 3; // Cập nhật trạng thái sau khi shipper hoàn thành giao hàng

                // Điều hướng đến trang đơn hàng hoàn thành
                return RedirectToAction("OrderCompleted");
            }

            return RedirectToAction("OrderAccepted");
        }

        // Phương thức xử lý đơn hàng shipper đã xác nhận (status = 4)
        public async Task<IActionResult> OrderAccepted()
        {
            var customerEmail = _context.HttpContext.Session.GetString("customerEmail");
            var customerID = _context.HttpContext.Session.GetString("customerID");

            if (string.IsNullOrEmpty(customerEmail))
            {
                return RedirectToAction("Login", "Customer");
            }

            ViewBag.CustomerID = customerID;
            //var order = await _cartService.GetCartOrderStatus(1);
            //var shipperId = order?.FirstOrDefault()?.ShipperId;
            // Lấy danh sách đơn hàng có trạng thái là 4 (Shipper xác nhận)
            var orderDetail = await _cartService.GetCartsByShipperConfirmedStatus(customerID);
            //var customerId = orderDetail?.FirstOrDefault()?.CustomerId;
            //var customerDtos = await _cartService.GetUserById(customerId);

            if (orderDetail == null || !orderDetail.Any())
            {
                TempData["Message"] = "Không có đơn hàng nào đã được shipper xác nhận.";
                return View("NoOrders");
            }

            // Lấy danh sách khách hàng từ API và đưa vào list
            var customerDtosList = new List<CustomerDtos1>();

            foreach (var order in orderDetail)
            {
                // Gọi API để lấy thông tin khách hàng từ CustomerId của mỗi đơn hàng
                var customer = await _cartService.GetUserById(order.CustomerId);  // Nếu bạn không dùng ID, thay thế bằng thông tin khác

                if (customer != null)
                {
                    customerDtosList.Add(customer);
                }
            }
            var storeDtosList =new List<StoreDtos>();
            foreach (var order in orderDetail)
            {
                // Gọi API để lấy thông tin khách hàng từ CustomerId của mỗi đơn hàng
                var store = await _storeService.DetailStore(order.StoreId);  // Nếu bạn không dùng ID, thay thế bằng thông tin khác

                if (store != null)
                {
                    storeDtosList.Add(store);
                }
            }
            // Tạo ViewModel kết hợp đơn hàng và thông tin khách hàng
            var viewModelList = orderDetail
                .Select(order => new CustomerAndCart
                {
                    Carts = new List<CartDtos> { order },
                    // Ở đây không kiểm tra CustomerId mà chỉ gán customerDtos vào trực tiếp
                    CustomerDtos1 = customerDtosList.FirstOrDefault(c => c.CustomerID == order.CustomerId), // Bạn có thể chọn khách hàng đầu tiên hoặc logic khác
                    StoreDtos = storeDtosList.FirstOrDefault(s => s.Id == order.StoreId)
                }).ToList();

            // Truyền ViewModel vào View
            return View(viewModelList);
        }

        // Phương thức xử lý đơn hàng đã hoàn thành (status = 3)
        public async Task<IActionResult> OrderCompleted()
        {
            var customerEmail = _context.HttpContext.Session.GetString("customerEmail");
            var customerID = _context.HttpContext.Session.GetString("customerID");

            if (string.IsNullOrEmpty(customerEmail))
            {
                return RedirectToAction("Login", "Customer");
            }

           // ViewBag.CustomerID = customerID;
            //var order = await _cartService.GetCartOrderStatus(1);
            //var shipperId = order?.FirstOrDefault()?.ShipperId;
            // Lấy danh sách đơn hàng có trạng thái là 3 (Hoàn thành)
            var orderDetail = await _cartService.GetCartOrderStatusDone(3,customerID);
            //var customerId = orderDetail?.FirstOrDefault()?.CustomerId;
            //var customerDtos = await _cartService.GetUserById(customerId);

            if (orderDetail == null || !orderDetail.Any())
            {
                return View("NoOrders");
            }


            // Lấy danh sách khách hàng từ API và đưa vào list
            var customerDtosList = new List<CustomerDtos1>();

            foreach (var order in orderDetail)
            {
                // Gọi API để lấy thông tin khách hàng từ CustomerId của mỗi đơn hàng
                var customer = await _cartService.GetUserById(order.CustomerId);  // Nếu bạn không dùng ID, thay thế bằng thông tin khác

                if (customer != null)
                {
                    customerDtosList.Add(customer);
                }
            }
            var storeDtosList = new List<StoreDtos>();
            foreach (var order in orderDetail)
            {
                // Gọi API để lấy thông tin khách hàng từ CustomerId của mỗi đơn hàng
                var store = await _storeService.DetailStore(order.StoreId);  // Nếu bạn không dùng ID, thay thế bằng thông tin khác

                if (store != null)
                {
                    storeDtosList.Add(store);
                }
            }
            // Tạo ViewModel kết hợp đơn hàng và thông tin khách hàng
            var viewModelList = orderDetail
                .Select(order => new CustomerAndCart
                {
                    Carts = new List<CartDtos> { order },
                    // Ở đây không kiểm tra CustomerId mà chỉ gán customerDtos vào trực tiếp
                    CustomerDtos1 = customerDtosList.FirstOrDefault(c => c.CustomerID == order.CustomerId),
                    StoreDtos = storeDtosList.FirstOrDefault(s => s.Id == order.StoreId)
                }).ToList();

            // Truyền ViewModel vào View
            return View(viewModelList);
        }


        public async Task<IActionResult> Vnpay(int typePaymentVN, int foodId, int storeId)
        {
            var vnpay = new VnPayLibrary();
            var vnp_Url = _configuration["vnp_Url"];
            var vnp_TmnCode = _configuration["vnp_TmnCode"];
            var vnp_HashSecret = _configuration["vnp_HashSecret"];
            var vnp_Returnurl = _configuration["vnp_Returnurl"];

            var food = await _storeService.DetailFood(foodId);
            if (food == null)
            {
                return NotFound();
            }

            var store = await _storeService.DetailStore(storeId);
            if (store == null)
            {
                return NotFound();
            }

            long amount = (long)(food.Price * 100);

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", amount.ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_context.HttpContext));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán món {food.Name} tại cửa hàng {store.Name}");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", Guid.NewGuid().ToString("N"));

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return Redirect(paymentUrl);
        }
    }
}

