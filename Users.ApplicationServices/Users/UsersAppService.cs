using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.Core;
using Users.Core.Users;
using Users.Users.Dto;

namespace Users.ApplicationServices.Users
{
    public class UsersAppService : IUsersAppService
    {
        /*
        private UserManager<User> _userManager;
        private static IHttpContextAccessor _accessor;
        private readonly IOptions<JwtConfig> config;

        public UsersAppService(UserManager<User> userManager,
            IHttpContextAccessor contextAccessor,
            IOptions<JwtConfig> config)
        {
            _userManager = userManager;
            _accessor = contextAccessor;
            this.config = config;
        }
        
        public async Task<AuthenticateResponseDto> AuthenticateAsync(AuthenticateRequestDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Results.Forbid();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return Results.Ok(new
            {
                AccessToken = jwt
            });
        }

        public string generateToken(AuthenticateRequestDto model)
        {
            throw new NotImplementedException();
        }*/
        public AuthenticateResponseDto AuthenticateAsync(AuthenticateRequestDto model)
        {
            throw new NotImplementedException();
        }

        public string generateToken(AuthenticateRequestDto model)
        {
            throw new NotImplementedException();
        }
    }
}
