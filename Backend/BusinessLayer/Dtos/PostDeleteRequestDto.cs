namespace BusinessLayer.Dtos;

public class PostDeleteRequestDto
{
  public Guid PostId { get; set; }
  public Guid UserId { get; set; }
  public bool IsDeleted { get; set; }
}