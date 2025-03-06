using ClearTask.Data;
using ClearTask.Models;
using ClearTask.ViewModels;

namespace ClearTask.Views
{
    public partial class TaskDetail : ContentPage
    {
        public TaskDetail(Task_ task)
        {
            InitializeComponent();
            BindingContext = new TaskDetailViewModel(task);

            // Only show the Edit button if the current user is a Manager
            if (UserStorage.UserRole == Role.Manager || UserStorage.UserRole == Role.Admin)
            {
                EditButton.IsVisible = true;
            }
            else
            {
                EditButton.IsVisible = false;
            }
            if(UserStorage.UserRole != Role.Handyman)
            {
                startbutton.IsVisible = false;
            }
            if (task.photo == null)
            {
                ImageElement.IsVisible = false;
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
            await DisplayAlert("Start Task", "Task started!", "OK");
        }



    }
}
