using AutoMapper;
using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Services.DbServices;

public class PostDbService : GenericDbService<PostDto, Post>, IPostDbService
{
  private readonly IPostRepository _repository;

  public PostDbService(IPostRepository repository, IMapper mapper) : base(repository, mapper)
  {
    _repository = repository;
  }

  public async Task<IDataResult<PostDto>> CreatePost(PostDto postDto)
  {
    var postEntity = _mapper.Map<Post>(postDto);
    var result = await _repository.CreatePost(postEntity);

    var newPostDto = _mapper.Map<PostDto>(result.Data);
    
    return new SuccessDataResult<PostDto>(result.Message, newPostDto);
  }

  public async Task<IDataResult<PostDto>> VotePost(Guid postId, int voteValue)
  {
    var result = await _repository.VotePost(postId, voteValue);
    var postDto = _mapper.Map<PostDto>(result.Data);
    return new SuccessDataResult<PostDto>(result.Message, postDto);
  }
}