using LifeQuest.Api.Processors;
using LifeQuest.Api.Models.API;
using LifeQuest.Api.Storage;
using Moq;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Tests.Processors
{
    public class RewardProcessorTests
    {
        private readonly Mock<IRewardStorage> _mockStorage;
        private readonly Mock<IAccountProcessor> _mockAccountProcessor;
        private readonly RewardProcessor _rewardProcessor;

        public RewardProcessorTests()
        {
            _mockStorage = new Mock<IRewardStorage>();
            _mockAccountProcessor = new Mock<IAccountProcessor>();
            _rewardProcessor = new RewardProcessor(_mockStorage.Object, _mockAccountProcessor.Object);
        }

        [Fact]
        public async Task GetRewardsAsync_ReturnsRewards()
        {
            // Arrange
            var rewards = new List<RewardApiModel> { new RewardApiModel { Id = "1", Name = "Reward", Description="Description", Redeemed=false, Value=100 } };
            _mockStorage.Setup(s => s.GetRewardsAsync()).ReturnsAsync(rewards.Select(r => r.ToStorageModel()));

            // Act
            var result = await _rewardProcessor.GetRewardsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Reward", result.First().Name);
        }

        [Fact]
        public async Task AddRewardAsync_CallsStorageAdd()
        {
            // Arrange
            var reward = new RewardApiModel { Id = "1", Name = "Reward", Description = "Description", Redeemed = false, Value = 100 };

            // Act
            await _rewardProcessor.AddRewardAsync(reward);

            // Assert
            _mockStorage.Verify(s => s.AddRewardAsync(It.IsAny<RewardStorageModel>()), Times.Once);
        }

        [Fact]
        public async Task RedeemRewardAsync_UpdatesRewardAndSpendsPoints()
        {
            // Arrange
            var reward = new RewardStorageModel { Id = "1", Name = "Reward", Description = "Description", Redeemed = false, Value = 100 };
            _mockStorage.Setup(s => s.GetRewardByIdAsync("1")).ReturnsAsync(reward);

            // Act
            var result = await _rewardProcessor.RedeemRewardAsync("user", "1");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Redeemed);
            _mockStorage.Verify(s => s.UpdateRewardAsync(It.IsAny<RewardStorageModel>()), Times.Once);
            _mockAccountProcessor.Verify(a => a.SpendPointsAsync("user", 100), Times.Once);
        }

        [Fact]
        public async Task DeleteRewardAsync_CallsStorageDelete()
        {
            // Act
            await _rewardProcessor.DeleteRewardAsync("1");

            // Assert
            _mockStorage.Verify(s => s.DeleteRewardAsync("1"), Times.Once);
        }
    }
}