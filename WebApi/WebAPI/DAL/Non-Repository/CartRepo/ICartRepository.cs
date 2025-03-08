using DAL.Entities;
using DAL.ModelsRequest;

namespace DAL.Non_Repository.CartRepo
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetCartsByStatus(int status);
        Task<List<Cart>> GetCartsByStatusDone(int status, string shipper);
        Task<List<Cart>> GetCartsByShipperConfirmedStatus(string shipper);
        Task<Customer> GetShipperInfoByCartId(int cartId);
        Task AcceptOrder(int cartId, string userId);
        Task CompleteOrder(int cartId, string userId);
        Task<Cart> AddCart(int StoreID,string CustomerID,string DeliveryAddress,int Phone,int Status);
        Task<DetailCart> AddDetailCart(int FoodID, int Quantity, double Price,int CartID);
        Task<double> GetCartTotalToday(int StoreID);
        Task<Cart> UpdateStatusOrder(int CartID, int Status);
        Task<List<Cart>> ListCartByStore(SearchCartReq Request);
        Task<Cart> SelectCartById(int CartID);

        Task<List<DetailCart>> GetDetailCartsForDate(int storeID, DateTime date);
        Task<double> GetCartTotalForMonth(int storeID, int month, int year);
        Task<List<object>> GetMonthlyRevenue(int storeID, int year);
        Task<List<int>> GetCartIdsByEmail(string email);

        //Task<double> GetCartTotalToday(int storeID, DateTime dateTime);
        //Task<List<Cart>> GetOrdersByStoreAndMonth(int StoreID, int month);
        Task<List<object>> GetRevenueForLast7Days(int storeID);
        Task<List<object>> GetRevenueForLast8Weeks(int storeID);
        Task<List<object>> GetRevenueForLast12Months(int storeID);
        //Task<List<object>> GetRevenueForEachStore(DateTime startDate, DateTime endDate);
        Task<double> GetRevenueForStoreId(int storeId, DateTime startDate, DateTime endDate);
      //  Task UpdatePaymentStatus(int cartId, string paymentStatus, string paymentMethod, DateTime? paymentTime);
        Task<Cart> GetCartForPayment(int cartId);
        Task UpdateMomoStatus(int cartId, string MomoTransactionId, string MomoStatus);
    }
}
