using AutoMapper;
using CarRental.DAL;
using CarRental.Entities.Dtos.Cars;
using CarRental.Models;
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
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CarsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id) 
        {
            Car? result = await _context.Cars.Where(c => c.Id == id).FirstOrDefaultAsync();
            GetCarDto getCarDto = _mapper.Map<GetCarDto>(result);

            if (result == null) return NotFound();

            return StatusCode((int)HttpStatusCode.OK, getCarDto);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Car> cars = await _context.Cars.ToListAsync();
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

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.Created, car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, UpdateCarDto updateCar)
        {
            Car? result = await _context.Cars.Where(c => c.Id == updateCar.Id)
                .FirstOrDefaultAsync();
            if (result == null) return NotFound();

            result.BrandId = updateCar.BrandId;
            result.ColorId = updateCar.ColorId;
            result.ModelYear = updateCar.ModelYear;
            result.DailyPrice = updateCar.DailyPrice;
            result.Description = updateCar.Description;

            await _context.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.Accepted, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Car? result = await _context.Cars.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (result == null) return NotFound();

            _context.Remove(result);
            await _context.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
