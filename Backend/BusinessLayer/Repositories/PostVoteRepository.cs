using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using DataLayer;

namespace BusinessLayer.Repositories;

public class PostVoteRepository : GenericRepository<PostVote>,  IPostVoteRepository
{
  public PostVoteRepository(AppDbContext dbContext) : base(dbContext) {}
}