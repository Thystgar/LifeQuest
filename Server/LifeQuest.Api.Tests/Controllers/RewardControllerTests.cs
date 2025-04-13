using Azure.Storage.Blobs.Models;
using LifeQuest.Api.Controllers;
using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

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

            var webAppFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        Program.RegisterServices(services);
                        // Replace the IRewardStorage and IAccountStorage with the mocks
                        services.AddSingleton(_rewardStorageMock.Object);
                        services.AddSingleton(_accountStorageMock.Object);

                        // Explicitly add controllers from the LifeQuest.Api assembly
                        services.AddControllers().AddApplicationPart(typeof(RewardController).Assembly);
                    });

                    builder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });

            _client = webAppFactory.CreateClient();
        }

        [Fact]
        public async Task GetRewardsAsync_ReturnsOkWithRewards()
        {
            // Arrange
            var mockRewards = new List<RewardStorageModel>
            {
                new RewardStorageModel { Id = "1", Name = "Reward1", Description = "Description1", Value = 10, Redeemed = false },
                new RewardStorageModel { Id = "2", Name = "Reward2", Description = "Description2", Value = 20, Redeemed = false }
            };

            _rewardStorageMock.Setup(rs => rs.GetRewardsAsync()).ReturnsAsync(mockRewards);

            // Act
            var response = await _client.GetAsync("/reward");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var rewards = JsonSerializer.Deserialize<List<RewardApiModel>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(rewards);
            Assert.Equal(2, rewards.Count);
            Assert.Equal("Reward1", rewards[0].Name);
            Assert.Equal("Reward2", rewards[1].Name);
        }

        [Fact]
        public async Task RedeemRewardAsync_ReturnsOkWithRedeemedReward()
        {
            // Arrange
            var rewardId = "1";
            var accountId = "userIdTBD";
            var mockReward = new RewardStorageModel { Id = rewardId, Name = "Reward1", Description = "Description1", Value = 10, Redeemed = false };
            var mockAccount = new AccountStorageModel { Id = accountId, Points = 50, Name = "Name" };

            _accountStorageMock.Setup(s => s.GetAccountByIdAsync(accountId)).ReturnsAsync(mockAccount);
            _accountStorageMock.Setup(s => s.UpdateAccountAsync(It.IsAny<AccountStorageModel>())).Returns(Task.CompletedTask);
            _rewardStorageMock.Setup(rs => rs.GetRewardByIdAsync(rewardId)).ReturnsAsync(mockReward);
            _rewardStorageMock.Setup(rs => rs.UpdateRewardAsync(It.IsAny<RewardStorageModel>())).Returns(Task.CompletedTask);

            // Act
            var response = await _client.PutAsync($"/Reward/{rewardId}/redeem", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var reward = JsonSerializer.Deserialize<RewardApiModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(reward);
            Assert.Equal(rewardId, reward.Id);
            Assert.True(reward.Redeemed);
        }

        [Fact]
        public async Task RedeemRewardAsync_UpdatesRewardInStorage()
        {
            // Arrange
            var rewardId = "1";
            var accountId = "userIdTBD";
            var mockReward = new RewardStorageModel { Id = rewardId, Name = "Reward1", Description = "Description1", Value = 10, Redeemed = false };
            var mockAccount = new AccountStorageModel { Id = accountId, Points = 50, Name = "Name" };

            _accountStorageMock.Setup(s => s.GetAccountByIdAsync(accountId)).ReturnsAsync(mockAccount);
            _rewardStorageMock.Setup(rs => rs.GetRewardByIdAsync(rewardId)).ReturnsAsync(mockReward);
            _rewardStorageMock.Setup(rs => rs.UpdateRewardAsync(It.IsAny<RewardStorageModel>())).Returns(Task.CompletedTask);

            // Act
            var response = await _client.PutAsync($"/Reward/{rewardId}/redeem", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _rewardStorageMock.Verify(rs => rs.UpdateRewardAsync(It.Is<RewardStorageModel>(r => r.Id == rewardId && r.Redeemed)), Times.Once);
        }

        [Fact]
        public async Task RedeemRewardAsync_DeductsPointsFromAccount()
        {
            // Arrange
            var rewardId = "1";
            var accountId = "userIdTBD";
            var mockReward = new RewardStorageModel { Id = rewardId, Name = "Reward1", Description = "Description1", Value = 10, Redeemed = false };
            var mockAccount = new AccountStorageModel { Id = accountId, Points = 50, Name="Name" };

            _rewardStorageMock.Setup(rs => rs.GetRewardByIdAsync(rewardId)).ReturnsAsync(mockReward);
            _rewardStorageMock.Setup(rs => rs.UpdateRewardAsync(It.IsAny<RewardStorageModel>())).Returns(Task.CompletedTask);

            _accountStorageMock.Setup(s => s.GetAccountByIdAsync(accountId)).ReturnsAsync(mockAccount);
            _accountStorageMock.Setup(s => s.UpdateAccountAsync(It.IsAny<AccountStorageModel>())).Returns(Task.CompletedTask);

            // Act
            var response = await _client.PutAsync($"/Reward/{rewardId}/redeem", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _accountStorageMock.Verify(s => s.GetAccountByIdAsync(accountId), Times.Once);
            _accountStorageMock.Verify(s => s.UpdateAccountAsync(It.Is<AccountStorageModel>(a => a.Points == 40)), Times.Once);
        }

        [Fact]
        public async Task AddRewardAsync_ReturnsOkWithAddedReward()
        {
            // Arrange
            var newReward = new RewardApiModel { Id = "3", Name = "Reward3", Description = "Description3", Value = 30, Redeemed = false };
            var storageReward = new RewardStorageModel { Id = "3", Name = "Reward3", Description = "Description3", Value = 30, Redeemed = false };

            _rewardStorageMock.Setup(rs => rs.AddRewardAsync(It.IsAny<RewardStorageModel>())).Returns(Task.CompletedTask);

            var content = new StringContent(JsonSerializer.Serialize(newReward), System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Reward", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var reward = JsonSerializer.Deserialize<RewardApiModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(reward);
            Assert.Equal(newReward.Id, reward.Id);
            Assert.Equal(newReward.Name, reward.Name);
        }

        [Fact]
        public async Task DeleteRewardAsync_ReturnsNoContent()
        {
            // Arrange
            var rewardId = "1";

            _rewardStorageMock.Setup(rs => rs.DeleteRewardAsync(rewardId)).Returns(Task.CompletedTask);

            // Act
            var response = await _client.DeleteAsync($"/Reward/{rewardId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRewardAsync_CallsDeleteMethod()
        {
            // Arrange
            var rewardId = "1";

            _rewardStorageMock.Setup(rs => rs.DeleteRewardAsync(rewardId)).Returns(Task.CompletedTask);

            // Act
            var response = await _client.DeleteAsync($"/Reward/{rewardId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            _rewardStorageMock.Verify(rs => rs.DeleteRewardAsync(rewardId), Times.Once);
        }
    }
}