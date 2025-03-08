using BLL.Model.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using BLL.Model.Cart;
using BLL.Request;
using BLL.Model.Customer;
using BLL.Model.Address;
using BLL.Model.Comment;
namespace BLL.IService
{
    public interface IAdminService
    {
        Task<StoreDtos> AddStore(CreateStoreRequest createStoreRequest);
        Task<List<CityDtos>> GetCities();
        Task<List<DistrictDtos>> GetDistrictsByCityId(int cityId);
        Task<List<WardDtos>> GetWardsByDistrictId(int districtId);
        Task AddUser(CreateCustomerRequest createCustomerRequest);
        //Task AssignRoleToUser(int storeId, string roleName);
        Task<bool> DeleteStore(int storeId);
        Task DeleteUser(string customerId);
        Task<List<CartDtos>> GetAllOrders();
        Task<List<CartDtos>> GetAllOrdersToday();
        Task<List<StoreDtos>> GetAllStores();
        Task<List<CustomerDtos>> GetAllUsers();
        Task<List<CartDtos>> GetOrdersByStoreId(int storeId);
        Task<double> GetTotalRevenueForAllStoresToday();
        Task<List<double>> GetTotalRevenueForLast8Weeks();
        Task<List<double>> GetTotalRevenueForLast12Months();
        Task<double> GetRevenueByStoreId(int storeId);
        Task<double> GetRevenueByStoreIdForToday(int storeId);
        Task<double> GetRevenueByStoreIdForThisMonth(int storeId);
        Task<double> GetRevenueByStoreIdForDate(int storeId, DateTime date);
        Task<double> GetRevenueByStoreIdForMonth(int storeId, int year, int month);
        Task<double> GetRevenueByStoreIdForYear(int storeId, int year);

        Task AssignRoleToUser(int storeId, string roleName, string loginName);
        Task RemoveRoleFromUser(int storeId, string roleName, string loginName);
        Task<List<string>> GetUserRoles(int storeId, string username);
        Task<bool> IsUserGlobalAdmin(int storeId, string loginName);
        Task<List<StoreDtos>> GetStoresByStatus(int status);
        Task<CustomerDtos> GetUserById(string customerId);
        // Task<List<string>> GetUserRoles(int storeId);
        //Task LockOrUnlockStore(int storeId, bool isLocked);
        // Task RemoveRoleFromUser(int storeId, string roleName);
        Task LockStore(int storeId);
        Task UnlockStore(int storeId);
        Task DeactivateStore(int storeId);
        Task ActivateStore(int storeId);
        Task<List<CommentDtos>> GetAllCommentStores();
        Task<bool> DeleteComment(int commentId);
    }
}
