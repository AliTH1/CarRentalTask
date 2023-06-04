using CarRental.DAL;
using CarRental.Entities.Dtos.Brands;
using CarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Brand? brand = await _context.Brands.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (brand == null) return NotFound();

            return StatusCode((int)HttpStatusCode.OK, brand);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Brand> brands = await _context.Brands.ToListAsync();
            if (brands.Count == 0) return NoContent();

            return StatusCode((int)HttpStatusCode.OK, brands);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateBrandDto createBrand)
        {
            Brand newBrand = new()
            {
                Name = createBrand.Name
            };

            await _context.Brands.AddAsync(newBrand);
            await _context.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.Created, newBrand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateBrandDto updateBrand)
        {
            Brand? brand = await _context.Brands.FindAsync(updateBrand.Id);
            if (brand == null) return NotFound();

            brand.Name = updateBrand.Name;

            await _context.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.Accepted, brand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Brand? brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();

            _context.Remove(brand);
            await _context.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
