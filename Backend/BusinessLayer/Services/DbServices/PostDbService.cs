using AutoMapper;
using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Entities;

namespace BusinessLayer.Services.DbServices;

public class PostDbService : GenericDbService<PostDto, Post>, IPostDbService
{
  public PostDbService(IGenericRepository<Post> repository, IMapper mapper) : base(repository, mapper) {}
    
}