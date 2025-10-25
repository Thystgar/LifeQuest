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
        private readonly Mock<IGroupStorage> _mockGroupStorage;
        private readonly Mock<IUserContext> _mockUserContext;
        private readonly AccountProcessor _accountProcessor;

        public AccountProcessorTests()
        {
            _mockStorage = new Mock<IAccountStorage>();
            _mockGroupStorage = new Mock<IGroupStorage>();
            _mockUserContext = new Mock<IUserContext>();
            _accountProcessor = new AccountProcessor(_mockStorage.Object, _mockUserContext.Object, _mockGroupStorage.Object);
        }

        [Fact]
        public async Task GetAccountAsync_ReturnsAccount()
        {
            var account = new AccountApiModel { Id = "1", Name = "Test Account", Points = 100, GroupId = "group", TermsAccepted = false };
            _mockStorage.Setup(s => s.GetAccountByIdAsync("1")).ReturnsAsync(account.ToStorageModel());
            _mockUserContext.Setup(uc => uc.GetUserId()).Returns("1");

            var result = await _accountProcessor.GetAccountOrCreateAsync();

            Assert.NotNull(result);
            Assert.Equal("Test Account", result.Name);
            Assert.Equal(100, result.Points);
        }
    }
}