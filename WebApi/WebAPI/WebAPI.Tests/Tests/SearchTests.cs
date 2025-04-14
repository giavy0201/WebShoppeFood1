using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using WebAPI.Tests.Controllers;
using WebAPI.Tests.Models;
using WebAPI.Tests.Services;

namespace WebAPI.Tests.Tests
{
    public class SearchTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<  ILogger<ProductsController>> _mockLogger;
        private readonly ProductsController _controller;

        public SearchTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockProductService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SearchProducts_ValidKeyword_ReturnsOkResult()
        {
            // Arrange
            var keyword = "test";
            var products = new List<ContentProductDtos>
            {
                new ContentProductDtos { Id = 1, Name = "Test Product 1" },
                new ContentProductDtos { Id = 2, Name = "Test Product 2" }
            };
            var response = new ApiResponse<IEnumerable<ContentProductDtos>>(true, "Tìm kiếm thành công", products);
            _mockProductService.Setup(x => x.SearchProducts(keyword, null)).ReturnsAsync(response);

            // Act
            var result = await _controller.SearchProducts(keyword);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ContentProductDtos>>>(okResult.Value);
            Assert.True(apiResponse.IsSuccess);
            Assert.Equal(2, apiResponse.Data.Count());
        }

        [Fact]
        public async Task SearchProducts_ValidKeywordAndCategory_ReturnsOkResult()
        {
            // Arrange
            var keyword = "test";
            var categoryId = 1;
            var products = new List<ContentProductDtos>
            {
                new ContentProductDtos { Id = 1, Name = "Test Product 1", CategoryID = categoryId }
            };
            var response = new ApiResponse<IEnumerable<ContentProductDtos>>(true, "Tìm kiếm thành công", products);
            _mockProductService.Setup(x => x.SearchProducts(keyword, categoryId)).ReturnsAsync(response);

            // Act
            var result = await _controller.SearchProducts(keyword, categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ContentProductDtos>>>(okResult.Value);
            Assert.True(apiResponse.IsSuccess);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task SearchProducts_EmptyKeyword_ReturnsBadRequest()
        {
            // Arrange
            var keyword = "";

            // Act
            var result = await _controller.SearchProducts(keyword);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.False(apiResponse.IsSuccess);
            Assert.Equal("Từ khóa tìm kiếm không được để trống", apiResponse.Message);
        }

        [Fact]
        public async Task SearchProducts_NullKeyword_ReturnsBadRequest()
        {
            // Arrange
            string keyword = null;

            // Act
            var result = await _controller.SearchProducts(keyword);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.False(apiResponse.IsSuccess);
            Assert.Equal("Từ khóa tìm kiếm không được để trống", apiResponse.Message);
        }

        [Fact]
        public async Task SearchProducts_NoResults_ReturnsNotFound()
        {
            // Arrange
            var keyword = "nonexistent";
            var response = new ApiResponse<IEnumerable<ContentProductDtos>>(false, "Không tìm thấy sản phẩm phù hợp", null);
            _mockProductService.Setup(x => x.SearchProducts(keyword, null)).ReturnsAsync(response);

            // Act
            var result = await _controller.SearchProducts(keyword);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.False(apiResponse.IsSuccess);
            Assert.Equal("Không tìm thấy sản phẩm phù hợp", apiResponse.Message);
        }

        [Fact]
        public async Task SearchProducts_InvalidCategoryId_ReturnsNotFound()
        {
            // Arrange
            var keyword = "test";
            var categoryId = -1;
            var response = new ApiResponse<IEnumerable<ContentProductDtos>>(false, "Không tìm thấy sản phẩm phù hợp", null);
            _mockProductService.Setup(x => x.SearchProducts(keyword, categoryId)).ReturnsAsync(response);

            // Act
            var result = await _controller.SearchProducts(keyword, categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.False(apiResponse.IsSuccess);
            Assert.Equal("Không tìm thấy sản phẩm phù hợp", apiResponse.Message);
        }

        [Fact]
        public async Task SearchProducts_ServiceException_ReturnsBadRequest()
        {
            // Arrange
            var keyword = "test";
            _mockProductService.Setup(x => x.SearchProducts(keyword, null)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.SearchProducts(keyword);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.False(apiResponse.IsSuccess);
            Assert.Equal("Có lỗi xảy ra khi tìm kiếm sản phẩm", apiResponse.Message);
        }
    }
} 