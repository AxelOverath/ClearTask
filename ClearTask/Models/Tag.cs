namespace ClearTask.Models;

public class Tag
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Tag(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
