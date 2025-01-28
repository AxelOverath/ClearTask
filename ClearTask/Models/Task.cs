using System;
using System.Collections.Generic;

namespace ClearTask.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Photo { get; set; }
        public List<string> Tags { get; set; }
        public DateTime? Deadline { get; set; }
        public TaskStatus Status { get; set; }
        public Handyman? AssignedTo { get; set; }
        public Sector Sector { get; set; }
    }

    public class Handyman
    {
        public string Name { get; set; }
    }

    public class Sector
    {
        public string Name { get; set; }
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
}
