using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Users.Core.Users;
using Users.Users.Dto;

namespace Users.API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        [HttpGet("/")]
        public string Get(int id)
        {
            return "Funciona";
        }

        /*
        [HttpPost("/")]
        public async Task<IResult> Post(AuthenticateRequestDto request, UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync(request.UserName);

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return Results.Forbid();
            }

            var roles = await userManager.GetRolesAsync(user);
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

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: builder.Configuration["Jwt:Issuer"],
                audience: builder.Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return Results.Ok(new
            {
                AccessToken = jwt
            });
        }
        */
    }
}
