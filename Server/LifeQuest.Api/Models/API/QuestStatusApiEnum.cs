using System.Runtime.Serialization;

namespace LifeQuest.Api.Models.API
{
    public enum QuestStatusApiEnum
    {
        [EnumMember(Value = "Accepted")] Accepted,
        [EnumMember(Value = "Active")] Active,
        [EnumMember(Value = "Completed")] Completed
    }
}
