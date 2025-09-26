namespace LifeQuest.Api.Models.API
{
    public class AccountApiModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required int Points { get; set; }
        public required string GroupId { get; set; }
    }
}