using AutoMapper;
using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Entities;

namespace BusinessLayer.Services.DbServices;

public class UserDbService : GenericDbService<AppUserDto, AppUser>, IUserDbService
{
  public UserDbService(IGenericRepository<AppUser> repository, IMapper mapper) : base(repository, mapper) {}
}