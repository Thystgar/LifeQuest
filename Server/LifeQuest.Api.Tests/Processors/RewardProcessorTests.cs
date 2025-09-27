using LifeQuest.Api.Processors;
using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;
using Moq;

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
            var rewards = new List<RewardApiModel> { new RewardApiModel { Id = "1", Name = "Reward", Description="Description", Redeemed=false, Value=100, GroupId = "group" } };
            _mockStorage.Setup(s => s.GetRewardsAsync()).ReturnsAsync(rewards.Select(r => r.ToStorageModel()));

            var result = await _rewardProcessor.GetRewardsAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Reward", result.First().Name);
        }

        [Fact]
        public async Task AddRewardAsync_CallsStorageAdd()
        {
            var reward = new RewardApiModel { Id = "1", Name = "Reward", Description = "Description", Redeemed = false, Value = 100, GroupId = "group" };

            await _rewardProcessor.AddRewardAsync(reward);

            _mockStorage.Verify(s => s.AddRewardAsync(It.IsAny<RewardStorageModel>()), Times.Once);
        }

        [Fact]
        public async Task RedeemRewardAsync_UpdatesRewardAndSpendsPoints()
        {
            var reward = new RewardStorageModel { Id = "1", Name = "Reward", Description = "Description", Redeemed = false, Value = 100, GroupId = "group" };
            _mockStorage.Setup(s => s.GetRewardByIdAsync("1")).ReturnsAsync(reward);

            var result = await _rewardProcessor.RedeemRewardAsync("user", "1");

            Assert.NotNull(result);
            Assert.True(result.Redeemed);
            _mockStorage.Verify(s => s.UpdateRewardAsync(It.IsAny<RewardStorageModel>()), Times.Once);
            _mockAccountProcessor.Verify(a => a.SpendPointsAsync(100), Times.Once);
        }

        [Fact]
        public async Task DeleteRewardAsync_CallsStorageDelete()
        {
            await _rewardProcessor.DeleteRewardAsync("1");

            _mockStorage.Verify(s => s.DeleteRewardAsync("1"), Times.Once);
        }
    }
}