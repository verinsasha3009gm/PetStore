using AutoMapper;
using PetStore.Markets.Domain.Dto.Address;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Mapping
{
    public class AddressMapping : Profile
    {
        public AddressMapping()
        {
            CreateMap<Address,AddressDto>()
                .ForCtorParam("Country", p=>p.MapFrom(p=>p.Country))
                .ForCtorParam("Region",p=>p.MapFrom(p=>p.Region))
                .ForCtorParam("City", p => p.MapFrom(p => p.City))
                .ForCtorParam("Street", p => p.MapFrom(p => p.Street))
                .ReverseMap();
            CreateMap<Address, AddressGuidDto>()
                .ForCtorParam("GuidId",p=>p.MapFrom(p=>p.GuidId))
                .ForCtorParam("Country", p => p.MapFrom(p => p.Country))
                .ForCtorParam("Region", p => p.MapFrom(p => p.Region))
                .ForCtorParam("City", p => p.MapFrom(p => p.City))
                .ForCtorParam("Street", p => p.MapFrom(p => p.Street))
                .ReverseMap();
        }
    }
}
