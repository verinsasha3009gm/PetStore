using AutoMapper;
using PetStore.Users.Domain.Dto.Cart;
using PetStore.Users.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Application.Mapping
{
    public class CartMapping : Profile
    {
        public CartMapping()
        {
            CreateMap<Cart, CartDto>()
                .ForCtorParam("Count", p => p.MapFrom(o => o.Count))
                .ReverseMap();
        }
    }
}
