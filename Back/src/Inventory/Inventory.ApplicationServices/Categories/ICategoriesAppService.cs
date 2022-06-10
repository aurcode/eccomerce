using Inventory.Categories.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ApplicationServices.Categories
{
    public interface ICategoriesAppService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<int> AddAsync(CategoryDto category);
        Task DeleteAsync(int categoryId);
        Task<CategoryDto> GetAsync(int categoryId);
        Task EditAsync(CategoryDto category);        
    }
}
