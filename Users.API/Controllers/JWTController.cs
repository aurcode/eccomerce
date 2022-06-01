using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.ApplicationServices.Users;
using Users.Core;
using Users.Core.Users;
using Users.Users.Dto;

namespace Users.API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private UserManager<User> _userManager;
        private static IHttpContextAccessor _accessor;
        private readonly IOptions<JwtConfig> config;
        private readonly IUsersAppService _usersAppServices;

        public JWTController(UserManager<User> userManager,
            IUsersAppService usersAppServices,
            IHttpContextAccessor contextAccessor,
            IOptions<JwtConfig> config)
        {
            _userManager = userManager;
            _usersAppServices = usersAppServices;
            _accessor = contextAccessor;
            this.config = config;
        }

        [Authorize]
        [HttpGet("/token")]
        public IResult Get()
        {
            var user = _accessor.HttpContext.User;

            return Results.Ok(new
            {
                Claims = user.Claims.Select(s => new
                {
                    s.Type,
                    s.Value
                }).ToList(),
                user.Identity.Name,
                user.Identity.IsAuthenticated,
                user.Identity.AuthenticationType
            });
        }

        [HttpPost("/token")]
        public async Task<IResult> Post(AuthenticateRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Results.Forbid();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("UserName", user.UserName),
                new Claim("LastName", user.FirstName),
                new Claim("FisrtName", user.LastName),
                new Claim("FullName", $"{user.FirstName} {user.LastName}")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("Role", role));
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
    }
}
