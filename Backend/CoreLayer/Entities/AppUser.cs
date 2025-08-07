using Microsoft.AspNetCore.Identity;

namespace CoreLayer.Entities;

public class AppUser : IdentityUser<Guid>, IBaseEntity
{
    public string Nickname { get; set; }
    public int Karma { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsModerator { get; set; }
    public bool IsBanned { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
    public virtual ICollection<PostVote> Votes { get; set; } = new List<PostVote>();
    public virtual ICollection<PreviousNickname> PreviousNicknames { get; set; }
}