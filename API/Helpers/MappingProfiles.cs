
using Core.Entities;
using API.DTO;
using AutoMapper;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
            .ForMember(d =>d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d =>d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            //
            CreateMap<ProductToAddDto, Product>()
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => Enum.Parse(typeof(ProductType), src.ProductType)));
        // Other mappings...
            
            }

    //
    }
}