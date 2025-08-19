using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Repositories;

public interface IAuthRepository
{
  public Task<IDataResult<AppUser>> CreateUser(AppUser user,  string password);
  public Task<IDataResult<AppUser>> GetUserByEmail(string email);
}