using CarRental.DAL;
using CarRental.Entities.Dtos.Cars;
using CarRental.Models;
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

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id) 
        {
            Car? result = await _context.Cars.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (result == null) return NotFound();

            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Car> cars = await _context.Cars.ToListAsync();

            if (cars.Count == 0) return NotFound();

            return StatusCode((int)HttpStatusCode.OK, cars);
        }

        [HttpPost]
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
