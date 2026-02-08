using Archi.Entities;
using Archi.Exceptions;
using Archi.Models.OrderModel;
using Archi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Phones.Services
{
    public class OrderService : IOrderService
    {
        private readonly ArchiDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(ArchiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int CreateOrder(CreateOrderDto dto)
        {
            var order = _mapper.Map<Order>(dto);

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return order.Id;
        }

        public void DeleteOrder(int id)
        {
            var order = _dbContext
                 .Orders
                 .FirstOrDefault(r => r.Id == id);
            if (order is null)
                throw new NotFoundException("Order not found");
            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
        }

        public OrderDto GetById(int id)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == id);
            if (order is null)
                throw new NotFoundException("Order not found");
            var result = _mapper.Map<OrderDto>(order);
            return result;
        }

        public void UpdateOrder(int id, UpdateOrderDto dto)
        {
            var order = _dbContext
                  .Orders
                  .FirstOrDefault(r => r.Id == id);
            if (order is null)
                throw new NotFoundException("Order not found");
            order.Name = dto.Name;
            
            _dbContext.SaveChanges();
        }

        ActionResult<IEnumerable<Order>> IOrderService.GetAll()
        {
            var result = _dbContext.Orders.ToList();
            return result;
        }
    }
}
