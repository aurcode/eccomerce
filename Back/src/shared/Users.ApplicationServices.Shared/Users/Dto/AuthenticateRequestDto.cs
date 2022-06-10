using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Users.Dto;
public record struct AuthenticateRequestDto(string UserName, string Password);