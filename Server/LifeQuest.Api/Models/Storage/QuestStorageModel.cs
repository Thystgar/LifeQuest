namespace LifeQuest.Api.Models.Storage
{
    public class QuestStorageModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public  QuestStatusStorage Status { get; set; }
    }
}
