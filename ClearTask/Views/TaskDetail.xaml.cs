using ClearTask.Data;
using ClearTask.Models;
using ClearTask.ViewModels;
using MongoDB.Bson;
using TaskStatus = ClearTask.Models.TaskStatus;

namespace ClearTask.Views
{
    public partial class TaskDetail : ContentPage
    {
        public ObjectId TaskId { get; set; }
        private Task_ _task;
        
        public TaskDetail(Task_ task)
        {
            InitializeComponent();
            BindingContext = new TaskDetailViewModel(task);
            TaskId = task.Id;
            _task = task;

            // Only show the Edit button if the current user is a Manager
            if (UserStorage.UserRole == Role.Manager || UserStorage.UserRole == Role.Admin)
            {
                EditButton.IsVisible = true;
                deletebutton.IsVisible = true;
            }
            else
            {
                EditButton.IsVisible = false;
                deletebutton.IsVisible = false;
            }
            if(UserStorage.UserRole != Role.Handyman || task.status == TaskStatus.InProgress)
            {
                    startbutton.IsVisible = false;
            }
            if (task.photo == null)
            {
                ImageElement.IsVisible = false;
            }
            if(task.status == TaskStatus.InProgress && task.startedBy == UserStorage.Id)
            {
                endbutton.IsVisible = true;
            }
            else
            {
                endbutton.IsVisible = false;
            }
            if(task.assignedTo != null)
            {
                AssignedBox.IsVisible = true;
            }
            else if(task.startedBy == null)
            {
                AssignedBox.IsVisible = false;
                StartedBox.IsVisible = false;
            }
            else
            {
                StartedBox.IsVisible= true;
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
            ObjectId HandyId = UserStorage.Id;

            await DisplayAlert("Start Task", "Task started.", "OK");
            await Navigation.PopAsync();

            DatabaseService.UpdateStartedBy(TaskId, HandyId);

        }

        private async void OnEndButtonClicked(object sender, EventArgs e)
        {
            await DatabaseService.EndTask(TaskId);

            await DisplayAlert("Task Completed", "Task has been Completed.", "OK");

            await Navigation.PopAsync();
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            await DatabaseService.DeleteTask(TaskId);

            await DisplayAlert("Task Deleted", "Task has been Deleted.", "OK");

            await Navigation.PopAsync(); // Navigates back to the previous page
        }

    }
}
