using FluentAssertions;
using Microsoft.Extensions.Logging;
using Warehouse.Core.Domain.Entities;
using Xunit.Abstractions;

namespace Warehouse.IntegrationTests
{
    public class UserTests : IClassFixture<DatabaseFixture>
    {
        private readonly ILogger<UserTests> _logger;

        public UserTests(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
        {
            Fixture = fixture;
            Fixture.Configure(options =>
            {
                var loggerProvider = new XUnitLoggerProvider(testOutputHelper);
                options.LoggerFactory = LoggerFactory.Create(builder => builder.AddProvider(loggerProvider));
                //options.ConnectionString = "Server=192.168.10.11;Port=3306;Database=viot;Uid=admin;Pwd=~1q2w3e4r!;";
            });
            Fixture.Initialize();

            _logger = XUnitLogger.CreateLogger<UserTests>(testOutputHelper);
        }

        public DatabaseFixture Fixture { get; }
        
        [Theory]
        [InlineData("anton_sup")]
        public void CreateUsers(string username)
        {
            var user = new UserEntity($"{username}@vayosoft.com")
            {
                Email = $"{username}@vayosoft.com",
                PasswordHash = "VBbXzW7xlaD3YiqcVrVehA==",
                Phone = "0500000000",
                Registered = DateTime.UtcNow
            };

            using var context = Fixture.CreateContext();
            context.Users.Add(user);
            context.SaveChanges();

            _logger.LogInformation("userId: {UserId}", user.Id);

            user.Id.Should().BeGreaterThan(0);
        }
    }
}