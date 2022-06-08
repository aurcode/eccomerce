using Ordering.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.ApplicationServices.Users
{
    public interface IUsersAppService
    {
        Task<GetUserResponseDto> FindUserByTokenAsync(string token);
    }
}
