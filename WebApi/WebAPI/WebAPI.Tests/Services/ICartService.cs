using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Tests.Models;

namespace WebAPI.Tests.Services
{
    public interface ICartService
    {
        Task<ApiResponse<CartItemDtos>> AddToCart(int userId, int productId, int quantity);
        Task<ApiResponse<IEnumerable<CartItemDtos>>> GetCartItems(int userId);
        Task<ApiResponse<bool>> RemoveFromCart(int userId, int productId);
        Task<ApiResponse<bool>> UpdateCartItemQuantity(int userId, int productId, int quantity);
    }
} 