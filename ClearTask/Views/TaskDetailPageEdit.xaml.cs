using ClearTask.Models;
using ClearTask.Data;
using Microsoft.Maui.Controls;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MongoDB.Driver;
using CustomTag = ClearTask.Models.Tag;
namespace ClearTask.Views;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class TaskDetailPageEdit : ContentPage
{
    public string TaskId { get; set; }
    public Task_ Task { get; set; }
    public List<string> StatusOptions { get; set; } = new() { "Pending", "InProgress", "Completed" };
    public ObservableCollection<CustomTag> AvailableTags { get; set; } = new();
    public ObservableCollection<CustomTag> SelectedTags { get; set; } = new();

    public TaskDetailPageEdit()
    {
        InitializeComponent();
        BindingContext = this;
        LoadTags();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!string.IsNullOrEmpty(TaskId))
        {
            var objectId = new ObjectId(TaskId);
            Task = await DatabaseService.TaskCollection.Find(t => t.Id == objectId).FirstOrDefaultAsync();
            SelectedTags = new ObservableCollection<CustomTag>(Task?.taglist ?? new List<CustomTag>());
            OnPropertyChanged(nameof(Task));
            OnPropertyChanged(nameof(SelectedTags));
        }
    }

    private async void LoadTags()
    {
        var tags = await DatabaseService.TagCollection.Find(_ => true).ToListAsync();
        AvailableTags = new ObservableCollection<CustomTag>(tags);
        OnPropertyChanged(nameof(AvailableTags));
    }

    private void OnAvailableTagClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is CustomTag selectedTag)
        {
            if (AvailableTags.Contains(selectedTag))
            {
                Task.taglist.Add(selectedTag);
                AvailableTags.Remove(selectedTag);
                // ObservableCollection updateert de UI automatisch
            }
        }
    }

    private void OnSelectedTagClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is CustomTag selectedTag)
        {
            if (Task.taglist.Contains(selectedTag))
            {
                Task.taglist.Remove(selectedTag);
                AvailableTags.Add(selectedTag); // UI update automatisch
            }
        }
    }




    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (Task != null)
        {
            Task.taglist = new List<CustomTag>(SelectedTags);
            await DatabaseService.TaskCollection.ReplaceOneAsync(t => t.Id == Task.Id, Task);
            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
