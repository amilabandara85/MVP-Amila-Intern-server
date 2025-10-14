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
    public class ProductsController : ControllerBase
    {
        private readonly AmilaOnboardingContext _context;

        public ProductsController(AmilaOnboardingContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {

                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound("This record not in your DB");
                }

                return product;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An unexpected error occurred while retrieving the product record.");
            
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            

            try
            { 
                if (!ProductExists(id))
                {
                return NotFound("This Record not in youe DB");
                }

                _context.Entry(product).State = EntityState.Modified;

                try
                {
                await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                {
                    return NotFound("This Record not in youe DB");
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
                return StatusCode(StatusCodes.Status500InternalServerError,"An unexpected error occurred while retrieving the product record.");
            }
        
        
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An unexpected error occurred while retrieving the product record.");
            }
            

            
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An unexpected error occurred while retrieving the product record.");
            }
            
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
