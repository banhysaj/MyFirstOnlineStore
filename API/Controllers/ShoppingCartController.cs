using API.DTO;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/shoppingCarts")] 
    [ApiController]
    public class ShoppingCartController: BaseApiController
    {
        private readonly StoreContext _context;

        public ShoppingCartController(StoreContext context){
            _context =context;
        }

        [HttpGet("user/{Id}")]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartByUserId(int Id)
        {
            var cart = await _context.Shopping_Carts
                .Include(x => x.Cart_Items)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == Id);

            if (cart == null)
            {
                return NotFound("Shopping cart not found for the specified user.");
            }

            var cartDto = new ShoppingCartDto
            {
                Id = cart.Id,
                UserId = cart.User.Id,
                CartItems = cart.Cart_Items.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    Quantity = ci.Quantity,
                    Product = new ProductDto{
                            Id = ci.Product.Id,
                            Name = ci.Product.Name,
                            Price = ci.Product.Price
                    }
                }).ToList()
            };

            return Ok(cartDto);
        }

        // [HttpPut("updateCart/{Id}")]
        // public IActionResult UpdateCart(int Id, [FromBody] ShoppingCartDto requestDto)
        // {
        //     // Retrieve the shopping cart to be updated
        //     var cart = _context.Shopping_Carts
        //                         .Include(x => x.Cart_Items)
        //                         .FirstOrDefault(x => x.Id == Id);

        //     if (cart == null)
        //     {
        //         return NotFound("Shopping cart not found");
        //     }

        //     // Update items in the cart
        //     foreach (var cartItemDto in requestDto.CartItems)
        //     {
        //         var existingCartItem = cart.Cart_Items.FirstOrDefault(ci => ci.Id == cartItemDto.Id);
        //         if (existingCartItem == null)
        //         {
        //             return NotFound($"Cart item with ID {cartItemDto.Id} not found in the shopping cart");
        //         }

        //         existingCartItem.Quantity = cartItemDto.Quantity;
        //         existingCartItem.Product = _context.Products.FirstOrDefault(p => p.Id == cartItemDto.Product.Id);
        //         if (existingCartItem.Product == null)
        //         {
        //             return NotFound($"Product with ID {cartItemDto.Product.Id} not found");
        //         }
        //     }

        //     _context.SaveChanges();

        //     var updatedCartDto = new ShoppingCartDto
        //     {
        //         Id = cart.Id,
        //         UserId = cart.User.Id,
        //         CartItems = cart.Cart_Items.Select(ci => new CartItemDto
        //         {
        //             Id = ci.Id,
        //             Quantity = ci.Quantity,
        //             Product = new ProductDto{
        //                     Id = ci.Product.Id,
        //                     Name = ci.Product.Name,
        //                     Price = ci.Product.Price
        //             }
        //         }).ToList()
        //     };

        //     return Ok(updatedCartDto);
        // }

        

        [HttpGet("{Id}")]
        public IActionResult GetCart(int Id)
        {
            
            var cart = _context.Shopping_Carts
                                .Include(x => x.Cart_Items)
                                .ThenInclude(x => x.Product)
                                .Include(x => x.User)
                                .FirstOrDefault(x => x.Id == Id);

            
            if (cart == null)
            {
                return NotFound();
            }

            
            var cartDto = new ShoppingCartDto
            {
                Id = cart.Id,
                UserId = cart.User.Id,
                CartItems = new List<CartItemDto>()
            };

            
            foreach (var cartItem in cart.Cart_Items)
            {
                var cartItemDto = new CartItemDto
                {
                    Id = cartItem.Id,
                    Quantity = cartItem.Quantity,
                    Product = new ProductDto{
                            Id = cartItem.Product.Id,
                            Name = cartItem.Product.Name,
                            Price = cartItem.Product.Price
                    }
                };
                cartDto.CartItems.Add(cartItemDto);
            }

            return Ok(cartDto);
        }

        [HttpGet]
        public List<ShoppingCartDto> GetAllShoppingCarts()
        {
            var carts = _context.Shopping_Carts
                                .Include(x => x.Cart_Items)
                                .ThenInclude(x => x.Product)
                                .Include(x => x.Cart_Items)
                                .Include(x => x.User)
                                .ToList();

            var shoppingCartDtos = new List<ShoppingCartDto>();

            foreach(var cart in carts)
            {
                var cartDto = new ShoppingCartDto
                {
                    Id = cart.Id,
                    UserId = cart.User.Id,
                    CartItems = new List<CartItemDto>()
                };

                foreach(var cartItem in cart.Cart_Items)
                {
                    var cartItemDto = new CartItemDto
                    {
                        Id = cartItem.Id,
                        Quantity = cartItem.Quantity,
                        Product = new ProductDto{
                            Id = cartItem.Product.Id,
                            Name = cartItem.Product.Name,
                            Price = cartItem.Product.Price
                    } 
                    };

                    cartDto.CartItems.Add(cartItemDto);
                }

                shoppingCartDtos.Add(cartDto);
            }

            return shoppingCartDtos;
        }
        
        [HttpPost("addCart")]
        public IActionResult AddCart([FromBody] ShoppingCartDto requestDto)
        {

                var user = _context.Users.FirstOrDefault(u => u.Id == requestDto.UserId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var cart = new ShoppingCart
                {
                    User = user,
                    Cart_Items = new List<Cart_Item>()
                };

                foreach (var cartItemDto in requestDto.CartItems)
                {
                    var product = _context.Products.FirstOrDefault(p => p.Id == cartItemDto.Product.Id);
                    if (product == null)
                    {
                        return NotFound($"Product with ID {cartItemDto.Product.Id} not found");
                    }

                    var cartItem = new Cart_Item
                    {
                        Product = product,
                        Quantity = cartItemDto.Quantity
                    };
                    cart.Cart_Items.Add(cartItem);
                }

                _context.Shopping_Carts.Add(cart);
                _context.SaveChanges();

                var cartDto = new ShoppingCartDto
                {
                    Id = cart.Id,
                    UserId = cart.User.Id,
                    CartItems = cart.Cart_Items.Select(ci => new CartItemDto
                    {
                        Id = ci.Id,
                        Quantity = ci.Quantity,
                        Product = new ProductDto{
                            Id = ci.Product.Id,
                            Name = ci.Product.Name,
                            Price = ci.Product.Price
                    }
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetCart), new { id = cartDto.Id }, cartDto);
            }

            [HttpPost("addItemTocart/{Id}")]
            public async Task<IActionResult> AddItemToCart(int Id, AddToCartDto addToCartDto){

                var shoppingCart = await _context.Shopping_Carts
                .Include(sc => sc.Cart_Items)
                .FirstOrDefaultAsync(sc => sc.User.Id == Id);

            if (shoppingCart == null)
            {
                return NotFound("Shopping cart not found for the specified user.");
            }

            // Retrieve the product based on the provided ProductId
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == addToCartDto.ProductId);

            if (product == null)
            {
                return NotFound($"Product with ID {addToCartDto.ProductId} not found.");
            }

            // Create a new Cart_Item with the provided Product and Quantity
            var cartItem = new Cart_Item
            {
                Product = product,
                Quantity = addToCartDto.Quantity
            };

            // Add the new Cart_Item to the shopping cart
            shoppingCart.Cart_Items.Add(cartItem);

            await _context.SaveChangesAsync();

            // Return a response with the updated shopping cart DTO
            /*
            var updatedCartDto = new ShoppingCartDto
            {
                Id = shoppingCart.Id,
                UserId = shoppingCart.User.Id,
                CartItems = shoppingCart.Cart_Items.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    Quantity = ci.Quantity,
                    Product = new ProductDto{
                            Id = cartItem.Product.Id,
                            Name = cartItem.Product.Name,
                            Price = cartItem.Product.Price
                    }
                }).ToList() 
            };
                */
            return Ok("New item had been added in shoppng cart");
        }
    }
}