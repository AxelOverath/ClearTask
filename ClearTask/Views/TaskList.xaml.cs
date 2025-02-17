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
}