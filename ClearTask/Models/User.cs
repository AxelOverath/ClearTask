using MongoDB.Bson;

namespace ClearTask.Models;

public class User
{

        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role UserRole { get; set; }

}
public enum Role
{
    Admin,
    Manager,
    Handyman,
    Employee
}
