using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private UserManager<User> _userManager;
        private static IHttpContextAccessor _accessor;
        private readonly IOptions<JwtConfig> config;
        private readonly IMapper _mapper;

        public UsersAppService(UserManager<User> userManager,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IOptions<JwtConfig> config)
        {
            _mapper = mapper;
            _userManager = userManager;
            _accessor = contextAccessor;
            this.config = config;
        }

        public async Task<IQueryable<UserDto>> GetAllUsersAsync()
        {
            IQueryable<UserDto> users= _mapper.ProjectTo<UserDto>(_userManager.Users);
            return users;
        }

        public async Task<UserDto> FindUserByIdAsync(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            UserDto _mappedUser = _mapper.Map<UserDto>(user);

            return _mappedUser;
        }
        public async Task DeleteUserAsync(string id)
        {

        }
        public async Task CreateUserAsync(CreateUserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);

            if ( !(userDto.Role == 1 || userDto.Role == 2) )
            {
                throw new Exception("Invalid role");
            }

            IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.ToString());
            }

            switch (userDto.Role)
            {
                case 1:
                    await _userManager.AddToRoleAsync(user, "user");
                    break;
                case 2:
                    await _userManager.AddToRoleAsync(user, "admin");
                    break;
                default:
                    throw new Exception("Assing rol error, only can write between 1 and 2");
                    break;
            }

        }

        public async Task<User> AuthenticateAsync(AuthenticateRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new Exception("Invalid username or password");
            }

            return user;
        }
        
        public async Task<List<Claim>> GetClaimAsync(User user)
        {
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

            return claims;
        }
        
        public async Task<string> GenerateTokenAsync(List<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return jwt;
        }

        public async Task<ClaimsPrincipal> GetUserFromContext()
        {
            return _accessor.HttpContext.User;
        }
    }
}
