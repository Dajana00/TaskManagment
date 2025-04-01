using Trello.Model;

namespace Trello.Service.IService
{
    public interface IJwtService
    {
        (string accessToken, string refreshToken) GenerateTokens(User user);
    }
}
