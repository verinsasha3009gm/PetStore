using AutoMapper;
using PetStore.Markets.Domain.Dto.Product;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                .ForCtorParam("Name", p => p.MapFrom(p => p.Name))
                .ForCtorParam("Description", p => p.MapFrom(p => p.Description))
                .ForCtorParam("Price", p => p.MapFrom(p => p.Price))
                .ReverseMap();
            CreateMap<Product, ProductGuidDto>()
                .ForCtorParam("GuidId",p=>p.MapFrom(p => p.GuidId))
                .ForCtorParam("Name", p => p.MapFrom(p => p.Name))
                .ForCtorParam("Description", p => p.MapFrom(p => p.Description))
                .ForCtorParam("Price", p => p.MapFrom(p => p.Price))
                .ReverseMap();
        }
    }
}
