namespace ClearTask.Models;

public class Admin : User
{
    public Admin()
    {
        userRole = Role.Admin;
    }
}