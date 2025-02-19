using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CartController(StoreContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart() 
        {
            var cart = await FindCartFromCookies();

            if(cart == null) return NoContent();

            return cart.MapToCartDto();
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> AddItemToCart(int productId, int quantity)
        {
            var cart = await FindCartFromCookies();
            cart ??= CreateCart();
            
            var product = await context.Products.FindAsync(productId);
            if(product == null) return BadRequest("Problem adding item to cart");

            cart.AddItem(product, quantity);
            
            var result = await context.SaveChangesAsync() > 0;
            if(result) return CreatedAtAction(nameof(GetCart), cart.MapToCartDto());

            return BadRequest("Problem updating cart");
        }


        [HttpDelete]
        public async Task<ActionResult> RemoveCartItem(int productId, int quantity)
        {
            var cart = await FindCartFromCookies();
            if(cart == null) return BadRequest("Unable to find the cart");

            cart.RemoveItem(productId, quantity);  

            var result = await context.SaveChangesAsync() > 0;
            if(result) return Ok(); 

            return BadRequest("Problem updating cart");
        }

        private async Task<Cart?> FindCartFromCookies()
        {
            return await context.Carts
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.CartId == Request.Cookies["cartId"]);
        }

        private Cart CreateCart()
        {
            var cartId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(30)
            };
            Response.Cookies.Append("cartId", cartId, cookieOptions);
            var cart = new Cart() { CartId = cartId };
            context.Carts.Add(cart);
            return cart;
        }

    }
}
