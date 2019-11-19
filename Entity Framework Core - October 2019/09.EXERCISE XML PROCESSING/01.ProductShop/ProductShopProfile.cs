using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<ImportUserDto, User>();

            CreateMap<ImportProductDto, Product>();

            CreateMap<ImportCategoryDto, Category>();

            CreateMap<ImportCategoryProductDto, CategoryProduct>();

            CreateMap<Product, ProductsInRangeDto>()
                .ForMember(p => p.Buyer, x => x.MapFrom(y => $"{y.Buyer.FirstName} {y.Buyer.LastName}"));

            //CreateMap<Category, ExportAllCategoriesDto>()
            //    .ForMember(p => p.Count,
            //    opt => opt.MapFrom(src => src.CategoryProducts.Count))
            //    .ForMember(p => p.AveragePrice,
            //    opt => opt.MapFrom(src => src.CategoryProducts.Average(cp => cp.Product.Price)))
            //    .ForMember(p => p.TotalRevenue,
            //    opt => opt.MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));
        }
    }
}
