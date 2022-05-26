using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Core.Users;
using Users.Users.Dto;

namespace Users.ApplicationServices.Users
{
    public class UsersAppService : IUsersAppService
    {
        public AuthenticateResponseDto Authenticate(AuthenticateRequestDto model)
        {
            throw new NotImplementedException();
        }

        public string generateToken(AuthenticateRequestDto model)
        {
            throw new NotImplementedException();
        }
    }
}
