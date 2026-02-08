using Archi.Entities;
using Archi.Exceptions;
using Archi.Models.OrderModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Archi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ArchiDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderRepository(ArchiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = _mapper.Map<Order>(dto);

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order.Id;
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _dbContext
                 .Orders
                 .FirstOrDefaultAsync(r => r.Id == id);
            if (order is null)
                throw new NotFoundException("Order not found");
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order is null)
                throw new NotFoundException("Order not found");
            var result = _mapper.Map<OrderDto>(order);
            return result;
        }

        public async Task UpdateOrderAsync(int id, UpdateOrderDto dto)
        {
            var order = await _dbContext
                  .Orders
                  .FirstOrDefaultAsync(r => r.Id == id);
            if (order is null)
                throw new NotFoundException("Order not found");
            order.Name = dto.Name;
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var result = await _dbContext.Orders.ToListAsync();
            return result;
        }
    }
}
