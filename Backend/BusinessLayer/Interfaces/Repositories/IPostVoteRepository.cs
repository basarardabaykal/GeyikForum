using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Repositories;

public interface IPostVoteRepository : IGenericRepository<PostVote>
{
  public Task<IDataResult<PostVote>> CreatePostVote(PostVote postVote);
}