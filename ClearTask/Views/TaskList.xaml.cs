using ClearTask.Models;
using System.Linq;  // For LINQ operations (e.g., FirstOrDefault)
using ClearTask.ViewModels;

namespace ClearTask.Views
{
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

        // Handle item selection and navigate to TaskDetailPage
        private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedTask = e.CurrentSelection.FirstOrDefault() as Task_;
            if (selectedTask != null)
            {
                // Navigate to TaskDetailPage and pass taskId as a query parameter
                await Shell.Current.GoToAsync($"{nameof(TaskDetailPageEdit)}?taskId={selectedTask.Id}");
            }
        }
    }
}
