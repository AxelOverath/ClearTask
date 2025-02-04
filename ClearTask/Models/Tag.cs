using MongoDB.Bson;

namespace ClearTask.Models;

public class Tag
{
    public ObjectId Id { get; set; }
    public string name { get; set; }
    public string description { get; set; }

}
