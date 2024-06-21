using AutoMapper;
using PetStore.Markets.Domain.Dto.EmployePassport;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Mapping
{
    public class EmployePassportMapping : Profile
    {
        public EmployePassportMapping()
        {
            CreateMap<EmployePassport, EmployePassportDto>()
                .ForCtorParam("Salary",p=>p.MapFrom(p=>p.Salary))
                .ForCtorParam("Expirience", p => p.MapFrom(p => p.Experience))
                .ForCtorParam("Post", p => p.MapFrom(p => p.Post))
                .ReverseMap();
            CreateMap<EmployePassport, EmployePassportGuidDto>()
                .ForCtorParam("GuidId", p => p.MapFrom(p => p.GuidId))
                .ForCtorParam("Salary", p => p.MapFrom(p => p.Salary))
                .ForCtorParam("Expirience", p => p.MapFrom(p => p.Experience))
                .ForCtorParam("Post", p => p.MapFrom(p => p.Post))
                .ReverseMap();
        }
    }
}
