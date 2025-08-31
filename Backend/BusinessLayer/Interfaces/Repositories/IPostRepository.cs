using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Repositories;

public interface IPostRepository : IGenericRepository<Post>
{
  public Task<IDataResult<Post>> CreatePost(Post post);
  public Task<IDataResult<Post>> VotePost(Guid postId, int voteValue);
}