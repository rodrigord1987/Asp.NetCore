using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using webAPI.Context;
using webAPI.Models;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cars : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public Cars(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet] // Get All
        public async Task<IActionResult> GetCars()
        {
            return Ok(new
            {
                sucess = true,
                data = await _appDbContext.Cars.ToListAsync()
            });
        }

        [HttpGet("{id}")] // Get one
        public IActionResult GetCar(int id)
        {
            Car car = _appDbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPost] // Create
        public async Task<IActionResult> PostCar(Car car)
        {
            _appDbContext.Cars.Add(car);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                sucess = true,
                data = car
            });
        }

        [HttpPut("{id}")] // Update
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            var ExistCar = await _appDbContext.Cars.AnyAsync(c => c.Id == id && car.Id == id);

            if (!ExistCar)
            {
                return NotFound();
            }

            _appDbContext.Entry(car).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(new
            {
                sucess = true,
                data = car
            });
        }

        [HttpDelete("{id}")] // Delete
        public async Task<IActionResult> DeleteCar(int id)
        {
            Car car = _appDbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            _appDbContext.Cars.Remove(car);

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

    }
}
