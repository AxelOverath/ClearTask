using ClearTask.Models;
using System.Linq;  // For LINQ operations (e.g., FirstOrDefault)
using ClearTask.ViewModels;

namespace ClearTask.Views;

public partial class TaskList : ContentPage
{
    public TaskList()
    {
        InitializeComponent();
        BindingContext = new TaskListViewModel();
    }

    private async void NavigateToAddTaskPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddTask());
    }
    private async void OnTaskTapped(object sender, EventArgs e)
    {
        if (sender is View view && view.BindingContext is Task_ selectedTask)
        {
            await Navigation.PushAsync(new TaskDetail(selectedTask));
        }
    }
}
