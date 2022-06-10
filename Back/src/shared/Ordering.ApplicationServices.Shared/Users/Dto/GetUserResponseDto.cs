using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Users.Dto
{
    public class GetUserResponseDto
    {
        public Value value { get; set; }
        public int statusCode { get; set; }
        public object contentType { get; set; }

        public class Value
        {
            public Claims claims { get; set; }
            public object name { get; set; }
            public bool isAuthenticated { get; set; }
            public string authenticationType { get; set; }
        }

        public class Claims
        {
            public string id { get; set; }
            public string UserName { get; set; }
            public string LastName { get; set; }
            public string FisrtName { get; set; }
            public string FullName { get; set; }
            public string Role { get; set; }
            public string exp { get; set; }
            public string iss { get; set; }
            public string aud { get; set; }
        }
    }
}
