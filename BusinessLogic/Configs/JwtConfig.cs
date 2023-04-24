namespace BusinessLogic.Configs;

public class JwtConfig
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    
    public int LifetimeSeconds { get; set; }
    public int RefreshLifetimeSeconds { get; set; }
}