using AutoMapper;
using Inventory.Brands.Dto;
using Inventory.Core.Brands;
using Inventory.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ApplicationServices.Brands
{
    public class BrandsAppService : IBrandsAppService
    {
        private readonly IRepository<int, Brand> _repository;
        private readonly IMapper _mapper;

        public BrandsAppService(IRepository<int, Brand> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(BrandDto brand)
        {
            Brand _mappedBrand = _mapper.Map<Brand>(brand);
            await _repository.AddAsync(_mappedBrand);
            return _mappedBrand.Id;
        }

        public async Task DeleteAsync(int brandId)
        {
            await _repository.DeleteAsync(brandId);
        }

        public async Task EditAsync(BrandDto brand)
        {
            Brand _mappedBrand = _mapper.Map<Brand>(brand);
            await _repository.UpdateAsync(_mappedBrand);
        }

        public async Task<BrandDto> GetAsync(int brandId)
        {
            var brand = await _repository.GetAsync(brandId);
            BrandDto _mappedDto = _mapper.Map<BrandDto>(brand);
            return _mappedDto;
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brands = await _repository.GetAll().ToListAsync();
            List<BrandDto> _mappedDtos = _mapper.Map<List<BrandDto>>(brands);
            return _mappedDtos;
        }
    }
}
