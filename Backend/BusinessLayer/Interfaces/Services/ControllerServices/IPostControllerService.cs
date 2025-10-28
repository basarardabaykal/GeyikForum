using BusinessLayer.Dtos;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.ControllerServices;

public interface IPostControllerService :  IGenericControllerService<PostDto>
{
  public Task<IDataResult<PostDto>> CreatePost(PostDto dto);
  public Task<IDataResult<PostDto>> VotePost(PostVoteDto postVoteDto);

  public Task<IDataResult<PostDto>> SetPinStatus(PostPinRequestDto dto);
  public Task<IDataResult<PostDto>> SetDeleteStatus(PostDeleteRequestDto dto);

  public Task<IDataResult<List<PostDto>>> GetPage(int page, int pageSize);
}