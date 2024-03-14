
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationservicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //In here we have added the DB context since we will need these to make queries to our DB
            services.AddDbContext<StoreContext>(opt => 
            {
                //Used the Connection string we initialized in appsettings.dev...
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });


            //In here we are adding Redis to effectively managing the customer basket (Cart)
            //Redis will make it possible for the cart to be temporarily be saved in the memory of our Server
            
            // services.AddSingleton<IConnectionMultiplexer>(c =>
            // {

            //     var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
            //     return ConnectionMultiplexer.Connect(options);
            // });

            //services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<ApiBehaviorOptions>(options => 
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>

                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                })
            );

            return services;
        }
    }
}