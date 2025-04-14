using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using WebAPI.Tests.Models;
using WebAPI.Tests.Services;
using WebAPI.Tests.Controllers;

namespace WebAPI.Tests.Tests
{
    public class AddToCartTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly Mock<ILogger<CartController>> _mockLogger;
        private readonly CartController _cartController;

        public AddToCartTests()
        {
            _mockCartService = new Mock<ICartService>();
            _mockLogger = new Mock<ILogger<CartController>>();
            _cartController = new CartController(_mockCartService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddToCart_ValidData_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var item = new CartItemDtos { ProductId = 1, Quantity = 2 };
            _mockCartService.Setup(service => service.AddToCart(userId, item.ProductId, item.Quantity))
                .ReturnsAsync(new ApiResponse<CartItemDtos>(true, "Thêm vào giỏ hàng thành công", item));

            // Act
            var result = await _cartController.AddToCart(userId, item);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CartItemDtos>>(okResult.Value);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task AddToCart_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var userId = 999;
            var item = new CartItemDtos { ProductId = 1, Quantity = 1 };
            _mockCartService.Setup(service => service.AddToCart(userId, item.ProductId, item.Quantity))
                .ReturnsAsync(new ApiResponse<CartItemDtos>(false, "Người dùng không tồn tại", null));

            // Act
            var result = await _cartController.AddToCart(userId, item);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CartItemDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task AddToCart_InvalidProduct_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var item = new CartItemDtos { ProductId = 999, Quantity = 1 };
            _mockCartService.Setup(service => service.AddToCart(userId, item.ProductId, item.Quantity))
                .ReturnsAsync(new ApiResponse<CartItemDtos>(false, "Sản phẩm không tồn tại", null));

            // Act
            var result = await _cartController.AddToCart(userId, item);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CartItemDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task AddToCart_ZeroQuantity_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var item = new CartItemDtos { ProductId = 1, Quantity = 0 };
            _mockCartService.Setup(service => service.AddToCart(userId, item.ProductId, item.Quantity))
                .ReturnsAsync(new ApiResponse<CartItemDtos>(false, "Số lượng phải lớn hơn 0", null));

            // Act
            var result = await _cartController.AddToCart(userId, item);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CartItemDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task AddToCart_NegativeQuantity_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var item = new CartItemDtos { ProductId = 1, Quantity = -1 };
            _mockCartService.Setup(service => service.AddToCart(userId, item.ProductId, item.Quantity))
                .ReturnsAsync(new ApiResponse<CartItemDtos>(false, "Số lượng không được âm", null));

            // Act
            var result = await _cartController.AddToCart(userId, item);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CartItemDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task AddToCart_ExceedStock_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var item = new CartItemDtos { ProductId = 1, Quantity = 1000 };
            _mockCartService.Setup(service => service.AddToCart(userId, item.ProductId, item.Quantity))
                .ReturnsAsync(new ApiResponse<CartItemDtos>(false, "Số lượng vượt quá tồn kho", null));

            // Act
            var result = await _cartController.AddToCart(userId, item);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CartItemDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task AddToCart_ServiceException_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var item = new CartItemDtos { ProductId = 1, Quantity = 1 };
            _mockCartService.Setup(service => service.AddToCart(userId, item.ProductId, item.Quantity))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _cartController.AddToCart(userId, item);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CartItemDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }
    }
} 