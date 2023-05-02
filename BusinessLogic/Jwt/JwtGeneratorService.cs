using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogic.Configs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogic.Jwt;

public interface IJwtGeneratorService
{
    JwtGenerationResult GenerateToken(Guid userId);
}

public class JwtGeneratorService : IJwtGeneratorService
{
    private readonly JwtConfig _config;

    private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();

    public JwtGeneratorService(IOptions<JwtConfig> options)
    {
        _config = options.Value;
    }

    public JwtGenerationResult GenerateToken(Guid userId)
    {
        var issuer = _config.Issuer;
        var audience = _config.Audience;

        var key = Encoding.ASCII.GetBytes(_config.Key);

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenJwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            notBefore: null,
            claims: new[]
            {
                new Claim(KnownJwtClaims.UserId, userId.ToString()),
                // the JTI is used for our refresh token which we will be covering in the next video
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid()
                        .ToString()
                )
            },
            expires: DateTime.UtcNow.AddSeconds(_config.LifetimeSeconds),
            signingCredentials: signingCredentials
        );

        var refreshJwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            notBefore: null,
            claims: new[]
            {
                new Claim(KnownJwtClaims.UserId, userId.ToString()),
                // the JTI is used for our refresh token which we will be covering in the next video
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid()
                        .ToString()
                )
            },
            expires: DateTime.UtcNow.AddSeconds(_config.RefreshLifetimeSeconds),
            signingCredentials: signingCredentials
        );

        var token = JwtSecurityTokenHandler.WriteToken(tokenJwt);
        var refreshToken = JwtSecurityTokenHandler.WriteToken(refreshJwt);

        return new(token, refreshToken);
    }
}