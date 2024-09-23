using CustomerWebApi.DbContexts;
using CustomerWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add logging

namespace CustomerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDbContext _customerDbContext;
        private readonly ILogger<CustomerController> _logger; // Logger

        public CustomerController(CustomerDbContext customerDbContext, ILogger<CustomerController> logger)
        {
            _customerDbContext = customerDbContext;
            _logger = logger; // Inject logger
        }

        // GET: api/<CustomerController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get()
        {
            try
            {
                var customers = await _customerDbContext.Customers.ToListAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customers.");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/<CustomerController>/5
        [HttpGet("{customerId}")]
        public async Task<ActionResult<Customer>> GetById(int customerId)
        {
            try
            {
                var customer = await _customerDbContext.Customers.FindAsync(customerId);
                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching customer with ID {customerId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Customer customer)
        {
            try
            {
                await _customerDbContext.Customers.AddAsync(customer);
                await _customerDbContext.SaveChangesAsync();
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new customer.");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<CustomerController>/5
        [HttpPut()]
        public async Task<ActionResult> Put([FromBody] Customer customer)
        {
            try
            {
                _customerDbContext.Customers.Update(customer);
                await _customerDbContext.SaveChangesAsync();
                return Ok(customer);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while updating the customer.");
                return Conflict("Concurrency conflict occurred.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the customer.");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{customerId:int}")]
        public async Task<IActionResult> Delete(int customerId)
        {
            try
            {
                var customer = await _customerDbContext.Customers.FindAsync(customerId);
                if (customer == null)
                {
                    return NotFound();
                }

                _customerDbContext.Customers.Remove(customer);
                await _customerDbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting customer with ID {customerId}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
