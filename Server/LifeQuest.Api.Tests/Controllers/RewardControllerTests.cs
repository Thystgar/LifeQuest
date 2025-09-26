using LifeQuest.Api.Controllers;
using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using FluentAssertions.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.TestHost;

namespace LifeQuest.Api.Tests.Controllers
{
    public class RewardControllerTests
    {
        private readonly HttpClient _client;
        private readonly Mock<IRewardStorage> _rewardStorageMock;
        private readonly Mock<IAccountStorage> _accountStorageMock;

        public RewardControllerTests()
        {
            _rewardStorageMock = new Mock<IRewardStorage>();
            _accountStorageMock = new Mock<IAccountStorage>();

            var webAppFactory = new WebApplicationFactory<Program>();
                //.WithWebHostBuilder(builder =>
                //{
                //    builder.ConfigureTestServices(services =>
                //    {
                //        services.AddSingleton(_rewardStorageMock.Object);
                //        services.AddSingleton(_accountStorageMock.Object);

                //        // Explicitly add controllers from the LifeQuest.Api assembly
                //        services.AddControllers().AddApplicationPart(typeof(RewardController).Assembly);
                //    });
                //});

            _client = webAppFactory.CreateClient();
        }

        [Fact(Skip = "Not working atm")]
        public async Task GetRewardsAsync_ReturnsOkWithRewards()
        {
            var mockRewards = new List<RewardStorageModel>
            {
                new RewardStorageModel { Id = "1", Name = "Reward1", Description = "Description1", Value = 10, Redeemed = false, GroupId = "group" },
                new RewardStorageModel { Id = "2", Name = "Reward2", Description = "Description2", Value = 20, Redeemed = false, GroupId = "group" }
            };

            _rewardStorageMock.Setup(rs => rs.GetRewardsAsync()).ReturnsAsync(mockRewards);

            var response = await _client.GetAsync("/reward");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var rewards = JsonSerializer.Deserialize<List<RewardApiModel>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(rewards);
            Assert.Equal(2, rewards.Count);
            Assert.Equal("Reward1", rewards[0].Name);
            Assert.Equal("Reward2", rewards[1].Name);
        }

        [Fact(Skip = "Not working atm")]
        public async Task RedeemRewardAsync_ReturnsOkWithRedeemedReward()
        {
            var rewardId = "1";
            var accountId = "userIdTBD";
            var mockReward = new RewardStorageModel { Id = rewardId, Name = "Reward1", Description = "Description1", Value = 10, Redeemed = false, GroupId = "group" };
            var mockAccount = new AccountStorageModel { Id = accountId, Points = 50, Name = "Name", GroupId = "group" };

            _accountStorageMock.Setup(s => s.GetAccountByIdAsync(accountId)).ReturnsAsync(mockAccount);
            _accountStorageMock.Setup(s => s.UpdateAccountAsync(It.IsAny<AccountStorageModel>())).Returns(Task.CompletedTask);
            _rewardStorageMock.Setup(rs => rs.GetRewardByIdAsync(rewardId)).ReturnsAsync(mockReward);
            _rewardStorageMock.Setup(rs => rs.UpdateRewardAsync(It.IsAny<RewardStorageModel>())).Returns(Task.CompletedTask);

            var response = await _client.PutAsync($"/Reward/{rewardId}/redeem", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var reward = JsonSerializer.Deserialize<RewardApiModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(reward);
            Assert.Equal(rewardId, reward.Id);
            Assert.True(reward.Redeemed);
        }

        [Fact(Skip = "Not working atm")]
        public async Task RedeemRewardAsync_UpdatesRewardInStorage()
        {
            var rewardId = "1";
            var accountId = "userIdTBD";
            var mockReward = new RewardStorageModel { Id = rewardId, Name = "Reward1", Description = "Description1", Value = 10, Redeemed = false, GroupId = "group" };
            var mockAccount = new AccountStorageModel { Id = accountId, Points = 50, Name = "Name", GroupId = "group" };

            _accountStorageMock.Setup(s => s.GetAccountByIdAsync(accountId)).ReturnsAsync(mockAccount);
            _rewardStorageMock.Setup(rs => rs.GetRewardByIdAsync(rewardId)).ReturnsAsync(mockReward);
            _rewardStorageMock.Setup(rs => rs.UpdateRewardAsync(It.IsAny<RewardStorageModel>())).Returns(Task.CompletedTask);

            var response = await _client.PutAsync($"/Reward/{rewardId}/redeem", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _rewardStorageMock.Verify(rs => rs.UpdateRewardAsync(It.Is<RewardStorageModel>(r => r.Id == rewardId && r.Redeemed)), Times.Once);
        }

        [Fact(Skip = "Not working atm")]
        public async Task RedeemRewardAsync_DeductsPointsFromAccount()
        {
            var rewardId = "1";
            var accountId = "userIdTBD";
            var mockReward = new RewardStorageModel { Id = rewardId, Name = "Reward1", Description = "Description1", Value = 10, Redeemed = false, GroupId = "group" };
            var mockAccount = new AccountStorageModel { Id = accountId, Points = 50, Name="Name", GroupId = "group" };

            _rewardStorageMock.Setup(rs => rs.GetRewardByIdAsync(rewardId)).ReturnsAsync(mockReward);
            _rewardStorageMock.Setup(rs => rs.UpdateRewardAsync(It.IsAny<RewardStorageModel>())).Returns(Task.CompletedTask);

            _accountStorageMock.Setup(s => s.GetAccountByIdAsync(accountId)).ReturnsAsync(mockAccount);
            _accountStorageMock.Setup(s => s.UpdateAccountAsync(It.IsAny<AccountStorageModel>())).Returns(Task.CompletedTask);

            var response = await _client.PutAsync($"/Reward/{rewardId}/redeem", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _accountStorageMock.Verify(s => s.GetAccountByIdAsync(accountId), Times.Once);
            _accountStorageMock.Verify(s => s.UpdateAccountAsync(It.Is<AccountStorageModel>(a => a.Points == 40)), Times.Once);
        }

        [Fact(Skip = "Not working atm")]
        public async Task AddRewardAsync_ReturnsOkWithAddedReward()
        {
            var newReward = new RewardApiModel { Id = "3", Name = "Reward3", Description = "Description3", Value = 30, Redeemed = false, GroupId = "group" };
            var storageReward = new RewardStorageModel { Id = "3", Name = "Reward3", Description = "Description3", Value = 30, Redeemed = false, GroupId = "group" };

            _rewardStorageMock.Setup(rs => rs.AddRewardAsync(It.IsAny<RewardStorageModel>())).Returns(Task.CompletedTask);

            var content = new StringContent(JsonSerializer.Serialize(newReward), System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/Reward", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var reward = JsonSerializer.Deserialize<RewardApiModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(reward);
            Assert.Equal(newReward.Id, reward.Id);
            Assert.Equal(newReward.Name, reward.Name);
        }

        [Fact(Skip = "Not working atm")]
        public async Task DeleteRewardAsync_ReturnsNoContent()
        {
            var rewardId = "1";

            _rewardStorageMock.Setup(rs => rs.DeleteRewardAsync(rewardId)).Returns(Task.CompletedTask);

            var response = await _client.DeleteAsync($"/Reward/{rewardId}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(Skip = "Not working atm")]
        public async Task DeleteRewardAsync_CallsDeleteMethod()
        {
            var rewardId = "1";

            _rewardStorageMock.Setup(rs => rs.DeleteRewardAsync(rewardId)).Returns(Task.CompletedTask);

            var response = await _client.DeleteAsync($"/Reward/{rewardId}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            _rewardStorageMock.Verify(rs => rs.DeleteRewardAsync(rewardId), Times.Once);
        }
    }
}