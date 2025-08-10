
using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using DataLayer;

namespace BusinessLayer.Repositories;

public class PostRepository : GenericRepository<Post>,  IPostRepository
{
  public PostRepository(AppDbContext dbContext) : base(dbContext) { }
}