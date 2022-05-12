using Inventory.ApplicationServices.Products;
using Inventory.Products.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsAppService _productsAppService;
        private readonly ILogger _logger;

        public ProductsController(IProductsAppService productsAppService, ILogger<ProductsController> logger)
        {
            _productsAppService = productsAppService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/<Products>
        [HttpGet]
        public async Task<IEnumerable<ProductCategoryBrandDto>> Get()
        {
            return await _productsAppService.GetAllAsync();
        }

        // GET api/<Products>/5
        [HttpGet("{id}")]
        public async Task<ProductCategoryBrandDto> Get(int id)
        {
            return await _productsAppService.GetAsync(id);
        }

        // POST api/<Products>
        [HttpPost]
        public async Task Post([FromBody] ProductDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _productsAppService.AddAsync(entity);
        }

        // PUT api/<Products>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] ProductDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Id = id;

            await _productsAppService.EditAsync(entity);
        }

        // DELETE api/<Products>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _productsAppService.DeleteAsync(id);
        }
    }
}
