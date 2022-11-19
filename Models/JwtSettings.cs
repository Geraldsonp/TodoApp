namespace TodoApp.Models;

public class JwtSettings
{
    public int? HoursValidFor { get; set; }
    public string? Audience { get; set; }
    public string? Issuer { get; set; }
    public string? SecretKey { get; set; }
}