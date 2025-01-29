namespace ClearTask.Models;

public class Manager : User
{
    public Manager()
    {
        UserRole = Role.Manager;
    }
}