using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;

namespace BusinessLayer.Services.ControllerServices;

public class UserControllerService : GenericControllerService<AppUserDto>, IUserControllerService
{
  public UserControllerService(IGenericDbService<AppUserDto> dbService) : base(dbService) {} 
}