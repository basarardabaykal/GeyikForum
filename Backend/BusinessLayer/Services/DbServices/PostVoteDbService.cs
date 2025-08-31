using AutoMapper;
using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Services.DbServices;

public class PostVoteDbService : GenericDbService<PostVoteDto, PostVote>,  IPostVoteDbService
{
  private readonly IPostVoteRepository _postVoteRepository;

  public PostVoteDbService(IPostVoteRepository repository, IMapper mapper) : base(repository, mapper)
  {
    _postVoteRepository = repository;
  }

  public async Task<IDataResult<PostVoteDto>> CreatePostVote(PostVoteDto postVote)
  {
   var postVoteEntity = _mapper.Map<PostVote>(postVote);
   var result = await _postVoteRepository.CreatePostVote(postVoteEntity);
   var newPostVoteDto = _mapper.Map<PostVoteDto>(result.Data);
   if (!result.Success)
   {
     return new ErrorDataResult<PostVoteDto>(result.StatusCode, result.Message);
   }

   return new SuccessDataResult<PostVoteDto>(result.Message, newPostVoteDto);
  }
}