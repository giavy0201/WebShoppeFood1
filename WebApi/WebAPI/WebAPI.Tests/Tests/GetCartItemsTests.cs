using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using WebAPI.Tests.Models;
using WebAPI.Tests.Services;
using WebAPI.Tests.Controllers;

namespace WebAPI.Tests.Tests
{
    public class GetCartItemsTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly Mock<ILogger<CartController>> _mockLogger;
        private readonly CartController _cartController;

        public GetCartItemsTests()
        {
            _mockCartService = new Mock<ICartService>();
            _mockLogger = new Mock<ILogger<CartController>>();
            _cartController = new CartController(_mockCartService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCartItems_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var expectedItems = new List<CartItemDtos>
            {
                new CartItemDtos { Id = 1, ProductId = 1, Quantity = 2 }
            };
            _mockCartService.Setup(service => service.GetCartItems(userId))
                .ReturnsAsync(new ApiResponse<IEnumerable<CartItemDtos>>(true, "Lấy giỏ hàng thành công", expectedItems));

            // Act
            var result = await _cartController.GetCartItems(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CartItemDtos>>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetCartItems_InvalidUser_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;
            _mockCartService.Setup(service => service.GetCartItems(userId))
                .ReturnsAsync(new ApiResponse<IEnumerable<CartItemDtos>>(false, "Không tìm thấy giỏ hàng", null));

            // Act
            var result = await _cartController.GetCartItems(userId);

            // Assert
            var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CartItemDtos>>>(notFoundResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task GetCartItems_EmptyCart_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            _mockCartService.Setup(service => service.GetCartItems(userId))
                .ReturnsAsync(new ApiResponse<IEnumerable<CartItemDtos>>(true, "Giỏ hàng trống", new List<CartItemDtos>()));

            // Act
            var result = await _cartController.GetCartItems(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CartItemDtos>>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Empty(response.Data);
        }

        [Fact]
        public async Task GetCartItems_NegativeUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = -1;
            _mockCartService.Setup(service => service.GetCartItems(userId))
                .ReturnsAsync(new ApiResponse<IEnumerable<CartItemDtos>>(false, "ID người dùng không hợp lệ", null));

            // Act
            var result = await _cartController.GetCartItems(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CartItemDtos>>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task GetCartItems_ZeroUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = 0;
            _mockCartService.Setup(service => service.GetCartItems(userId))
                .ReturnsAsync(new ApiResponse<IEnumerable<CartItemDtos>>(false, "ID người dùng không hợp lệ", null));

            // Act
            var result = await _cartController.GetCartItems(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CartItemDtos>>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task GetCartItems_InactiveUser_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            _mockCartService.Setup(service => service.GetCartItems(userId))
                .ReturnsAsync(new ApiResponse<IEnumerable<CartItemDtos>>(false, "Tài khoản đã bị khóa", null));

            // Act
            var result = await _cartController.GetCartItems(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CartItemDtos>>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task GetCartItems_ServiceException_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            _mockCartService.Setup(service => service.GetCartItems(userId))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _cartController.GetCartItems(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CartItemDtos>>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }
    }
} 