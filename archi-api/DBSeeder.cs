using Archi.Entities;

namespace Archi;
public class DBSeeder
{
    private readonly ArchiDbContext _dbContext;

    public DBSeeder(ArchiDbContext dbContext)
    {

        _dbContext = dbContext;
    }
    public async Task SeedAsync()
    {
        if (await _dbContext.Database.CanConnectAsync())
        {
            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                await _dbContext.Roles.AddRangeAsync(roles);
                await _dbContext.SaveChangesAsync();
            }
            if (!_dbContext.Orders.Any())
            {
                var orders = GetOrders();
                await _dbContext.Orders.AddRangeAsync(orders);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>()
        {
            new Role
            {
                Name = "SimpleUser",
              
            },
            new Role
            {
                Name = "Admin",
            }

         };
        return roles;
    }
    private IEnumerable<Order> GetOrders()
    {
        var phones = new List<Order>()
        {
            new Order()
            {
                Name = "Test - 1",
            },
            new Order()
            {
                Name = "Test - 2",
            }

         };
        return phones;
    }
}