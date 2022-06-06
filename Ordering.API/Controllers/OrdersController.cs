using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public OrdersController(IOrdersAppService ordersAppService, ILogger<OrdersController> logger)
        {
            _ordersAppService = ordersAppService;
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

            await _ordersAppService.AddAsync(entity);
        }
    }
}
