using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Core.Users;
using Users.Users.Dto;

namespace Users.ApplicationServices
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateProjection<User, UserDto>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
