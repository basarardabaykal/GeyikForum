using BusinessLayer.Dtos;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.DbServices;

public interface IPostDbService : IGenericDbService<PostDto>
{
  public Task<IDataResult<PostDto>> CreatePost(PostDto dto);
}