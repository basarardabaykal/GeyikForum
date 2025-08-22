using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Repositories;

public interface IAuthRepository
{
  public Task<IDataResult<AppUser>> CreateUser(AppUser user,  string password);
  public Task<IDataResult<AppUser>> GetUserByEmail(string email);
  public Task<IDataResult<List<string>>> GetUserRoles(string email);
  public Task<IDataResult<bool>> AssignRole(AppUser user, string role);
}