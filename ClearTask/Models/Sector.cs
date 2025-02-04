using ClearTask.Models;

public class Sector
{
    private int id;
    private string name;
    private List<Task_> tasks;
    private List<Employee> employees;
    private List<Handyman> handymen;

    public Sector(int id, string name)
    {
        this.id = id;
        this.name = name;
        this.tasks = new List<Task_>();
        this.employees = new List<Employee>();
        this.handymen = new List<Handyman>();
    }

    public void AddTask(Task_ task)
    {
        tasks.Add(task);
    }

    public void RemoveTask(int taskId)
    {
        tasks.RemoveAll(t => t.Id == taskId);
    }

    // Getters and setters
    public int Id => id;
    public string Name => name;
    public List<Task_> Tasks => tasks;
    public List<Employee> Employees => employees;
    public List<Handyman> Handymen => handymen;
}