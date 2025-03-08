using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Model.Cart;
using BLL.Model.Customer;
using BLL.Model.ModelRequest;

namespace BLL.IService
{
    public interface ICartService
    {
        Task<List<int>> GetOrCreateCartIdByEmail(string? email);
        Task<CartDtos> GetOrderHistory(int CartId);
        Task<List<CartDtos>> GetOrderHistoryByEmail(string email);
        Task<List<CartDtos>> GetCartOrderStatus(int statusId);
        Task<List<CartDtos>> GetCartOrderStatusDone(int statusId, string shipper);
        Task<ApiRequest> UpdateStatusOrder(SetStatusRequest setReq);
        Task<CartDtos> SelectCartByID(int CartID);
        Task<List<CartDtos>> GetCartsByShipperConfirmedStatus(string shipper);
        Task<CustomerDtos> GetShipperInfoByCartId(int cartId);
        Task <bool> AcceptOrder(int cartId, string userId);
        Task<bool> CompleteOrder(int cartId, string userId);
        Task<CustomerDtos1> GetUserById(string customerId);
    }
}
