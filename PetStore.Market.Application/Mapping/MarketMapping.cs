using AutoMapper;
using PetStore.Markets.Domain.Dto.Market;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Mapping
{
    public class MarketMapping : Profile
    {
        public MarketMapping()
        {
            CreateMap<Market, MarketDto>()
                .ForCtorParam("NameMarket", p => p.MapFrom(p=>p.Name))
                .ForCtorParam("guidId",p=>p.MapFrom(p=>p.GuidId))
                .ReverseMap();
        }
    }
}
