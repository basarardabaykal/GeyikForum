namespace CoreLayer.Entities;

public class PostVote : IBaseEntity
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public Guid UserId {get; set; }
    public Guid PostId {get; set; }
    public int VoteValue {get; set; }
    public DateTime CreatedAt {get; set; }
    public DateTime? UpdatedAt {get; set; }
    public virtual AppUser User { get; set; }
    public virtual Post Post { get; set; }
}