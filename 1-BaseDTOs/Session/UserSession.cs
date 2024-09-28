namespace _1_BaseDTOs.Session;

public class UserSession
{
    private string? _userId;

    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? UserId { get; set; }
    public bool IsDarkMode { get; set; }
}
