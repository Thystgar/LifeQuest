using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Api.Models.Storage
{
    public class GroupStorageModel
    {
        [Key]
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string InviteCode { get; set; }
    }
}
