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
    public class CartLineMapping :Profile
    {
        public CartLineMapping()
        {
            CreateMap<CartLine,CartLineDto>()
                .ForCtorParam("Count",p=>p.MapFrom(o=>o.Count))
                .ForCtorParam("ProdName",p=>p.MapFrom(o=>o.Product.Name))
                .ReverseMap();
        }
    }
}
