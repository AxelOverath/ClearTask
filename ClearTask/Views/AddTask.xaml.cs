using ClearTask.Data;
using ClearTask.Models;
using ClearTask.ViewModels;
using MongoDB.Bson;
using TaskStatus = ClearTask.Models.TaskStatus;


namespace ClearTask.Views
{
    public partial class AddTask : ContentPage
    {
        private readonly TaskListViewModel _viewModel;
        private ObjectId? uploadedImageId = null;

        public AddTask()
        {
            InitializeComponent();
            _viewModel = new TaskListViewModel();
            BindingContext = _viewModel;
        }

        //  Handle Image Selection & Upload
        private async void OnPickImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select an image"
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                    }

                    await DisplayAlert("Success", "Image uploaded successfully!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
            }
        }

        //  Save Task with Image ID
        private async void OnSaveTaskClicked(object sender, EventArgs e)
        {
            var newTask = new Task_
            {
                title = tasktitle.Text,
                description = taskdescription.Text,
                photo = uploadedImageId?.ToString(), // Save Image ID
                tags = new List<ObjectId>(),
                deadline = DeadlinePicker.Date,
                status = TaskStatus.Pending,
                assignedTo = ObjectId.Empty,
                sector = ObjectId.Empty
            };

            await _viewModel.AddTask(newTask);
            await DisplayAlert("Success", "Task added successfully!", "OK");
            await Navigation.PopAsync();
        }

        //  Back Button
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
