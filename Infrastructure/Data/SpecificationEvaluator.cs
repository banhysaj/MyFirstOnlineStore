
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity: BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> spec){

            var query = inputQuery;

            if(spec.Criteria != null){
                query = query.Where(spec.Criteria); 
            }
            
            query = spec.Includes.Aggregate(query, (current, include)=>current.Include(include));

            return query;
        }
    }
}

/*We have done all of this because when we use generics we are unable to use a method 
we had used before...

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .FirstOrDefaultAsync(p => p.Id == id);
        }
This is the method we couldnt use when we use generics. 
*/
