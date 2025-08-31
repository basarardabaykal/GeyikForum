using BusinessLayer.Dtos;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.ControllerServices;

public interface IPostVoteControllerService : IGenericControllerService<PostVoteDto>
{
  public Task<IDataResult<PostVoteDto>> CreatePostVote(PostVoteDto postVote);
}