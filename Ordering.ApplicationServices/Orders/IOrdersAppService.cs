using Microsoft.AspNetCore.Mvc;
using Ordering.Orders.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.ApplicationServices.Orders
{
    public interface IOrdersAppService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<int> AddAsync(OrderDto order);
        Task<IActionResult> DeleteAsync(int orderId);
        Task<OrderDto> GetAsync(int orderId);
        Task<IActionResult> EditAsync(OrderDto order);
    }
}
