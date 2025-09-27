namespace LifeQuest.Api.Models.API
{
    public class GroupApiModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string InviteCode { get; set; }
    }

    public class NewGroupApiModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
