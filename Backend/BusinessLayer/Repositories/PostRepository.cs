using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repositories;

public class PostRepository : GenericRepository<Post>,  IPostRepository
{
  public PostRepository(AppDbContext dbContext) : base(dbContext) { }

  public async Task<IDataResult<Post>> CreatePost(Post post)
  {
    var result = await _dbSet.AddAsync(post);
    if (result.Entity != null)
    {
      await _dbContext.SaveChangesAsync();
      return new SuccessDataResult<Post>("Gönderi başarıyla oluşturuldu.", result.Entity);
    }
    
    return new ErrorDataResult<Post>(500,"Gönderi oluşturulamadı.");
  }

  public async Task<IDataResult<Post>> VotePost(Guid postId, int voteValue)
  {
    var post = await _dbSet.FindAsync(postId);
    if (post == null)
    {
      return new ErrorDataResult<Post>(404, "Gönderi bulunamadı.");
    }
    post.VoteScore += voteValue;
    await _dbContext.SaveChangesAsync();
    return new SuccessDataResult<Post>("Gönderi başarıyla oylandı.", post);
  }

  public async Task<IDataResult<Post>> SetPinStatus(Guid postId, bool isPinned)
  {
    var post = await _dbContext.Set<Post>().FirstOrDefaultAsync(p => p.Id == postId);
    if (post.ParentId != null) return new ErrorDataResult<Post>(400, "Yorumlar sabitlenemez.");

    post.IsPinned = isPinned;
    post.UpdatedAt = DateTime.UtcNow;
    await _dbContext.SaveChangesAsync();

    return new SuccessDataResult<Post>(isPinned ? "Gönderi sabitlendi" : "Gönderi sabitlenmesi kaldırıldı.", post);
  }

  public async Task<IDataResult<Post>> SetDeleteStatus(Guid postId, bool isDeleted)
  {
    var post = await _dbContext.Set<Post>().FirstOrDefaultAsync(p => p.Id == postId);
    if (post is null) return new ErrorDataResult<Post>(StatusCodes.Status404NotFound, "Post not found");

    post.IsDeleted = isDeleted;
    post.UpdatedAt = DateTime.UtcNow;
    await _dbContext.SaveChangesAsync();

    return new SuccessDataResult<Post>(isDeleted ? "Gönderi silindi." : "Gönderi tekrar açıldı.", post);
  }
}