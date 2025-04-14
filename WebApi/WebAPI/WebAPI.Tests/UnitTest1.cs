using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using WebAPI.Tests.Models;
using WebAPI.Tests.Services;
using WebAPI.Tests.Controllers;

namespace WebAPI.Tests
{
public class UnitTest1
{
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ICartService> _mockCartService;
    private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ILogger<UsersController>> _mockUserLogger;
        private readonly Mock<ILogger<CartController>> _mockCartLogger;
        private readonly Mock<ILogger<ProductsController>> _mockProductLogger;
        private readonly UsersController _userController;
        private readonly CartController _cartController;
        private readonly ProductsController _productController;

    public UnitTest1()
    {
            _mockUserService = new Mock<IUserService>();
            _mockCartService = new Mock<ICartService>();
        _mockProductService = new Mock<IProductService>();
            _mockUserLogger = new Mock<ILogger<UsersController>>();
            _mockCartLogger = new Mock<ILogger<CartController>>();
            _mockProductLogger = new Mock<ILogger<ProductsController>>();
            _userController = new UsersController(_mockUserService.Object, _mockUserLogger.Object);
            _cartController = new CartController(_mockCartService.Object, _mockCartLogger.Object);
            _productController = new ProductsController(_mockProductService.Object, _mockProductLogger.Object);
        }

        // Login Tests - 7 cases
        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var request = new LoginRequest { Username = "testuser", Password = "password123" };
            var expectedUser = new UserDtos { Id = 1, Username = "testuser", IsActive = true };
            _mockUserService.Setup(service => service.Login(request))
                .ReturnsAsync(new ApiResponse<UserDtos>(true, "Đăng nhập thành công", expectedUser));

            // Act
            var result = await _userController.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<UserDtos>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(expectedUser.Id, response.Data.Id);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest { Username = "testuser", Password = "wrongpassword" };
            _mockUserService.Setup(service => service.Login(request))
                .ReturnsAsync(new ApiResponse<UserDtos>(false, "Sai tên đăng nhập hoặc mật khẩu", null));

            // Act
            var result = await _userController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<UserDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task Login_EmptyUsername_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest { Username = "", Password = "password123" };

            // Act
            var result = await _userController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.Contains("Tên đăng nhập không được để trống", response.Message);
        }

        [Fact]
        public async Task Login_EmptyPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest { Username = "testuser", Password = "" };

            // Act
            var result = await _userController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.Contains("Mật khẩu không được để trống", response.Message);
        }

        [Fact]
        public async Task Login_InactiveUser_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest { Username = "inactiveuser", Password = "password123" };
            _mockUserService.Setup(service => service.Login(request))
                .ReturnsAsync(new ApiResponse<UserDtos>(false, "Tài khoản đã bị khóa", null));

            // Act
            var result = await _userController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<UserDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task Login_NullRequest_ReturnsBadRequest()
        {
            // Arrange
            LoginRequest request = null;

            // Act
            var result = await _userController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task Login_ServiceException_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest { Username = "testuser", Password = "password123" };
            _mockUserService.Setup(service => service.Login(request))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _userController.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<UserDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        // Register Tests - 7 cases
        [Fact]
        public async Task Register_ValidData_ReturnsOkResult()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "newuser",
                Password = "password123",
                Email = "test@example.com",
                Phone = "0123456789",
                Address = "Test Address"
            };
            var expectedUser = new UserDtos { Id = 1, Username = "newuser", IsActive = true };
            _mockUserService.Setup(service => service.Register(request))
                .ReturnsAsync(new ApiResponse<UserDtos>(true, "Đăng ký thành công", expectedUser));

            // Act
            var result = await _userController.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<UserDtos>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(expectedUser.Id, response.Data.Id);
        }

        [Fact]
        public async Task Register_ShortUsername_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "user", // Too short
                Password = "password123",
                Email = "test@example.com",
                Phone = "0123456789",
                Address = "Test Address"
            };

            // Act
            var result = await _userController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.Contains("Tên đăng nhập phải có ít nhất 6 ký tự", response.Message);
        }

        [Fact]
        public async Task Register_ShortPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "newuser",
                Password = "pass", // Too short
                Email = "test@example.com",
                Phone = "0123456789",
                Address = "Test Address"
            };

            // Act
            var result = await _userController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.Contains("Mật khẩu phải có ít nhất 6 ký tự", response.Message);
        }

        [Fact]
        public async Task Register_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "newuser",
                Password = "password123",
                Email = "invalid-email",
                Phone = "0123456789",
                Address = "Test Address"
            };

            // Act
            var result = await _userController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.Contains("Email không hợp lệ", response.Message);
        }

        [Fact]
        public async Task Register_InvalidPhone_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "newuser",
                Password = "password123",
                Email = "test@example.com",
                Phone = "invalid-phone",
                Address = "Test Address"
            };

            // Act
            var result = await _userController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.Contains("Số điện thoại không hợp lệ", response.Message);
        }

        [Fact]
        public async Task Register_EmptyAddress_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "newuser",
                Password = "password123",
                Email = "test@example.com",
                Phone = "0123456789",
                Address = "" // Empty
            };

            // Act
            var result = await _userController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.Contains("Địa chỉ không được để trống", response.Message);
        }

        [Fact]
        public async Task Register_DuplicateUsername_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "existinguser",
                Password = "password123",
                Email = "test@example.com",
                Phone = "0123456789",
                Address = "Test Address"
            };
            _mockUserService.Setup(service => service.Register(request))
                .ReturnsAsync(new ApiResponse<UserDtos>(false, "Tên đăng nhập đã tồn tại", null));

            // Act
            var result = await _userController.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<UserDtos>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        // AddToCart Tests - 7 cases
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

        // GetCartItems Tests - 7 cases
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

        // UpdateUserStatus Tests - 7 cases
        [Fact]
        public async Task UpdateUserStatus_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var isActive = true;
            _mockUserService.Setup(service => service.UpdateUserStatus(userId, isActive))
                .ReturnsAsync(new ApiResponse<bool>(true, "Cập nhật trạng thái thành công", true));

            // Act
            var result = await _userController.UpdateUserStatus(userId, isActive);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUserStatus_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var userId = 999;
            var isActive = true;
            _mockUserService.Setup(service => service.UpdateUserStatus(userId, isActive))
                .ReturnsAsync(new ApiResponse<bool>(false, "Không tìm thấy người dùng", false));

            // Act
            var result = await _userController.UpdateUserStatus(userId, isActive);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUserStatus_NegativeUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = -1;
            var isActive = true;
            _mockUserService.Setup(service => service.UpdateUserStatus(userId, isActive))
                .ReturnsAsync(new ApiResponse<bool>(false, "ID người dùng không hợp lệ", false));

            // Act
            var result = await _userController.UpdateUserStatus(userId, isActive);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUserStatus_ZeroUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = 0;
            var isActive = true;
            _mockUserService.Setup(service => service.UpdateUserStatus(userId, isActive))
                .ReturnsAsync(new ApiResponse<bool>(false, "ID người dùng không hợp lệ", false));

            // Act
            var result = await _userController.UpdateUserStatus(userId, isActive);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateUserStatus_AlreadyActive_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var isActive = true;
            _mockUserService.Setup(service => service.UpdateUserStatus(userId, isActive))
                .ReturnsAsync(new ApiResponse<bool>(true, "Tài khoản đã được kích hoạt", true));

        // Act
            var result = await _userController.UpdateUserStatus(userId, isActive);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.IsSuccess);
    }

    [Fact]
        public async Task UpdateUserStatus_AlreadyInactive_ReturnsOkResult()
    {
        // Arrange
            var userId = 1;
            var isActive = false;
            _mockUserService.Setup(service => service.UpdateUserStatus(userId, isActive))
                .ReturnsAsync(new ApiResponse<bool>(true, "Tài khoản đã bị khóa", true));

        // Act
            var result = await _userController.UpdateUserStatus(userId, isActive);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.IsSuccess);
    }

    [Fact]
        public async Task UpdateUserStatus_ServiceException_ReturnsBadRequest()
    {
        // Arrange
            var userId = 1;
            var isActive = true;
            _mockUserService.Setup(service => service.UpdateUserStatus(userId, isActive))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _userController.UpdateUserStatus(userId, isActive);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
        }

        // ListContent Tests - 7 cases
        [Fact]
        public async Task ListContent_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var expectedContents = new List<ContentProductDtos>
            {
                new ContentProductDtos { Id = 1, Name = "Test Content", CategoryID = 1 }
            };
            _mockProductService.Setup(service => service.GetListContent())
                .ReturnsAsync(expectedContents);

        // Act
            var result = await _productController.ListContent();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<ContentProductDtos>>>(okResult.Value);
            Assert.Equal(1, response.Data.Count());
        Assert.Equal("Truy Xuất Thành Công", response.Message);
    }

    [Fact]
        public async Task GetContentById_WithValidId_ReturnsOkResult()
    {
        // Arrange
            var id = 1;
            var expectedContent = new ContentProductDtos { Id = id, Name = "Test Content", CategoryID = 1 };
            _mockProductService.Setup(service => service.GetContentById(id))
                .ReturnsAsync(expectedContent);

        // Act
            var result = await _productController.GetContentById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<ContentProductDtos>>(okResult.Value);
            Assert.Equal(expectedContent.Id, response.Data.Id);
        Assert.Equal("Truy Xuất Thành Công", response.Message);
    }

    [Fact]
        public async Task GetContentById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _mockProductService.Setup(service => service.GetContentById(id))
                .ReturnsAsync((ContentProductDtos)null);

            // Act
            var result = await _productController.GetContentById(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.Equal("Không Tìm Thấy Nội Dung", response.Message);
        }

        [Fact]
        public async Task ListContentByCate_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var categoryId = 1;
            var expectedContents = new List<ContentProductDtos>
        {
                new ContentProductDtos { Id = 1, Name = "Test Content", CategoryID = categoryId }
        };
            _mockProductService.Setup(service => service.GetListContentByCate(categoryId))
                .ReturnsAsync(expectedContents);

        // Act
            var result = await _productController.ListContentByCate(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<IEnumerable<ContentProductDtos>>>(okResult.Value);
            Assert.Equal(1, response.Data.Count());
        Assert.Equal("Truy Xuất Thành Công", response.Message);
    }

    [Fact]
        public async Task ListContentByCate_WithEmptyData_ReturnsNotFound()
    {
        // Arrange
            var categoryId = 1;
            var emptyList = new List<ContentProductDtos>();
            _mockProductService.Setup(x => x.GetListContentByCate(categoryId)).ReturnsAsync(emptyList);

            // Act
            var result = await _productController.ListContentByCate(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.False(apiResponse.IsSuccess);
            Assert.Equal("Không Tìm Thấy Sản Phẩm Trong Danh Mục Này", apiResponse.Message);
    }

    [Fact]
        public async Task ListCategory_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var expectedCategories = new List<CateDtos>
            {
                new CateDtos { Id = 1, Name = "Test Category" }
            };
            _mockProductService.Setup(service => service.GetListCategory())
                .ReturnsAsync(expectedCategories);

            // Act
            var result = await _productController.ListCategory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CateDtos>>>(okResult.Value);
            Assert.Equal(1, response.Data.Count());
            Assert.Equal("Truy Xuất Thành Công", response.Message);
        }

        [Fact]
        public async Task GetCateById_WithValidId_ReturnsOkResult()
    {
        // Arrange
            var id = 1;
            var expectedCategory = new CateDtos { Id = id, Name = "Test Category" };
            _mockProductService.Setup(service => service.GetCateById(id))
                .ReturnsAsync(expectedCategory);

        // Act
            var result = await _productController.GetCateById(id);

        // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CateDtos>>(okResult.Value);
            Assert.Equal(expectedCategory.Id, response.Data.Id);
            Assert.Equal("Truy Xuất Thành Công", response.Message);
    }

    [Fact]
        public async Task GetCateById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
            var id = 999;
            _mockProductService.Setup(service => service.GetCateById(id))
                .ReturnsAsync((CateDtos)null);

            // Act
            var result = await _productController.GetCateById(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.Equal("Không Tìm Thấy Danh Mục", response.Message);
        }
    }
}