namespace LifeQuest.Api.Models.Storage
{
    public class RewardStorageModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Value { get; set; }
        public required bool Redeemed { get; set; }
    }
}
