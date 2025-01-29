namespace ClearTask.Models;
public class Employee : User
{
    public Employee()
    {
        UserRole = Role.Employee;
    }
}
