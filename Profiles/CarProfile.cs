using AutoMapper;
using CarRental.Entities.Dtos.Cars;
using CarRental.Models;

namespace CarRental.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, GetCarDto>();
        }
    }
}
