using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public CitiesController(DataContext context)
        {
            _context = context;
        }

		[HttpGet]
		public async Task<ActionResult> GetAsync([FromQuery] PaginationDTO pagination)
		{
			var queryable = _context.Cities
				.Where(x => x.State!.Id == pagination.id)
				.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(
                                            x => x.Name
                                                    .ToLower()
                                                    .Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
				.OrderBy(x => x.Name)
				.Paginate(pagination)
				.ToListAsync()
				);
		}

		[HttpGet("TotalPages")]
		public async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
		{
			var queryable = _context.Cities
				.Where(x => x.State!.Id != pagination.id)
				.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(
                                            x => x.Name
                                                    .ToLower()
                                                    .Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
			double totalPages = Math.Ceiling(count / pagination.RecordsNumber);

			return Ok(totalPages);
		}

		[HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(City city)
        {
            try
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {

                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("already exist a city with the same name");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(City city)
        {
            try
            {
                _context.Update(city);
                await _context.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {

                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("already exist a city with the same name");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
