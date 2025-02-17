using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ClearTask.Models;

public class User
{

        public ObjectId Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Role userRole { get; set; }

}
public enum Role
{
    Admin,
    Manager,
    Handyman,
    Employee
}
