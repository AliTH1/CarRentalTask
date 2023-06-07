using AutoMapper;
using CarRental.Entities.Auth;
using CarRental.Entities.Dtos.Auth;

namespace CarRental.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<LoginDto, AppUser>();
        }
    }
}
