using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ordering.ApplicationServices.Products;
using Ordering.ApplicationServices.Users;
using Ordering.Core.Orders;
using Ordering.DataAccess.Repositories;
using Ordering.Orders.Dto;
using Ordering.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.ApplicationServices.Orders
{
    public class OrdersAppService : IOrdersAppService
    {
        private readonly IUsersAppService _usersAppService;
        private readonly IProductsAppService _productsAppService;
        private readonly IRepository<int, Order> _repository;
        private readonly IMapper _mapper;

        public OrdersAppService(IUsersAppService usersAppService,
                                IProductsAppService productsAppService,
                                IRepository<int, Order> repository,
                                IMapper mapper)
        {
            _usersAppService = usersAppService;
            _productsAppService = productsAppService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(OrderDto order, string token)
        {
            GetUserResponseDto user = await _usersAppService.FindUserByTokenAsync(token);
            order.clientId = user.value.claims.id;
            //var g = responseUser.Content.ReadAsStringAsync().Result; ;


            HttpResponseMessage response = await _productsAppService.createOrderAsync(order.ProductId, order.Qty);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Order error");
            }

            Order _mappedOrder = _mapper.Map<Order>(order);
            //_mappedOrder.clientId = response

            await _repository.AddAsync(_mappedOrder);
            return _mappedOrder.Id;

        }

        public Task<IActionResult> DeleteAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> EditAsync(OrderDto order)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _repository.GetAll().ToListAsync();
            List<OrderDto> _mappedDtos = _mapper.Map<List<OrderDto>>(orders);
            return _mappedDtos;
        }

        public async Task<OrderDto> GetAsync(int orderId)
        {
            var order = await _repository.GetAsync(orderId);
            OrderDto _mappedDto = _mapper.Map<OrderDto>(order);
            return _mappedDto;
        }
    }
}
