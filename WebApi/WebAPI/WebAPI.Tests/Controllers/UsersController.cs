using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using WebAPI.Tests.Models;
using WebAPI.Tests.Services;

namespace WebAPI.Tests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly Regex _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private readonly Regex _phoneRegex = new Regex(@"^[0-9]{10}$");

        public UsersController(IUserService userService, ILogger<UsersController> logger = null)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse<string>(false, "Dữ liệu đăng nhập không hợp lệ", string.Empty));
                }

                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return BadRequest(new ApiResponse<string>(false, "Tên đăng nhập không được để trống", string.Empty));
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new ApiResponse<string>(false, "Mật khẩu không được để trống", string.Empty));
                }

                var result = await _userService.Login(request);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi đăng nhập");
                return BadRequest(new ApiResponse<UserDtos>(false, "Có lỗi xảy ra khi đăng nhập", null));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse<string>(false, "Dữ liệu đăng ký không hợp lệ", string.Empty));
                }

                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return BadRequest(new ApiResponse<string>(false, "Tên đăng nhập không được để trống", string.Empty));
                }

                if (request.Username.Length < 6)
                {
                    return BadRequest(new ApiResponse<string>(false, "Tên đăng nhập phải có ít nhất 6 ký tự", string.Empty));
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new ApiResponse<string>(false, "Mật khẩu không được để trống", string.Empty));
                }

                if (request.Password.Length < 6)
                {
                    return BadRequest(new ApiResponse<string>(false, "Mật khẩu phải có ít nhất 6 ký tự", string.Empty));
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new ApiResponse<string>(false, "Email không được để trống", string.Empty));
                }

                if (!_emailRegex.IsMatch(request.Email))
                {
                    return BadRequest(new ApiResponse<string>(false, "Email không hợp lệ", string.Empty));
                }

                if (string.IsNullOrWhiteSpace(request.Phone))
                {
                    return BadRequest(new ApiResponse<string>(false, "Số điện thoại không được để trống", string.Empty));
                }

                if (!_phoneRegex.IsMatch(request.Phone))
                {
                    return BadRequest(new ApiResponse<string>(false, "Số điện thoại không hợp lệ", string.Empty));
                }

                if (string.IsNullOrWhiteSpace(request.Address))
                {
                    return BadRequest(new ApiResponse<string>(false, "Địa chỉ không được để trống", string.Empty));
                }

                var result = await _userService.Register(request);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi đăng ký");
                return BadRequest(new ApiResponse<string>(false, "Có lỗi xảy ra khi đăng ký", string.Empty));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] bool isActive)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<bool>(false, "ID người dùng không hợp lệ", false));
                }

                var result = await _userService.UpdateUserStatus(id, isActive);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, "Lỗi khi cập nhật trạng thái người dùng");
                return BadRequest(new ApiResponse<bool>(false, "Có lỗi xảy ra khi cập nhật trạng thái người dùng", false));
            }
        }
    }
} 