
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using ecom.Models;

using System;

using System.Linq;

using System.Threading.Tasks;
 
namespace ecom.Controllers;

    [ApiController]

    [Route("api/[controller]")]

    public class CustomerController : ControllerBase

    {

        private readonly EcomContext _context;
 
        public CustomerController(EcomContext context)

        {

            _context = context;

        }
        [HttpGet]
        public async Task<IActionResult> getAllCustomers()
        {
           var customer = await _context.Customers.ToListAsync();
           return Ok(customer);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> getCustomerById(int id)
        {
            var customers = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId==id);
            if(customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

    }