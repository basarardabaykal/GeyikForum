namespace BusinessLayer.Dtos;

public class PreviousNicknameDto
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid UserId { get; set; }
  public string Nickname { get; set; }
}