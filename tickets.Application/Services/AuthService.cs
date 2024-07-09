using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Models;
using tickets.Domain.Requests.Auth;
using tickets.Domain.Responses.Auth;
using tickets.Domain.Responses.Base;
using tickets.Interfaces.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using UserEntity = tickets.Domain.Entities.User;

namespace tickets.Application.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly UserManager<UserEntity> _userManager;

        private readonly SignInManager<UserEntity> _signInManager;

        private readonly IConfiguration _configuration;

        public AuthService(
            IMapper mapper,
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            IConfiguration configuration
        ) : base(mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<BaseResponse<AuthCurrentUserResponse>> GetCurrentUser(ClaimsPrincipal userClaim)
        {
            if (userClaim.Identity.IsAuthenticated)
            {
                var userName = userClaim.Identity.Name;
                var userEmail = userClaim.FindFirstValue(ClaimTypes.Email);
                var userId = userClaim.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByNameAsync(userName);
                var userModel = _mapper.Map<UserModel>(user);
                return new BaseResponse<AuthCurrentUserResponse>
                {
                    Data = new AuthCurrentUserResponse
                    {
                        User = userModel
                    }
                };
            }
            throw new UnauthorizedAccessException("The user is not authenticated.");
        }

        public async Task<BaseResponse<AuthLoginResponse>> Login(AuthLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
                    signingCredentials: creds
                );

                return new BaseResponse<AuthLoginResponse>
                {
                    Data = new AuthLoginResponse
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    }
                };
            }
            throw new UnauthorizedAccessException("The user is not unauthorized.");
        }

        public async Task<BaseResponse<AuthRegisterResponse>> Register(AuthRegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ArgumentException("Passwords do not match.");
            }

            var user = _mapper.Map<UserEntity>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new BaseResponse<AuthRegisterResponse>
                {
                    Data = new AuthRegisterResponse
                    {
                        Message = $"{request.UserName} created successfully."
                    }
                };
            }

            var listerrors = result.Errors.Select(error => error.Description).ToList();
            string errors = String.Join(", ", listerrors);
            throw new ArgumentException($"Errors : {errors}");
        }
    }
}
