namespace LifeQuest.Api.Models
{
    public class Reward
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public RewardState State { get; set; }
    }
}
