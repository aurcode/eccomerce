using Inventory.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ApplicationServices.Products
{
    public interface IProductsAppService
    {
        Task<IActionResult> createOrderAsync(int productId, int qty);
        Task<List<ProductCategoryBrandDto>> GetAllAsync();
        Task<int> AddAsync(ProductDto product);
        Task<IActionResult> DeleteAsync(int productId);
        Task<ProductCategoryBrandDto> GetAsync(int productId);
        Task<IActionResult> EditAsync(ProductDto product);
    }
}
