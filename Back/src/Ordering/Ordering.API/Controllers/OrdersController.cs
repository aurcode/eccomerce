using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Ordering.ApplicationServices.Orders;
using Ordering.Orders.Dto;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersAppService _ordersAppService;
        private readonly ILogger _logger;
        private IHttpContextAccessor _httpContextAccessor;
        
        public OrdersController(IOrdersAppService ordersAppService, IHttpContextAccessor httpContextAccessor, ILogger<OrdersController> logger)
        {
            _ordersAppService = ordersAppService;
            _httpContextAccessor = httpContextAccessor;            
        }

        [HttpGet]
        public async Task<IEnumerable<OrderDto>> Get()
        {
            return await _ordersAppService.GetAllAsync();
        }

        [HttpPost]
        public async Task Post([FromBody] OrderDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];


            await _ordersAppService.AddAsync(entity, accessToken);
        }
    }
}
