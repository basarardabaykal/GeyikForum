using AutoMapper;
using BusinessLayer.Dtos;
using CoreLayer.Entities;

namespace BusinessLayer.Profiles;

public class PreviousNicknameProfile : Profile
{
  public PreviousNicknameProfile()
  {
    CreateMap<PreviousNickname, PreviousNicknameDto>()
      .ReverseMap();
  }
}