using BLL.IService;
using BLL.Models.DTOs;
using BLL.Models.DTOs.Cart;
using BLL.Models.DTOs.Customer;
using BLL.Models.Request;
using BLL.Service;
using DAL.Entities;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Util;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class CartController : Controller
    {
        private readonly ICartService _cartService;
    

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            
        }

        //[HttpGet("monthly")]
        //public async Task<IActionResult> GetOrdersByMonth(int storeID, int month)
        //{
        //    try
        //    {
        //        // Lấy danh sách các đơn hàng từ service
        //        var orders = await _cartService.GetOrdersByStoreAndMonth(storeID, month);

        //        // Kiểm tra nếu danh sách đơn hàng không tồn tại
        //        if (orders == null || !orders.Any())
        //        {
        //            return BadRequest(ApiResponse<string>.BadRequest("Không có đơn hàng trong tháng này cho cửa hàng này"));
        //        }

        //        // Group các đơn hàng theo tháng và đếm số lượng đơn hàng trong mỗi tháng
        //        var ordersByMonth = orders.GroupBy(o => o.TimeOrder?.Month)
        //                                  .Where(g => g.Key.HasValue) // Lọc ra những nhóm có tháng không null
        //                                  .Select(g => new
        //                                  {
        //                                      Month = g.Key,
        //                                      OrderCount = g.Count()
        //                                  })
        //                                  .ToList();

        //        return Ok(ordersByMonth);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}
        [HttpGet("Status")]
            public async Task<IActionResult> GetCartOrderStatus(int status)
            {
            try
            {
                // Lấy danh sách đơn hàng theo trạng thái
                var carts = await _cartService.GetCartsByStatus(status);

                // Kiểm tra nếu danh sách đơn hàng rỗng hoặc null
                if (carts == null || !carts.Any())
                {
                    return NotFound(ApiResponse<string>.NotFound("Không tìm thấy đơn hàng với trạng thái này."));
                }

                // Trả về kết quả thành công
                return Ok(ApiResponse<List<CartSystem>>.Success("Truy xuất thành công.", carts));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi không mong muốn
                return StatusCode(500, ApiResponse<string>.Error($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpGet("StatusDone")]
        public async Task<IActionResult> GetCartOrderStatusDone(int status, string shipper)
        {
            try
            {
                // Lấy danh sách đơn hàng theo trạng thái
                var carts = await _cartService.GetCartsByStatusDone(status, shipper);

                // Kiểm tra nếu danh sách đơn hàng rỗng hoặc null
                if (carts == null || !carts.Any())
                {
                    return NotFound(ApiResponse<string>.NotFound("Không tìm thấy đơn hàng với trạng thái này."));
                }

                // Trả về kết quả thành công
                return Ok(ApiResponse<List<CartSystem>>.Success("Truy xuất thành công.", carts));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi không mong muốn
                return StatusCode(500, ApiResponse<string>.Error($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }
        [HttpGet("GetCartsWithShipperInfo")]
        public async Task<IActionResult> GetCartsWithShipperInfo(string shipper)
        {
            try
            {
                var result = await _cartService.GetCartsByShipperConfirmedStatus(shipper);

                // Kiểm tra nếu không có đơn hàng nào
                if (result == null || !result.Any())
                {
                    return NotFound(ApiResponse<string>.NotFound("Không tìm thấy đơn hàng nào đã xác nhận shipper."));
                }

                return Ok(ApiResponse<List<CartSystem>>.Success("Truy xuất thành công.", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }


        // Lấy thông tin shipper theo cartId
        [HttpGet("GetShipperInfoByCartId/{cartId}")]
        public async Task<IActionResult> GetShipperInfoByCartId(int cartId)
        {
            try
            {
                var shipper = await _cartService.GetShipperInfoByCartId(cartId);

                // Kiểm tra nếu không tìm thấy thông tin shipper
                if (shipper == null)
                {
                    return NotFound(ApiResponse<string>.NotFound("Không tìm thấy thông tin shipper cho đơn hàng này."));
                }

                return Ok(ApiResponse<CustomerDtos>.Success("Thông tin shipper được tìm thấy.", shipper));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }


        // Xử lý khi shipper nhận đơn hàng
        [HttpPost("AcceptOrder/{cartId}/{userId}")]
        public async Task<IActionResult> AcceptOrder(int cartId, string userId)
        {
            try
            {
                // Xử lý khi shipper xác nhận đơn hàng
                await _cartService.AcceptOrder(cartId, userId); // No need to assign it to a variable

                // Trả về phản hồi thành công
                return Ok(ApiResponse<string>.Success("Đơn hàng đã được xác nhận thành công.", null)); // Pass null for data
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }
        [HttpPost("CompleteOrder/{cartId}/{userId}")]
        public async Task<IActionResult> CompleteOrder(int cartId, string userId)
        {
            try
            {
                // Xử lý khi shipper xác nhận đơn hàng
                await _cartService.CompleteOrder(cartId, userId); // No need to assign it to a variable

                // Trả về phản hồi thành công
                return Ok(ApiResponse<string>.Success("Đơn hàng đã được xác nhận thành công.", null)); // Pass null for data
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Đã xảy ra lỗi: {ex.Message}"));
            }

        }


        [HttpPost("/Carts")]
        public async Task<IActionResult> AddCart(CartRequest cartRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var validate = Validate.ValidateInput(ModelState);
                    return BadRequest(ApiResponse<string>.BadRequest(validate));
                }
                var result = await _cartService.InsertCart(cartRequest);
                if (result == null)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Order Thất Bại"));
                }
                return Ok(ApiResponse<Cart>.Success("Order Thành Công", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpGet("{CartID}")]
        public async Task<IActionResult> CartByID(int CartID)
        {
            var cart = await _cartService.CartByID(CartID);
            if (cart == null)
            {
                return BadRequest(ApiResponse<string>.BadRequest("Truy Xuất Thất Bại"));
            }
            return Ok(ApiResponse<CartSystem>.Success("Truy Xuất Thành Công", cart));
        }

        [HttpPost("Search")]
        public async Task<IActionResult> ListCartByStore(SearchCartRequest searchCartRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var validate = Validate.ValidateInput(ModelState);
                    return BadRequest(ApiResponse<string>.BadRequest(validate));
                }
                var result = await _cartService.ListCartByStore(searchCartRequest);
                if (!result.Any())
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Truy Xuất Thất Bại"));
                }
                return Ok(ApiResponse<List<CartSystem>>.Success("Truy Xuất Thành Công", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpPost("Status")]
        public async Task<IActionResult> UpdateCartOrder(SetStatusCartReq setStatusCartRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var validate = Validate.ValidateInput(ModelState);
                    return BadRequest(ApiResponse<string>.BadRequest(validate));
                }
                var result = await _cartService.UpdateCartOrder(setStatusCartRequest);
                if (result == null)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Cập Nhập Thất Bại"));
                }
                return Ok(ApiResponse<ModelApiRequest>.Success("Cập Nhập Thành Công", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpGet("/TotalMoneyForMonth")]
        public async Task<IActionResult> GetTotalMoneyForMonth(int storeID, int month, int year)
        {
            try
            {
                // Kiểm tra đầu vào
                if (storeID <= 0 || month < 1 || month > 12 || year < 1)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Thông tin không hợp lệ."));
                }

                // Gọi phương thức để tính tổng doanh thu trong một tháng
                var totalMoney = await _cartService.TotalMoneyForMonth(storeID, month, year);

                // Trả về kết quả thành công
                return Ok(ApiResponse<double>.Success("Tổng doanh thu trong tháng đã được truy xuất thành công.", totalMoney));
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có ngoại lệ xảy ra
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("yearly/{storeID}/{year}")]
        public async Task<IActionResult> GetYearlyRevenue(int storeID, int year)
        {
            try
            {
                // Kiểm tra đầu vào
                if (storeID <= 0 || year < 1)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Thông tin không hợp lệ."));
                }

                // Gọi phương thức để tính tổng doanh thu trong năm
                var yearlyRevenue = await _cartService.TotalMoneyForYear(storeID, year);

                // Trả về kết quả thành công
                return Ok(ApiResponse<List<double>>.Success("Danh sách doanh thu theo năm đã được truy xuất thành công.", yearlyRevenue));
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có ngoại lệ xảy ra
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        //[HttpGet("7days/{storeID}")]
        //public async Task<IActionResult> GetTotalMoneyForLast7Days(int storeID)
        //{
        //    try
        //    {
        //        // Kiểm tra đầu vào
        //        if (storeID <= 0)
        //        {
        //            return BadRequest(ApiResponse<string>.BadRequest("Thông tin không hợp lệ."));
        //        }

        //        // Gọi phương thức để tính tổng doanh thu trong 7 ngày gần nhất
        //        var totalMoneyForLast7Days = await _cartService.TotalMoneyForLast7Days(storeID);

        //        // Trả về kết quả thành công
        //        return Ok(ApiResponse<List<double>>.Success("Danh sách doanh thu trong 7 ngày gần nhất đã được truy xuất thành công.", totalMoneyForLast7Days));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Trả về lỗi nếu có ngoại lệ xảy ra
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}
        //[HttpGet("8weeks/{storeID}")]
        //public async Task<IActionResult> GetTotalMoneyForLast8Weeks(int storeID)
        //{
        //    try
        //    {
        //        // Kiểm tra đầu vào
        //        if (storeID <= 0)
        //        {
        //            return BadRequest(ApiResponse<string>.BadRequest("Thông tin không hợp lệ."));
        //        }

        //        // Gọi phương thức để tính tổng doanh thu trong 8 tuần gần nhất
        //        var totalMoneyForLast8Weeks = await _cartService.TotalMoneyForLast8Weeks(storeID);

        //        // Trả về kết quả thành công
        //        return Ok(ApiResponse<List<double>>.Success("Danh sách doanh thu trong 8 tuần gần nhất đã được truy xuất thành công.", totalMoneyForLast8Weeks));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Trả về lỗi nếu có ngoại lệ xảy ra
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}
        //[HttpGet("12months/{storeID}")]
        //public async Task<IActionResult> GetTotalMoneyForLast12Months(int storeID)
        //{
        //    try
        //    {
        //        // Kiểm tra đầu vào
        //        if (storeID <= 0)
        //        {
        //            return BadRequest(ApiResponse<string>.BadRequest("Thông tin không hợp lệ."));
        //        }

        //        // Gọi phương thức để tính tổng doanh thu trong 12 tháng gần nhất
        //        var totalMoneyForLast12Months = await _cartService.TotalMoneyForLast12Months(storeID);

        //        // Trả về kết quả thành công
        //        return Ok(ApiResponse<List<double>>.Success("Danh sách doanh thu trong 12 tháng gần nhất đã được truy xuất thành công.", totalMoneyForLast12Months));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Trả về lỗi nếu có ngoại lệ xảy ra
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}
        [HttpGet("GetTotalRevenueForEachStoreLast7Days")]
        public async Task<IActionResult> GetTotalRevenueForEachStoreLast7Days(int storeId)
        {
            try
            {
                var storesRevenue = new List<object>();
                DateTime today = DateTime.Now;
                DateTime sevenDaysAgo = today.AddDays(-6);

                // Iterate over the last 7 days and get revenue for each day
                for (int i = 0; i < 7; i++)
                {
                    DateTime startOfDay = sevenDaysAgo.AddDays(i); // Each day starting from sevenDaysAgo
                    DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1); // End of the day (just before midnight)

                    // Get revenue for the specified store for this day
                    var revenueForStore = await _cartService.GetRevenueForStoreId(storeId, startOfDay, endOfDay);

                    storesRevenue.Add(new
                    {
                        Date = startOfDay.ToShortDateString(),
                        TotalRevenue = revenueForStore
                    });
                }

                return Ok(ApiResponse<List<object>>.Success("Total revenue for the specified store in the last 7 days retrieved successfully", storesRevenue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpGet("GetTotalRevenueForEachStoreLast8Weeks")]
        public async Task<IActionResult> GetTotalRevenueForEachStoreLast8Weeks(int storeId)
        {
            try
            {
                var weeksRevenue = new List<object>();
                DateTime today = DateTime.Now;

                // Loop through each of the last 8 weeks
                for (int i = 0; i < 8; i++)
                {
                    DateTime endOfWeek = today.AddDays(-7 * i); // End of the current week
                    DateTime startOfWeek = endOfWeek.AddDays(-6); // Start of the current week

                    // Get revenue for the specified store for this week
                    var revenueForStore = await _cartService.GetRevenueForStoreId(storeId, startOfWeek, endOfWeek);
                   
                    weeksRevenue.Add(new
                    {
                        WeekNumber = i + 1,
                        StartDate = startOfWeek.ToShortDateString(),
                        EndDate = endOfWeek.ToShortDateString(),
                    
                        TotalRevenue = revenueForStore
                    });
                }

                return Ok(ApiResponse<List<object>>.Success("Total revenue for the specified store in the last 8 weeks retrieved successfully", weeksRevenue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("GetTotalRevenueForEachStoreLast12Months")]
        public async Task<IActionResult> GetTotalRevenueForEachStoreLast12Months(int storeId)
        {
            try
            {
                var monthsRevenue = new List<object>();
                DateTime today = DateTime.Now;

                // Loop through each of the last 12 months
                for (int i = 0; i < 12; i++)
                {
                    DateTime currentMonth = today.AddMonths(-i);
                    DateTime startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    // Get revenue for the specified store for this month
                    var revenueForStore = await _cartService.GetRevenueForStoreId(storeId, startOfMonth, endOfMonth);
                   
                    monthsRevenue.Add(new
                    {
                        MonthNumber = i + 1,
                        MonthName = currentMonth.ToString("MMMM yyyy"), // For example: "November 2024"
                        StartDate = startOfMonth.ToShortDateString(),
                        EndDate = endOfMonth.ToShortDateString(),
                      
                        TotalRevenue = revenueForStore
                    });
                }

                return Ok(ApiResponse<List<object>>.Success("Total revenue for the specified store in the last 12 months retrieved successfully", monthsRevenue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }



        [HttpGet("GetOrCreateCartId/{email}")]
        public async Task<IActionResult> GetCartByEmail(string email)
        {
            try
            {
                // Kiểm tra xem email có giá trị hợp lệ hay không
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Email không được để trống."));
                }

                // Gọi phương thức từ service để lấy danh sách ID của giỏ hàng dựa trên email
                var cartIds = await _cartService.GetCartIdByEmail(email);

                // Kiểm tra xem có giỏ hàng nào được tìm thấy hay không
                if (cartIds == null || !cartIds.Any())
                {
                    return NotFound(ApiResponse<string>.NotFound("Không tìm thấy giỏ hàng cho email đã cho."));
                }

                // Trả về danh sách ID của giỏ hàng thành công
                return Ok(ApiResponse<List<int>>.Success("Lấy ID giỏ hàng thành công", cartIds));
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có ngoại lệ xảy ra
                return StatusCode(500, ApiResponse<string>.Error($"Lỗi: {ex.Message}"));
            }
        }
        //[HttpPost("{cartId}/payment")]
        //public async Task<IActionResult> CreatePayment(int cartId, decimal amount)
        //{
        //    string returnUrl = "https://localhost:7203/Cart";
        //    string paymentUrl = await _cartService.CreateVNPayPaymentUrl(cartId, amount,returnUrl);
        //    return Ok(new { paymentUrl });
        //}
        //[HttpGet("payment/callback")]
        //public async Task<IActionResult> VNPayCallback([FromQuery] int vnp_TxnRef, [FromQuery] string vnp_ResponseCode)
        //{
        //    await _cartService.HandleVNPayCallbackAsync(vnp_TxnRef, vnp_ResponseCode);
        //    return Ok("Payment processed successfully.");
        //}
        //[HttpPost("create-payment/{cartId}")]
        //public IActionResult CreatePayment(int cartId)
        //{
        //    // Retrieve cart and validate
        //    var cart = _cartService.CartByID(cartId);
        //    if (cart == null)
        //    {
        //        return NotFound("Cart not found");
        //    }

        //    // Create VNPay payment URL
        //    string paymentUrl = _paymentService.CreatePaymentUrl(cart);

        //    // Update cart payment status to pending
        //    cart.Status= "Pending";
        //    cart = "VNPay";
        //    _cartService.UpdateCart(cart);

        //    return Ok(new { paymentUrl });
        //}

        //[HttpGet("payment-return")]
        //public async Task<IActionResult> PaymentReturn()
        //{
        //    var queryCollection = Request.Query;

        //    // Process payment response
        //    var paymentResult = await _paymentService.ProcessPaymentResponse(queryCollection);

        //    if (paymentResult.Success)
        //    {
        //        // Retrieve and update cart
        //        var cart = _cartService.CartByID(paymentResult);
        //        cart.PaymentStatus = "Success";
        //        cart.PaymentTime = DateTime.Now;
        //        cart.Status = 2; // Paid and processed

        //        _cartService.UpdateCart(cart);

        //        return Redirect($"/payment-success?orderId={cart.Id}");
        //    }
        //    else
        //    {
        //        return Redirect($"/payment-failed?reason={paymentResult.Message}");
        //    }
        //}

        //[HttpGet("payment-ipn")]
        //public async Task<IActionResult> PaymentIPN()
        //{
        //    var queryCollection = Request.Query;

        //    // Process IPN (Instant Payment Notification)
        //    var paymentResult = await _vnPayService.ProcessPaymentResponse(queryCollection);

        //    if (paymentResult.Success)
        //    {
        //        // Log successful payment or perform additional processing
        //        return Ok("Payment processed successfully");
        //    }

        //    return BadRequest("Payment processing failed");
        //}
    }
}
