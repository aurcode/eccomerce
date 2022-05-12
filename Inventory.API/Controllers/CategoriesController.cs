using Inventory.ApplicationServices.Categories;
using Inventory.Categories.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesAppService _categoriesAppService;
        private readonly ILogger _logger;

        public CategoriesController(ICategoriesAppService categoriesAppService, ILogger<CategoriesController> logger)
        {
            _categoriesAppService = categoriesAppService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/<Categories>
        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> Get()
        {
            return await _categoriesAppService.GetAllAsync();
        }

        // GET api/<Categories>/5
        [HttpGet("{id}")]
        public async Task<CategoryDto> Get(int id)
        {
            return await _categoriesAppService.GetAsync(id);
        }

        // POST api/<Categories>
        [HttpPost]
        public async Task Post([FromBody] CategoryDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _categoriesAppService.AddAsync(entity);
        }

        // PUT api/<Categories>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] CategoryDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Id = id;

            await _categoriesAppService.EditAsync(entity);
        }

        // DELETE api/<Categories>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _categoriesAppService.DeleteAsync(id);
        }
    }
}
