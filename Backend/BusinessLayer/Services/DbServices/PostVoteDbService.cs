using AutoMapper;
using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Entities;

namespace BusinessLayer.Services.DbServices;

public class PostVoteDbService : GenericDbService<PostVoteDto, PostVote>,  IPostVoteDbService
{
  public PostVoteDbService(IGenericRepository<PostVote> repository, IMapper mapper) : base(repository, mapper) {}
}