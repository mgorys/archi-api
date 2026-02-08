using Archi.Entities;
using Archi.Models.OrderModel;
using Microsoft.AspNetCore.Mvc;

namespace Archi.Repository;

public interface IOrderRepository
{
    public Task<IEnumerable<Order>> GetAllAsync();
    public Task<OrderDto> GetByIdAsync(int id);
    public Task<int> CreateOrderAsync(CreateOrderDto dto);
    public Task UpdateOrderAsync(int id, UpdateOrderDto dto);
    public Task DeleteOrderAsync(int id);
}