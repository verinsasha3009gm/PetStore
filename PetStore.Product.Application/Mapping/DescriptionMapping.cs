using AutoMapper;
using PetStore.Products.Domain.Dto.Description;
using PetStore.Products.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Application.Mapping
{
    public class DescriptionMapping : Profile
    {
        public DescriptionMapping()
        {
            CreateMap<Description,DescriptionDto>()
                //.ForCtorParam(ctorParamName:"Id",p=>p.MapFrom(p=>p.Id))
                .ForCtorParam(ctorParamName:"Culture",p=>p.MapFrom(p=>p.Culture))
                .ForCtorParam(ctorParamName:"Detail",p=>p.MapFrom(p=>p.Detail))
                .ReverseMap();
        }
    }
}
