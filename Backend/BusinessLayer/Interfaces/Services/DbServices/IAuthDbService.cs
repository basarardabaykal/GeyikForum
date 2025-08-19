using BusinessLayer.Dtos.Auth;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.DbServices;

public interface IAuthDbService
{
  public Task<IDataResult<AppUser>> Register(RegisterRequestDto registerRequestDto);
}