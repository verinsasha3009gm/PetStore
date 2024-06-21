using AutoMapper;
using PetStore.Users.Domain.Dto.User;
using PetStore.Users.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User,UserDto>()
                .ForCtorParam("Login",p=>p.MapFrom(p=>p.Login))
                .ForCtorParam("Email",p=>p.MapFrom(p=>p.Email))
                .ReverseMap();
        }
    }
}
