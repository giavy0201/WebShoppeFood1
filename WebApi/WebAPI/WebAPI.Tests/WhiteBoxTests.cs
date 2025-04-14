//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using WebAPI.Controllers;
//using WebAPI.Models;
//using WebAPI.Services;
//using Xunit;

//namespace WebAPI.Tests
//{
//    public class WhiteBoxTests
//    {
//        // Test case 1: Login với username và password hợp lệ
//        [Fact]
//        public void Login_ValidCredentials_ReturnsOkResult()
//        {
//            // Arrange
//            var mockUserService = new Mock<IUserService>();
//            var loginRequest = new LoginRequest
//            {
//                Username = "test@example.com",
//                Password = "Password123!"
//            };
//            var expectedUser = new UserDtos
//            {
//                Id = 1,
//                Username = "test@example.com",
//                IsActive = true
//            };

//            mockUserService.Setup(x => x.Login(loginRequest.Username, loginRequest.Password))
//                .Returns(expectedUser);

//            var controller = new UsersController(mockUserService.Object);

//            // Act
//            var result = controller.Login(loginRequest);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<UserDtos>>(okResult.Value);
//            Assert.True(response.Success);
//            Assert.Equal(expectedUser.Id, response.Data.Id);
//            Assert.Equal(expectedUser.Username, response.Data.Username);
//        }

//        // Test case 2: Login với username không tồn tại
//        [Fact]
//        public void Login_InvalidUsername_ReturnsBadRequest()
//        {
//            // Arrange
//            var mockUserService = new Mock<IUserService>();
//            var loginRequest = new LoginRequest
//            {
//                Username = "nonexistent@example.com",
//                Password = "Password123!"
//            };

//            mockUserService.Setup(x => x.Login(loginRequest.Username, loginRequest.Password))
//                .Throws(new Exception("User not found"));

//            var controller = new UsersController(mockUserService.Object);

//            // Act
//            var result = controller.Login(loginRequest);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
//            Assert.False(response.Success);
//            Assert.Contains("User not found", response.Message);
//        }

//        // Test case 3: Login với password sai
//        [Fact]
//        public void Login_InvalidPassword_ReturnsBadRequest()
//        {
//            // Arrange
//            var mockUserService = new Mock<IUserService>();
//            var loginRequest = new LoginRequest
//            {
//                Username = "test@example.com",
//                Password = "WrongPassword"
//            };

//            mockUserService.Setup(x => x.Login(loginRequest.Username, loginRequest.Password))
//                .Throws(new Exception("Invalid password"));

//            var controller = new UsersController(mockUserService.Object);

//            // Act
//            var result = controller.Login(loginRequest);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
//            Assert.False(response.Success);
//            Assert.Contains("Invalid password", response.Message);
//        }

//        // Test case 4: Login với user không active
//        [Fact]
//        public void Login_InactiveUser_ReturnsBadRequest()
//        {
//            // Arrange
//            var mockUserService = new Mock<IUserService>();
//            var loginRequest = new LoginRequest
//            {
//                Username = "inactive@example.com",
//                Password = "Password123!"
//            };

//            mockUserService.Setup(x => x.Login(loginRequest.Username, loginRequest.Password))
//                .Throws(new Exception("User is inactive"));

//            var controller = new UsersController(mockUserService.Object);

//            // Act
//            var result = controller.Login(loginRequest);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
//            Assert.False(response.Success);
//            Assert.Contains("User is inactive", response.Message);
//        }

//        // Test case 5: Register với thông tin hợp lệ
//        [Fact]
//        public void Register_ValidData_ReturnsOkResult()
//        {
//            // Arrange
//            var mockUserService = new Mock<IUserService>();
//            var registerRequest = new RegisterRequest
//            {
//                Username = "newuser@example.com",
//                Password = "Password123!",
//                Email = "newuser@example.com",
//                Phone = "0123456789",
//                Address = "123 Test Street"
//            };
//            var expectedUser = new UserDtos
//            {
//                Id = 2,
//                Username = "newuser@example.com",
//                IsActive = true
//            };

//            mockUserService.Setup(x => x.Register(registerRequest))
//                .Returns(expectedUser);

//            var controller = new UsersController(mockUserService.Object);

//            // Act
//            var result = controller.Register(registerRequest);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<UserDtos>>(okResult.Value);
//            Assert.True(response.Success);
//            Assert.Equal(expectedUser.Id, response.Data.Id);
//            Assert.Equal(expectedUser.Username, response.Data.Username);
//        }

//        // Test case 6: Register với username đã tồn tại
//        [Fact]
//        public void Register_DuplicateUsername_ReturnsBadRequest()
//        {
//            // Arrange
//            var mockUserService = new Mock<IUserService>();
//            var registerRequest = new RegisterRequest
//            {
//                Username = "existing@example.com",
//                Password = "Password123!",
//                Email = "existing@example.com",
//                Phone = "0123456789",
//                Address = "123 Test Street"
//            };

//            mockUserService.Setup(x => x.Register(registerRequest))
//                .Throws(new Exception("Username already exists"));

//            var controller = new UsersController(mockUserService.Object);

//            // Act
//            var result = controller.Register(registerRequest);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
//            Assert.False(response.Success);
//            Assert.Contains("Username already exists", response.Message);
//        }

//        // Test case 7: Register với email không hợp lệ
//        [Fact]
//        public void Register_InvalidEmail_ReturnsBadRequest()
//        {
//            // Arrange
//            var mockUserService = new Mock<IUserService>();
//            var registerRequest = new RegisterRequest
//            {
//                Username = "newuser",
//                Password = "Password123!",
//                Email = "invalid-email",
//                Phone = "0123456789",
//                Address = "123 Test Street"
//            };

//            mockUserService.Setup(x => x.Register(registerRequest))
//                .Throws(new Exception("Invalid email format"));

//            var controller = new UsersController(mockUserService.Object);

//            // Act
//            var result = controller.Register(registerRequest);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
//            Assert.False(response.Success);
//            Assert.Contains("Invalid email format", response.Message);
//        }

//        // Test case 8: Register với password quá ngắn
//        [Fact]
//        public void Register_ShortPassword_ReturnsBadRequest()
//        {
//            // Arrange
//            var mockUserService = new Mock<IUserService>();
//            var registerRequest = new RegisterRequest
//            {
//                Username = "newuser@example.com",
//                Password = "123",
//                Email = "newuser@example.com",
//                Phone = "0123456789",
//                Address = "123 Test Street"
//            };

//            mockUserService.Setup(x => x.Register(registerRequest))
//                .Throws(new Exception("Password must be at least 6 characters"));

//            var controller = new UsersController(mockUserService.Object);

//            // Act
//            var result = controller.Register(registerRequest);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
//            Assert.False(response.Success);
//            Assert.Contains("Password must be at least 6 characters", response.Message);
//        }

//        // Test case 9: AddToCart với thông tin hợp lệ
//        [Fact]
//        public void AddToCart_ValidData_ReturnsOkResult()
//        {
//            // Arrange
//            var mockCartService = new Mock<ICartService>();
//            var cartItem = new CartItem
//            {
//                UserId = 1,
//                ProductId = 1,
//                Quantity = 2
//            };

//            mockCartService.Setup(x => x.AddToCart(cartItem))
//                .Returns(true);

//            var controller = new CartController(mockCartService.Object);

//            // Act
//            var result = controller.AddToCart(cartItem);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
//            Assert.True(response.Success);
//            Assert.True(response.Data);
//        }

//        // Test case 10: AddToCart với số lượng vượt quá tồn kho
//        [Fact]
//        public void AddToCart_ExceedStock_ReturnsBadRequest()
//        {
//            // Arrange
//            var mockCartService = new Mock<ICartService>();
//            var cartItem = new CartItem
//            {
//                UserId = 1,
//                ProductId = 1,
//                Quantity = 100
//            };

//            mockCartService.Setup(x => x.AddToCart(cartItem))
//                .Throws(new Exception("Quantity exceeds available stock"));

//            var controller = new CartController(mockCartService.Object);

//            // Act
//            var result = controller.AddToCart(cartItem);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<bool>>(badRequestResult.Value);
//            Assert.False(response.Success);
//            Assert.Contains("Quantity exceeds available stock", response.Message);
//        }

//        // Test case 11: GetCartItems với user hợp lệ
//        [Fact]
//        public void GetCartItems_ValidUser_ReturnsOkResult()
//        {
//            // Arrange
//            var mockCartService = new Mock<ICartService>();
//            var userId = 1;
//            var expectedItems = new List<CartItem>
//            {
//                new CartItem { Id = 1, UserId = 1, ProductId = 1, Quantity = 2 }
//            };

//            mockCartService.Setup(x => x.GetCartItems(userId))
//                .Returns(expectedItems);

//            var controller = new CartController(mockCartService.Object);

//            // Act
//            var result = controller.GetCartItems(userId);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<List<CartItem>>>(okResult.Value);
//            Assert.True(response.Success);
//            Assert.Single(response.Data);
//            Assert.Equal(expectedItems[0].Id, response.Data[0].Id);
//        }

//        // Test case 12: GetCartItems với user không tồn tại
//        [Fact]
//        public void GetCartItems_InvalidUser_ReturnsNotFound()
//        {
//            // Arrange
//            var mockCartService = new Mock<ICartService>();
//            var userId = 999;

//            mockCartService.Setup(x => x.GetCartItems(userId))
//                .Throws(new Exception("User not found"));

//            var controller = new CartController(mockCartService.Object);

//            // Act
//            var result = controller.GetCartItems(userId);

//            // Assert
//            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//            var response = Assert.IsType<ApiResponse<List<CartItem>>>(notFoundResult.Value);
//            Assert.False(response.Success);
//            Assert.Contains("User not found", response.Message);
//        }
//    }
//} 