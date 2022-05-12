using Inventory.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ApplicationServices.Products
{
    public interface IProductsAppService
    {
        Task<List<ProductCategoryBrandDto>> GetAllAsync();
        Task<int> AddAsync(ProductDto product);
        Task DeleteAsync(int productId);
        Task<ProductCategoryBrandDto> GetAsync(int productId);
        Task EditAsync(ProductDto product);
    }
}
