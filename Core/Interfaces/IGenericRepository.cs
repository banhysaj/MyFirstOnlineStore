
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{

    //Here are using generics, T means we can pass any type of data
    //In our case it can be a Product, ProductBrand or Type
    //The constraint here is that T can only derive from BaseEntity
    public interface IGenericRepository<T> where T: BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();

        //This is where we use the Specification Pattern
        Task<T> GetEntityWithSpec(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        //
        Task<int> SaveChangesAsync();
        void Add(T entity);
        //
    } 
}