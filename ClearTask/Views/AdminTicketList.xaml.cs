using ClearTask.ViewModels;
namespace ClearTask.Views;

public partial class AdminTicketList : ContentPage
{
    public AdminTicketList()
    {
        InitializeComponent();
        BindingContext = new AdminTicketListModel();
    }
    private async void NavigateToAddTaskPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddTask());
    }
}