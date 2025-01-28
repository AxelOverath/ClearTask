using ClearTask.ViewModels;

namespace ClearTask;

public partial class TaskList : ContentPage
{
    public TaskList()
    {
        InitializeComponent();
        BindingContext = new TaskListViewModel();
    }
}