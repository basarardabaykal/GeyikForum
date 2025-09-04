using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repositories;

public class PostVoteRepository : GenericRepository<PostVote>,  IPostVoteRepository
{
  public PostVoteRepository(AppDbContext dbContext) : base(dbContext) {}

  public async Task<IDataResult<PostVote>> CreatePostVote(PostVote postVote)
  {
    var existingPostVote = await _dbSet.FirstOrDefaultAsync(e => e.PostId == postVote.PostId && e.UserId == postVote.UserId);
    if (existingPostVote != null)
    {
      return new ErrorDataResult<PostVote>(500, "Duplicate vote.");
    }
    
    var result = await _dbSet.AddAsync(postVote);
    
    if (result.Entity != null)
    {
      await _dbContext.SaveChangesAsync();
      return new SuccessDataResult<PostVote>("Post Vote created successfully.", result.Entity);
    }
    
    return new ErrorDataResult<PostVote>(500,"Failed to create post vote.");
  }
}