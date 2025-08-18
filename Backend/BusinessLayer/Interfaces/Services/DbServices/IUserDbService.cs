using BusinessLayer.Dtos;
using CoreLayer.Entities;

namespace BusinessLayer.Interfaces.Services.DbServices;

public interface IUserDbService : IGenericDbService<AppUserDto>
{
  
}