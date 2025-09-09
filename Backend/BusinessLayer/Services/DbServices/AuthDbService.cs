using AutoMapper;
using BusinessLayer.Dtos;
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
  private readonly IMapper _mapper;

  public AuthDbService(IAuthRepository authRepository, IConfiguration configuration,  IMapper mapper)
  {
    _authRepository = authRepository;
    _defaultRole = configuration.GetValue<string>("DefaultRole") ?? "User";
    _mapper = mapper;
  }
  
  public async Task<IDataResult<AppUser>> Register(RegisterRequestDto registerRequestDto)
  {
    var existingUserResult = await _authRepository.GetUserByEmail(registerRequestDto.Email);
    if (existingUserResult.Success)
    {
      return new ErrorDataResult<AppUser>(400, "Bu e-postaya sahip kullanıcı zaten var.");
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
  
  public async Task<IDataResult<AppUser>> Login(LoginRequestDto loginRequestDto)
  {
    var result = await _authRepository.GetUserByEmail(loginRequestDto.Email);
    var user = result.Data;
    
    if (!result.Success)
    {
      return result;
    }

    else {
      return await _authRepository.CheckPassword(user, loginRequestDto.Password);
    }
  }

  public async Task<IDataResult<AppUserDto>> GetCurrentUser(string uid)
  {
    var result = await _authRepository.GetUserByUid(uid);
    if (!result.Success)
    {
      return new ErrorDataResult<AppUserDto>(result.StatusCode, result.Message);
    }
    
    var userEntity = result.Data;
    AppUserDto userDto = _mapper.Map<AppUserDto>(userEntity);
    return new SuccessDataResult<AppUserDto>(result.Message, userDto);
  }
  
  public async Task<IDataResult<List<string>>> GetUserRoles(string email)
  {
    return await _authRepository.GetUserRoles(email);
  }
}