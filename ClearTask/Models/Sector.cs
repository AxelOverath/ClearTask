using ClearTask.Models;
using MongoDB.Bson;

public class Sector
{
    public ObjectId Id { get; set; }
    public string name { get; set; }
    public List<ObjectId> taskIds { get; set; }
    public List<ObjectId> handymanIds { get; set; }
}