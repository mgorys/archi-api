using Archi.Entities;
using Archi.Models.OrderModel;
using Microsoft.AspNetCore.Mvc;

namespace Archi.Services;

public interface IOrderService
{
    ActionResult<IEnumerable<Order>> GetAll();
    OrderDto GetById(int id);
    public int CreateOrder(CreateOrderDto dto);
    void DeleteOrder(int id);
    void UpdateOrder(int id, UpdateOrderDto dto);
}