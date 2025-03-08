using BLL.IService;
using DAL.Entities;
using DAL.ModelsRequest.StoreRequest;
using DAL.Non_Repository.AdminRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models.Request.Store;
using AutoMapper;
using BLL.Models.DTOs.Cart;
using BLL.Models.DTOs;
using BLL.Models.DTOs.Customer;
using DAL;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Logging;
using BLL.Models.Request;
using DAL.ModelsRequest;
using System.Runtime.CompilerServices;
using BLL.Models.DTOs.Comment;
using DAL.Non_Repository.CommentRepo;
namespace BLL.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountCustomer> _logger;
        private readonly ILogger<Store> _logger1;

        // Constructor injects IAdminRepo
        public AdminService(IAdminRepository adminRepo, IMapper mapper,ICommentRepository commentRepo)
        {
            _mapper = mapper;
            _adminRepo = adminRepo;
            _commentRepo = commentRepo;
        }

        public async Task<StoreSystem> AddStore(CreateStoreRequest createStoreRequest)
        {
            var createStoreReq = new CreateStoreReq
            {
                Name = createStoreRequest.Name,
                NameNoDiacritic = createStoreRequest.NameNoDiacritic,
                Image = createStoreRequest.Image,
                TimeOpen = createStoreRequest.TimeOpen,
                TimeClose = createStoreRequest.TimeClose,
                Preferential = createStoreRequest.Preferential,
                StarEvaluate = createStoreRequest.StarEvaluate,
                Address = createStoreRequest.Address,
                AddressNoDiacritic = createStoreRequest.AddressNoDiacritic,
                WardID = createStoreRequest.WardID,
                CityID = createStoreRequest.CityID,
                DistrictID  = createStoreRequest.DistrictID,
                ContentID = createStoreRequest.ContentID,
                AdminName = createStoreRequest.AdminName,
                AdminTime = createStoreRequest.AdminTime,
                Status = createStoreRequest.Status,
            };

            // Thêm Store vào database qua repository và trả về StoreSystem đã thêm
            var store = await _adminRepo.AddStore(createStoreReq);

            // Sử dụng AutoMapper để chuyển đổi Store sang StoreSystem
            return _mapper.Map<StoreSystem>(store);
        }

        public async Task AddUser(CreateCustomerRequest createCustomerRequest)
        {
            var createCustomerReq = new CreateCustomerReq
            {
                FirstName = createCustomerRequest.FirstName,
                LastName = createCustomerRequest.LastName,
                Birthday = createCustomerRequest.Birthday,
                Gender = createCustomerRequest.Gender,
                Location = createCustomerRequest.Location,
                Image = createCustomerRequest.Image,
                Email = createCustomerRequest.Email,
                Password = createCustomerRequest.Password
            };

            // Gọi repository để thêm người dùng vào database
            await _adminRepo.AddUser(createCustomerReq);
        }

        //public async Task AssignRoleToUser(int storeId, string roleName)
        //{
        //    await _adminRepo.AssignRoleToUser(storeId, roleName);
        //}

        public async Task<bool> DeleteStore(int storeId)
        {
            return await _adminRepo.DeleteStore(storeId);
        }

        public async Task DeleteUser(string customerId)
        {
            await _adminRepo.DeleteUser(customerId);
        }

        public async Task<List<CartSystem>> GetAllOrders()
        {
            var orders = await _adminRepo.GetAllOrders();

            // Chuyển đổi danh sách giỏ hàng thành danh sách CartSystem DTO
            var cartSystemList = orders.Select(order => new CartSystem
            {
                Id = order.Id,
                StoreId = order.StoreId,
                CustomerId = order.CustomerId,
                Delivery = order.Delivery,
                PhoneNumber = order.PhoneNumber,
                ShipperId = order.ShipperId,
                TimeOrder = order.TimeOrder,
                MomoStatus = order.MomoStatus,
                Status = order.Status,
                DetailCarts = order.DetailCarts.Select(detail => new SelectCart
                {
                    Id = detail.Id,
                    FoodId = detail.FoodId,
                    Quantity = detail.Quantity,
                    Price = detail.Price,
                    CartID = detail.CartID,
                    FoodName = detail.Food.Name // Giả sử Food là một đối tượng có thuộc tính Name
                }).ToList()
            }).ToList();

            return cartSystemList;

        }

        public async Task<List<StoreSystem>> GetAllStores()
        {
            var stores = await _adminRepo.GetAllStores();

            // Ánh xạ từ Store sang StoreSystem (DTO)
            var storeSystems = stores.Select(s => new StoreSystem
            {
                Id = s.Id,
                Name = s.Name,
                Image = s.Image,
                TimeOpen = s.TimeOpen,
                TimeClose = s.TimeClose,
                Preferential = s.Preferential,
                StarEvaluate = s.StarEvaluate,
                Address = s.Address,
                WardID = s.WardID,
                AddressLocation = s.Ward?.Name,  // Ánh xạ tên Ward nếu có
                ContentID = s.ContentProduct?.Id ?? 0,  // Ánh xạ ContentID từ ContentProduct nếu có
                AdminName = s.AdminName,
                AdminTime = s.AdminTime,
                Status = s.Status
            }).ToList();

            return storeSystems;
        }

        public async Task<List<CustomerDtos>> GetAllUsers()
        {
            var customers = await _adminRepo.GetAllUsers();

            // Ánh xạ từ Customer sang CustomerDtos
            var customerDtos = customers.Select(c => new CustomerDtos
            {
                CustomerId = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Birthday = c.Birthday,
                Gender = c.Gender,
                Location = c.Location,
                PhoneNumber = c.PhoneNumber,
                Image = c.Image
            }).ToList();

            return customerDtos;
        }

        public async Task<List<CartSystem>> GetOrdersByStoreId(int storeId)
        {
            // Gọi phương thức từ repository để lấy danh sách các đơn hàng theo StoreId
            var orders = await _adminRepo.GetOrdersByStoreId(storeId);

            // Ánh xạ từ danh sách các đơn hàng (DAL) sang danh sách DTO (CartSystem)
            var cartSystems = orders.Select(order => new CartSystem
            {
                Id = order.Id,
                StoreId = order.StoreId,
                CustomerId = order.CustomerId,
                Delivery = order.Delivery,
                ShipperId = order.ShipperId,
                PhoneNumber = order.PhoneNumber,
                TimeOrder = order.TimeOrder,
                MomoStatus = order.MomoStatus,
                Status = order.Status,
                // Ánh xạ danh sách các DetailCarts vào CartSystem
                DetailCarts = order.DetailCarts.Select(detail => new SelectCart
                {
                    Id = detail.Id,
                    FoodId = detail.FoodId,
                    Quantity = detail.Quantity,
                    Price = detail.Price,
                    CartID = detail.CartID,
                    FoodName = detail.Food.Name // Lấy tên món ăn từ thông tin Food
                }).ToList()
            }).ToList();

            // Trả về danh sách DTO
            return cartSystems;
        }
        public async Task<double> GetTotalRevenueForAllStoresToday()
        {
            return await _adminRepo.GetTotalRevenueForAllStoresToday();
        }
        public async Task<double> GetTotalRevenueForAllStores(DateTime startDate, DateTime endDate)
        {
            return await _adminRepo.GetTotalRevenueForAllStores(startDate, endDate);
        }
        public async Task<double> GetRevenueByStoreId(int storeId)
        {
            return await _adminRepo.GetRevenueByStoreId(storeId);
        }
        public async Task<double>  GetRevenueByStoreIdForToday(int storeId)
        {
            // Tính doanh thu theo ngày
            return await _adminRepo.GetRevenueByStoreIdForToday(storeId);
        }
        public async Task<double> GetRevenueByStoreIdForThisMonth(int storeId)
        {
            // Tính doanh thu theo ngày
            return await _adminRepo.GetRevenueByStoreIdForToday(storeId);
        }


        public async Task<List<StoreSystem>> GetStoresByStatus(int status)
        {
            var stores = await _adminRepo.GetStoresByStatus(status);

            // Ánh xạ từ Store sang StoreSystem (DTOs)
            var storeSystems = stores.Select(s => new StoreSystem
            {
                Id = s.Id,
                Name = s.Name,
                Image = s.Image,
                TimeOpen = s.TimeOpen,
                TimeClose = s.TimeClose,
                Preferential = s.Preferential,
                StarEvaluate = s.StarEvaluate,
                Address = s.Address,
                WardID = s.WardID,
                AddressLocation = s.Ward?.Name, // Ánh xạ thông tin địa chỉ từ Ward nếu có
                ContentID = s.ContentProduct?.Id ?? 0, // Gán giá trị mặc định nếu ContentProduct hoặc Id là null
                AdminName = s.AdminName,
                AdminTime = s.AdminTime,
                Status = s.Status ?? 0 // Gán giá trị mặc định nếu Status là null
            }).ToList();

            return storeSystems;
        }

        public async Task<CustomerDtos> GetUserById(string customerId)
        {
            var customer = await _adminRepo.GetUserById(customerId);

            // Kiểm tra xem người dùng có tồn tại không
            if (customer == null)
            {
                return null; // Hoặc có thể ném ra ngoại lệ nếu cần
            }

            // Ánh xạ từ Customer sang CustomerDtos
            var customerDtos = new CustomerDtos
            {
                CustomerId = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Birthday = customer.Birthday,
                Gender = customer.Gender,
                PhoneNumber = customer.PhoneNumber,
                Location = customer.Location,
                Image = customer.Image
            };

            return customerDtos;
        }

        //public async Task<List<string>> GetUserRoles(int storeId)
        //{
        //    return await _adminRepo.GetUserRoles(storeId);
        //}

        //public async Task LockOrUnlockStore(int storeId, bool isLocked)
        //{
        //    await _adminRepo.LockOrUnlockStore(storeId, isLocked);
        //}
        public async Task LockStore(int storeId)
        {
            try
            {
                await _adminRepo.LockStore(storeId);
                _logger1.LogInformation($"Store {storeId} has been locked successfully.");
            }
            catch (Exception ex)
            {
                _logger1.LogError($"Error locking store {storeId}: {ex.Message}");
                throw new Exception($"Error locking store: {ex.Message}", ex);
            }
        }

        // Phương thức mở khoá cửa hàng
        public async Task UnlockStore(int storeId)
        {
            try
            {
                await _adminRepo.UnlockStore(storeId);
                _logger1.LogInformation($"Store {storeId} has been unlocked successfully.");
            }
            catch (Exception ex)
            {
                _logger1.LogError($"Error unlocking store {storeId}: {ex.Message}");
                throw new Exception($"Error unlocking store: {ex.Message}", ex);
            }
        }

        // Phương thức dừng hoạt động cửa hàng
        public async Task DeactivateStore(int storeId)
        {
            try
            {
                await _adminRepo.DeactivateStore(storeId);
                _logger1.LogInformation($"Store {storeId} has been deactivated successfully.");
            }
            catch (Exception ex)
            {
                _logger1.LogError($"Error deactivating store {storeId}: {ex.Message}");
                throw new Exception($"Error deactivating store: {ex.Message}", ex);
            }
        }

        // Phương thức kích hoạt lại cửa hàng
        public async Task ActivateStore(int storeId)
        {
            try
            {
                await _adminRepo.ActivateStore(storeId);
                _logger1.LogInformation($"Store {storeId} has been activated successfully.");
            }
            catch (Exception ex)
            {
                _logger1.LogError($"Error activating store {storeId}: {ex.Message}");
                throw new Exception($"Error activating store: {ex.Message}", ex);
            }
        }

        public async Task<double> GetRevenueByStoreIdForDate(int storeId, DateTime date)
        {
            return await _adminRepo.GetRevenueByStoreIdForDate(storeId,date);
        }

        public async Task<double> GetRevenueByStoreIdForMonth(int storeId, int year, int month)
        {
            return await _adminRepo.GetRevenueByStoreIdForMonth(storeId, year,month);
        }

        public async Task<double> GetRevenueByStoreIdForYear(int storeId, int year)
        {
            return await _adminRepo.GetRevenueByStoreIdForYear(storeId, year);
        }

        public async Task AssignRoleToUser(int storeId, string roleName, string loginName)
        {
            try
            {
                // Gọi DAL để gán vai trò cho người dùng
                await _adminRepo.AssignRoleToUser(storeId, roleName, loginName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error assigning role to user: {ex.Message}");
                throw; // Đẩy lỗi lên trên nếu có lỗi
            }
        }

        public async Task RemoveRoleFromUser(int storeId, string roleName, string loginName)
        {
            try
            {
                // Gọi DAL để xóa vai trò của người dùng
                await _adminRepo.RemoveRoleFromUser(storeId, roleName, loginName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing role from user: {ex.Message}");
                throw; // Đẩy lỗi lên trên nếu có lỗi
            }
        }

        public async Task<List<string>> GetUserRoles(int storeId, string username)
        {
            try
            {
                // Gọi DAL để lấy danh sách vai trò của người dùng
                return await _adminRepo.GetUserRoles(storeId, username);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user roles: {ex.Message}");
                throw; // Đẩy lỗi lên trên nếu có lỗi
            }
        }
        public async Task<List<string>> GetCustomerRoles(string userId)
        {
            try
            {
                return await _adminRepo.GetCustomerRoles(userId);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error retrieving user roles: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> IsUserGlobalAdmin(int storeId, string loginName)
        {
            try
            {
                // Gọi DAL để kiểm tra người dùng có quyền GlobalAdmin hay không
                var isGlobalAdmin = await _adminRepo.IsUserGlobalAdmin(storeId, loginName);

                // Trả về kết quả
                return isGlobalAdmin;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có lỗi xảy ra
                _logger.LogError($"Error checking if user {loginName} is a global admin in store {storeId}: {ex.Message}");
                throw new Exception("An error occurred while checking user role.");
            }
        }

        public Task<List<CartSystem>> GetOrdersByOrderId(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CartSystem>> GetOrdersToday()
        {
            var orders = await _adminRepo.GetOrdersToday();

            // Chuyển đổi danh sách giỏ hàng thành danh sách CartSystem DTO
            var cartSystemList = orders.Select(order => new CartSystem
            {
                Id = order.Id,
                StoreId = order.StoreId,
                CustomerId = order.CustomerId,
                Delivery = order.Delivery,
                PhoneNumber = order.PhoneNumber,
                TimeOrder = order.TimeOrder,
                ShipperId  = order.ShipperId,
                MomoStatus = order.MomoStatus,
                Status = order.Status,
                DetailCarts = order.DetailCarts.Select(detail => new SelectCart
                {
                    Id = detail.Id,
                    FoodId = detail.FoodId,
                    Quantity = detail.Quantity,
                    Price = detail.Price,
                    CartID = detail.CartID,
                    FoodName = detail.Food.Name // Giả sử Food là một đối tượng có thuộc tính Name
                }).ToList()
            }).ToList();

            return cartSystemList;

        }

        public async Task<List<CommentDtos>> GetAllCommentStores()
        {
            var comments = await _adminRepo.GetAllCommentStores();
            return _mapper.Map<List<CommentDtos>>(comments);
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            try
            {
                // Tìm bình luận cần xóa
                
                // Xóa bình luận
                 _adminRepo.DeleteComment(commentId);
                return true;

                
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                _logger.LogError($"Error while deleting comment: {ex.Message}");
                return false;
            }
        }

        public Task<CommentDtos> GetCommentById(int commentId)
        {
            throw new NotImplementedException();
        }

        //public async Task RemoveRoleFromUser(int storeId, string roleName)
        //{
        //    await _adminRepo.RemoveRoleFromUser(storeId, roleName);
        //}
    }
}
