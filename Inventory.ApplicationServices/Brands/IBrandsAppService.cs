using Inventory.Brands.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ApplicationServices.Brands
{
    public interface IBrandsAppService
    {
        Task<List<BrandDto>> GetAllAsync();
        Task<int> AddAsync(BrandDto brand);
        Task DeleteAsync(int brandId);
        Task<BrandDto> GetAsync(int brandId);
        Task EditAsync(BrandDto brand);
    }
}
