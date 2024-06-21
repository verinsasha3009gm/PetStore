using AutoMapper;
using PetStore.Products.Domain.Dto.Product;
using PetStore.Products.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Application.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product,ProductDto>()
                //.ForCtorParam(ctorParamName: "Id", m => m.MapFrom(s => s.Id))
                .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(s => s.Name))
                .ForCtorParam(ctorParamName: "Description", m => m.MapFrom(s => s.Description))
                .ForCtorParam(ctorParamName: "Price",m=>m.MapFrom(s=>s.Price))
                .ReverseMap();
            CreateMap<Product, ProductGuidDto>()
                .ForCtorParam(ctorParamName: "GuidId", m => m.MapFrom(s => s.GuidId))
                .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(s => s.Name))
                .ForCtorParam(ctorParamName: "Description", m => m.MapFrom(s => s.Description))
                .ForCtorParam(ctorParamName: "Price", m => m.MapFrom(s => s.Price))
                .ReverseMap();
        }
    }
}
