using BusinessLayer.Dtos;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.DbServices;

public interface IPostVoteDbService : IGenericDbService<PostVoteDto>
{
  public Task<IDataResult<PostVoteDto>> CreatePostVote(PostVoteDto postVote);
}