using Archi.Models.OrderModel;
using Archi.Services;
using Archi.Repository;
using Moq;

namespace Archi.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockRepo;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockRepo = new Mock<IOrderRepository>();
            _service = new OrderService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCallRepositoryAndReturnId()
        {
            var dto = new CreateOrderDto { Name = "Test Order" };
            _mockRepo.Setup(r => r.CreateOrderAsync(dto)).ReturnsAsync(1);

            var id = await _service.CreateOrderAsync(dto);

            Assert.Equal(1, id);
            _mockRepo.Verify(r => r.CreateOrderAsync(dto), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOrdersFromRepository()
        {
            var orders = new List<Archi.Entities.Order> { new Archi.Entities.Order { Id = 1, Name = "A" } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("A", ((List<Archi.Entities.Order>)result)[0].Name);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldCallRepository()
        {
            int id = 1;
            _mockRepo.Setup(r => r.DeleteOrderAsync(id)).Returns(Task.CompletedTask);

            await _service.DeleteOrderAsync(id);

            _mockRepo.Verify(r => r.DeleteOrderAsync(id), Times.Once);
        }
    }
}