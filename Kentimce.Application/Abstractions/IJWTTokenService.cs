using Kentimce.Domain;
using System.Security.Claims;


namespace Kentimce.Application.Abstractions
{
    public interface IJWTTokenService
    {
        string GenerateAccessToken(Kullanici kullanici, List<string> roller);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
