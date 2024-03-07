
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{

    //In this class we are making use of the Specification Pattern as well
    //We are specifying that we need the Brand and Type for both scenarios, retrieving all products or just one of them
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification()
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}