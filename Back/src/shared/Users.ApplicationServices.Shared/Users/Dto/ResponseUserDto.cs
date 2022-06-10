using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Users.Dto
{
    public class ResponseUserDto
    {
        public UserDto value { get; set; }
        public int statusCode { get; set; }
        public object contentType { get; set; }
    }
}
