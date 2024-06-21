using AutoMapper;
using PetStore.Markets.Domain.Dto.Employe;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Mapping
{
    public class EmployeMapping : Profile
    {
        public EmployeMapping()
        {
            CreateMap<Employe, EmployeDto>()
                .ForCtorParam("Name",p=>p.MapFrom(p=>p.Name))
                .ForCtorParam("Gender", p => p.MapFrom(p => p.Gender))
                .ReverseMap();
            CreateMap<Employe, EmployeGuidDto>()
                .ForCtorParam("Email", p => p.MapFrom(p => p.Email))
                .ForCtorParam("GuidId", p => p.MapFrom(p => p.GuidId))
                .ReverseMap();
        }
    }
}
