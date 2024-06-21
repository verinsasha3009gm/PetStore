using AutoMapper;
using PetStore.Markets.Domain.Dto.MarketCapital;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Application.Mapping
{
    public class MarketCapitalMapping : Profile
    {
        public MarketCapitalMapping()
        {
            CreateMap<MarketCapital,MarketCapitalDto>()
                .ForCtorParam("DailyIncome",p=>p.MapFrom(p=>p.DailyIncome))
                .ForCtorParam("DateTime",p=>p.MapFrom(p=>p.Date))
                .ReverseMap();
        }
    }
}
