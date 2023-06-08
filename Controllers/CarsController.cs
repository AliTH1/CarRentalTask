using AutoMapper;
using CarRental.DAL;
using CarRental.DAL.Repositories.Abstracts;
using CarRental.Entities.Dtos.Cars;
using CarRental.Models;
using CarRental.UnitOfWork.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CarsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Car? result = await _unitOfWork.CarRepository.Get(c => c.Id == id);
            GetCarDto getCarDto = _mapper.Map<GetCarDto>(result);

            if (result == null) return NotFound();

            return StatusCode((int)HttpStatusCode.OK, getCarDto);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Car> cars = await _unitOfWork.CarRepository.GetAll();
            List<GetCarDto> getCarDtos = _mapper.Map<List<GetCarDto>>(cars);

            if (cars.Count == 0) return NotFound();

            return StatusCode((int)HttpStatusCode.OK, getCarDtos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CreateCarDto createCar)
        {
            Car car = new Car()
            {
                BrandId = createCar.BrandId,
                ColorId = createCar.ColorId,
                ModelYear = createCar.ModelYear,
                DailyPrice = createCar.DailyPrice,
                Description = createCar.Description,
            };

            await _unitOfWork.CarRepository.AddAsync(car);
            await _unitOfWork.SaveAsync();

            return StatusCode((int)HttpStatusCode.Created, car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateCarDto updateCar)
        {
            Car? result = await _unitOfWork.CarRepository.Get(c => c.Id == updateCar.Id);
            if (result == null) return NotFound();

            result.BrandId = updateCar.BrandId;
            result.ColorId = updateCar.ColorId;
            result.ModelYear = updateCar.ModelYear;
            result.DailyPrice = updateCar.DailyPrice;
            result.Description = updateCar.Description;

            await _unitOfWork.SaveAsync();

            return StatusCode((int)HttpStatusCode.Accepted, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Car? result = await _unitOfWork.CarRepository.Get(c => c.Id == id);
            if (result == null) return NotFound();

            await _unitOfWork.CarRepository.DeleteAsync(result);
            await _unitOfWork.SaveAsync();
            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
