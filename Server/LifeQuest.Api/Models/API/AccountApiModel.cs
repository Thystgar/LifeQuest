using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Api.Models.API
{
    public class AccountApiModel
    {
        [Key]
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required int Points { get; set; }
    }
}