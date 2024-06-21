using AutoMapper;
using PetStore.Markets.Domain.Dto.User;
using PetStore.Markets.Domain.Entity;

namespace PetStore.Markets.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User,UserGuidDto>()
                .ForCtorParam("GuidId",p=>p.MapFrom(p=>p.GuidId))
                .ForCtorParam("Login",p=>p.MapFrom(p=>p.Login))
                .ForCtorParam("Email",p=>p.MapFrom(p=>p.Email))
                .ForCtorParam("Role", p=>p.MapFrom(p=>p.Role))
                .ReverseMap();

            CreateMap<User,UserDto>()
                .ForCtorParam("Login", p => p.MapFrom(p => p.Login))
                .ForCtorParam("Email", p => p.MapFrom(p => p.Email))
                .ForCtorParam("Role", p => p.MapFrom(p => p.Role))
                .ReverseMap();
        }
    }
}
