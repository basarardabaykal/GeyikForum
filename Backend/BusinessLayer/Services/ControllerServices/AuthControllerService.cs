using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLayer.Dtos;
using BusinessLayer.Dtos.Auth;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Services.ControllerServices;

public class AuthControllerService : IAuthControllerService
{
  private readonly IAuthDbService  _authDbService;
  private readonly IConfiguration _configuration;

  public AuthControllerService(IAuthDbService authDbService,  IConfiguration configuration)
  {
    _authDbService = authDbService;
    _configuration = configuration;
  }
  
  public async Task<IDataResult<RegisterResponseDto>> Register(RegisterRequestDto registerRequestDto)
  {
    var result = await _authDbService.Register(registerRequestDto);
    var user = result.Data;
    if (!result.Success)
    {
      return new ErrorDataResult<RegisterResponseDto>(result.StatusCode, result.Message);
    }
    
    var rolesResult = await _authDbService.GetUserRoles(user.Email);  
    var roles = rolesResult.Success ? rolesResult.Data : new List<string>();

    var token = GenerateJwtToken(user, roles);

    var data = new RegisterResponseDto
    {
      UserDto = new AppUserDto()
      {
        Id = user.Id,
        Email = user.Email,
        Nickname = user.Nickname,
        Karma = user.Karma,
        IsAdmin = user.IsAdmin,
        IsModerator = user.IsModerator,
        IsBanned = user.IsBanned,
        Roles = roles,
      },
      Token = token
    };

    return new SuccessDataResult<RegisterResponseDto>(result.Message, data);
  }
  private string GenerateJwtToken(AppUser user, List<string> roles)
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new(ClaimTypes.Name, user.UserName),
      new(ClaimTypes.Email, user.Email) //can be simplified.
    };

    foreach (var role in roles)
    {
      claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]));

    var token = new JwtSecurityToken(
      issuer: _configuration["Jwt:Issuer"],
      audience: _configuration["Jwt:Audience"],
      claims: claims,
      expires: expiresAt,
      signingCredentials: credentials
    );

    return (new JwtSecurityTokenHandler().WriteToken(token));
  }
}