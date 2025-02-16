using ClearTask.ViewModels;
namespace ClearTask.Views;

public partial class TaskList : ContentPage
{
    public TaskList()
    {
        InitializeComponent();
        BindingContext = new TaskListViewModel();
    }
}