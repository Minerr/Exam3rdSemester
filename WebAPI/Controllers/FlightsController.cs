using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DAL;
using WebApplication.Models;

namespace WebAPI.Controllers
{
	[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly MainDbContext _context;

        public FlightsController(MainDbContext context)
        {
            _context = context;
        }

		/// <summary>
		/// A method that returns a list of all flights.
		/// </summary>
		/// <returns>Returns a list with all flight from the database, it will be empty if there is no flights in the database.</returns>
        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            return await _context.Flights.ToListAsync();
        }

		/// <summary>
		/// A method that returns a specific flight using the specified id.
		/// </summary>
		/// <param name="id">The flight id to find.</param>
		/// <returns>
		/// Returns StatusCode NotFound, if the flight cannot be found with the specified id. 
		/// Returns the flight object if the database contains a flight with the specified id.
		/// </returns>
		// GET: api/Flights/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return flight;
        }

		/// <summary>
		/// A method that returns a list of flights containing the specified fromLocation and toLocation.
		/// </summary>
		/// <param name="from">The flights departure location.</param>
		/// <param name="to">The flights arrival location.</param>
		/// <returns>Returns a list of flights, it will be empty if there is no flights containing the specified fromLocation or toLocation in the database.</returns>
		// GET: api/Flights/FromAndTo
		[HttpGet("FromAndTo")]
		public async Task<ActionResult<IEnumerable<Flight>>> GetFlightsFromAndTo(string from, string to)
		{
			return await _context.Flights.Where(f => f.FromLocation == from && f.ToLocation == to).ToListAsync();
		}

		/// <summary>
		/// A method to change a flights information using the specified id. It overwrites the data with the specified flight object.
		/// </summary>
		/// <param name="id">The id of the flight to change</param>
		/// <param name="flight">A flight object containing the updated data.</param>
		/// <returns>
		/// Returns StatusCode 400 Bad Request, if the specified id and the flight object id is not the same. 
		/// Returns StatusCode 404 Not Found, if the specified id does not exist in the database. 
		/// Returns StatusCode 200 Ok, if successful.
		/// </returns>
		// PUT: api/Flights/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(int id, Flight flight)
        {
            if (id != flight.FlightId)
            {
                return BadRequest();
            }

            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

		/// <summary>
		/// Stores a flight in the database using the specified flight object.
		/// </summary>
		/// <param name="flight">
		/// The flight object to store in the database. 
		/// The value of the id in the flight object does not matter. 
		/// The database auto-generates it.
		/// </param>
		/// <returns>
		/// Returns StatusCode 201 Created if successful.
		/// Returns 500 Internal Server Error if not.
		/// </returns>
        // POST: api/Flights
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.FlightId }, flight);
        }

		/// <summary>
		/// Removes a flight from the database with the specified id.
		/// </summary>
		/// <param name="id">The id of the flight to remove.</param>
		/// <returns>
		/// Returns StatusCode 404 Not Found, if the specified id does not exist in the database.
		/// Returns the flight object if successful.
		/// </returns>
        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Flight>> DeleteFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return flight;
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }
    }
}
