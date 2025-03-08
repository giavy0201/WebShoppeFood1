using BLL.IService;
using BLL.Models.Authentication;
using BLL.Models.DTOs;
using BLL.Models.DTOs.Cart;
using BLL.Models.DTOs.Customer;
using BLL.Models.Request;
using BLL.Models.Request.Store;
using BLL.Service;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using static BLL.Models.Validate.ValidateGeneric;
namespace WebAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly ICartService _cartService;
        private readonly IAdminService _adminService;
        private readonly IAccountCustomer _accountRepo;
        private readonly IAddressService _addressService;
        private readonly IProductService _productService;

        public AdminController(IStoreService storeService, ICartService cartService, IAdminService adminService, IAccountCustomer accountRepo, IAddressService 
            addressService,IProductService productService)
        {
            _storeService = storeService;
            _cartService = cartService;
            _adminService = adminService;
            _accountRepo = accountRepo;
            _addressService = addressService;
            _productService = productService;
        }

        [HttpPost("AddStore")]
        public async Task<IActionResult> AddStore(CreateStoreRequest createStoreRequest)
        {
            try
            {
                var storeSystem = await _adminService.AddStore(createStoreRequest);
                if (storeSystem == null)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Failed to create store"));
                }
                return Ok(ApiResponse<StoreSystem>.Success("Store created successfully", storeSystem));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(SignUpModel model)
        {
            var (status, message) = await _accountRepo.SignUpAsync(model); // Giả sử AddUserAsync trả về tuple status và message.

            if (status == 0)
            {
                return BadRequest(message);
            }

            return CreatedAtAction(nameof(AddUser), model); // Tạo phản hồi với thông tin của người dùng mới được thêm.
        }


        //[HttpPost("AssignRole")]
        //public async Task<IActionResult> AssignRoleToUser(int storeId, string roleName)
        //{
        //    try
        //    {
        //        await _adminService.AssignRoleToUser(storeId, roleName);
        //        return Ok(ApiResponse<string>.Success("Role assigned successfully", roleName));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}

        [HttpDelete("DeleteStore/{storeId}")]
        public async Task<IActionResult> DeleteStore(int storeId)
        {
            try
            {
                var result = await _adminService.DeleteStore(storeId);
                if (!result)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Failed to delete store"));
                }
                return Ok(ApiResponse<string>.Success("Store deleted successfully", storeId.ToString()));


            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpDelete("DeleteUser/{customerId}")]
        public async Task<IActionResult> DeleteUser(string customerId)
        {
            try
            {
                await _adminService.DeleteUser(customerId);
                return Ok(ApiResponse<string>.Success("User deleted successfully", customerId)); // Return customerId instead of customer object
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                // Lấy danh sách đơn hàng từ BLL, đây có thể là List<CartSystem>
                var orders = await _adminService.GetAllOrders();

                // Trả về kết quả với ApiResponse
                return Ok(ApiResponse<List<CartSystem>>.Success("Orders retrieved successfully", orders));
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("GetAllOrdersToday")]
        public async Task<IActionResult> GetAllOrdersToday()
        {
            try
            {
                // Lấy danh sách đơn hàng từ BLL, đây có thể là List<CartSystem>
                var orders = await _adminService.GetOrdersToday();

                // Trả về kết quả với ApiResponse
                return Ok(ApiResponse<List<CartSystem>>.Success("Orders retrieved successfully", orders));
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("GetAllStores")]
        public async Task<IActionResult> GetAllStores()
        {
            try
            {
                // Gọi đến service để lấy danh sách StoreDtos
                var stores = await _adminService.GetAllStores();

                // Trả về ApiResponse với kiểu danh sách StoreDtos thay vì Store
                return Ok(ApiResponse<List<StoreSystem>>.Success("Stores retrieved successfully", stores));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                // Lấy danh sách khách hàng từ BLL (Danh sách DTOs)
                var users = await _adminService.GetAllUsers();

                // Trả về API response với danh sách DTOs
                return Ok(ApiResponse<List<CustomerDtos>>.Success("Users retrieved successfully", users));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về mã lỗi 500
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpGet("GetOrdersByStoreId/{storeId}")]
        public async Task<IActionResult> GetOrdersByStoreId(int storeId)
        {
            try
            {
                // Gọi dịch vụ BLL để lấy các đơn hàng
                var orders = await _adminService.GetOrdersByStoreId(storeId);

                // Trả về ApiResponse với kiểu đúng
                return Ok(ApiResponse<List<CartSystem>>.Success("Orders retrieved successfully", orders));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("GetTotalRevenueForAllStoresToday")]
        public async Task<IActionResult> GetTotalRevenueForAllStoresToday()
        {
            try
            {
                // Call the service method to get the total revenue for all stores for today
                var totalRevenue = await _adminService.GetTotalRevenueForAllStoresToday();

                // Return the response with a success message and the total revenue
                return Ok(ApiResponse<double>.Success("Total revenue for all stores retrieved successfully", totalRevenue));
            }
            catch (Exception ex)
            {
                // Handle any errors and return an error response
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("GetTotalRevenueForAllStoresForWeek")]
        public async Task<IActionResult> GetTotalRevenueForAllStoresForWeek([FromQuery] string period = "week")
        {
            try
            {
                DateTime startDate;
                DateTime endDate = DateTime.Now; // End date is the current date

                // Calculate start date based on the selected period (week or month)
                if (period.ToLower() == "week")
                {
                    startDate = endDate.AddDays(-7); // 7 days ago
                }
                else if (period.ToLower() == "month")
                {
                    startDate = new DateTime(endDate.Year, endDate.Month, 1); // First day of this month
                }
                else
                {
                    return BadRequest(ApiResponse<string>.Error("Invalid period. Use 'week' or 'month'."));
                }

                // Call the service method to get the total revenue for all stores for the specified period
                var totalRevenue = await _adminService.GetTotalRevenueForAllStores(startDate, endDate);

                // Return the response with a success message and the total revenue
                return Ok(ApiResponse<double>.Success($"Total revenue for all stores from {startDate.ToShortDateString()} to {endDate.ToShortDateString()} retrieved successfully", totalRevenue));
            }
            catch (Exception ex)
            {
                // Handle any errors and return an error response
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("GetTotalRevenueForAllStoresForMonth")]
        public async Task<IActionResult> GetTotalRevenueForAllStoresForMonth([FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            try
            {
                DateTime startDate;
                DateTime endDate;

                // Sử dụng tháng và năm hiện tại nếu không được cung cấp
                if (!month.HasValue)
                {
                    month = DateTime.Now.Month;
                }
                if (!year.HasValue)
                {
                    year = DateTime.Now.Year;
                }

                // Kiểm tra tính hợp lệ của tháng và năm
                if (month < 1 || month > 12 || year < 1)
                {
                    return BadRequest(ApiResponse<string>.Error("Invalid month or year. Month must be between 1 and 12."));
                }

                // Tính ngày bắt đầu và ngày kết thúc của tháng được chỉ định
                startDate = new DateTime(year.Value, month.Value, 1); // Ngày đầu tiên của tháng
                endDate = startDate.AddMonths(1).AddDays(-1); // Ngày cuối cùng của tháng

                // Gọi service để lấy tổng doanh thu
                var totalRevenue = await _adminService.GetTotalRevenueForAllStores(startDate, endDate);

                // Trả về phản hồi với thông điệp thành công và tổng doanh thu
                return Ok(ApiResponse<double>.Success(
                    $"Total revenue for all stores from {startDate.ToShortDateString()} to {endDate.ToShortDateString()} retrieved successfully",
                    totalRevenue));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi lỗi
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        //[HttpGet("GetLast8Weeks")]
        //public IActionResult GetLast8Weeks()
        //{
        //    try
        //    {
        //        var weeks = new List<object>();
        //        DateTime today = DateTime.Now;

        //        for (int i = 0; i < 8; i++)
        //        {
        //            DateTime endOfWeek = today.AddDays(-7 * i); // Ngày cuối tuần
        //            DateTime startOfWeek = endOfWeek.AddDays(-6); // Ngày đầu tuần
        //            weeks.Add(new
        //            {
        //                WeekNumber = i + 1,
        //                StartDate = startOfWeek.ToShortDateString(),
        //                EndDate = endOfWeek.ToShortDateString()
        //            });
        //        }

        //        return Ok(ApiResponse<List<object>>.Success("Last 8 weeks retrieved successfully", weeks));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}
        //[HttpGet("GetLast12Months")]
        //public IActionResult GetLast12Months()
        //{
        //    try
        //    {
        //        var months = new List<object>();
        //        DateTime today = DateTime.Now;

        //        for (int i = 0; i < 12; i++)
        //        {
        //            DateTime currentMonth = today.AddMonths(-i);
        //            DateTime startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
        //            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
        //            months.Add(new
        //            {
        //                MonthNumber = i + 1,
        //                MonthName = currentMonth.ToString("MMMM yyyy"), // Ví dụ: "November 2024"
        //                StartDate = startOfMonth.ToShortDateString(),
        //                EndDate = endOfMonth.ToShortDateString()
        //            });
        //        }

        //        return Ok(ApiResponse<List<object>>.Success("Last 12 months retrieved successfully", months));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}
        [HttpGet("GetTotalRevenueForLast8Weeks")]
        public async Task<IActionResult> GetTotalRevenueForLast8Weeks()
        {
            try
            {
                var weeks = new List<object>();
                DateTime today = DateTime.Now;

                for (int i = 0; i < 8; i++)
                {
                    DateTime endOfWeek = today.AddDays(-7 * i); // Ngày cuối tuần
                    DateTime startOfWeek = endOfWeek.AddDays(-6); // Ngày đầu tuần

                    // Gọi service để lấy tổng doanh thu cho tuần này
                    var totalRevenue = await _adminService.GetTotalRevenueForAllStores(startOfWeek, endOfWeek);

                    weeks.Add(new
                    {
                        WeekNumber = i + 1,
                        StartDate = startOfWeek.ToShortDateString(),
                        EndDate = endOfWeek.ToShortDateString(),
                        TotalRevenue = totalRevenue
                    });
                }

                return Ok(ApiResponse<List<object>>.Success("Total revenue for the last 8 weeks retrieved successfully", weeks));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpGet("GetTotalRevenueForLast12Months")]
        public async Task<IActionResult> GetTotalRevenueForLast12Months()
        {
            try
            {
                var months = new List<object>();
                DateTime today = DateTime.Now;

                for (int i = 0; i < 12; i++)
                {
                    DateTime currentMonth = today.AddMonths(-i);
                    DateTime startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    // Gọi service để lấy tổng doanh thu cho tháng này
                    var totalRevenue = await _adminService.GetTotalRevenueForAllStores(startOfMonth, endOfMonth);

                    months.Add(new
                    {
                        MonthNumber = i + 1,
                        MonthName = currentMonth.ToString("MMMM yyyy"), // Ví dụ: "November 2024"
                        StartDate = startOfMonth.ToShortDateString(),
                        EndDate = endOfMonth.ToShortDateString(),
                        TotalRevenue = totalRevenue
                    });
                }

                return Ok(ApiResponse<List<object>>.Success("Total revenue for the last 12 months retrieved successfully", months));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }




        [HttpGet("GetRevenueByStoreId/{storeId}")]
        public async Task<IActionResult> GetRevenueByStoreId(int storeId)
        {
            try
            {
                var revenue = await _adminService.GetRevenueByStoreId(storeId);
                return Ok(ApiResponse<double>.Success("Revenue retrieved successfully", revenue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("GetRevenueByStoreIdDayOrMonth/{storeId}")]
        public async Task<IActionResult> GetRevenueByStoreIdDayOrMonth(int storeId, [FromQuery] string period = "day")
        {
            try
            {
                double revenue;

                // Kiểm tra xem người dùng muốn tính doanh thu theo ngày hay tháng
                if (period.ToLower() == "month")
                {
                    revenue = await _adminService.GetRevenueByStoreIdForThisMonth(storeId); // Tính doanh thu theo tháng
                }
                else
                {
                    revenue = await _adminService.GetRevenueByStoreIdForToday(storeId); // Tính doanh thu theo ngày
                }

                return Ok(ApiResponse<double>.Success("Revenue retrieved successfully", revenue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }
        [HttpGet("GetRevenueByStoreIdDayOrMonthOrYear/{storeId}")]
        public async Task<IActionResult> GetRevenueByStoreIdDayOrMonthOrYear(int storeId, [FromQuery] string period = "day", [FromQuery] int? year = null, [FromQuery] int? month = null, [FromQuery] DateTime? date = null)
        {
            try
            {
                double revenue;

                // Kiểm tra nếu người dùng yêu cầu doanh thu theo ngày
                if (period.ToLower() == "day" && date.HasValue)
                {
                    revenue = await _adminService.GetRevenueByStoreIdForDate(storeId, date.Value);
                }
                // Kiểm tra nếu người dùng yêu cầu doanh thu theo tháng
                else if (period.ToLower() == "month" && year.HasValue && month.HasValue)
                {
                    revenue = await _adminService.GetRevenueByStoreIdForMonth(storeId, year.Value, month.Value);
                }
                // Kiểm tra nếu người dùng yêu cầu doanh thu theo năm
                else if (period.ToLower() == "year" && year.HasValue)
                {
                    revenue = await _adminService.GetRevenueByStoreIdForYear(storeId, year.Value);
                }
                else
                {
                    return BadRequest(ApiResponse<string>.Error("Invalid parameters."));
                }

                return Ok(ApiResponse<double>.Success("Revenue retrieved successfully", revenue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }


        [HttpGet("GetStoresByStatus/{status}")]
        public async Task<IActionResult> GetStoresByStatus(int status)
        {
            try
            {
                var stores = await _adminService.GetStoresByStatus(status);
                return Ok(ApiResponse<List<StoreSystem>>.Success("Stores retrieved successfully", stores));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        [HttpGet("GetUserById/{customerId}")]
        public async Task<IActionResult> GetUserById(string customerId)
        {
            try
            {
                var user = await _adminService.GetUserById(customerId);
                return Ok(ApiResponse<CustomerDtos>.Success("User retrieved successfully", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
            }
        }

        //[HttpGet("GetUserRoles/{storeId}")]
        //public async Task<IActionResult> GetUserRoles(int storeId)
        //{
        //    try
        //    {
        //        var roles = await _adminService.GetUserRoles(storeId);
        //        return Ok(ApiResponse<List<string>>.Success("User roles retrieved successfully", roles));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}

        //[HttpPut("LockOrUnlockStore/{storeId}")]
        //public async Task<IActionResult> LockOrUnlockStore(int storeId, [FromQuery] bool isLocked)
        //{
        //    try
        //    {
        //        await _adminService.LockOrUnlockStore(storeId, isLocked);
        //        return Ok(ApiResponse<string>.Success(isLocked ? "Store locked successfully" : "Store unlocked successfully", storeId.ToString()));

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}
        // Phương thức khoá cửa hàng
        [HttpPut("lock-store/{storeId}")]
        public async Task<IActionResult> LockStore(int storeId)
        {
            try
            {
                await _adminService.LockStore(storeId);
                return Ok(new { message = $"Store {storeId} has been locked successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error locking store {storeId}: {ex.Message}" });
            }
        }

        // Phương thức mở khoá cửa hàng
        [HttpPut("unlock-store/{storeId}")]
        public async Task<IActionResult> UnlockStore(int storeId)
        {
            try
            {
                await _adminService.UnlockStore(storeId);
                return Ok(new { message = $"Store {storeId} has been unlocked successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error unlocking store {storeId}: {ex.Message}" });
            }
        }

        // Phương thức dừng hoạt động cửa hàng
        [HttpPut("deactivate-store/{storeId}")]
        public async Task<IActionResult> DeactivateStore(int storeId)
        {
            try
            {
                await _adminService.DeactivateStore(storeId);
                return Ok(new { message = $"Store {storeId} has been deactivated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error deactivating store {storeId}: {ex.Message}" });
            }
        }

        // Phương thức kích hoạt lại cửa hàng
        [HttpPut("activate-store/{storeId}")]
        public async Task<IActionResult> ActivateStore(int storeId)
        {
            try
            {
                await _adminService.ActivateStore(storeId);
                return Ok(new { message = $"Store {storeId} has been activated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error activating store {storeId}: {ex.Message}" });
            }
        }

        //[HttpPut("RemoveRoleFromUser/{storeId}")]
        //public async Task<IActionResult> RemoveRoleFromUser(int storeId, string roleName)
        //{
        //    try
        //    {
        //        await _adminService.RemoveRoleFromUser(storeId, roleName);
        //        return Ok(ApiResponse<string>.Success("Role removed from user successfully", roleName));

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<string>.Error($"Error: {ex.Message}"));
        //    }
        //}
        [HttpPut("UpdateUser/{email}")]
        public async Task<IActionResult> UpdateUserInfo(string email, [FromBody] UpdateUserInfoRequest updateRequest)
        {
            if (updateRequest == null || string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Invalid request body or email");
            }

            if (updateRequest.Email != email)
            {
                return BadRequest("Email mismatch");
            }

            var (status, message) = await _accountRepo.UpdateUserInfoAsync(updateRequest);

            switch (status)
            {
                case 0:
                    return BadRequest(message);
                case 1:
                    return Ok(message);
                default:
                    return StatusCode(500, message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateInfoStore(UpdateStoreRequest updateStoreRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var validate = Validate.ValidateInput(ModelState);
                    return BadRequest(ApiResponse<string>.BadRequest(validate));
                }
                var StoreID = await _storeService.GetStoreById(updateStoreRequest.StoreID);
                if (StoreID == null)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Cửa Hàng Không Tồn Tại"));
                }
                if (!IsTimeValid(updateStoreRequest.TimeOpen) || !IsTimeValid(updateStoreRequest.TimeClose))
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Nhập Giờ Không Hợp Lệ"));
                }
                var WardID = await _addressService.GetWardById((int)updateStoreRequest.WardID);
                if (WardID == null)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Phường Không Tồn Tại"));
                }
                var ContentID = _productService.GetContentById((int)updateStoreRequest.ContentID);
                if (ContentID == null)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Loại Cửa Hàng Không Tồn Tại"));
                }
                var result = await _storeService.UpdateInfoStore(updateStoreRequest);
                if (result == null)
                {
                    return BadRequest(ApiResponse<string>.BadRequest("Cập Nhập Thất Bại"));
                }
                return Ok(ApiResponse<string>.SuccessCRUD("Cập Nhập Thành Công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error($"{ex.Message}"));
            }
        }
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser(int storeId, string roleName, string loginName)
        {
            try
            {
                await _adminService.AssignRoleToUser(storeId, roleName, loginName);
                return Ok("Role assigned successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error assigning role: {ex.Message}");
            }
        }

        // API: Xóa vai trò của người dùng
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRoleFromUser(int storeId, string roleName, string loginName)
        {
            try
            {
                await _adminService.RemoveRoleFromUser(storeId, roleName, loginName);
                return Ok("Role removed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error removing role: {ex.Message}");
            }
        }

        // API: Lấy danh sách vai trò của người dùng
        [HttpGet("get-user-roles")]
        public async Task<IActionResult> GetUserRoles(int storeId, string username)
        {
            try
            {
                var roles = await _adminService.GetUserRoles(storeId, username);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving roles: {ex.Message}");
            }
        }
        [HttpGet("get-customer-roles")]
        public async Task<IActionResult> GetCustomerRoles(string userId)
        {
            try
            {
                var roles = await _adminService.GetCustomerRoles(userId);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving roles: {ex.Message}");
            }
        }

        // API: Kiểm tra người dùng có phải là Global Admin hay không
        [HttpGet("is-global-admin")]
        public async Task<IActionResult> IsUserGlobalAdmin(int storeId, string loginName)
        {
            try
            {
                var isGlobalAdmin = await _adminService.IsUserGlobalAdmin(storeId, loginName);
                return Ok(isGlobalAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error checking if user is a global admin: {ex.Message}");
            }
        }
        [HttpGet("get-all-comments")]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                var comments = await _adminService.GetAllCommentStores();
                if (comments == null || !comments.Any())
                {
                    return NotFound("No comments found.");
                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving comments: {ex.Message}");
            }
        }
        //[HttpDelete("delete-comment/{commentId}")]
        //public async Task<IActionResult> DeleteComment(int commentId)
        //{
        //    try
        //    {
        //        var isDeleted = await _adminService.DeleteComment(commentId);
        //        if (isDeleted)
        //        {
        //            // Thay đổi thông báo phản hồi ở đây nếu cần
        //            return Ok(new { message = "Comment deleted successfully." });
        //        }
        //        else
        //        {
        //            return NotFound(new { message = $"Comment with ID {commentId} not found." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = $"Error deleting comment: {ex.Message}" });
        //    }
        //}

    }
}
