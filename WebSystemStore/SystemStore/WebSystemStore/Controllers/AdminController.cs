using BLL.IService;
using BLL.Model;
using BLL.Model.Address;
using BLL.Model.Store;
using BLL.Request;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using static NuGet.Packaging.PackagingConstants;
namespace WebSystemStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IAdminService _adminService;
        private readonly IStoreService _storeService;
        private readonly ICartService _cartService;
        public AdminController(IHttpContextAccessor context, IAdminService adminService, IStoreService storeService, ICartService cartService)
        {
            _context = context;
            _adminService = adminService;
            _storeService = storeService;
            _cartService = cartService;
        }
        //public async Task<IActionResult> Index()
        //{
        //    // Giả sử bạn có các phương thức để lấy thông tin hệ thống
        //    var storeCount = await _adminService.GetAllStores();
        //    var userCount = await _adminService.GetAllUsers();
        //    // var totalRevenue = await _adminService.();

        //    // Truyền dữ liệu sang view thông qua ViewBag hoặc ViewModel
        //    ViewBag.StoreCount = storeCount;
        //    ViewBag.UserCount = userCount;
        //    // ViewBag.TotalRevenue = totalRevenue;

        //    return View();
        //}

        //// List of all stores with pagination
        //public async Task<IActionResult> ListStores(int? page)
        //{
        //    if (_context.HttpContext.Session.GetInt32("storeID") == null)
        //    {
        //        return RedirectToAction("Login", "Store");
        //    }

        //    if (page == null) page = 1;
        //    int pageSize = 10;
        //    int pageNum = page ?? 1;

        //    var storeList = await _adminService.GetAllStores();
        //    return View(storeList.ToPagedList(pageNum, pageSize));
        //}

        //// Create a new store
        //[HttpPost]
        //public async Task<IActionResult> CreateStore(CreateStoreRequest createStoreRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(createStoreRequest);
        //    }

        //    await _adminService.AddStore(createStoreRequest);
        //    return RedirectToAction("ListStores");
        //}

        //// Delete a store
        //public async Task<IActionResult> DeleteStore(int storeId)
        //{
        //    await _adminService.DeleteStore(storeId);
        //    return RedirectToAction("ListStores");
        //}

        //// List all users (admins/customers)
        //public async Task<IActionResult> ListUsers(int? page)
        //{
        //    if (_context.HttpContext.Session.GetInt32("storeID") == null)
        //    {
        //        return RedirectToAction("Login", "Store");
        //    }

        //    if (page == null) page = 1;
        //    int pageSize = 10;
        //    int pageNum = page ?? 1;

        //    var userList = await _adminService.GetAllUsers();
        //    return View(userList.ToPagedList(pageNum, pageSize));
        //}

        //// Delete a user
        //public async Task<IActionResult> DeleteUser(string customerId)
        //{
        //    await _adminService.DeleteUser(customerId);
        //    return RedirectToAction("ListUsers");
        //}

        //// Assign role to a user
        //public async Task<IActionResult> AssignRoleToUser(int storeId, string roleName, string loginName)
        //{
        //    await _adminService.AssignRoleToUser(storeId, roleName, loginName);
        //    return RedirectToAction("ListUsers");
        //}

        //// Remove role from a user
        //public async Task<IActionResult> RemoveRoleFromUser(int storeId, string roleName, string loginName)
        //{
        //    await _adminService.RemoveRoleFromUser(storeId, roleName, loginName);
        //    return RedirectToAction("ListUsers");
        //}

        //// Get revenue details for a store
        //public async Task<IActionResult> StoreRevenue(int storeId)
        //{
        //    var revenue = await _adminService.GetRevenueByStoreId(storeId);
        //    ViewBag.Revenue = revenue;
        //    return View();
        //}

        //// Get revenue for today for a specific store
        //public async Task<IActionResult> StoreRevenueToday(int storeId)
        //{
        //    var revenue = await _adminService.GetRevenueByStoreIdForToday(storeId);
        //    ViewBag.Revenue = revenue;
        //    return View();
        //}

        //// Get revenue for a specific month for a store
        //public async Task<IActionResult> StoreRevenueThisMonth(int storeId)
        //{
        //    var revenue = await _adminService.GetRevenueByStoreIdForThisMonth(storeId);
        //    ViewBag.Revenue = revenue;
        //    return View();
        //}

        //// Get revenue for a specific date for a store
        //public async Task<IActionResult> StoreRevenueByDate(int storeId, DateTime date)
        //{
        //    var revenue = await _adminService.GetRevenueByStoreIdForDate(storeId, date);
        //    ViewBag.Revenue = revenue;
        //    return View();
        //}

        //// Get revenue for a specific year for a store
        //public async Task<IActionResult> StoreRevenueByYear(int storeId, int year)
        //{
        //    var revenue = await _adminService.GetRevenueByStoreIdForYear(storeId, year);
        //    ViewBag.Revenue = revenue;
        //    return View();
        //}

        //// Get revenue for a specific month and year for a store
        //public async Task<IActionResult> StoreRevenueByMonth(int storeId, int year, int month)
        //{
        //    var revenue = await _adminService.GetRevenueByStoreIdForMonth(storeId, year, month);
        //    ViewBag.Revenue = revenue;
        //    return View();
        //}

        //// Lock a store
        //public async Task<IActionResult> LockStore(int storeId)
        //{
        //    await _adminService.LockStore(storeId);
        //    return RedirectToAction("ListStores");
        //}

        //// Unlock a store
        //public async Task<IActionResult> UnlockStore(int storeId)
        //{
        //    await _adminService.UnlockStore(storeId);
        //    return RedirectToAction("ListStores");
        //}

        //// Deactivate a store
        //public async Task<IActionResult> DeactivateStore(int storeId)
        //{
        //    await _adminService.DeactivateStore(storeId);
        //    return RedirectToAction("ListStores");
        //}

        //// Activate a store
        //public async Task<IActionResult> ActivateStore(int storeId)
        //{
        //    await _adminService.ActivateStore(storeId);
        //    return RedirectToAction("ListStores");
        //}

        //// Get roles of a user
        //public async Task<IActionResult> GetUserRoles(int storeId, string loginName)
        //{
        //    var roles = await _adminService.GetUserRoles(storeId, loginName);
        //    ViewBag.Roles = roles;
        //    return View();
        //}

        //// Check if a user is a global admin
        //public async Task<IActionResult> IsUserGlobalAdmin(int storeId, string loginName)
        //{
        //    var isGlobalAdmin = await _adminService.IsUserGlobalAdmin(storeId, loginName);
        //    ViewBag.IsGlobalAdmin = isGlobalAdmin;
        //    return View();
        //}

        //// Get stores by status
        //public async Task<IActionResult> ListStoresByStatus(int status)
        //{
        //    var storeList = await _adminService.GetStoresByStatus(status);
        //    return View(storeList);
        //}

        //// View store details by ID
        //public async Task<IActionResult> StoreDetails(int storeId)
        //{
        //    var store = await _adminService.GetOrdersByStoreId(storeId);
        //    if (store == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(store);
        //}

        //// Get user details by ID
        //public async Task<IActionResult> UserDetails(string customerId)
        //{
        //    var user = await _adminService.GetUserById(customerId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string sortOrder = "desc")
        {
            try
            {
                
                var stores = await _adminService.GetAllStores();
                var pagedStores = stores.ToPagedList(page, pageSize);
                var customers = await _adminService.GetAllUsers();
                var pagedCustomers = customers.ToPagedList(page, pageSize);
                var carts = await _adminService.GetAllOrders(); // Get all orders (carts)
                if (sortOrder == "asc")
                {
                    carts = carts.OrderBy(o => o.TimeOrder).ToList();
                }
                else
                {
                    carts = carts.OrderByDescending(o => o.TimeOrder).ToList();
                }
                var pagedCarts = carts.ToPagedList(page, pageSize); // Use the correct variable here

                //ViewBag.StoreCount = storeCount.Count;
                //ViewBag.UserCount = userCount.Count;
                var comments = await _adminService.GetAllCommentStores();
                var pagedComments = comments.ToPagedList(page, pageSize);
                var model = new AdminDashboardViewModel
                {
                    Stores = pagedStores, // Truyền danh sách cửa hàng vào ViewModel
                    Customers = pagedCustomers, // Truyền danh sách khách hàng vào ViewModel
                    Carts = pagedCarts,
                    Comments = pagedComments
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading the dashboard: " + ex.Message;
                return View();
            }
        }

        // List of all stores with pagination
        public async Task<IActionResult> ListStores(int? page, string searchTerm)
        {
            try
            {
                if (_context.HttpContext.Session.GetInt32("storeID") == null)
                {
                    return RedirectToAction("Login", "Store");
                }

                int pageSize = 10;
                int pageNum = page ?? 1;

                var storeList = await _adminService.GetAllStores();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    storeList = storeList.Where(s => s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                s.Address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                ViewBag.SearchTerm = searchTerm;
              
                return View(storeList.ToPagedList(pageNum, pageSize));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the store list: " + ex.Message;
                return View();
            }
        }
        //[HttpGet("Admin/ListUsers")]
        public async Task<IActionResult> ListUsers(int? page, string sortOrder = "asc", string searchTerm = "")
        {
            //try
            //{
            //    int pageSize = 10;
            //    int pageNum = page ?? 1;

            //    // Fetch all users (customers)
            //    var users = await _adminService.GetAllUsers();

            //    // Apply sorting based on the sortOrder parameter
            //    if (sortOrder == "asc")
            //    {
            //        users = users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToList();
            //    }
            //    else
            //    {
            //        users = users.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName).ToList();
            //    }

            //    // Apply pagination
            //    var pagedUsers = users.ToPagedList(pageNum, pageSize);

            //    return View(pagedUsers);
            //}
            //catch (Exception ex)
            //{
            //    TempData["Error"] = "An error occurred while fetching the user list: " + ex.Message;
            //    return View();
            //}
            try
            {
                // Kiểm tra session nếu được yêu cầu
                if (_context.HttpContext.Session.GetInt32("storeID") == null)
                {
                    return RedirectToAction("Login", "Store");
                }

                int pageSize = 10; // Số lượng mục trên mỗi trang
                int pageNum = page ?? 1; // Trang hiện tại, mặc định là trang 1

                // Lấy danh sách người dùng
                var userList = await _adminService.GetAllUsers();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    userList = userList.Where(u => u.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                    u.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                // Sắp xếp danh sách người dùng
                userList = sortOrder == "asc"
                    ? userList.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToList()
                    : userList.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName).ToList();
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SortOrder = sortOrder;
                // Trả về view với danh sách đã phân trang
                return View(userList.ToPagedList(pageNum, pageSize));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                TempData["Error"] = "An error occurred while fetching the user list: " + ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> ListOrders(int? page, string sortOrder = "desc", string searchTerm = "")
        {
            try
            {
                int pageSize = 10;
                int pageNum = page ?? 1;

                // Fetch all orders (carts)
                var carts = await _adminService.GetAllOrders();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    carts = carts.Where(o =>
                        o.StoreId.ToString().Contains(searchTerm)

                    )
                  .ToList();
                }


                // Apply sorting based on the sortOrder parameter
                if (sortOrder == "asc")
                    {
                        carts = carts.OrderBy(o => o.TimeOrder).ToList();
                    }
                    else
                    {
                        carts = carts.OrderByDescending(o => o.TimeOrder).ToList();
                    }

                    // Apply pagination
                    var pagedCarts = carts.ToPagedList(pageNum, pageSize);
                    ViewBag.SearchTerm = searchTerm;
                    ViewBag.SortOrder = sortOrder;
                   
                    return View(pagedCarts);
                }
            
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the cart list: " + ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> ListComments(int? page, string sortOrder = "desc", string searchTerm = "" )
        {
            try
            {
               
                int pageSize = 10;
                int pageNum = page ?? 1;

                var comments = await _adminService.GetAllCommentStores();

                if (comments == null)
                {
                    TempData["Error"] = "No comments found.";
                    return View();
                }
                // Apply search filter based on the searchTerm
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    comments = comments.Where(c =>
                        c.StoreId.ToString().Contains(searchTerm)  // Search by Store ID
                      // Search by Customer Name
                    ).ToList();
                }
                // Apply sorting based on the sortOrder parameter
                if (sortOrder == "asc")
                {
                    comments = comments.OrderBy(o => o.UpdatedAt).ToList();
                }
                else
                {
                    comments = comments.OrderByDescending(o => o.UpdatedAt).ToList();
                }
                // Apply star rating filter if provided
               

                // Apply pagination
                var pagedComments = comments.ToPagedList(pageNum, pageSize);
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SortOrder = sortOrder;
               
                return View("~/Views/Admin/ListComments.cshtml", pagedComments);

            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                return View();
            }
        }
        [HttpPost]
        [Route("Admin/DeleteComment")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var result = await _adminService.DeleteComment(commentId);

                if (result)
                {
                    TempData["SuccessMessage"] = "Comment deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the comment. Please try again.";
                }

                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
               
            }
            return RedirectToAction("ListComments");
        }


        //[HttpGet]
        //public async Task<IActionResult> CreateStore()
        //{
        //    // Fetch cities from the service
        //    var cities = await _adminService.GetCities();

        //    // Check if the cities list is null or empty
        //    ViewBag.Cities = cities; // Ensure we handle null or empty list safely

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateStore(CreateStoreRequest createStoreRequest)
        //{
        //    // Check if model state is valid
        //    if (!ModelState.IsValid)
        //    {
        //        // Fetch cities to rebind the ViewBag for dropdown lists
        //        var cities = await _adminService.GetCities();
        //        ViewBag.Cities = cities;

        //        // Fetch districts if a city is selected
        //        if (createStoreRequest.CityID != 0)
        //        {
        //            var districts = await _adminService.GetDistrictsByCityId(createStoreRequest.CityID);
        //            ViewBag.Districts = districts ?? new List<DistrictDtos>();

        //            // Fetch wards if a district is selected
        //            if (createStoreRequest.DistrictID != 0)
        //            {
        //                var wards = await _adminService.GetWardsByDistrictId(createStoreRequest.DistrictID);
        //                ViewBag.Wards = wards ?? new List<WardDtos>();
        //            }
        //        }

        //        return View(createStoreRequest);
        //    }

        //    try
        //    {
        //        // Add the store using the provided request data
        //        await _adminService.AddStore(createStoreRequest);

        //        // Success message
        //        TempData["Success"] = "Store created successfully!";

        //        // Redirect to the store list page
        //        return RedirectToAction("ListStores");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error message
        //        TempData["Error"] = $"An error occurred while creating the store: {ex.Message}";

        //        // Fetch cities again for re-binding to the view
        //        var cities = await _adminService.GetCities();
        //        ViewBag.Cities = cities;

        //        // Fetch districts if a city is selected
        //        if (createStoreRequest.CityID != 0)
        //        {
        //            var districts = await _adminService.GetDistrictsByCityId(createStoreRequest.CityID);
        //            ViewBag.Districts = districts ?? new List<DistrictDtos>();

        //            // Fetch wards if a district is selected
        //            if (createStoreRequest.DistrictID != 0)
        //            {
        //                var wards = await _adminService.GetWardsByDistrictId(createStoreRequest.DistrictID);
        //                ViewBag.Wards = wards ?? new List<WardDtos>();
        //            }
        //        }

        //        // Return the form with the request data
        //        return View(createStoreRequest);
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> CreateStore()
        {
            // Fetch cities and initialize ViewBag
            await PopulateLocationDataAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore(CreateStoreRequest createStoreRequest)
        {
            // Check if model state is valid
            if (!ModelState.IsValid)
            {
                // Re-bind the ViewBag data in case of validation errors
                await PopulateLocationDataAsync(createStoreRequest.CityID, createStoreRequest.DistrictID);

                return View(createStoreRequest);
            }

            try
            {
                // Attempt to add the store
                await _adminService.AddStore(createStoreRequest);

                // Success message and redirect
                TempData["Success"] = "Store created successfully!";
                return RedirectToAction("ListStores");
            }
            catch (Exception ex)
            {
                // Log error and provide a failure message
                TempData["Error"] = $"An error occurred while creating the store: {ex.Message}";

                // Re-bind the location data for the view in case of failure
                await PopulateLocationDataAsync(createStoreRequest.CityID, createStoreRequest.DistrictID);

                return View(createStoreRequest);
            }
        }
        public async Task<IActionResult> UpdateStoreById(ReqUpdateStore updateStore)
        {
            var storeID = _context.HttpContext.Session.GetInt32("storeID");
            if (updateStore.formFile != null)
            {
                //Save image to wwwroot/image
                string extension = Path.GetExtension(updateStore.formFile.FileName);
                string fileName = Path.GetFileNameWithoutExtension(updateStore.formFile.FileName) + extension;
                string path = $"C:\\Users\\Vy\\OneDrive\\Desktop\\WebShoppeFood\\ImgShoppe\\ImageShoppeFood\\ListStore\\Store-{storeID}\\" + fileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await updateStore.formFile.CopyToAsync(fileStream);
                }
                updateStore.Img = fileName;
            }
            var request = await _storeService.UpdateInfoStore(updateStore);
            return Json(request);
        }
      
        private async Task PopulateLocationDataAsync(int? cityId = null, int? districtId = null)
        {
            // Fetch cities and initialize ViewBag
            var cities = await _adminService.GetCities();
            ViewBag.Cities = cities ?? new List<CityDtos>();

            // Fetch districts if a city is selected
            if (cityId.HasValue && cityId.Value != 0)
            {
                var districts = await _adminService.GetDistrictsByCityId(cityId.Value);
                ViewBag.Districts = districts ?? new List<DistrictDtos>();
            }
            else
            {
                ViewBag.Districts = new List<DistrictDtos>();
            }

            // Fetch wards if a district is selected
            if (districtId.HasValue && districtId.Value != 0)
            {
                var wards = await _adminService.GetWardsByDistrictId(districtId.Value);
                ViewBag.Wards = wards ?? new List<WardDtos>();
            }
            else
            {
                ViewBag.Wards = new List<WardDtos>();
            }
        }

        [HttpGet]
        [HttpPost]
        // Delete a store
        public async Task<IActionResult> DeleteStore(int storeId)
        {
            try
            {
                await _adminService.DeleteStore(storeId);
                TempData["Success"] = "Store deleted successfully!";
                return RedirectToAction("ListStores");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the store: " + ex.Message;
                return RedirectToAction("ListStores");
            }
        }

        //// List all users (admins/customers)
        //[HttpGet("Admin/ListUsersWithSession")]
        //public async Task<IActionResult> ListUsers(int? page)
        //{
        //    try
        //    {
        //        if (_context.HttpContext.Session.GetInt32("storeID") == null)
        //        {
        //            return RedirectToAction("Login", "Store");
        //        }

        //        int pageSize = 10;
        //        int pageNum = page ?? 1;

        //        var userList = await _adminService.GetAllUsers();
        //        return View(userList.ToPagedList(pageNum, pageSize));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "An error occurred while fetching the user list: " + ex.Message;
        //        return View();
        //    }
        //}

        // Delete a user
        public async Task<IActionResult> DeleteUser(string customerId)
        {
            try
            {
                await _adminService.DeleteUser(customerId);
                TempData["Success"] = "User deleted successfully!";
                return RedirectToAction("ListUsers");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the user: " + ex.Message;
                return RedirectToAction("ListUsers");
            }
        }

        // Assign role to a user
        public async Task<IActionResult> AssignRoleToUser(int storeId, string roleName, string loginName)
        {
            try
            {
                await _adminService.AssignRoleToUser(storeId, roleName, loginName);
                TempData["Success"] = "Role assigned successfully!";
                return RedirectToAction("ListUsers");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while assigning the role: " + ex.Message;
                return RedirectToAction("ListUsers");
            }
        }

        // Remove role from a user
        public async Task<IActionResult> RemoveRoleFromUser(int storeId, string roleName, string loginName)
        {
            try
            {
                await _adminService.RemoveRoleFromUser(storeId, roleName, loginName);
                TempData["Success"] = "Role removed successfully!";
                return RedirectToAction("ListUsers");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while removing the role: " + ex.Message;
                return RedirectToAction("ListUsers");
            }
        }

        // Get revenue details for a store
        public async Task<IActionResult> StoreRevenue(int storeId)
        {
            try
            {
                var revenue = await _adminService.GetRevenueByStoreId(storeId);
                ViewBag.Revenue = revenue;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the revenue: " + ex.Message;
                return View();
            }
        }

        // Lock a store
        public async Task<IActionResult> LockStore(int storeId)
        {
            try
            {
                await _adminService.LockStore(storeId);
                TempData["Success"] = "Store locked successfully!";
                return RedirectToAction("ListStores");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while locking the store: " + ex.Message;
                return RedirectToAction("ListStores");
            }
        }

        // Unlock a store
        public async Task<IActionResult> UnlockStore(int storeId)
        {
            try
            {
                await _adminService.UnlockStore(storeId);
                TempData["Success"] = "Store unlocked successfully!";
                return RedirectToAction("ListStores");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while unlocking the store: " + ex.Message;
                return RedirectToAction("ListStores");
            }
        }

        // Deactivate a store
        public async Task<IActionResult> DeactivateStore(int storeId)
        {
            try
            {
                await _adminService.DeactivateStore(storeId);
                TempData["Success"] = "Store deactivated successfully!";
                return RedirectToAction("ListStores");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deactivating the store: " + ex.Message;
                return RedirectToAction("ListStores");
            }
        }

        // Activate a store
        public async Task<IActionResult> ActivateStore(int storeId)
        {
            try
            {
                await _adminService.ActivateStore(storeId);
                TempData["Success"] = "Store activated successfully!";
                return RedirectToAction("ListStores");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while activating the store: " + ex.Message;
                return RedirectToAction("ListStores");
            }
        }
        [HttpGet]
        [HttpPost]
        // View store details by ID
        public async Task<IActionResult> StoreDetails(int storeId)
        {

            // return View("StoreDetails");
            try
            {
                var store = await _storeService.DetailStore(storeId);
                if (store == null)
                {
                    TempData["Error"] = "Store not found.";
                    return RedirectToAction("StoreDetails");
                }

                return View(store);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the store details: " + ex.Message;
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        [HttpPost]
        // Get user details by ID
        public async Task<IActionResult> UserDetails(string customerId)
        {
            try
            {
                var user = await _adminService.GetUserById(customerId);
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("ListUsers");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the user details: " + ex.Message;
                return RedirectToAction("ListUsers");
            }
        }
        [HttpGet]
        [HttpPost]
        // View store details by ID
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            try
            {
                var order = await _cartService.GetCartByID(orderId);
                if (order == null)
                {
                    TempData["Error"] = "Order not found.";
                    return RedirectToAction("OrderDetails");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the store details: " + ex.Message;
                return RedirectToAction("Index");
            }
            //return View();
        }
        public async Task<IActionResult> StoreRevenueToday(int storeId)
        {
            try
            {
                var revenue = await _adminService.GetRevenueByStoreIdForToday(storeId);
                ViewBag.RevenueToday = revenue;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching today's revenue: " + ex.Message;
                return View();
            }
        }

        // Get this month's revenue for a specific store
        public async Task<IActionResult> StoreRevenueThisMonth(int storeId)
        {
            try
            {
                var revenue = await _adminService.GetRevenueByStoreIdForThisMonth(storeId);
                ViewBag.RevenueThisMonth = revenue;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching this month's revenue: " + ex.Message;
                return View();
            }
        }

        // Get revenue for a specific date
        public async Task<IActionResult> StoreRevenueForDate(int storeId, DateTime date)
        {
            try
            {
                var revenue = await _adminService.GetRevenueByStoreIdForDate(storeId, date);
                ViewBag.RevenueForDate = revenue;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the revenue for the date: " + ex.Message;
                return View();
            }
        }

        // Get revenue for a specific month and year
        public async Task<IActionResult> StoreRevenueForMonth(int storeId, int year, int month)
        {
            try
            {
                var revenue = await _adminService.GetRevenueByStoreIdForMonth(storeId, year, month);
                ViewBag.RevenueForMonth = revenue;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the revenue for the month: " + ex.Message;
                return View();
            }
        }

        // Get revenue for a specific year
        public async Task<IActionResult> StoreRevenueForYear(int storeId, int year)
        {
            try
            {
                var revenue = await _adminService.GetRevenueByStoreIdForYear(storeId, year);
                ViewBag.RevenueForYear = revenue;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the revenue for the year: " + ex.Message;
                return View();
            }
        }

        // Get all stores by status
        public async Task<IActionResult> ListStoresByStatus(int status)
        {
            try
            {
                var stores = await _adminService.GetStoresByStatus(status);
                return View(stores);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching the stores by status: " + ex.Message;
                return View();
            }
        }
     }
}
