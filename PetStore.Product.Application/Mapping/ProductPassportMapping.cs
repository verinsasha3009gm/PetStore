using AutoMapper;
using PetStore.Products.Domain.Dto.ProductPassport;
using PetStore.Products.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Application.Mapping
{
    public class ProductPassportMapping : Profile
    {
        public ProductPassportMapping()
        {
            CreateMap<ProductPassport, ProductPassportDto>()
                //.ForCtorParam(ctorParamName: "Id", m => m.MapFrom(s => s.Id))
                .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(s => s.Name))
                .ForCtorParam(ctorParamName:"Company",m=>m.MapFrom(s => s.Company))
                .ForCtorParam(ctorParamName: "Description", m => m.MapFrom(s => s.Description))
                .ReverseMap();
        }
    }
}
