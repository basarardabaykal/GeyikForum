namespace BusinessLayer.Dtos;

public class PostVoteDto
{
  public Guid Id { get; set; } =  Guid.NewGuid();
  public Guid UserId {get; set; }
  public Guid PostId {get; set; }
  public int VoteValue {get; set; }
  public int? PreviousVoteValue {get; set; }
}