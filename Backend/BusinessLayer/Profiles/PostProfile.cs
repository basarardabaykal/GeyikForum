using AutoMapper;
using BusinessLayer.Dtos;
using CoreLayer.Entities;

namespace BusinessLayer.Profiles;

public class PostProfile : Profile
{
  public PostProfile()
  {
    CreateMap<Post, PostDto>()
      .ReverseMap();
  }
  
}