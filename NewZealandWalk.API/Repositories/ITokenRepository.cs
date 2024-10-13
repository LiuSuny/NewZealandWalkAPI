using Microsoft.AspNetCore.Identity;
using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
