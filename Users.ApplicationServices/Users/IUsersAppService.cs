using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Core.Users;
using Users.Users.Dto;

namespace Users.ApplicationServices.Users
{
    public interface IUsersAppService
    {
        AuthenticateResponseDto AuthenticateAsync(AuthenticateRequestDto model);
        string generateToken(AuthenticateRequestDto model);
        //IEnumerable<User> GetAll();
        //User GetById(int id);
    }
}
