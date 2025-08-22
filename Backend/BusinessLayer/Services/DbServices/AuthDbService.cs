using BusinessLayer.Dtos.Auth;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services.DbServices;

public class AuthDbService : IAuthDbService
{
  private readonly IAuthRepository  _authRepository;
  private readonly string _defaultRole;

  public AuthDbService(IAuthRepository authRepository, IConfiguration configuration)
  {
    _authRepository = authRepository;
    _defaultRole = configuration.GetValue<string>("DefaultRole") ?? "User";
  }
  
  public async Task<IDataResult<AppUser>> Register(RegisterRequestDto registerRequestDto)
  {
    var existingUserResult = await _authRepository.GetUserByEmail(registerRequestDto.Email);
    if (existingUserResult.Success)
    {
      return new ErrorDataResult<AppUser>(400, "User with this email already exists.");
    }
    
    var newUser = new AppUser
    {
      UserName = registerRequestDto.Email,
      Email = registerRequestDto.Email,
      Nickname = registerRequestDto.Nickname,
      Karma = 0,
      IsAdmin = false,
      IsModerator = false,
      IsBanned = false,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow,
    };
            
    var result = await _authRepository.CreateUser(newUser, registerRequestDto.Password);
    if (result.Success)
    {
      await _authRepository.AssignRole(newUser, _defaultRole);
    }
    return result;
  }
  
  public async Task<IDataResult<List<string>>> GetUserRoles(string email)
  {
    return await _authRepository.GetUserRoles(email);
  }
}