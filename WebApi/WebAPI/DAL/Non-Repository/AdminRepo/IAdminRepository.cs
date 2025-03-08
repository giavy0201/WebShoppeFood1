using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.ModelsRequest.StoreRequest;
using DAL.ModelsRequest;
namespace DAL.Non_Repository.AdminRepo
{
    public interface IAdminRepository
    {
        // Quản lý Người dùng
        Task<List<Customer>> GetAllUsers();
        Task<Customer> GetUserById(string customerId);
        Task AddUser(CreateCustomerReq createCustomerReq);
        //Task UpdateUser(Customer customerId);
        Task DeleteUser(string customerId);

        // Quản lý Cửa hàng
        Task<List<Store>> GetAllStores();
        Task<Store> GetStoreById(int storeId);
        Task<Store> AddStore(CreateStoreReq createStoreRequest);
        //Task UpdateStore(UpdateStoreRequest updateStoreRequest);
        //Task<Store> UpdateStore(int storeId, UpdateStoreReq updateStoreRequest);
        Task<bool> DeleteStore(int storeId);

        // Quản lý Quyền
        Task AssignRoleToUser(int storeId, string roleName, string loginName);
        Task RemoveRoleFromUser(int storeId, string roleName, string loginName);
        Task<List<string>> GetUserRoles(int storeId, string username);
        Task<List<string>> GetCustomerRoles(string userId);
        Task<bool> IsUserGlobalAdmin(int storeId, string loginName);

        // Báo cáo và Đơn hàng
        Task<List<Cart>> GetOrdersByStoreId(int storeId);
        Task<List<Cart>> GetOrdersByOrderId(int orderId);
        Task<double> GetTotalRevenueForAllStoresToday();
        Task<double> GetTotalRevenueForAllStores(DateTime startDate, DateTime endDate);
        Task<double> GetRevenueByStoreId(int storeId);
        Task<double> GetRevenueByStoreIdForToday(int storeId);
        Task<double> GetRevenueByStoreIdForThisMonth(int storeId);
        Task<double> GetRevenueByStoreIdForDate(int storeId, DateTime date);
        Task<double> GetRevenueByStoreIdForMonth(int storeId, int year, int month);
        Task<double> GetRevenueByStoreIdForYear(int storeId, int year);

        Task<List<Cart>> GetAllOrders();
        Task<List<Cart>> GetOrdersToday();

        // Phương thức tùy chỉnh
        Task<List<Store>> GetStoresByStatus(int status);
        //Task LockOrUnlockStore(int storeId, bool isLocked);
        Task LockStore(int storeId);
        Task UnlockStore(int storeId);
        Task DeactivateStore(int storeId);
        Task ActivateStore(int storeId);
        Task<List<Comment>> GetAllCommentStores();
        Task<Comment> GetCommentById(int commentId);
        Task<bool> DeleteComment(int commentId);
    }
}
