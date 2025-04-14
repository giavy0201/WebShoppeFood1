using System.Threading.Tasks;
using WebAPI.Tests.Models;

namespace WebAPI.Tests.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserDtos>> Login(LoginRequest request);
        Task<ApiResponse<UserDtos>> Register(RegisterRequest request);
        Task<ApiResponse<UserDtos>> GetUserById(int id);
        Task<ApiResponse<bool>> UpdateUserStatus(int id, bool isActive);
    }
} 