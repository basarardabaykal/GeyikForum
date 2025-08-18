using AutoMapper;
using BusinessLayer.Dtos;
using CoreLayer.Entities;

namespace BusinessLayer.Profiles;

public class PostVoteProfile : Profile
{
  public PostVoteProfile()
  {
    CreateMap<PostVote, PostVoteDto>()
      .ReverseMap();
  }
}