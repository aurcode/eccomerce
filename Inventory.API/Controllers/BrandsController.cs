using Inventory.ApplicationServices.Brands;
using Inventory.Brands.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.API.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandsAppService _brandsAppService;
        private readonly ILogger _logger;

        public BrandsController(IBrandsAppService brandsAppService, ILogger<BrandsController> logger)
        {
            _brandsAppService = brandsAppService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/<Brands>
        [HttpGet]
        public async Task<IEnumerable<BrandDto>> Get()
        {
            return await _brandsAppService.GetAllAsync();
        }

        // GET api/<Brands>/5
        [HttpGet("{id}")]
        public async Task<BrandDto> Get(int id)
        {
            return await _brandsAppService.GetAsync(id);
        }

        // POST api/<Brands>
        [HttpPost]
        public async Task Post([FromBody] BrandDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _brandsAppService.AddAsync(entity);
        }

        // PUT api/<Brands>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] BrandDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Id = id;

            await _brandsAppService.EditAsync(entity);
        }

        // DELETE api/<Brands>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _brandsAppService.DeleteAsync(id);
        }
    }
}
