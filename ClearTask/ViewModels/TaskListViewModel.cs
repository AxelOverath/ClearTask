using ClearTask.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClearTask.Models;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ClearTask.ViewModels;

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
        DatabaseService.TasksUpdated += async () => await LoadTasks(); // **Luisteren naar DB updates**
    }

    public async Task LoadTasks()
    {
        try
        {
            var tasksFromDb = await DatabaseService.TaskCollection.Find(_ => true).ToListAsync();
            Tasks = new ObservableCollection<Task_>(tasksFromDb);
            OnPropertyChanged(nameof(Tasks)); // **Forceer UI update**
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
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding task: {ex.Message}");
        }
    }
}
