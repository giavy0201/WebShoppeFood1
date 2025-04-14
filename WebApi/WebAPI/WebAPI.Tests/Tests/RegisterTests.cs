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
    public class RegisterTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _userController;

        public RegisterTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _userController = new UsersController(_mockUserService.Object, _mockLogger.Object);
        }

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
    }
} 