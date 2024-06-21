using AutoMapper;
using PetStore.Users.Domain.Dto.Role;
using PetStore.Users.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Application.Mapping
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<Role,RoleDto>()
                .ForCtorParam("Name",p=>p.MapFrom(o=>o.Name))
                .ReverseMap();
        }
    }
}
