namespace BusinessLayer.Dtos.Auth;

public class LoginResponseDto
{
  public AppUserDto UserDto { get; set; }
  public string Token { get; set; }
}