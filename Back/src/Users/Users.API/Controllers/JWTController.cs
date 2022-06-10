using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        private readonly IUsersAppService _usersAppServices;

        public JWTController(IUsersAppService usersAppServices)
        {
            _usersAppServices = usersAppServices;
        }

        [Authorize]
        [HttpGet("/token")]
        public async Task<IResult> GetAsync()
        {
            ClaimsPrincipal user = await _usersAppServices.GetUserFromContext();

            return Results.Ok(new
            {
                Claims = user.Claims.Select(x => new { x.Type, x.Value }).ToList()
                .GroupBy(claim => claim.Value)
                .ToDictionary(group => group.Last().Type, 
                              group => group.Last().Value),
                user.Identity.Name,
                user.Identity.IsAuthenticated,
                user.Identity.AuthenticationType
            });
        }

        [HttpPost("/token")]
        public async Task<IResult> Post(AuthenticateRequestDto request)
        {
            User user = await _usersAppServices.AuthenticateAsync(request);
            List<Claim> claims = await _usersAppServices.GetClaimAsync(user);
            string token = await _usersAppServices.GenerateTokenAsync(claims);

            return Results.Ok(new
            {
                AccessToken = token
            });
        }
    }
}
