using LifeQuest.Api.Processors;
using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Storage;
using Moq;

namespace LifeQuest.Api.Tests.Processors
{
    public class AccountProcessorTests
    {
        private readonly Mock<IAccountStorage> _mockStorage;
        private readonly AccountProcessor _accountProcessor;

        public AccountProcessorTests()
        {
            _mockStorage = new Mock<IAccountStorage>();
            _accountProcessor = new AccountProcessor(_mockStorage.Object);
        }

        [Fact]
        public async Task GetAccountAsync_ReturnsAccount()
        {
            var account = new AccountApiModel { Id = "1", Name = "Test Account", Points = 100, GroupId = "group" };
            _mockStorage.Setup(s => s.GetAccountByIdAsync("1")).ReturnsAsync(account.ToStorageModel());

            var result = await _accountProcessor.GetAccountAsync("1");

            Assert.NotNull(result);
            Assert.Equal("Test Account", result.Name);
            Assert.Equal(100, result.Points);
        }
    }
}