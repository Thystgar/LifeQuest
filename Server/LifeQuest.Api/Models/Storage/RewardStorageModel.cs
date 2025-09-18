using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Api.Models.Storage
{
    public class RewardStorageModel
    {
        [Key]
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Value { get; set; }
        public required bool Redeemed { get; set; }
        public required string GroupId { get; set; }
    }
}
