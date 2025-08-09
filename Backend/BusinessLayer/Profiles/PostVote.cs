using AutoMapper;
using BusinessLayer.Dtos;
using CoreLayer.Entities;

namespace BusinessLayer.Profiles;

public class PostVote : Profile
{
  public PostVote()
  {
    CreateMap<PostVote, PostVoteDto>()
      .ReverseMap();
  }
}