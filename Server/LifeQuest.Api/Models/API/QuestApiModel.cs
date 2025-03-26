using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Api.Models.API
{
    public class QuestApiModel
    {
        [Key]
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Value { get; set; }
        public required QuestStatusApiEnum Status { get; set; }
    }
}