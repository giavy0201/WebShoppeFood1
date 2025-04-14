using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Tests.Models;
using WebAPI.Tests.Services;

namespace WebAPI.Tests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger = null)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpPost("users/{userId}/items")]
        public async Task<IActionResult> AddToCart(int userId, [FromBody] CartItemDtos item)
        {
            try
            {
                if (item == null)
                {
                    return BadRequest(new ApiResponse<CartItemDtos>(false, "Dữ liệu không hợp lệ", null));
                }

                var result = await _cartService.AddToCart(userId, item.ProductId, item.Quantity);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi thêm vào giỏ hàng");
                return BadRequest(new ApiResponse<CartItemDtos>(false, "Có lỗi xảy ra khi thêm vào giỏ hàng", null));
            }
        }

        [HttpGet("users/{userId}/items")]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new ApiResponse<IEnumerable<CartItemDtos>>(false, "ID người dùng không hợp lệ", null));
                }

                var result = await _cartService.GetCartItems(userId);
                if (!result.IsSuccess)
                {
                    // Return NotFound for invalid users (when user doesn't exist)
                    if (result.Message == "Không tìm thấy người dùng")
                    {
                        return NotFound(result);
                    }
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi lấy danh sách giỏ hàng");
                return BadRequest(new ApiResponse<IEnumerable<CartItemDtos>>(false, "Có lỗi xảy ra khi lấy danh sách giỏ hàng", null));
            }
        }

        [HttpDelete("users/{userId}/items/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            var result = await _cartService.RemoveFromCart(userId, productId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("users/{userId}/items/{productId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(int userId, int productId, [FromBody] int quantity)
        {
            var result = await _cartService.UpdateCartItemQuantity(userId, productId, quantity);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
} 