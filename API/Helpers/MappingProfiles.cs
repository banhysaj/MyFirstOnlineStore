
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
            CreateMap<ShoppingCart, ShoppingCartDto>()
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
            .ForMember(d => d.CartItems, o=> o.MapFrom(s => s.Cart_Items.Select(ci => new CartItemDto {Id = ci.Id} )));
            }
            

    //
    }
}