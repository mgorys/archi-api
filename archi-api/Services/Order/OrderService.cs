using Archi.Entities;
using Archi.Exceptions;
using Archi.Models.OrderModel;
using Archi.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Archi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto dto)
        {
            return (await _orderRepository.CreateOrderAsync(dto));
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task UpdateOrderAsync(int id, UpdateOrderDto dto)
        {
            await _orderRepository.UpdateOrderAsync(id,dto);
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
    }
}
