using DAL.Entities;
using DAL.ModelsRequest.StoreRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models.DTOs;
using BLL.Models.Request.Store;
using BLL.Models.DTOs.Cart;
using BLL.Models.DTOs.Customer;
using BLL.Models.Request;
using BLL.Models.DTOs.Comment;
namespace BLL.IService
{
    public interface IAdminService
    {
        Task<StoreSystem> AddStore(CreateStoreRequest createStoreRequest);
        Task AddUser(CreateCustomerRequest createCustomerRequest);
        //Task AssignRoleToUser(int storeId, string roleName);
        Task<bool> DeleteStore(int storeId);
        Task DeleteUser(string customerId);
        Task<List<CartSystem>> GetAllOrders();
        Task<List<CartSystem>> GetOrdersToday();
        Task<List<StoreSystem>> GetAllStores();
        Task<List<CustomerDtos>> GetAllUsers();
        Task<List<CartSystem>> GetOrdersByStoreId(int storeId);
        Task<List<CartSystem>> GetOrdersByOrderId(int orderId);
        Task<double> GetTotalRevenueForAllStoresToday();
        Task<double> GetTotalRevenueForAllStores(DateTime startDate, DateTime endDate);
        Task<double> GetRevenueByStoreId(int storeId);
        Task<double> GetRevenueByStoreIdForToday(int storeId);
        Task<double> GetRevenueByStoreIdForThisMonth(int storeId);
        Task<double> GetRevenueByStoreIdForDate(int storeId, DateTime date);
        Task<double> GetRevenueByStoreIdForMonth(int storeId, int year, int month);
        Task<double> GetRevenueByStoreIdForYear(int storeId, int year);

        Task AssignRoleToUser(int storeId, string roleName, string loginName);
        Task RemoveRoleFromUser(int storeId, string roleName, string loginName);
        Task<List<string>> GetUserRoles(int storeId, string username);
        Task<List<string>> GetCustomerRoles(string userId);
        Task<bool> IsUserGlobalAdmin(int storeId, string loginName);
        Task<List<StoreSystem>> GetStoresByStatus(int status);
        Task<CustomerDtos> GetUserById(string customerId);
        // Task<List<string>> GetUserRoles(int storeId);
        //Task LockOrUnlockStore(int storeId, bool isLocked);
        // Task RemoveRoleFromUser(int storeId, string roleName);
        Task LockStore(int storeId);
        Task UnlockStore(int storeId);
        Task DeactivateStore(int storeId);
        Task ActivateStore(int storeId);
        Task<List<CommentDtos>> GetAllCommentStores();
        Task<CommentDtos> GetCommentById(int commentId);
        Task<bool> DeleteComment(int commentId);
    }
}
