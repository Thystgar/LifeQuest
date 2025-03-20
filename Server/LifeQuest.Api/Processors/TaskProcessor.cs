using LifeQuest.Api.Mappers;
using LifeQuest.Api.Models;
using LifeQuest.Api.Repository;

namespace LifeQuest.Api.Processors
{
    public class TaskProcessor
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
