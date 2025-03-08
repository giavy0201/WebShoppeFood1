using BLL.IService;
using BLL.Model.Address;
using BLL.Model.Store;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace WebSystemStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IStoreService _storeService;
        private readonly IAdminService _adminService;
        public StoreController(IHttpContextAccessor context, IStoreService storeService,IAdminService adminService)
        {
            _context = context;
            _storeService = storeService;
            _adminService = adminService;
            
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Login([FromBody] ReqLogin db)
        //{
        //    var request = await _storeService.LoginSystemStore(db);
        //    if (request.IsSuccess == false)
        //    {
        //        return Json(request);
        //    }
        //    else
        //    {
        //        _context.HttpContext.Session.SetString("customer", request.Data.Username);
        //        _context.HttpContext.Session.SetInt32("storeID", request.Data.StoreID);
        //        return Json(request);
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] ReqLogin db)
        {
            //Thực hiện đăng nhập qua service
            var request = await _storeService.LoginSystemStore(db);

            //Kiểm tra nếu đăng nhập không thành công
            if (request.IsSuccess == false)
            {
                return Json(request);  // Trả về thông tin lỗi nếu đăng nhập không thành công
            }
            else
            {
               // Lưu thông tin người dùng vào Session
                _context.HttpContext.Session.SetString("customer", request.Data.Username);
                _context.HttpContext.Session.SetInt32("storeID", request.Data.StoreID);
               

                //Lấy vai trò của người dùng
                var userRoles = await _adminService.GetUserRoles(request.Data.StoreID, request.Data.Username); // Phương thức lấy vai trò người dùng

               // Kiểm tra vai trò người dùng
                if (userRoles.Contains("Admin"))
                {
                    // Nếu là Admin, chuyển hướng đến trang quản lý Admin
                    //return RedirectToAction("Index", "Admin");   // Chuyển hướng đến trang quản lý Admin
                    _context.HttpContext.Session.SetString("customerRole", "Admin");
                    return Json(request);
                }
                else if (userRoles.Contains("Manage"))
                {
                    //Nếu là StoreManager(quản lý cửa hàng), chuyển hướng đến trang quản lý cửa hàng
                    /* return RedirectToAction("InfoStore", "Store"); */ // Chuyển hướng đến trang quản lý cửa hàng
                    _context.HttpContext.Session.SetString("customerRole", "Manage");
                    return Json(request);
                }
                else
                {
                   // Nếu không phải Admin hoặc StoreManager, có thể chuyển hướng đến trang khác hoặc trả về lỗi
                    return Json(new { IsSuccess = false, Message = "Bạn không có quyền truy cập" });
                }
            }
        }

        public ActionResult Logout()
        {
            _context.HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> InfoStore()
        {
            if (_context.HttpContext.Session.GetInt32("storeID") == null)
            {
                return RedirectToAction("Login", "Store");
            }
            var storeID = _context.HttpContext.Session.GetInt32("storeID");
            var store = await _storeService.DetailStore((int)storeID);
            return View(store);
        }

        public async Task<StoreDtos> DetailStore(int StoreID)
        {
            var store = await _storeService.DetailStore(StoreID);
            return store;
        }

        public async Task<List<DistrictDtos>> ListDistrictByCity(int CityID)
        {
            var districts = await _storeService.GetListDistrictByCity(CityID);
            return districts;
        }
        public async Task<List<WardDtos>> ListWardByDistrict(int DistrictID)
        {
            var wards = await _storeService.GetListWardByDistrict(DistrictID);
            return wards;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStore(ReqUpdateStore updateStore)
        {
            updateStore.AdminName = _context.HttpContext.Session.GetString("customer");
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

        public async Task<IActionResult> SetStatusStore(int StoreID)
        {
            await _storeService.UpdateStatusStore(StoreID);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<ViewIDLocation> ListLocationID(int WardID)
        {
            var listID = await _storeService.LocationIDByWard(WardID);
            return listID;
        }
        public async Task<IActionResult> ListCommentsStore( int? page, string sortOrder = "desc")
        {
            try
            {
                int pageSize = 10; // Number of comments per page
                int pageNumber = page ?? 1; // Default to page 1 if no page is provided
                var storeID = _context.HttpContext.Session.GetInt32("storeID");
                // Fetch comments from the service
                var comments = await _storeService.GetCommentByStoreId((int)storeID);

                // Check if comments are null or empty
                if (comments == null || !comments.Any())
                {
                    TempData["Error"] = "No comments found for this store.";
                    return RedirectToAction("Home/Index"); // Redirect to store info or other appropriate view
                }

                // Apply sorting
                comments = sortOrder.ToLower() == "asc"
                    ? comments.OrderBy(c => c.UpdatedAt).ToList()
                    : comments.OrderByDescending(c => c.UpdatedAt).ToList();

                // Apply pagination
                var pagedComments = comments.ToPagedList(pageNumber, pageSize);

                // Pass paged comments to the view
                return View("ListCommentsStore", pagedComments);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                TempData["Error"] = $"An error occurred while retrieving comments: {ex.Message}";
                return RedirectToAction("HomeIndex"); // Redirect to an error-friendly view or back to store details
            }
        }


    }
}
