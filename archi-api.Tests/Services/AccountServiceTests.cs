using Archi.Entities;
using Archi.Exceptions;
using Archi.Models;
using Archi.Services.Account;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archi.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly Mock<IPasswordHasher<User>> _mockHasher;
        private readonly AccountService _service;
        private readonly AuthenticationSettings _authSettings;

        public AccountServiceTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _mockHasher = new Mock<IPasswordHasher<User>>();
            _authSettings = new AuthenticationSettings
            {
                JwtKey = "SuperDługiTestowyKlucz1234567890!",
                JwtExpireDays = 5,
                JwtIssuer = "http://localhost"
            };

            _service = new AccountService(_mockHasher.Object, _authSettings, _mockRepo.Object);
        }

        [Fact]
        public async Task GenerateJwtAsync_ShouldReturnToken_WhenUserIsValid()
        {
            var dto = new LoginDto { Email = "test@test.com", Password = "password" };
            var user = new User
            {
                Id = 1,
                Email = "test@test.com",
                PasswordHash = "hashedPassword",
                FirstName = "John",
                LastName = "Doe",
                Role = new Role { Name = "Admin" }
            };

            _mockRepo.Setup(r => r.GetUserAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mockHasher.Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, dto.Password))
                       .Returns(PasswordVerificationResult.Success);

            var token = await _service.GenerateJwtAsync(dto);

            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task GenerateJwtAsync_ShouldThrow_WhenUserNotFound()
        {
            var dto = new LoginDto { Email = "noone@test.com", Password = "password" };
            _mockRepo.Setup(r => r.GetUserAsync(It.IsAny<User>())).ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<BadRequestException>(() => _service.GenerateJwtAsync(dto));
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldCallRepository()
        {
            var dto = new RegisterUserDto
            {
                Email = "new@test.com",
                Password = "password",
                FirstName = "Jane",
                LastName = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-20),
                RoleId = 1
            };

            _mockHasher.Setup(h => h.HashPassword(It.IsAny<User>(), dto.Password))
                       .Returns("hashedPassword");

            _mockRepo.Setup(r => r.RegisterUserAsync(It.IsAny<User>()))
                     .Returns(Task.CompletedTask);

            await _service.RegisterUserAsync(dto);

            _mockRepo.Verify(r => r.RegisterUserAsync(It.Is<User>(u => u.Email == dto.Email)), Times.Once);
        }
    }
}
