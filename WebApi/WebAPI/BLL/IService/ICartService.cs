using BLL.Helpers;
using BLL.Models.DTOs;
using BLL.Models.DTOs.Cart;
using BLL.Models.DTOs.Customer;
using BLL.Models.Request;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using NHibernate.Criterion;

namespace BLL.IService
{
    public interface ICartService
    {
        Task<List<CartSystem>> GetCartsByStatus(int status);
        Task<List<CartSystem>> GetCartsByStatusDone(int status, string shipper);
        Task<List<CartSystem>> GetCartsByShipperConfirmedStatus(string shipper);
        Task<CustomerDtos> GetShipperInfoByCartId(int cartId);
        Task AcceptOrder(int cartId, string userId);
        Task CompleteOrder(int cartId, string userId);
        Task<Cart> InsertCart(CartRequest reqCart);
        Task<double> TotalMoneyToday(int StoreID);
        Task<ModelApiRequest> UpdateCartOrder(SetStatusCartReq request);
        Task<List<CartSystem>> ListCartByStore(SearchCartRequest model);
        Task<CartSystem> CartByID(int CartID);

        
        Task<double> TotalMoneyForMonth(int storeID, int month, int year);

        Task<List<double>> TotalMoneyForYear(int storeID, int year);
        Task<List<double>> TotalMoneyForLast7Days(int storeID);
        Task<List<double>> TotalMoneyForLast8Weeks(int storeID);
        Task<List<double>> TotalMoneyForLast12Months(int storeID);
        //Task<List<object>> GetRevenueForEachStore(DateTime startDate, DateTime endDate);
        Task<double> GetRevenueForStoreId(int storeId, DateTime startDate, DateTime endDate);
        Task<List<int>> GetCartIdByEmail(string email);
        //Task<List<CartSystem>> GetOrdersByStoreAndMonth(int storeId, int month);
        Task<string> CreateVNPayPaymentUrl(int cartId, decimal amount,string returnUrl);
        //Task HandleVNPayCallbackAsync(int cartId, string responseCode);
        
    }
}
