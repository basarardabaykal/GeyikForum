using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;

namespace BusinessLayer.Services.ControllerServices;

public class PostVoteControllerService : GenericControllerService<PostVoteDto>,  IPostVoteControllerService
{
  public PostVoteControllerService(IGenericDbService<PostVoteDto> dbService) : base(dbService) {}
}