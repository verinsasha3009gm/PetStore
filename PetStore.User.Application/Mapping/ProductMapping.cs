using AutoMapper;
using PetStore.Users.Domain.Dto.Product;
using PetStore.Users.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Application.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                .ForCtorParam("Name",p=>p.MapFrom(o=>o.Name))
                .ForCtorParam("Description",p=>p.MapFrom(o=>o.Description))
                .ForCtorParam("GuidId",p=>p.MapFrom(o=>o.GuidId))
                .ReverseMap();
        }
    }
}
