namespace LifeQuest.Api.Processors
{
    using LifeQuest.Api.Mappers;
    using LifeQuest.Api.Models;
    using LifeQuest.Api.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LifeGamificationProcessor
    {
        public async Task<IEnumerable<ActiveTask>> GetActiveTasksAsync(string user)
        {
            var date = DateTime.UtcNow;
            var repository = new ActiveTasksRepository(user);
            var tasks = await repository.GetActiveTasksAsync(date);

            if (tasks == null)
            {
                tasks = await GenerateActiveTasksAsync(user);
                await repository.SetActiveTasksAsync(date, tasks);
            }

            return tasks;
        }

        public async Task<IEnumerable<TaskType>> GetTasksTypesAsync(string user)
        {
            var repository = new TaskTypesRepository(user);
            var taskTypes = await repository.GetTaskTypesAsync();
            return taskTypes;
        }

        public async Task AddTaskTypeAsync(string user, TaskType task)
        {
            var repository = new TaskTypesRepository(user);
            var taskTypes = await repository.GetTaskTypesAsync();

            if (taskTypes.Any(t => t.Name == task.Name))
            {
                throw new InvalidOperationException($"Task type with name {task.Name} already exists");
            }

            var newTaskTypes = taskTypes;
            newTaskTypes.Add(task);

            await repository.SetTaskTypesAsync(newTaskTypes);
        }

        public async Task UpdateTaskTypeAsync(string user, TaskType task)
        {
            var repository = new TaskTypesRepository(user);
            var taskTypes = await repository.GetTaskTypesAsync();

            if (!taskTypes.Any(t => t.Name == task.Name))
            {
                throw new InvalidOperationException($"Task type with name {task.Name} does not exist");
            }

            var newTaskTypes = taskTypes.Where(t => t.Name != task.Name).ToList();
            newTaskTypes.Add(task);

            await repository.SetTaskTypesAsync(newTaskTypes);
        }

        public async Task DeleteTaskTypeAsync(string user, TaskType task)
        {
            var repository = new TaskTypesRepository(user);
            var taskTypes = await repository.GetTaskTypesAsync();

            if (!taskTypes.Any(t => t.Name == task.Name))
            {
                throw new InvalidOperationException($"Task type with name {task.Name} does not exist");
            }

            var newTaskTypes = taskTypes.Where(t => t.Name != task.Name).ToList();

            await repository.SetTaskTypesAsync(newTaskTypes);
        }

        public async Task CompleteActiveTaskAsync(string user, string taskName)
        {
            var date = DateTime.UtcNow;
            var repository = new ActiveTasksRepository(user);
            var tasks = await repository.GetActiveTasksAsync(date);
            var completedTask = tasks.FirstOrDefault(t => t.TaskType.Name == taskName);

            if (completedTask == null)
            {
                throw new InvalidOperationException($"Task with name {taskName} does not exist in active tasks for day {date}");
            }

            completedTask.State = TaskState.Completed;

            var newActiveTasks = tasks.Where(t => t.TaskType.Name != taskName).ToList();
            newActiveTasks.Add(completedTask);

            await repository.SetActiveTasksAsync(date, newActiveTasks);

            // update progress
        }

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

        private async Task<List<ActiveTask>> GenerateActiveTasksAsync(string user)
        {
            var date = DateTime.UtcNow.Date;

            var repository = new TaskTypesRepository(user);
            var taskTypes = await repository.GetTaskTypesAsync();

            if (taskTypes == null || !taskTypes.Any())
            {
                return new List<ActiveTask> { };
            }

            var activeTasksRepository = new ActiveTasksRepository(user);
            var previousTasks = await activeTasksRepository.GetActiveTasksAsync(date);

            var activeTasks = new List<ActiveTask>();

            foreach (var category in Enum.GetValues(typeof(Category)).Cast<Category>())
            {
                var categoryTasks = taskTypes.Where(t => t.Category == category).ToArray();

                if (!categoryTasks.Any())
                {
                    continue;
                }

                var random = new Random();

                activeTasks.Add(categoryTasks[random.Next(0, categoryTasks.Length)].ToActiveTask());
            }

            return activeTasks;
        }
    }
}