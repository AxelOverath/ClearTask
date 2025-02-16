namespace ClearTask.Views;

using ClearTask.Models;
using ClearTask.ViewModels;
using System;
using MongoDB.Bson;

public partial class AddTask : ContentPage
{
    private readonly TaskListViewModel _viewModel;

    public AddTask()
    {
        InitializeComponent();
        _viewModel = new TaskListViewModel();
        BindingContext = _viewModel;
    }

    private async void OnSaveTaskClicked(object sender, EventArgs e)
    {
        var newTask = new Task_
        {
            title = tasktitle.Text,
            description = taskdescription.Text,
            photo = "https://example.com/walls.jpg",
            // tags = new List<ObjectId>(),
            deadline = DeadlinePicker.Date,
            status = TaskStatus.Pending, // Default status
            
        };

        await _viewModel.AddTask(newTask);
        await DisplayAlert("Success", "Task added successfully!", "OK");
        await Navigation.PopAsync();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
