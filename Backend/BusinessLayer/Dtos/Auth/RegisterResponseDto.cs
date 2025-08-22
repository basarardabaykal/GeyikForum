namespace BusinessLayer.Dtos.Auth;

public class RegisterResponseDto
{
  public AppUserDto UserDto { get; set; }
  public string Token  { get; set; }
}