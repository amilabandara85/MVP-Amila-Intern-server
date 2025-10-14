using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmilaOnboarding.Server.Models;

namespace AmilaOnboarding.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly AmilaOnboardingContext _context;

        public SalesController(AmilaOnboardingContext context)
        {
            _context = context;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            try
            {
                
                return await _context.Sales
                    .Include(s => s.Customer)
                    .Include(s => s.Product)
                    .Include(s => s.Store)
                    .ToArrayAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while retrieving sales data.");
            }
        }

        

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            try
            {
                var sale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Product)
                .Include(s => s.Store)
                .FirstOrDefaultAsync(s => s.Id == id);

                if (sale == null)
                {
                    return NotFound("This record not in your DB");
                }

                return sale;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while retrieving the sale record.");
            }
        }

        // PUT: api/Sales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }
            try
            {
                if (!SaleExists(id))
                    {
                    return NotFound("This record not in your DB");
                    }

                _context.Entry(sale).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(id))
                    {
                        return NotFound("This record not in your DB");
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while updating the sale record.");
            }
        }

        // POST: api/Sales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            try
            {
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetSale", new { id = sale.Id }, sale);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while creating the sale record.");
            }
            
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            try
            {
                var sale = await _context.Sales.FindAsync(id);
                if (sale == null)
                {
                    return NotFound("This record not in your DB");
                }

                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while creating the sale record.");
            }
            
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
