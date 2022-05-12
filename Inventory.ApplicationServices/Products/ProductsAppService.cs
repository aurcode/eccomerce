using AutoMapper;
using Inventory.Core.Brands;
using Inventory.Core.Categories;
using Inventory.Core.Products;
using Inventory.DataAccess.Repositories;
using Inventory.Products.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ApplicationServices.Products
{
    public class ProductsAppService : IProductsAppService
    {
        private readonly IRepository<int, Brand> _repositoryBrand;
        private readonly IRepository<int, Category> _repositoryCategory;        
        private readonly IRepository<int, Product> _repository;        
        private readonly IMapper _mapper;

        public ProductsAppService(IRepository<int, Product> repository,
                                  IRepository<int, Brand> repositoryBrand,
                                  IRepository<int, Category> repositoryCategory,
                                  IMapper mapper)
        {
            _repositoryBrand = repositoryBrand;
            _repositoryCategory = repositoryCategory;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(ProductDto product)
        {
            var brand = await _repositoryBrand.GetAsync(product.BrandId);
            if (brand == null)
                throw new Exception("Brand not found");
            
            var category = await _repositoryCategory.GetAsync(product.CategoryId);
            if (category == null)
                throw new Exception("Category not found");

            Product _mappedProduct = _mapper.Map<Product>(product);
            _mappedProduct.Brand = brand;
            _mappedProduct.Category = category;
            
            await _repository.AddAsync(_mappedProduct);
            return _mappedProduct.Id;
        }

        public async Task DeleteAsync(int productId)
        {
            await _repository.DeleteAsync(productId);
        }

        public async Task EditAsync(ProductDto product)
        {
            Product _mappedProduct = _mapper.Map<Product>(product);
            _mappedProduct.Brand = await _repositoryBrand.GetAsync(_mappedProduct.Brand.Id);
            _mappedProduct.Category = await _repositoryCategory.GetAsync(_mappedProduct.Category.Id);            
            await _repository.UpdateAsync(_mappedProduct);
        }

        public async Task<ProductCategoryBrandDto> GetAsync(int productId)
        {
            var product = await _repository.GetAsync(productId);
            ProductCategoryBrandDto _mappedDto = _mapper.Map<ProductCategoryBrandDto>(product);
            return _mappedDto;
        }

        public async Task<List<ProductCategoryBrandDto>> GetAllAsync()
        {
            var products = await _repository.GetAll().ToListAsync();
            List<ProductCategoryBrandDto> _mappedDtos = _mapper.Map<List<ProductCategoryBrandDto>>(products);
            return _mappedDtos;
        }
    }
}
