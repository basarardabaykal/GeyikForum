using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Repositories;

public interface IPostRepository : IGenericRepository<Post>
{
  public Task<IDataResult<Post>> CreatePost(Post post);
  public Task<IDataResult<Post>> VotePost(Guid postId, int voteValue);

  public Task<IDataResult<Post>> SetPinStatus(Guid postId, bool isPinned);
  public Task<IDataResult<Post>> SetDeleteStatus(Guid postId, bool isDeleted);

  public Task<IDataResult<List<Post>>> GetParentsWithRepliesPaged(int page, int pageSize);
}