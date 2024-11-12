
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

    public class CartController : ControllerBase

    {

        private readonly EcomContext _context;
 
        public CartController(EcomContext context)

        {

            _context = context;

        }

//        [HttpPost("add-item")]
//        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemRequest request)
//        {
//     if (!ModelState.IsValid)
//     {
//         return BadRequest(ModelState);
//     }

//     try
//     {
//         var product = await _context.Products
//             .FirstOrDefaultAsync(p => p.ProductId == request.ProductId && !p.IsDeleted);

//         if (product == null)
//         {
//             return NotFound($"Product with ID {request.ProductId} not found.");
//         }

//         // Assuming no CustomerId is required for this operation
//         var cartItem = new CartItem
//         {
//             ProductId = request.ProductId,
//             Quantity = 1, // Set a default quantity or handle as per your application's logic
//             // CustomerId = null; // If CustomerId is non-nullable, this will cause an error
//             // Adjust CustomerId based on your application logic
//             CustomerId = 1  // Example: Set a default CustomerId or handle appropriately
//         };

//         _context.CartItems.Add(cartItem);
//         await _context.SaveChangesAsync();

//         return Ok($"Added 1 {product.Name}(s) to cart.");
//     }
//     catch (Exception ex)
//     {
//         return StatusCode(500, $"Internal server error: {ex.Message}");
//     }
// }
[HttpPost("add-item")]
public async Task<IActionResult> AddCartItem([FromBody] AddCartItemRequest request)
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
            return NotFound(new { message = $"Product with ID {request.ProductId} not found." });
        }

        var cartItem = new CartItem
        {
            ProductId = request.ProductId,
            Quantity = 1,
            CustomerId = 1 // Adjust based on your logic
        };

        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Added 1 {product.Name}(s) to cart." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
    }
}

[HttpPut("update-item/{productId}")]
public async Task<IActionResult> UpdateCartItem(int productId, [FromQuery] int quantity)
{
    if (quantity <= 0)
    {
        return BadRequest(new { message = "Quantity must be greater than zero." });
    }

    try
    {
        // Find the cart item based on the product ID
        var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId);

        if (cartItem == null)
        {
            return NotFound(new { message = $"Cart item with Product ID {productId} not found." });
        }

        // Update the quantity of the cart item
        cartItem.Quantity = quantity;

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Cart item with Product ID {productId} updated successfully." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
    }
}



[HttpDelete("delete-item/{productId}")]
public async Task<IActionResult> DeleteCartItem(int productId)
{
    try
    {
        // Find all cart items with the specified productId
        var cartItems = await _context.CartItems
            .Where(ci => ci.ProductId == productId)
            .ToListAsync();

        if (cartItems == null || cartItems.Count == 0)
        {
            // Return a JSON response with an appropriate status code
            return NotFound(new { message = $"No cart items found for product with ID {productId}." });
        }

        // Remove all cart items for the productId found
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        // Return a JSON response with a success message
        return Ok(new { message = $"All cart items for product ID {productId} deleted successfully." });
    }
    catch (Exception ex)
    {
        // Return a JSON response with an error message
        return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
    }
}

  // GET: api/cart/items
[HttpGet("items")]
public async Task<IActionResult> GetCartItems()
{
    try
    {
        var cartItems = await _context.CartItems
            .Include(ci => ci.Product) // Include related Product information
            .Where(ci => ci.Quantity > 0) // Optional: Include any filtering condition as needed
            .Select(ci => new 
            {
                ci.ProductId,
                ci.CustomerId,
                ci.Quantity,
                ci.Product.ProductImage,
                ci.Product.Description,
                ci.Product.Price
            })
            .ToListAsync();

        return Ok(cartItems);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}

// [HttpDelete("delete-all")]
// public async Task<IActionResult> DeleteAllCartItems()
// {
//     try{
//         var cartItems = await _context.CartItems.ToListAsync();
//         if(cartItems == null || cartItems.Count == 0)
//         {
//             return NotFound("No cart items found");
//         }
//         _context.CartItems.RemoveRange(cartItems);
//         await _context.SaveChangesAsync();
//         return Ok("All cart items deleted sucessfully.");
//     }
//     catch(Exception ex)
//     {
//         return StatusCode(500,$"Internal server error: {ex.Message}");
//     }
// }


[HttpDelete("delete-all")]
public async Task<IActionResult> DeleteAllCartItems()
{
    try
    {
        var cartItems = await _context.CartItems.ToListAsync();

        // Check if there are no cart items
        if (cartItems == null || cartItems.Count == 0)
        {
            // Return No Content instead of Not Found
            return NoContent(); // 204 No Content
        }

        // Remove all cart items
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        // Return success response
        return Ok("All cart items deleted successfully.");
    }
    catch (Exception ex)
    {
        // Log the exception details (you should have a logger instance)
        // _logger.LogError(ex, "An error occurred while deleting cart items.");

        // Return an error response
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}


 }
 
    // Request DTOs for cart operations

    public class AddCartItemRequest

    {

        public int ProductId { get; set; }

        public int CustomerId { get; set; }

        public int Quantity { get; set; }

       

    }
 
    public class UpdateCartItemRequest

    {
                public int Quantity { get; set; }
    }

}
