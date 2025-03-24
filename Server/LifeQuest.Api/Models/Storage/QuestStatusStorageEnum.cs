using System.Runtime.Serialization;

namespace LifeQuest.Api.Models.Storage
{
	public enum QuestStatusStorageEnum
	{
        [EnumMember(Value = "Accepted")] Accepted,
        [EnumMember(Value = "Active")] Active,
        [EnumMember(Value = "Completed")] Completed
    }
}
