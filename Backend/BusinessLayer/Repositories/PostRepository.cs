
using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using DataLayer;

namespace BusinessLayer.Repositories;

public class PostRepository : GenericRepository<Post>,  IPostRepository
{
  public PostRepository(AppDbContext dbContext) : base(dbContext) { }

  public async Task<IDataResult<Post>> CreatePost(Post post)
  {
    var result = await _dbSet.AddAsync(post);
    if (result.Entity != null)
    {
      await _dbContext.SaveChangesAsync();
      return new SuccessDataResult<Post>("Post created successfully.", result.Entity);
    }
    
    return new ErrorDataResult<Post>(500,"Failed to create post.");
  }

  public async Task<IDataResult<Post>> VotePost(Guid postId, int voteValue)
  {
    var post = await _dbSet.FindAsync(postId);
    if (post == null)
    {
      return new ErrorDataResult<Post>(404, "Post not found.");
    }
    post.VoteScore += voteValue;
    await _dbContext.SaveChangesAsync();
    return new SuccessDataResult<Post>("Post voted successfully.", post);
  }
  
}