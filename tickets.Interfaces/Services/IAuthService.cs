using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Requests.Auth;
using tickets.Domain.Responses.Auth;
using tickets.Domain.Responses.Base;

namespace tickets.Interfaces.Services
{
    public interface IAuthService
    {
        Task<BaseResponse<AuthRegisterResponse>> Register(AuthRegisterRequest request);

        Task<BaseResponse<AuthLoginResponse>> Login(AuthLoginRequest request);

        Task<BaseResponse<AuthCurrentUserResponse>> GetCurrentUser(ClaimsPrincipal userClaim);
    }
}
