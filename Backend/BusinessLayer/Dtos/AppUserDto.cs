namespace BusinessLayer.Dtos;

public class AppUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }
    public int Karma { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsModerator { get; set; }
    public bool IsBanned { get; set; }
}