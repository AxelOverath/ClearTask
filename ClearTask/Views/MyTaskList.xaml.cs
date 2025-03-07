using ClearTask.Models;
using ClearTask.ViewModels;
namespace ClearTask.Views;

public partial class MyTaskList : ContentPage
{
    public MyTaskList()
    {
        InitializeComponent();
        BindingContext = new MyTaskListViewModel();
    }
    private async void OnTaskTapped(object sender, EventArgs e)
    {
        if (sender is View view && view.BindingContext is Task_ selectedTask)
        {
            await Navigation.PushAsync(new TaskDetail(selectedTask));
        }
    }
}