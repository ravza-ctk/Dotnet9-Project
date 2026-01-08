using System.Threading.Tasks;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Models;

namespace WebApplication1.Core.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse<string>> LoginAsync(UserLoginDto dto);
    Task<ServiceResponse<UserDto>> RegisterAsync(UserRegisterDto dto);
}
