using AutoMapper;
using BusinessLayer.Dtos;
using CoreLayer.Entities;

namespace BusinessLayer.Profiles;

public class AppUserProfile : Profile
{
  public AppUserProfile()
  {
    CreateMap<AppUser, AppUserDto>()
      .ReverseMap();
  }
}