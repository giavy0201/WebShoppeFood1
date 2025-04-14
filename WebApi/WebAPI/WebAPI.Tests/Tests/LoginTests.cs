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
    public class LoginTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _userController;

        public LoginTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _userController = new UsersController(_mockUserService.Object, _mockLogger.Object);
        }

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
    }
} 