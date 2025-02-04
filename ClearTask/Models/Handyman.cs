namespace ClearTask.Models;

public class Handyman : User
{
    public List<Tag> Tags { get; set; }
    public List<Task_> AssignedTasks { get; set; }

    public Handyman()
    {
        UserRole = Role.Handyman; // Set default role
        Tags = new List<Tag>();
        AssignedTasks = new List<Task_>();
    }

    public void AddTag(Tag tag)
    {
        if (!Tags.Contains(tag))
        {
            Tags.Add(tag);
        }
    }

    public void AssignTask(Task_ task)
    {
        AssignedTasks.Add(task);
    }

    public void RemoveTask(int taskId)
    {
        AssignedTasks.RemoveAll(t => t.Id == taskId);
    }
}
