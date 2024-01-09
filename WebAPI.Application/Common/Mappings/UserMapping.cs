using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Common.Mappings
{
    public class UserMapping:Profile
    {
        public UserMapping()
        {
            CreateMap<User, CreateUserDto>();
			CreateMap<CreateUserDto, User>();
			CreateMap<User, UpdateUserNameDto>().ReverseMap();
            CreateMap<User, ResponseUserDto>().ReverseMap();
            

		}
    }
}
