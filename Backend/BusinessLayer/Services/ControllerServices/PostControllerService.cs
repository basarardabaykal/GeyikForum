using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;

namespace BusinessLayer.Services.ControllerServices;

public class PostControllerService : GenericControllerService<PostDto>,  IPostControllerService
{
    public PostControllerService(IGenericDbService<PostDto> dbService) : base(dbService) {}
}