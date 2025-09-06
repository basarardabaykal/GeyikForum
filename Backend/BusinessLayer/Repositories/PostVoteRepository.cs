using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BusinessLayer.Repositories;

public class PostVoteRepository : GenericRepository<PostVote>,  IPostVoteRepository
{
  public PostVoteRepository(AppDbContext dbContext) : base(dbContext) {}

  public async Task<IDataResult<PostVote>> CreatePostVote(PostVote postVote)
  {
    var existingPostVote = await _dbSet.FirstOrDefaultAsync(e => e.PostId == postVote.PostId && e.UserId == postVote.UserId);
    
    EntityEntry<PostVote>? result = null;
    
    if (postVote.VoteValue == 1)
    {
      if (existingPostVote == null)
      {
        result = await _dbSet.AddAsync(postVote);
      }
      else if (existingPostVote.VoteValue == 1)
      {
        _dbSet.Remove(existingPostVote);
      }
      else
      {
        _dbSet.Remove(existingPostVote);
        postVote.PreviousVoteValue = existingPostVote.VoteValue;
        result = await _dbSet.AddAsync(postVote);
      }
    }
    else if (postVote.VoteValue == -1)
    {
      if (existingPostVote == null)
      {
        result = await _dbSet.AddAsync(postVote);
      }
      else if (existingPostVote.VoteValue == -1)
      {
        _dbSet.Remove(existingPostVote);
      }
      else
      {
        _dbSet.Remove(existingPostVote);
        postVote.PreviousVoteValue = existingPostVote.VoteValue;
        result = await _dbSet.AddAsync(postVote);
      }
    }
    else
    {
      return new ErrorDataResult<PostVote>(400, "Invalid vote value");
    }
    
    await _dbContext.SaveChangesAsync();
    return new SuccessDataResult<PostVote>("Post Vote created successfully.", result?.Entity);
  }
}