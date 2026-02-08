using Archi.Entities;
using Archi.Services.Account;
using Archi;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Archi.Tests.Repositories
{
    public class AccountRepositoryTests
    {
        private readonly ArchiDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authSettings;
        private readonly AccountRepository _repository;

        private readonly List<Role> _availableRoles;

        public AccountRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ArchiDbContext>()
                .UseInMemoryDatabase($"TestDB_Account_{Guid.NewGuid()}")
                .Options;

            _dbContext = new ArchiDbContext(options);

            _availableRoles = new List<Role>()
            {
                new Role { Id = 1, Name = "SimpleUser" },
                new Role { Id = 2, Name = "Admin" }
            };

            _dbContext.Roles.AddRange(_availableRoles);
            _dbContext.SaveChanges();

            _passwordHasher = new PasswordHasher<User>();
            _authSettings = new AuthenticationSettings
            {
                JwtKey = "SuperDługiTestowyKlucz1234567890!",
                JwtExpireDays = 5,
                JwtIssuer = "http://localhost"
            };

            _repository = new AccountRepository(_dbContext, _passwordHasher, _authSettings);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldAddUserToDb()
        {
            var user = new User
            {
                Email = "newuser@test.com",
                FirstName = "Jane",
                LastName = "Doe",
                Role = _availableRoles.First(x=>x.Id == 1)
            };

            await _repository.RegisterUserAsync(user);

            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == "newuser@test.com");
            Assert.NotNull(dbUser);
            Assert.Equal("Jane", dbUser.FirstName);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUser_WhenExists()
        {
            var user = new User
            {
                Email = "exist@test.com",
                FirstName = "John",
                LastName = "Doe",
                Role = _availableRoles.First(x => x.Id == 1)
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetUserAsync(new User { Email = "exist@test.com" });

            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
        }
    }
}
