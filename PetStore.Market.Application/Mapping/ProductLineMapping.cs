using AutoMapper;
using PetStore.Markets.Domain.Dto.ProductLine;
using PetStore.Markets.Domain.Entity;

namespace PetStore.Markets.Application.Mapping
{
    public class ProductLineMapping : Profile
    {
        public ProductLineMapping()
        {
            CreateMap<ProductLine, ProductLineDto>()
                .ForCtorParam("Name", p => p.MapFrom(p => p.Product.Name))
                .ForCtorParam("Count", p => p.MapFrom(p => p.Count))
                .ReverseMap();
        }
    }
}
