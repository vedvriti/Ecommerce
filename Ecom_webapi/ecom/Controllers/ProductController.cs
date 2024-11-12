
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using ecom.Models;

using System;

using System.Linq;

using System.Threading.Tasks;
 
namespace ecom.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class ProductController : ControllerBase

    {

        private readonly EcomContext _context;
 
        public ProductController(EcomContext context)

        {

            _context = context;

        }
 
         // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _context.Products
                    .Where(p => !p.IsDeleted)
                    .Select(p => new 
                    {
                        p.ProductId,
                        p.ProductImage,
                        p.Name,
                        p.Price,
                        p.Description
                    })
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
 
        // GET: api/product/5

        [HttpGet("{id}")]

        public async Task<IActionResult> GetProductById(int id)

        {

            try

            {

                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id && !p.IsDeleted);
 
                if (product == null)

                {

                    return NotFound($"Product with ID {id} not found.");

                }
 
                return Ok(product);

            }

            catch (Exception ex)

            {

                return StatusCode(500, $"Internal server error: {ex.Message}");

            }

        }
 
        // POST: api/product/add-to-cart

        [HttpPost("add-to-cart")]

        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)

        {

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState);

            }
 
            try

            {

                var product = await _context.Products

                    .FirstOrDefaultAsync(p => p.ProductId == request.ProductId && !p.IsDeleted);
 
                if (product == null)

                {

                    return NotFound($"Product with ID {request.ProductId} not found.");

                }
 
                // Here you would typically add the product to the user's cart in your application logic.

                // For simplicity, let's assume we are adding to the CartItem table directly.
 
                var customerId = 1; // Example: Replace with actual customer ID from your authentication
 
                var cartItem = await _context.CartItems

                    .FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == request.ProductId);
 
                if (cartItem == null)

                {

                    cartItem = new CartItem

                    {

                        CustomerId = customerId,

                        ProductId = request.ProductId,

                        Quantity = request.Quantity

                    };

                    _context.CartItems.Add(cartItem);

                }

                else

                {

                    cartItem.Quantity += request.Quantity;

                }
 
                await _context.SaveChangesAsync();
 
                return Ok($"Added {request.Quantity} {product.Name}(s) to cart.");

            }

            catch (Exception ex)

            {

                return StatusCode(500, $"Internal server error: {ex.Message}");

            }

        }

    }
 
    // Request DTO for adding product to cart

    public class AddToCartRequest

    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

    }

}
