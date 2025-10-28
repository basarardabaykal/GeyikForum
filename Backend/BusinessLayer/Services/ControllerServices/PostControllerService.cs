using System.Diagnostics.CodeAnalysis;
using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using DataLayer;

namespace BusinessLayer.Services.ControllerServices;

public class PostControllerService : GenericControllerService<PostDto>,  IPostControllerService
{
    private readonly IPostDbService _postDbService;
    private readonly IPostVoteControllerService _postVoteControllerService;
    private readonly AppDbContext  _dbContext;

    public PostControllerService(IPostDbService postDbService, IPostVoteControllerService postVoteControllerService, AppDbContext dbContext) : base(postDbService)
    {
        _postDbService = postDbService;
        _postVoteControllerService = postVoteControllerService;
        _dbContext = dbContext;
    }

    public async Task<IDataResult<PostDto>> CreatePost(PostDto postDto)
    {
        var result = await _postDbService.CreatePost(postDto);
        return result;
    }

    public async Task<IDataResult<PostDto>> VotePost(PostVoteDto postVoteDto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        var postVoteResult = await _postVoteControllerService.CreatePostVote(postVoteDto);
        if (!postVoteResult.Success)
        {
            await transaction.RollbackAsync();
            return new ErrorDataResult<PostDto>(postVoteResult.StatusCode, postVoteResult.Message);
        }

        IDataResult<PostDto> postResult;

        if (postVoteDto.VoteValue == 1)
        {
            if (postVoteResult.Data == null)
            {
                postResult = await _postDbService.VotePost(postVoteDto.PostId, -1);
            }
            else if (postVoteResult.Data.PreviousVoteValue == null)
            {
                postResult = await _postDbService.VotePost(postVoteDto.PostId, 1);
            }
            else
            {
                postResult = await _postDbService.VotePost(postVoteDto.PostId, 2);
            }
        }
        else
        {
            if (postVoteResult.Data == null)
            {
                postResult = await _postDbService.VotePost(postVoteDto.PostId, 1);
            }
            else if (postVoteResult.Data.PreviousVoteValue == null)
            {
                postResult = await _postDbService.VotePost(postVoteDto.PostId, -1);
            }
            else
            {
                postResult = await _postDbService.VotePost(postVoteDto.PostId, -2);
            }
        }
        
        if (!postResult.Success)
        {
            await transaction.RollbackAsync();
            return postResult;
        }
        
        await transaction.CommitAsync();
        return postResult;
    }

    public async Task<IDataResult<PostDto>> SetPinStatus(PostPinRequestDto dto)
    {
        var user = await _dbContext.Set<AppUser>().FindAsync(dto.UserId);
        if (user is null) return new ErrorDataResult<PostDto>(403, "Yetkiniz yok.");
        if (!user.IsAdmin) return new ErrorDataResult<PostDto>(403, "Yetkiniz yok.");

        return await _postDbService.SetPinStatus(dto.PostId, dto.IsPinned);
    }

    public async Task<IDataResult<PostDto>> SetDeleteStatus(PostDeleteRequestDto dto)
    {
        var user = await _dbContext.Set<AppUser>().FindAsync(dto.UserId);
        if (user is null) return new ErrorDataResult<PostDto>(403, "Yetkiniz yok.");
        if (!user.IsAdmin) return new ErrorDataResult<PostDto>(403, "Yetkiniz yok.");

        return await _postDbService.SetDeleteStatus(dto.PostId, dto.IsDeleted);
    }

    public async Task<IDataResult<List<PostDto>>> GetPage(int page, int pageSize)
    {
        return await _postDbService.GetPage(page,  pageSize);
    }
}