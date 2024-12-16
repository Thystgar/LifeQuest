namespace LifeQuest.Api.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Progress
    {
        public List<ActiveTask> CompletedTasks { get; set; }
        public int Points
        {
            get
            {
                return CompletedTasks.Sum(t => t.TaskType.Value) - CompletedRewards.Sum(r => r.Value);
            }
        }
        public List<Reward> CompletedRewards { get; set; }
        public List<Reward> AvailableRewards { get; set; }
    }
}
