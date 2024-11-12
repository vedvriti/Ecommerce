

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using ecom.Models;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;
 
namespace ecom.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class OrderController : ControllerBase

    {

        private readonly EcomContext _context;
 
        public OrderController(EcomContext context)

        {

            _context = context;

        }
 
        // POST: api/order/place-order

        [HttpPost("place-order")]

        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)

        {

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState);

            }
 
            try

            {

                // Fetch the customer details based on CustomerId

                var customer = await _context.Customers.FindAsync(request.CustomerId);

                if (customer == null)

                {

                    return NotFound($"Customer with ID {request.CustomerId} not found.");

                }
 
                // Fetch cart items for the customer

                var cartItems = await _context.CartItems

                    .Include(ci => ci.Product)

                    .Where(ci => ci.CustomerId == request.CustomerId && ci.Quantity > 0)

                    .ToListAsync();
 
                if (cartItems.Count == 0)

                {

                    return BadRequest("No items in the cart to place an order.");

                }
 
                // Calculate TotalAmount based on cart items

                decimal totalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);
 
                // Apply discount coupon if provided

                decimal discountAmount = 0;

                if (!string.IsNullOrEmpty(request.DiscountCouponCode))

                {

                    // Simulate discount coupon logic without a database table

                    if (request.DiscountCouponCode == "SUMMER2024")

                    {

                        // Example: Apply 10% discount if the coupon code is valid

                        discountAmount = totalAmount * 0.1m; // 10% discount

                        totalAmount -= discountAmount;

                    }

                    else

                    {

                        return BadRequest($"Discount coupon '{request.DiscountCouponCode}' is invalid or expired.");

                    }

                }
 
                // Prepare the order object

                var order = new Order

                {

                    CustomerId = request.CustomerId,

                    TotalAmount = totalAmount,

                    Status = "Pending", // Example: Default status for new orders

                    OrderDate = DateTime.UtcNow, // Use UTC time for consistency

                };
 
                // Save order to database

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();
 
                // Optional: Clear the cart items after placing the order

                // _context.CartItems.RemoveRange(cartItems);

                // await _context.SaveChangesAsync();
 
                return Ok($"Order placed successfully for customer: {customer.Name}. Total amount: {totalAmount}. Discount applied: {discountAmount}");

            }

            catch (Exception ex)

            {

                // Log exception details

                Console.WriteLine(ex.ToString());
 
                // Return a 500 Internal Server Error response with a generic error message

                return StatusCode(500, "An error occurred while processing your request. Please try again later.");

            }

        }
 
        // GET: api/order/orders/{customerId}
// GET: api/order/orders/{customerId}
[HttpGet("orders/{customerId}")]
public async Task<IActionResult> GetOrders(int customerId)
{
    try
    {
        // Fetch orders for the customer including related customer information
        var orders = await _context.Orders
            .Include(o => o.Customer) // Include the Customer navigation property
            .Include(o => o.OrderItems) // Include the OrderItems navigation property
            .ThenInclude(oi => oi.Product) // Include the Product navigation property if needed
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();

        if (orders == null || orders.Count == 0)
        {
            return NotFound($"No orders found for customer with ID {customerId}.");
        }

        // Project the orders into a simplified view model
        var ordersViewModel = orders.Select(o => new
        {
            CustomerName = o.Customer.Name,
            OrderId = o.OrderId,
            TotalAmountAfterDiscount = o.TotalAmount,
           
            OrderDate = o.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
            ShippingAddress = o.Customer.Address
        });

        return Ok(ordersViewModel);
    }
    catch (Exception ex)
    {
        // Log exception details
        Console.WriteLine(ex.ToString());
        
        // Return a 500 Internal Server Error response with a generic error message
        return StatusCode(500, "An error occurred while processing your request. Please try again later.");
    }
}

        // DELETE: api/order/delete/{orderId}
[HttpDelete("delete/{orderId}")]
public async Task<IActionResult> DeleteOrder(int orderId)
{
    try
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
        {
            return NotFound($"Order with ID {orderId} not found.");
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return Ok($"Order with ID {orderId} deleted successfully.");
    }
    catch (Exception ex)
    {
        // Log exception details
        Console.WriteLine(ex.ToString());

        // Return a 500 Internal Server Error response with a generic error message
        return StatusCode(500, "An error occurred while processing your request. Please try again later.");
    }
}

 
    // Request DTO for placing an order

    public class PlaceOrderRequest

    {

        public int CustomerId { get; set; }

        public string DiscountCouponCode { get; set; }

    }

    }
}
