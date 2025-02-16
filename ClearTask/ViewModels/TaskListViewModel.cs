namespace ClearTask.ViewModels;
using ClearTask.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClearTask.Models;
using MongoDB.Driver;

public class TaskListViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Task_> _tasks;

    public ObservableCollection<Task_> Tasks
    {
        get => _tasks;
        set
        {
            _tasks = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public TaskListViewModel()
    {
        LoadTasks();
    }

    public async Task LoadTasks()
    {
        try
        {
            var tasksFromDb = await DatabaseService.TaskCollection.Find(_ => true).ToListAsync();
            Console.WriteLine($"Fetched {tasksFromDb.Count} tasks from DB");

            var detailedTasks = new List<Task_>();

            foreach (var task in tasksFromDb)
            {
                try
                {
                    Console.WriteLine($"Fetching details for Task ID: {task.Id}");

                    var detailedTask = await DatabaseService.GetTaskWithDetails(task.Id);

                    if (detailedTask != null)
                    {
                        detailedTasks.Add(detailedTask);
                    }
                    else
                    {
                        Console.WriteLine($"Task {task.Id} returned null from GetTaskWithDetails()");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching details for Task {task.Id}: {ex.Message}");
                }
            }

            Tasks = new ObservableCollection<Task_>(detailedTasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tasks: {ex.Message}");
        }
    }

    public async Task AddTask(Task_ newTask)
    {
        try
        {
            await DatabaseService.InsertTaskAsync(newTask);
            Tasks.Add(newTask);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding task: {ex.Message}");
        }
    }
}
