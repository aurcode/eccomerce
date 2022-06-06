using AutoMapper;
using Inventory.Core.Brands;
using Inventory.Core.Categories;
using Inventory.Core.Products;
using Inventory.DataAccess.Repositories;
using Inventory.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

        public IActionResult NoContent()
        {
            return new NoContentResult();
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

        public async Task<IActionResult> DeleteAsync(int productId)
        {
            await _repository.DeleteAsync(productId);
            return NoContent();
        }

        public async Task<IActionResult> EditAsync(ProductDto product)
        {
            Product _mappedProduct = _mapper.Map<Product>(product);
            _mappedProduct.Brand = await _repositoryBrand.GetAsync(_mappedProduct.Brand.Id);
            _mappedProduct.Category = await _repositoryCategory.GetAsync(_mappedProduct.Category.Id);            
            await _repository.UpdateAsync(_mappedProduct);
            return NoContent();
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

        public async Task<int> getQtyProductAsync(int productId)
        {
            var product = await _repository.GetAsync(productId);
            ProductCategoryBrandDto _mappedDto = _mapper.Map<ProductCategoryBrandDto>(product);
            return _mappedDto.Qty;
        }

        public async Task<IActionResult> createOrderAsync(int productId, int qty)
        {
            var product = await _repository.GetAsync(productId);
            int qtyProduct = await getQtyProductAsync(productId);
            
            if (product == null)
                throw new Exception("Product not found");
            if (qtyProduct <= 0)
                throw new Exception("Product out of stock");
            if (qty < 0)
                throw new Exception("Bad requests, you need request 1 or more qty");
            if (qty > qtyProduct)
                throw new Exception("Product request is more that our stock, we have" + product.Qty);
            product.Qty = product.Qty - qty;
            await _repository.UpdateAsync(product);
            return NoContent();
        }
    }
}
