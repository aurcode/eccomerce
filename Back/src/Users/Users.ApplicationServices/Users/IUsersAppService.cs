using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.Core.Users;
using Users.Users.Dto;

namespace Users.ApplicationServices.Users
{
    public interface IUsersAppService
    {
        //Task DeleteUserAsync(string id);
        Task CreateUserAsync(CreateUserDto userDto);
        Task<IQueryable<UserDto>> GetAllUsersAsync();
        Task<UserDto> FindUserByIdAsync(string id);
        Task<User> AuthenticateAsync(AuthenticateRequestDto request);
        Task<List<Claim>> GetClaimAsync(User user);
        Task<string> GenerateTokenAsync(List<Claim> claims);
        Task<ClaimsPrincipal> GetUserFromContext();
        //string generateToken(AuthenticateRequestDto model);
        //IEnumerable<User> GetAll();
        //User GetById(int id);
    }
}
