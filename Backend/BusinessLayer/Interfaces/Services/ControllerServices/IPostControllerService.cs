using BusinessLayer.Dtos;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.ControllerServices;

public interface IPostControllerService :  IGenericControllerService<PostDto>
{
  public Task<IDataResult<PostDto>> CreatePost(PostDto dto);
  public Task<IDataResult<PostDto>> VotePost(PostVoteDto postVoteDto);
}