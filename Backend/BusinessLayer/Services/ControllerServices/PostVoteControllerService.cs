using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Services.ControllerServices;

public class PostVoteControllerService : GenericControllerService<PostVoteDto>,  IPostVoteControllerService
{
  private readonly IPostVoteDbService _postVoteDbService;

  public PostVoteControllerService(IPostVoteDbService postVoteDbService) : base(postVoteDbService)
  {
    _postVoteDbService = postVoteDbService;
  }

  public async Task<IDataResult<PostVoteDto>> CreatePostVote(PostVoteDto postVote)
  {
    return await   _postVoteDbService.CreatePostVote(postVote);
  }
}