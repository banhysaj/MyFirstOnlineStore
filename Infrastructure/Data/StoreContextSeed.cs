

using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context){
            if(!context.ProductBrands.Any()){
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
            }

            if(!context.ProductTypes.Any()){
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }

            if(!context.Products.Any()){
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);
            }
            if(!context.Cart_Items.Any()){
                var CartItemsData = File.ReadAllText("../Infrastructure/Data/SeedData/CartItems.json");
                var CartItems = JsonSerializer.Deserialize<List<Cart_Item>>(CartItemsData);
                context.Cart_Items.AddRange(CartItems);
            }
            if(!context.Shopping_Carts.Any()){
                var ShoppingCartsData = File.ReadAllText("../Infrastructure/Data/SeedData/ShoppingCarts.json");
                var ShoppingCarts = JsonSerializer.Deserialize<List<ShoppingCart>>(ShoppingCartsData);
                context.Shopping_Carts.AddRange(ShoppingCarts);
            }

            if(context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}