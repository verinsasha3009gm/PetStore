using AutoMapper;
using PetStore.Products.Domain.Dto.Category;
using PetStore.Products.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Application.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping() 
        {
            CreateMap<Category, CategoryDto>()
                //.ForCtorParam(ctorParamName: "Id", m => m.MapFrom(s => s.Id))
                .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(s => s.Name))
                //.ForCtorParam(ctorParamName: "Description", m => m.MapFrom(s => s.Description))
                .ReverseMap();
        }
    }
}
