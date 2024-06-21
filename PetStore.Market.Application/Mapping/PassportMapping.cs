using AutoMapper;
using PetStore.Markets.Domain.Dto.Passport;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Mapping
{
    public class PassportMapping : Profile
    {
        public PassportMapping()
        {
            CreateMap<Passport,PassportDto>()
                .ForCtorParam("Name",p=>p.MapFrom(p=>p.Name))
                .ForCtorParam("Familien",p=>p.MapFrom(p=>p.Familien))
                .ForCtorParam("Issued",p=>p.MapFrom(p=>p.Issued))
                .ForCtorParam("PlaceOfBirth",p=>p.MapFrom(p => p.PlaceOfBirth))
                .ReverseMap();
        }
    }
}
