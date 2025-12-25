using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Interfaces;
using WebApplication1.Core.Models;

namespace WebApplication1.Service.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(IGenericRepository<User> repository, IMapper mapper, IConfiguration configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<string>> LoginAsync(UserLoginDto dto)
    {
        var users = await _repository.FindAsync(x => x.Username == dto.Username);
        var user = System.Linq.Enumerable.FirstOrDefault(users);

        if (user == null || user.PasswordHash != dto.Password) // Simple plain text check for demo (or hash matching)
        {
             return new ServiceResponse<string> { Success = false, Message = "Wrong username or password" };
        }

        string token = CreateToken(user);
        return new ServiceResponse<string> { Data = token, Message = "Login Successful" };
    }

    public async Task<ServiceResponse<UserDto>> RegisterAsync(UserRegisterDto dto)
    {
        // Check if exists
        var users = await _repository.FindAsync(x => x.Username == dto.Username);
        if (System.Linq.Enumerable.Any(users))
        {
            return new ServiceResponse<UserDto> { Success = false, Message = "Username already exists" };
        }

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = dto.Password; // In production use hashing!
        
        await _repository.AddAsync(user);
        var userDto = _mapper.Map<UserDto>(user);
        return new ServiceResponse<UserDto> { Data = userDto };
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value ?? "AvailableKeyForDemoUsesOnlyVeryLongPlease12345_MustBeAtLeast512BitsLongToWorkWithTheAlgorithmHmacSha512"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: System.DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
