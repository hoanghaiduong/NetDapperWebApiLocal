using System.Security.Claims;
using NetDapperWebApi.DTO;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IAuthService
    {
        Task<string> GrantRolesToUser(AddRelationsMM<int,int> dto);
        Task<TokenModel> RefreshToken(RefreshTokenModel dto,string uid,string accessToken);
        Task<AuthResponse> SignInAsync(AuthDTO dto);
        Task<User> SignUpAsync(AuthDTO dto);
        Task<bool> Revoke(int uid);
    }
}