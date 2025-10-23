using BusinessLayer.Dtos;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.DbServices;

public interface IPostDbService : IGenericDbService<PostDto>
{
  public Task<IDataResult<PostDto>> CreatePost(PostDto dto);
  public Task<IDataResult<PostDto>> VotePost(Guid postId, int voteValue);

  public Task<IDataResult<PostDto>> SetPinStatus(Guid postId, bool isPinned);
  public Task<IDataResult<PostDto>> SetDeleteStatus(Guid postId, bool isDeleted);
}