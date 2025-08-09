namespace BusinessLayer.Dtos;

public class PostDto
{
  public Guid Id { get; set; } =  Guid.NewGuid();
  public Guid UserId { get; set; }
  public Guid? ParentId { get; set; } // null for main posts
  public string? Title { get; set; } //null for comments
  public string Content { get; set; }
  public bool IsPinned { get; set; }
  public int VoteScore { get; set; }
  public int CommentCount { get; set; }
  public int Depth { get; set; }
  public bool IsEdited { get; set; }
  public bool IsDeleted { get; set; }
}