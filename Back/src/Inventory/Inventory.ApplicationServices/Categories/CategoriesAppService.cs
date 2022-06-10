using AutoMapper;
using Inventory.Categories.Dto;
using Inventory.Core.Categories;
using Inventory.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ApplicationServices.Categories
{
    public class CategoriesAppService : ICategoriesAppService
    {
        private readonly IRepository<int, Category> _repository;
        private readonly IMapper _mapper;

        public CategoriesAppService(IRepository<int, Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(CategoryDto category)
        {
            Category _mappedCategory = _mapper.Map<Category>(category);
            await _repository.AddAsync(_mappedCategory);
            return _mappedCategory.Id;
        }

        public async Task DeleteAsync(int categoryId)
        {
            await _repository.DeleteAsync(categoryId);
        }

        public async Task EditAsync(CategoryDto category)
        {
            Category _mappedCategory = _mapper.Map<Category>(category);
            await _repository.UpdateAsync(_mappedCategory);
        }

        public async Task<CategoryDto> GetAsync(int categoryId)
        {
            var category = await _repository.GetAsync(categoryId);
            CategoryDto _mappedDto = _mapper.Map<CategoryDto>(category);
            return _mappedDto;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAll().ToListAsync();
            List<CategoryDto> _mappedDtos = _mapper.Map<List<CategoryDto>>(categories);
            return _mappedDtos;
        }
    }
}
