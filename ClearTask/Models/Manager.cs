namespace ClearTask.Models;

public class Manager : User
{
    public Manager()
    {
        userRole = Role.Manager;
    }
}