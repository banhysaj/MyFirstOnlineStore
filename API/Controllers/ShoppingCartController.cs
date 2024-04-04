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

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartByUserId(int userId)
        {
            var cart = await _context.Shopping_Carts
                .Include(x => x.Cart_Items)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == userId);

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
                            Price = ci.Product.Price,
                            PictureUrl = ci.Product.PictureUrl
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

        

        [HttpGet("{cartId}")]
        public IActionResult GetCart(int cartId)
        {
            
            var cart = _context.Shopping_Carts
                                .Include(x => x.Cart_Items)
                                .ThenInclude(x => x.Product)
                                .Include(x => x.User)
                                .FirstOrDefault(x => x.Id == cartId);

            
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
                            Price = cartItem.Product.Price,
                            PictureUrl = cartItem.Product.PictureUrl
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
                            Price = cartItem.Product.Price,
                            PictureUrl = cartItem.Product.PictureUrl
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
                            Price = ci.Product.Price,
                            PictureUrl = ci.Product.PictureUrl
                    }
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetCart), new { id = cartDto.Id }, cartDto);
            }

        [HttpPost("addItemToCart/{userId}")]
        public async Task<IActionResult> AddItemToCart(int userId, AddToCartDto addToCartDto)
        {
            var shoppingCart = await _context.Shopping_Carts
                .Include(x => x.Cart_Items)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == userId);

            if (shoppingCart == null)
            {
               
                shoppingCart = new ShoppingCart
                {
                    User = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId),
                    Cart_Items = new List<Cart_Item>()
                };
                _context.Shopping_Carts.Add(shoppingCart);
                await _context.SaveChangesAsync(); 
            }

            
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == addToCartDto.ProductId);

            if (product == null)
            {
                return NotFound($"Product with ID {addToCartDto.ProductId} not found.");
            }

            var existingCartItem = shoppingCart.Cart_Items.FirstOrDefault(ci => ci.Product.Id == addToCartDto.ProductId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                var cartItem = new Cart_Item
                {
                    Product = product,
                    Quantity = 1 
                };
                shoppingCart.Cart_Items.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok("Item has been added to the shopping cart");
        }
        
        [HttpPost("removeItemFromCart/{userId}")]
        public async Task<IActionResult> RemoveItemFromCart(int userId, AddToCartDto addToCartDto)
        {
            var shoppingCart = await _context.Shopping_Carts
                .Include(x => x.Cart_Items)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == userId);

            if (shoppingCart == null)
            {
                
                return NotFound("Shopping cart not found for the specified user.");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == addToCartDto.ProductId);

            if (product == null)
            {
                return NotFound($"Product with ID {addToCartDto.ProductId} not found.");
            }

            var cartItem = shoppingCart.Cart_Items.FirstOrDefault(ci => ci.Product.Id == addToCartDto.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity--;

                if (cartItem.Quantity <= 0)
                {
                    _context.Cart_Items.Remove(cartItem);
                }

                if (shoppingCart.Cart_Items.Count == 0)
                {
                    _context.Shopping_Carts.Remove(shoppingCart);
                }

                await _context.SaveChangesAsync();
                return Ok("Item has been removed from the shopping cart.");
            }
            else
            {
                return NotFound("Product not found in the shopping cart.");
            }
        }


    }
}