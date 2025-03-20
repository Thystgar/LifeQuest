using LifeQuest.Api.Models;
using LifeQuest.Api.Repository;

namespace LifeQuest.Api.Processors
{
    public class RewardProcessor
    {
        public async Task<IEnumerable<Reward>> GetRewardsAsync(string user)
        {
            var repository = new ProgressRepository(user);
            var progress = await repository.GetProgressAsync();

            return progress.AvailableRewards;
        }

        public async Task AddRewardAsync(string user, Reward reward)
        {
            var repository = new ProgressRepository(user);
            var progress = await repository.GetProgressAsync();

            if (progress.AvailableRewards.Any(r => r.Name == reward.Name))
            {
                throw new InvalidOperationException($"Reward with name {reward.Name} already exists");
            }

            reward.State = RewardState.Available;
            progress.AvailableRewards.Add(reward);

            await repository.SetProgressAsync(progress);
        }

        public async Task CompleteRewardAsync(string user, string rewardName)
        {
            var repository = new ProgressRepository(user);
            var progress = await repository.GetProgressAsync();

            var completedReward = progress.AvailableRewards.FirstOrDefault(r => r.Name == rewardName);

            if (completedReward == null)
            {
                throw new InvalidOperationException($"Reward with name {rewardName} does not exist");
            }
            if (completedReward.State != RewardState.Active)
            {
                throw new InvalidOperationException($"Reward with name {rewardName} is not active");
            }

            var newRewards = progress.AvailableRewards.Where(r => r.Name != rewardName).ToList();

            progress.AvailableRewards = newRewards;
            progress.CompletedRewards.Add(completedReward);

            await repository.SetProgressAsync(progress);
        }

        public async Task ActivateRewardAsync(string user, string rewardName)
        {
            var repository = new ProgressRepository(user);
            var progress = await repository.GetProgressAsync();

            if (!progress.AvailableRewards.Any(r => r.Name == rewardName))
            {
                throw new InvalidOperationException($"Reward with name {rewardName} does not exist");
            }

            var newRewards = progress.AvailableRewards.Select(r =>
            {
                if (r.Name == rewardName)
                {
                    r.State = RewardState.Active;
                }
                else
                {
                    r.State = RewardState.Available;
                }
                return r;
            }).ToList();

            progress.AvailableRewards = newRewards;
            await repository.SetProgressAsync(progress);
        }
        public async Task DeleteRewardAsync(string user, string rewardName)
        {
            var repository = new ProgressRepository(user);
            var progress = await repository.GetProgressAsync();

            if (!progress.AvailableRewards.Any(r => r.Name == rewardName))
            {
                throw new InvalidOperationException($"Reward with name {rewardName} does not exist");
            }

            var newRewards = progress.AvailableRewards.Where(r => r.Name != rewardName).ToList();

            progress.AvailableRewards = newRewards;
            await repository.SetProgressAsync(progress);
        }

        public async Task<Progress> GetProgress(string user)
        {
            var repository = new ProgressRepository(user);

            return await repository.GetProgressAsync();
        }
    }
}
