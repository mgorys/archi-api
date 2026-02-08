using Archi.Entities;
using Archi.Models.OrderModel;
using Archi.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archi.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly ArchiDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ArchiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
            _dbContext = new ArchiDbContext(options);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<CreateOrderDto, Order>()
                                                               .ForMember(dest => dest.Id, opt => opt.Ignore())
                                                               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)));
            _mapper = config.CreateMapper();

            _repository = new OrderRepository(_dbContext, _mapper);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldAddOrderToDb()
        {
            var dto = new CreateOrderDto { Name = "New Order" };

            var id = await _repository.CreateOrderAsync(dto);

            var order = await _dbContext.Orders.FindAsync(id);
            Assert.NotNull(order);
            Assert.Equal("New Order", order.Name);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldRemoveOrder()
        {
            var order = new Order { Name = "ToDelete" };
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            await _repository.DeleteOrderAsync(order.Id);

            var deleted = await _dbContext.Orders.FindAsync(order.Id);
            Assert.Null(deleted);
        }
    }
}
