namespace LifeQuest.Api.Models
{
    public class TaskType
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Frequency { get; set; }
        public Category Category { get; set; }
    }
}