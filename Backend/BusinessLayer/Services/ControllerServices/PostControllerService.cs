using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Services.ControllerServices;

public class PostControllerService : GenericControllerService<PostDto>,  IPostControllerService
{
    private readonly IPostDbService _dbService;

    public PostControllerService(IPostDbService dbService) : base(dbService)
    {
        _dbService = dbService;
    }

    public async Task<IDataResult<PostDto>> CreatePost(PostDto postDto)
    {
        var result = await _dbService.CreatePost(postDto);
        return result;
    }

    public async Task<IDataResult<PostDto>> VotePost(Guid postId, int voteValue)
    {
        return await  _dbService.VotePost(postId, voteValue);
    }
}