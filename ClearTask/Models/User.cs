using MongoDB.Bson;

namespace ClearTask.Models;

public class User
{

        public ObjectId Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public Role userRole { get; set; }

}
public enum Role
{
    Admin,
    Manager,
    Handyman,
    Employee
}
