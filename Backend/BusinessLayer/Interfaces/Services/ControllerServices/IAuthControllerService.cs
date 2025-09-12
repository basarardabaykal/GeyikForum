using BusinessLayer.Dtos;
using BusinessLayer.Dtos.Auth;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.ControllerServices;

public interface IAuthControllerService
{ 
  public Task<IDataResult<RegisterResponseDto>> Register(RegisterRequestDto registerDTO);
  public Task<IDataResult<LoginResponseDto>> Login(LoginRequestDto loginDto);
  public Task<IDataResult<AppUserDto>> GetCurrentUser(string jwtToken);
  public Task<IDataResult<object>> ConfirmEmail(ConfirmEmailRequestDto confirmEmailRequestDto);
}