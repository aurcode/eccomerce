using AutoMapper;
using Inventory.Brands.Dto;
using Inventory.Categories.Dto;
using Inventory.Core.Brands;
using Inventory.Core.Categories;
using Inventory.Core.Products;
using Inventory.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Brand, BrandDto>();
            CreateMap<BrandDto, Brand>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            //CreateMap<Product, ProductDto>()
            //    .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.Brand.Id))
            //    .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id));
            CreateMap<Product, ProductCategoryBrandDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => new Brand { Id = src.BrandId }))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Id = src.CategoryId }));
                //.ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryId));

            //CreateMap<Owner, Car>().ForMember(dest => dest.OwnerData,
            //    input => input.MapFrom(i => new Owner { Name = i.Name }));

            //CreateMap<UserDto, User>()
            //    .ForMember(
            //        dest => dest.FirstName,
            //        opt => opt.MapFrom(src => $"{src.FirstName}")
            //    )
        }
    }
}
