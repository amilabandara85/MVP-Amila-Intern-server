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
    public class StoresController : ControllerBase
    {
        private readonly AmilaOnboardingContext _context;

        public StoresController(AmilaOnboardingContext context)
        {
            _context = context;
        }

        // GET: api/Stores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            return await _context.Stores.ToListAsync();
        }

        // GET: api/Stores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            try
            {
                var store = await _context.Stores.FindAsync(id);

                if (store == null)
                {
                    return NotFound("This record not in your DB");
                }

                return store;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while retrieving the store record.");
            }
            
        }

        // PUT: api/Stores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, Store store)
        {
            
                if (id != store.Id)
                {
                    return BadRequest();
                }
                try
                {
                    if (!StoreExists(id))
                    {
                        return NotFound("This record not in your DB");
                    }

                    _context.Entry(store).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StoreExists(id))
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
                    return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while retrieving the store record.");
                }


            
        }

        // POST: api/Stores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754customers.map
        [HttpPost]
        public async Task<ActionResult<Store>> PostStore(Store store)
        {
            try
            {
                _context.Stores.Add(store);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetStore", new { id = store.Id }, store);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while retrieving the store record.");
            }
            
        }

        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            try
            {
                var store = await _context.Stores.FindAsync(id);
                if (store == null)
                {
                    return NotFound();
                }

                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while retrieving the store record.");
            }
            
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.Id == id);
        }
    }
}
