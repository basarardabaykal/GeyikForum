namespace BusinessLayer.Dtos.Auth;

public class ConfirmEmailRequestDto
{
  public Guid UserId { get; set; } 
  public string Token { get; set; }
}