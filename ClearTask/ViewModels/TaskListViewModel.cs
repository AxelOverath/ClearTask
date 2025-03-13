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
            List<Task_> tasksFromDb;

            // If user role is Handyman, load tasks only from their sector
            if (UserStorage.UserRole == Role.Handyman)
            {
                var allSectors = await DatabaseService.SectorCollection.Find(_ => true).ToListAsync();
                var userSector = allSectors.FirstOrDefault(sector => sector.handymanIds.Contains(UserStorage.Id));

                if (userSector == null)
                {
                    Console.WriteLine("User sector not found.");
                    Tasks = new ObservableCollection<Task_>();
                    return;
                }

                tasksFromDb = await DatabaseService.TaskCollection
                                    .Find(task => task.sector == userSector.Id)
                                    .ToListAsync();
            }
            else // If not Handyman, load all tasks
            {
                tasksFromDb = await DatabaseService.TaskCollection.Find(_ => true).ToListAsync();
            }

            var detailedTasks = new List<Task_>();

            foreach (var task in tasksFromDb)
            {
                try
                {
                    var detailedTask = await DatabaseService.GetTaskWithDetails(task.Id);

                    if (detailedTask != null)
                    {
                        detailedTasks.Add(detailedTask);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching details for Task {task.Id}: {ex.Message}");
                }
            }

            Tasks = new ObservableCollection<Task_>(detailedTasks);
            OnPropertyChanged(nameof(Tasks));
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
