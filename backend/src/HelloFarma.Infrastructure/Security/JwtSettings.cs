namespace HelloFarma.Infrastructure.Security;

public class JwtSettings
{
    public string SecretKey { get; set; } = default!;
    public string Issuer { get; set; } = "HelloFarma";
    public string Audience { get; set; } = "HelloFarma.Clients";
    public int ExpiracaoMinutos { get; set; } = 60;
}
