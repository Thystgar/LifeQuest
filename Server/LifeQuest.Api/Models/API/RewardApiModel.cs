namespace LifeQuest.Api.Models.API
{
    public class RewardApiModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Value { get; set; }
        public required bool Redeemed { get; set; }
    }
}