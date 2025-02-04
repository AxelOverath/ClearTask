using MongoDB.Bson;

namespace ClearTask.Models
{
    public class Task_
    {
        public ObjectId Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string? photo { get; set; }
        public List<ObjectId> tags { get; set; }
        public List<Tag>? taglist { get; set; }
        public DateTime? deadline { get; set; }
        public TaskStatus status { get; set; }
        public ObjectId assignedTo { get; set; }
        public Handyman? hassignedTo { get; set; }
        public ObjectId sector { get; set; }
        public Sector actualSector { get; set; }
    }
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
}
