namespace Core.Entities
{
    public class Product: BaseEntity
    {
        public string Name {get; set;}
        public string Description{get; set;}
        public decimal Price{get;set;}
        public string PictureUrl { get; set; }

        //This is called a Related Entity
        //This will help EntityFramework to determine the relationships
        //It will automatically create and set the ForeignKeys
        //Its like creating a diagram and showing what is connected and whats not(more or less)
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }

        //This is called a Related Entity
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
    }
}