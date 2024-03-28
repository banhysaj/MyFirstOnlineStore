using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
               
        }
        public DbSet<Product> Products{ get; set; }
        
        //(Once the migration is done and tables are created, Product will have FK to P.Brand and P.Type)
        public DbSet<ProductBrand> ProductBrands {get;set;}
        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<User> Users {get; set;}
        public DbSet<Cart_Item> Cart_Items { get; set; }
        public DbSet<ShoppingCart> Shopping_Carts {get; set;}
        public DbSet<Order> Orders {get; set;}
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Order>()
            .Property(o => o.OrderDate)
            .HasColumnType("TEXT");

            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach(var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach(var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }

        public void AddUser(User user){
            Users.Add(user);
            SaveChangesAsync();
        }

        public void AddCart(ShoppingCart cart){
            Shopping_Carts.Add(cart);
            SaveChangesAsync();
        }

        public User GetUserById(int id){
                return Users.FirstOrDefault(u => u.Id == id);
        }

        public ShoppingCart GetCartById(int id){
            return Shopping_Carts.FirstOrDefault(s => s.Id == id);
        }

        public List<User> GetAllUsers(){
            return Users.ToList();
        }

        public List<ShoppingCart> GetAllCarts(){
            return Shopping_Carts.ToList();
        }


        public void DeleteUserById(int Id){
            var userToDelete = GetUserById(Id);

            if (userToDelete != null)
            {
                
                Users.Remove(userToDelete);
                
                SaveChanges();
            }
        }

        public void UpdateUser(User updatedUser)
        {
            var existingUser = Users.Find(updatedUser.Id);
            if (existingUser != null)
            {
                Entry(existingUser).CurrentValues.SetValues(updatedUser);
                SaveChanges();
            }
        }


    }
}