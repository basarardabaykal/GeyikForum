namespace CoreLayer.Entities;

public class Post : IBaseEntity
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; }  // null for main posts
    public string? Title { get; set; } //null for comments
    public string Content { get; set; }
    public bool IsPinned { get; set; }
    public int VoteScore { get; set; }
    public int CommentCount { get; set; }
    public int Depth { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsEdited { get; set; }
    public bool IsDeleted { get; set; }
    
    public virtual AppUser User { get; set; }
    public virtual Post? Parent { get; set; }
    public virtual ICollection<Post> Children { get; set; } = new List<Post>();
    public virtual ICollection<PostVote> Votes { get; set; } = new List<PostVote>();
}