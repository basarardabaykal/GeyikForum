using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using DataLayer;

namespace BusinessLayer.Repositories;

public class PostVoteRepository : GenericRepository<PostVote>,  IPostVoteRepository
{
  public PostVoteRepository(AppDbContext dbContext) : base(dbContext) {}

  public async Task<IDataResult<PostVote>> CreatePostVote(PostVote postVote)
  {
    Console.WriteLine(postVote.UserId);
    var result = await _dbSet.AddAsync(postVote);
    
    if (result.Entity != null)
    {
      await _dbContext.SaveChangesAsync();
      return new SuccessDataResult<PostVote>("Post Vote created successfully.", result.Entity);
    }
    
    return new ErrorDataResult<PostVote>(500,"Failed to create post vote.");
  }
}