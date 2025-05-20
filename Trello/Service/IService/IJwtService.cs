using System.Security.Claims;
using Trello.Model;

namespace Trello.Service.IService
{
    public interface IJwtService
    {
        (string accessToken, string refreshToken) GenerateTokens(User user);

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

        bool ValidateRefreshToken(User user, string refreshToken);

    }
}
