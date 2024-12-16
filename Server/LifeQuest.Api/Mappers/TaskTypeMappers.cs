using LifeQuest.Api.Models;

namespace LifeQuest.Api.Mappers
{
    public static class TaskTypeMappers
    {
        public static ActiveTask ToActiveTask(this TaskType task)
        {
            return new ActiveTask
            {
                TaskType = task,
                State = TaskState.Active
            };
        }
    }
}
