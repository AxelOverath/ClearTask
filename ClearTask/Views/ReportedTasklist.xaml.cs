using ClearTask.Models;
using ClearTask.ViewModels;
namespace ClearTask.Views;

public partial class ReportedTasklist : ContentPage
{
    public ReportedTasklist()
    {
        InitializeComponent();
        BindingContext = new ReportedTasklistModel();
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