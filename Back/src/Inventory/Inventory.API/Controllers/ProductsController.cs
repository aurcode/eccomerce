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

            ProductDto dto = new ProductDto() {
                Name = "Wilson Men's Ace Red Sports Shoes",
                Description = "Wilson sports shoe, Rubber sole, American size",
                BrandId = 1,
                CategoryId = 1,
                ImageURL = "https://http2.mlstatic.com/D_NQ_NP_847793-MLV49398641869_032022-O.webp",
                Price = 30,
                Qty = 100
            };
            _productsAppService.AddAsync(dto);

            ProductDto dto2 = new ProductDto()
            {
                Name = "Table Tennis Shirt Ping Pong Butterfly",
                Description = "Shirts size: XL. size small.",
                BrandId = 2,
                CategoryId = 2,
                ImageURL = "https://http2.mlstatic.com/D_NQ_NP_774062-MLV48676399848_122021-O.webp",
                Price = 35,
                Qty = 20
            };
            _productsAppService.AddAsync(dto2);
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
        public async Task<IActionResult> Put(int id, [FromBody] ProductDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Id = id;

            return await _productsAppService.EditAsync(entity);
        }

        // DELETE api/<Products>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _productsAppService.DeleteAsync(id);
        }
        
        [HttpGet("{id}/{qty}")]
        public async Task<IActionResult> order(int id, int qty)
        {
            return await _productsAppService.createOrderAsync(id, qty);
        }
    }
}
