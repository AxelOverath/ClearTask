using ClearTask.Data;
using ClearTask.Models;
using ClearTask.ViewModels;

namespace ClearTask.Views
{
    public partial class TaskDetail : ContentPage
    {
        private Task_ _task;
        public TaskDetail(Task_ task)
        {
            InitializeComponent();
            BindingContext = new TaskDetailViewModel(task);
            _task = task;
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
            Console.WriteLine($"Edit button clicked. Task: {_task?.title ?? "NULL"}");

            if (_task != null)
            {
                await Navigation.PushAsync(new TaskDetailPageEdit(_task));
            }
            else
            {
                await DisplayAlert("Error", "Task data is missing!", "OK");
            }
        }


        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Start Task", "Task started!", "OK");
        }



    }
}
