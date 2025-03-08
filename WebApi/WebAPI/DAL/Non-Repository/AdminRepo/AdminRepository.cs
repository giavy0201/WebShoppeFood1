using DAL.Entities;
using DAL.Non_Repository.AddressRepo;
using DAL.Non_Repository.MenuRepo;
using DAL.Repository;
using DAL.ModelsRequest.StoreRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unidecode.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DAL.ModelsRequest;
using System.Globalization;

namespace DAL.Non_Repository.AdminRepo
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IRepository<Store> _storeRepo;
        private readonly IRepository<AccountStore> _accountStoreRepo;
        private readonly IAddressRepository _addressRepo;
        private readonly IMenuRepository _menuRepo;
        private readonly DataContext _dataContext;
        //private readonly UserManager<Store> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public AdminRepository(IRepository<Store> storeRepo, IAddressRepository addressRepository, DataContext dataContext, IRepository<AccountStore> accountStoreRepo, IMenuRepository menuRepo)
        {
            _storeRepo = storeRepo;
            _addressRepo = addressRepository;
            _dataContext = dataContext;
            _accountStoreRepo = accountStoreRepo;
            _menuRepo = menuRepo;
            // _userManager = userManager;
            //_roleManager = roleManager;
        }

        public async Task<Store> AddStore(CreateStoreReq createStoreRequest)
        {
            if (createStoreRequest == null)
                throw new ArgumentNullException(nameof(createStoreRequest), "Request data is null");
            // Correct string formatting
            // Correct format for TimeSpan
            TimeSpan timeOpen = TimeSpan.Parse(createStoreRequest.TimeOpen);
            TimeSpan timeClose = TimeSpan.Parse(createStoreRequest.TimeClose);

            var store = new Store
            {
                Name = createStoreRequest.Name,
                NameNoDiacritic = createStoreRequest.Name.Unidecode(),  // Chuyển đổi tên thành không dấu
                TimeOpen = timeOpen,  // Save as string
                TimeClose = timeClose,  // Save as string
                Preferential = createStoreRequest.Preferential ?? string.Empty,
                Address = createStoreRequest.Address,
                AddressNoDiacritic = createStoreRequest.Address.Unidecode(),
                
                DistrictID = createStoreRequest.DistrictID,  // Gán DistrictID
                CityID = createStoreRequest.CityID , // Gán CityID,// Chuyển đổi địa chỉ thành không dấu
                WardID = createStoreRequest.WardID,
                ContentID = createStoreRequest.ContentID,
                AdminName = createStoreRequest.AdminName ?? "Unknown Admin",
                AdminTime = DateTime.Now,  // Đặt thời gian hiện tại
                Status = createStoreRequest.Status ?? 0,  // Giá trị có thể là null, nên cần kiểm tra trước khi gán
                Image = createStoreRequest.Image  // Đảm bảo Image có thể null hoặc có giá trị
            };

            // Thêm store vào cơ sở dữ liệu thông qua repository
            _storeRepo.Insert(store);  // Thêm đối tượng store vào repository (sử dụng InsertAsync cho thao tác bất đồng bộ)
            _storeRepo.Save();  // Lưu thay đổi vào cơ sở dữ liệu (sử dụng SaveAsync để đồng bộ)


            return store;  // Trả về đối tượng store vừa được tạo
        }
          

        public async Task AddUser(CreateCustomerReq createCustomerReq)
        {
            var customer = new Customer
            {
                FirstName = createCustomerReq.FirstName,
                LastName = createCustomerReq.LastName,
                Birthday = createCustomerReq.Birthday,
                Gender = createCustomerReq.Gender,
                Location = createCustomerReq.Location,
                Image = createCustomerReq.Image,
                Email = createCustomerReq.Email,
                UserName = createCustomerReq.Email, // Sử dụng Email làm UserName
                                                    // Lưu ý: Mật khẩu cần được hash trước khi lưu vào database
                PasswordHash = new PasswordHasher<Customer>().HashPassword(null, createCustomerReq.Password),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Thêm đối tượng Customer vào bảng Users
            await _dataContext.Users.AddAsync(customer);
            await _dataContext.SaveChangesAsync(); 
        }

        //public async Task AssignRoleToUser(int storeId, string roleName)
        //{
        //    var user = await _userManager.FindByIdAsync(storeId.ToString());
        //    if (user == null)
        //    {
        //        throw new Exception("User not found.");
        //    }

        //    // Check if the role exists, if not, create it
        //    var role = await _roleManager.FindByNameAsync(roleName);
        //    if (role == null)
        //    {
        //        // Optionally, you can create the role if it does not exist
        //        role = new IdentityRole(roleName);
        //        await _roleManager.CreateAsync(role);
        //    }

        //    // Assign the role to the user
        //    var result = await _userManager.AddToRoleAsync(user, roleName);
        //    if (!result.Succeeded)
        //    {
        //        throw new Exception("Failed to assign role to user.");
        //    }
        //}

        public async Task<bool> DeleteStore(int storeId)
        {
            // Fetch the store entity by its ID asynchronously
            var store =  await _dataContext.Stores.FindAsync(storeId);
            if (store == null)
            {
                return false; // Return false if the store doesn't exist
            }

            // Delete the store and save changes asynchronously
            _dataContext.Stores.Remove(store);
            await _dataContext.SaveChangesAsync(); // Make sure SaveAsync is called for async operations

            return true; // Return true if the store was successfully deleted
        }

        public async Task DeleteUser(string customerId)
        {
            var customer = await _dataContext.Users.FindAsync(customerId);  // Find the customer by ID
            if (customer != null)
            {
                _dataContext.Users.Remove(customer);  // Remove the customer if found
                await _dataContext.SaveChangesAsync();    // Save changes to apply the deletion
            }
        }

        public async Task<List<Cart>> GetAllOrders()
        {
            var carts = await _dataContext.Carts.Include(c => c.Customer).Include(c => c.DetailCarts).ThenInclude(dc => dc.Food) // Include the related DetailCarts data
                                        .ToListAsync();  // Fetch the result as a List
            return carts;
        }
        public async Task<List<Cart>> GetOrdersToday()
        {
            DateTime today = DateTime.Today; // Lấy thời điểm bắt đầu của ngày hôm nay (00:00:00)
            DateTime tomorrow = today.AddDays(1); // Thời điểm bắt đầu của ngày mai (00:00:00)

            var carts = await _dataContext.Carts
                                          .Include(c => c.Customer)
                                          .Include(c => c.DetailCarts)
                                          .ThenInclude(dc => dc.Food)
                                          .Where(c => c.TimeOrder >= today && c.TimeOrder < tomorrow) // Lọc các đơn hàng trong ngày hôm nay
                                          .ToListAsync();

            return carts;
        }

        public async Task<List<Store>> GetAllStores()
        {
            var stores = await _dataContext.Stores
                                        .Include(s => s.Ward)  // Bao gồm dữ liệu Ward nếu cần
                                        .Include(s => s.ContentProduct) // Bao gồm dữ liệu ContentProduct nếu cần
                                        .ToListAsync(); // Chuyển đổi thành danh sách

            return stores;
        }

        public async Task<List<Customer>> GetAllUsers()
        {
            var users = await _dataContext.Users
                                   .Select(u => new Customer
                                   {
                                       Id = u.Id,
                                       FirstName = u.FirstName,
                                       LastName = u.LastName,
                                       Birthday = u.Birthday,
                                       Gender = u.Gender,
                                       PhoneNumber = u.PhoneNumber,
                                       Location = u.Location,
                                       Image = u.Image
                                   })
                                   .ToListAsync();  // Thực hiện bất đồng bộ

            return users;
        }

        public async Task<List<Cart>> GetOrdersByStoreId(int storeId)
        {
            var orders = await _dataContext.Carts
                                     .Where(c => c.StoreId == storeId) // Filter by storeId
                                     .Include(c => c.DetailCarts)
                                     .ThenInclude(dc => dc.Food)// Include related DetailCarts data
                                     .ToListAsync(); // Fetch the result as a List asynchronously

            return orders;
        }
        public async Task<List<Cart>> GetOrdersByOrderId(int orderId)
        {
            var orders = await _dataContext.Carts
       .Where(c => c.Id == orderId) // Filter by orderId
       .Include(c => c.DetailCarts) // Include related DetailCarts data
       .ThenInclude(dc => dc.Food) // Include related Food data for each DetailCart
       .ToListAsync(); // Fetch the result as a List asynchronously

            return orders;
        }
        public async Task<double> GetTotalRevenueForAllStoresToday()
        {
            var today = DateTime.Today; // Get today's date at midnight
            var totalRevenue = 0.0;

            var storeIds = await _dataContext.Stores.Select(store => store.Id).ToListAsync();

            foreach (var storeId in storeIds)
            {
                var revenueForStore = await (from cart in _dataContext.Carts
                                             join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                             where cart.StoreId == storeId
                                                   && cart.Status == ValueOrder.Done
                                                   && cart.TimeOrder >= today // Chỉ lấy đơn hàng trong ngày hôm nay
                                                   && cart.TimeOrder < today.AddDays(1) // Lọc các đơn hàng đến cuối ngày hôm nay
                                             select detail.Price * detail.Quantity)
                            .SumAsync();
                totalRevenue += revenueForStore;
            }

            return totalRevenue;
        }
        public async Task<double> GetTotalRevenueForAllStores(DateTime startDate, DateTime endDate)
        {
            var totalRevenue = 0.0;

            var storeIds = await _dataContext.Stores.Select(store => store.Id).ToListAsync();

            foreach (var storeId in storeIds)
            {
                var revenueForStore = await (from cart in _dataContext.Carts
                                             join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                             where cart.StoreId == storeId
                                                   && cart.Status == ValueOrder.Done
                                                   && cart.TimeOrder >= startDate // Check if the cart was created within the date range
                                                   && cart.TimeOrder < endDate
                                             select detail.Price * detail.Quantity)
                                            .SumAsync();

                totalRevenue += revenueForStore;
            }

            return totalRevenue;
        }

        public async Task<double> GetRevenueByStoreId(int storeId)
        {
            var revenue = await (from cart in _dataContext.Carts
                                 join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                 where cart.StoreId == storeId && cart.Status == ValueOrder.Done
                                 select detail.Price * detail.Quantity)
                            .SumAsync();

            return revenue;
        }
        public async Task<double> GetRevenueByStoreIdForToday(int storeId)
        {
            var today = DateTime.Today;  // Lấy ngày hôm nay
            var revenue = await (from cart in _dataContext.Carts
                                 join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                 where cart.StoreId == storeId
                                       && cart.Status == ValueOrder.Done
                                       && cart.TimeOrder >= today // Chỉ lấy đơn hàng trong ngày hôm nay
                                       && cart.TimeOrder < today.AddDays(1) // Lọc các đơn hàng đến cuối ngày hôm nay
                                 select detail.Price * detail.Quantity)
                            .SumAsync();

            return revenue;
        }
        public async Task<double> GetRevenueByStoreIdForThisMonth(int storeId)
        {
            var currentDate = DateTime.Today;
            var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);  // Lấy ngày đầu tháng
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);  // Lấy ngày cuối tháng

            var revenue = await (from cart in _dataContext.Carts
                                 join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                 where cart.StoreId == storeId
                                       && cart.Status == ValueOrder.Done
                                       && cart.TimeOrder >= startOfMonth // Lọc các đơn hàng từ ngày đầu tháng
                                       && cart.TimeOrder <= endOfMonth // Lọc các đơn hàng đến ngày cuối tháng
                                 select detail.Price * detail.Quantity)
                            .SumAsync();

            return revenue;
        }

        public async Task<double> GetRevenueByStoreIdForDate(int storeId, DateTime date)
        {
            var startOfDay = date.Date; // Lấy thời gian bắt đầu của ngày
            var endOfDay = startOfDay.AddDays(1); // Lấy thời gian kết thúc của ngày

            var revenue = await (from cart in _dataContext.Carts
                                 join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                 where cart.StoreId == storeId
                                       && cart.Status == ValueOrder.Done
                                       && cart.TimeOrder >= startOfDay // Lọc các đơn hàng bắt đầu từ đầu ngày
                                       && cart.TimeOrder < endOfDay // Lọc các đơn hàng đến cuối ngày
                                 select detail.Price * detail.Quantity)
                             .SumAsync();

            return revenue;
        }
        public async Task<double> GetRevenueByStoreIdForMonth(int storeId, int year, int month)
        {
            var startOfMonth = new DateTime(year, month, 1); // Lấy ngày đầu tháng
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // Lấy ngày cuối tháng

            var revenue = await (from cart in _dataContext.Carts
                                 join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                 where cart.StoreId == storeId
                                       && cart.Status == ValueOrder.Done
                                       && cart.TimeOrder >= startOfMonth // Lọc các đơn hàng từ ngày đầu tháng
                                       && cart.TimeOrder <= endOfMonth // Lọc các đơn hàng đến ngày cuối tháng
                                 select detail.Price * detail.Quantity)
                             .SumAsync();

            return revenue;
        }
        public async Task<double> GetRevenueByStoreIdForYear(int storeId, int year)
        {
            var startOfYear = new DateTime(year, 1, 1); // Lấy ngày đầu năm
            var endOfYear = new DateTime(year, 12, 31); // Lấy ngày cuối năm

            var revenue = await (from cart in _dataContext.Carts
                                 join detail in _dataContext.DetailCarts on cart.Id equals detail.CartID
                                 where cart.StoreId == storeId
                                       && cart.Status == ValueOrder.Done
                                       && cart.TimeOrder >= startOfYear // Lọc các đơn hàng từ ngày đầu năm
                                       && cart.TimeOrder <= endOfYear // Lọc các đơn hàng đến ngày cuối năm
                                 select detail.Price * detail.Quantity)
                             .SumAsync();

            return revenue;
        }

        //public Task<Store> GetStoreById(int storeId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<Store>> GetStoresByStatus(int status)
        {
            var stores = await _dataContext.Stores
                                  .Where(s => s.Status == status) // Filter by status
                                  .ToListAsync(); // Fetch the result as a List asynchronously

            return stores;
        }

        public async Task<Customer> GetUserById(string customerId)
        {
            var customer = await _dataContext.Users
                                     .FirstOrDefaultAsync(c => c.Id == customerId); // Use FirstOrDefaultAsync to find a matching customer

            return customer;
        }

        //public async Task<List<string>> GetUserRoles(int storeId)
        //{
        //    var user = await _userManager.FindByIdAsync(storeId.ToString());
        //    if (user == null)
        //    {
        //        return null; // or handle as needed
        //    }

        //    // Get the roles assigned to the user
        //    var roles = await _userManager.GetRolesAsync(user);

        //    // Return the list of roles
        //    return roles.ToList();
        //}

        public async Task LockStore(int storeId)
        {
            try
            {
                var store = await _dataContext.Stores.FirstOrDefaultAsync(s => s.Id == storeId);
                if (store == null)
                {
                    throw new Exception("Store not found.");
                }

                // Đánh dấu cửa hàng là khóa (ngừng nhận đơn hàng)
                store.Status = 2;
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc ném ngoại lệ lên trên
                throw new Exception($"Error locking store: {ex.Message}", ex);
            }
        }

        // Phương thức mở khóa cửa hàng
        public async Task UnlockStore(int storeId)
        {
            try
            {
                var store = await _dataContext.Stores.FirstOrDefaultAsync(s => s.Id == storeId);
                if (store == null)
                {
                    throw new Exception("Store not found.");
                }

                // Đánh dấu cửa hàng là mở khóa (có thể nhận đơn hàng lại)
                store.Status = 1;
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error unlocking store: {ex.Message}", ex);
            }
        }

        // Phương thức dừng hoạt động cửa hàng (deactivate)
        public async Task DeactivateStore(int storeId)
        {
            try
            {
                var store = await _dataContext.Stores.FirstOrDefaultAsync(s => s.Id == storeId);
                if (store == null)
                {
                    throw new Exception("Store not found.");
                }

                // Đánh dấu cửa hàng là không hoạt động
                store.Status = 0;
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deactivating store: {ex.Message}", ex);
            }
        }

        // Phương thức kích hoạt lại cửa hàng (activate)
        public async Task ActivateStore(int storeId)
        {
            try
            {
                var store = await _dataContext.Stores.FirstOrDefaultAsync(s => s.Id == storeId);
                if (store == null)
                {
                    throw new Exception("Store not found.");
                }

                // Đánh dấu cửa hàng là hoạt động lại
                store.Status = 1;
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error activating store: {ex.Message}", ex);
            }
        }
            //public async Task RemoveRoleFromUser(int storeId, string roleName)
            //{
            //    // Get the user by their ID
            //    var user = await _userManager.FindByIdAsync(storeId.ToString());

            //    // If the user is not found, handle the error as needed
            //    if (user == null)
            //    {
            //        // Log the error or return a response indicating that the user was not found
            //        throw new Exception($"User with ID {storeId} not found.");
            //    }

            //    // Check if the user is currently in the specified role
            //    if (await _userManager.IsInRoleAsync(user, roleName))
            //    {
            //        // Remove the role from the user
            //        var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            //        // Check if the removal was successful
            //        if (result.Succeeded)
            //        {
            //            // Optionally, return some success result or log the success
            //            Console.WriteLine($"Role '{roleName}' removed from user '{user.Name}' successfully.");
            //        }
            //        else
            //        {
            //            // Handle errors if the removal was not successful
            //            throw new Exception($"Failed to remove role '{roleName}' from user '{user.Name}'.");
            //        }
            //    }
            //    else
            //    {
            //        // Optionally handle the case when the user is not in the role
            //        Console.WriteLine($"User '{user.Name}' is not in the role '{roleName}'.");
            //    }
            //}

            //public async Task UpdateStore(UpdateStoreRequest updateStoreRequest)
            //{
            //    throw new NotImplementedException();
            //}

            //public Task UpdateUser(Customer customerId)
            //{
            //    throw new NotImplementedException();
            //}
            public async Task AssignRoleToUser(int storeId, string roleName, string loginName)
            {
                // Tìm tài khoản người dùng trong cửa hàng
                var accountStore = await _dataContext.AccountStores
                    .FirstOrDefaultAsync(a => (a.LoginName == loginName || a.UserName == loginName) && a.StoreID == storeId);

                if (accountStore == null)
                    throw new Exception("User not found in the specified store.");

                // Kiểm tra nếu tài khoản đã có vai trò này chưa
                if (accountStore.Role == roleName)
                    throw new Exception("User already has the specified role.");

                // Cập nhật vai trò cho người dùng
                accountStore.Role = roleName;
                await _dataContext.SaveChangesAsync();
            }
            public async Task RemoveRoleFromUser(int storeId, string roleName, string loginName)
            {
                // Tìm tài khoản người dùng trong cửa hàng
                var accountStore = await _dataContext.AccountStores
                    .FirstOrDefaultAsync(a => (a.LoginName == loginName || a.UserName == loginName) && a.StoreID == storeId);

                if (accountStore == null)
                    throw new Exception("User not found in the specified store.");

                // Kiểm tra xem tài khoản có vai trò này không
                if (accountStore.Role != roleName)
                    throw new Exception("User does not have the specified role.");

                // Xóa vai trò của người dùng
                accountStore.Role = null; // Bạn có thể gán về một giá trị mặc định, ví dụ "Staff" nếu cần.
                await _dataContext.SaveChangesAsync();
            }
            public async Task<List<string>> GetUserRoles(int storeId, string username)
            {
                var roles = await _dataContext.AccountStores
                    .Where(a => ( a.UserName == username) && a.StoreID == storeId)
                    .Select(a => a.Role)
                    .ToListAsync();

                return roles;
            }
            public async Task<List<string>> GetCustomerRoles(string userId)
            {
                var roles = await _dataContext.Users.Where(a => a.Id==userId)
                .Select(a => a.Role)
                .ToListAsync();
            return roles;
            }

            public async Task<bool> IsUserGlobalAdmin(int storeId, string loginName)
            {
                var accountStore = await _dataContext.AccountStores
                    .FirstOrDefaultAsync(a => (a.LoginName == loginName || a.UserName == loginName) && a.StoreID == storeId);

                return accountStore?.Role == "Admin" && accountStore.IsGlobalAdmin;
            }

        public Task<Store> GetStoreById(int storeId)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Comment>> GetAllCommentStores()
        {
            var commentStores = await _dataContext.Comments
         .Include(c => c.Customer) // Gọi đến Customer để lấy FirstName và LastName
         .Select(c => new Comment
         {
             Id = c.Id,
             Content = c.Content,
             StarRating = c.StarRating,
             CreatedAt = c.CreatedAt,
             UpdatedAt = c.UpdatedAt,
             Status = c.Status,
             StoreId = c.StoreId,
             CustomerId = c.Customer.Id,
             FirstName = c.Customer.FirstName,
             LastName = c.Customer.LastName
         })
         .ToListAsync();

            return commentStores;
        }
        public async Task<Comment> GetCommentById(int commentId)
        {
            return await _dataContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        }
        public async Task<bool> DeleteComment(int commentId)
        {
            try
            {
                // Tìm comment trong database
                var comment = await _dataContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

                if (comment == null)
                {
                    Console.WriteLine($"Comment with ID {commentId} not found.");
                    return false; // Comment không tồn tại
                }

                // Xóa comment
                _dataContext.Comments.Remove(comment);
                await _dataContext.SaveChangesAsync(); // Lưu thay đổi vào DB

                Console.WriteLine($"Comment with ID {commentId} has been deleted.");
                return true; // Thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting comment: {ex.Message}");
                return false; // Lỗi trong quá trình xóa
            }
        }

    }
}

