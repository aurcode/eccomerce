using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Core.Users;

namespace Users.Users.Dto;
public class AuthenticateResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }


    public AuthenticateResponseDto(User user, string token)
    {
        //Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        //Username = user.Username;
        Token = token;
    }
}