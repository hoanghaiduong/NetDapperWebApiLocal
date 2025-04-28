using System.Security.Claims;
using NetDapperWebApi_local.DTO;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Common.Interfaces
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