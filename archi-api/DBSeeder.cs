using Archi.Entities;

namespace Archi;
public class DBSeeder
{
    private readonly ArchiDbContext _dbContext;

    public DBSeeder(ArchiDbContext dbContext)
    {

        _dbContext = dbContext;
    }
    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
        {
            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
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
}