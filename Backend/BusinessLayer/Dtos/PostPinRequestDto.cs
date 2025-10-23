namespace BusinessLayer.Dtos;

public class PostPinRequestDto
{
  public Guid PostId { get; set; }
  public Guid UserId { get; set; }
  public bool IsPinned { get; set; }
}