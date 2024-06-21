using AutoMapper;
using PetStore.Products.Domain.Dto.Teg;
using PetStore.Products.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Application.Mapping
{
    public class TegMapping : Profile 
    {
        public TegMapping()
        {
            CreateMap<Teg,TegDto>()
                //.ForCtorParam(ctorParamName: "Id", p=>p.MapFrom(p=>p.Id))
                .ForCtorParam(ctorParamName:"Name",p=>p.MapFrom(p=>p.Name))
                .ReverseMap();
        }
    }
}
