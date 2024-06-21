using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Users.Domain.Dto.Address;
using PetStore.Users.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Application.Mapping
{
    public class AddressMapping : Profile
    {
        public AddressMapping()
        {
            CreateMap<Address, AddressDto>()
                .ForCtorParam(ctorParamName: "Region", p => p.MapFrom(o => o.Region))
                .ForCtorParam(ctorParamName: "City",p=>p.MapFrom(o=>o.City))
                .ForCtorParam(ctorParamName: "Street",p=>p.MapFrom(o=>o.Street))
                .ForCtorParam(ctorParamName: "Country", p=>p.MapFrom(o=>o.Country))
                .ForCtorParam(ctorParamName: "GuidId",p=>p.MapFrom(o=>o.GuidId))
                .ReverseMap();
        }
    }
}
