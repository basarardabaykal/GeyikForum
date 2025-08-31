using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Services.ControllerServices;

public class PostVoteControllerService : GenericControllerService<PostVoteDto>,  IPostVoteControllerService
{
  private readonly IPostVoteDbService _dbService;

  public PostVoteControllerService(IPostVoteDbService dbService) : base(dbService)
  {
    _dbService = dbService;
  }

  public async Task<IDataResult<PostVoteDto>> CreatePostVote(PostVoteDto postVote)
  {
    return await  _dbService.CreatePostVote(postVote);
  }
}