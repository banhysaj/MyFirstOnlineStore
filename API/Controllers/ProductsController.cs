
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]

    //Whenever we will want to reference a controller we will use this syntax
    [Route("api/[controller]")]

    //All our controllers will inherit from ControllerBase
    public class ProductsController: ControllerBase
    {

        //Here we make queries to DB
        //THIS IS DEPENDENCY INJECTION
        //When request is finished both Store context and the private _context are disposed
        //No need to worry for memory management
        private readonly IProductRepository _repo;
        
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }



        [HttpGet]

        //By wrapping our method with "Task" we make the code Async
        //Helps us make use of a thread, by creating tasks within threads
        //Helps us handle many concurrent requests
        public async Task<ActionResult<List<Product>>> GetProducts(){
            var products = await _repo.GetProductsAsync();

            //Ok is used here to send a http 200 response back, this is a positive response
            //Afterwards the products are returned
            return Ok(products);

        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            return await _repo.GetProductByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            return Ok(await _repo.GetProductBrandsAsync());
        }
        
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){
            return Ok(await _repo.GetProductTypesAsync());
        }
    }
}