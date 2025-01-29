namespace ClearTask.Models;

public class Admin : User
{
    public Admin()
    {
        UserRole = Role.Admin;
    }
}