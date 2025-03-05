using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClearTask.Models
{
    [BsonIgnoreExtraElements]
    public class Task_
    {
        public ObjectId Id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
       public byte[]? photo { get; set; } // Changed from string to byte[]
        public List<ObjectId>? tags { get; set; }
        public List<Tag>? taglist { get; set; }
        public DateTime? deadline { get; set; }

        [BsonRepresentation(BsonType.String)]  // Dit zorgt ervoor dat het als string wordt opgeslagen
        public TaskStatus status { get; set; } 
        public ObjectId assignedTo { get; set; }
        public ObjectId sector { get; set; }
        public ObjectId createdBy { get; set; }
        public Boolean? isAdmin { get; set; }
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
}
