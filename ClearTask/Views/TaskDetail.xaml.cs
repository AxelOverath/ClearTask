using ClearTask.Data;
using ClearTask.Models;
using ClearTask.ViewModels;
using MongoDB.Bson;

namespace ClearTask.Views
{
    public partial class TaskDetail : ContentPage
    {
        public ObjectId TaskId { get; set; }
        public TaskDetail(Task_ task)
        {
            InitializeComponent();
            BindingContext = new TaskDetailViewModel(task);
            TaskId = task.Id;

            // Only show the Edit button if the current user is a Manager
            if (UserStorage.UserRole == Role.Manager)
            {
                EditButton.IsVisible = true;
            }
            else
            {
                EditButton.IsVisible = false;
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Navigates back to the previous page
        }

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Edit Task", "Edit task screen will open.", "OK");
        }

        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            ObjectId HandyId = UserStorage.Id;

            DatabaseService.UpdateStartedBy(TaskId, HandyId);

            await DisplayAlert("Succes", "You took this task", "OK");
        }

    }
}
