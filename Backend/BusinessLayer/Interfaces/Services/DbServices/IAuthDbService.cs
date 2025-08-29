using BusinessLayer.Dtos;
using BusinessLayer.Dtos.Auth;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.DbServices;

public interface IAuthDbService
{
  public Task<IDataResult<AppUser>> Register(RegisterRequestDto registerRequestDto);
  public Task<IDataResult<AppUser>> Login(LoginRequestDto loginRequestDto);
  public Task<IDataResult<AppUserDto>> GetCurrentUser(string uid);
  public Task<IDataResult<List<string>>> GetUserRoles(string email);
}