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
}