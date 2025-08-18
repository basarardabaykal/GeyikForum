using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using DataLayer;

namespace BusinessLayer.Repositories;

public class UserRepository : GenericRepository<AppUser>,  IUserRepository
{
  public UserRepository(AppDbContext dbContext) : base(dbContext) { }
}