using ClearTask.Models;
using ClearTask.Data;
using Microsoft.Maui.Controls;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using CustomTag = ClearTask.Models.Tag;

namespace ClearTask.Views;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class TaskDetailPageEdit : ContentPage, INotifyPropertyChanged
{
    public string TaskId { get; set; }
    public Task_ Task { get; set; }
    public List<string> StatusOptions { get; set; } = new() { "Pending", "InProgress", "Completed" };

    // Available tags are those not yet selected for this task.
    public ObservableCollection<CustomTag> AvailableTags { get; set; } = new();
    // We expect Task.taglist to be an ObservableCollection<CustomTag>
    // (Als dat niet zo is, zorg dan dat je deze omzet of in het model wijzigt)

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public ObservableCollection<CustomTag> SelectedTags { get; set; } = new ObservableCollection<CustomTag>();

    public TaskDetailPageEdit(Task_ task)



    {
        InitializeComponent();
        Task = task;
        // Zorg ervoor dat Task.taglist is een ObservableCollection
        if (Task.taglist == null)
        {
            Task.taglist = SelectedTags.ToList();
        }
        BindingContext = this;

        // Initialiseer de geselecteerde tags
        OnPropertyChanged(nameof(Task));
        LoadTags();
        LoadUsers();
        LoadSectors();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!string.IsNullOrEmpty(TaskId))
        {
            var objectId = new ObjectId(TaskId);
            Task = await DatabaseService.TaskCollection.Find(t => t.Id == objectId).FirstOrDefaultAsync();
            if (Task.taglist == null)
            {
                Task.taglist = SelectedTags.ToList();
            }
            OnPropertyChanged(nameof(Task));
        }
    }

    private async void LoadTags()
    {
        try
        {
            var tags = await DatabaseService.TagCollection.Find(_ => true).ToListAsync();
            // Bereken AvailableTags als de tags die nog niet geselecteerd zijn:
            var available = tags.Except(Task.taglist, new TagComparer()).ToList();
            AvailableTags = new ObservableCollection<CustomTag>(available);
            OnPropertyChanged(nameof(AvailableTags));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load tags: {ex.Message}", "OK");
        }
    }

    // Vergelijker om tags te vergelijken op basis van hun Id.
    public class TagComparer : IEqualityComparer<CustomTag>
    {
        public bool Equals(CustomTag x, CustomTag y)
        {
            if (x == null || y == null)
                return false;
            return x.Id == y.Id;
        }
        public int GetHashCode(CustomTag obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    private void OnAvailableTagClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is CustomTag availableTag)
        {
            if (AvailableTags.Contains(availableTag))
            {
                // Voeg tag toe aan Task.taglist en verwijder uit AvailableTags
                Task.taglist.Add(availableTag);
                // Zorg ervoor dat Task.tags (lijst met ObjectIds) ook wordt bijgewerkt
                Task.tags.Add(availableTag.Id);
                AvailableTags.Remove(availableTag);

                // Forceer UI-update
                OnPropertyChanged(nameof(AvailableTags));
                OnPropertyChanged(nameof(Task));
            }
        }
    }

    private void OnSelectedTagClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is CustomTag selectedTag)
        {
            if (Task.taglist.Contains(selectedTag))
            {
                // Verwijder de tag uit Task.taglist en voeg terug aan AvailableTags
                Task.taglist.Remove(selectedTag);
                Task.tags.Remove(selectedTag.Id);
                AvailableTags.Add(selectedTag);

                // Forceer UI-update
                OnPropertyChanged(nameof(AvailableTags));
                OnPropertyChanged(nameof(Task));
            }
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (Task != null)
        {
            // Opslaan: vervang Task in de database
            await DatabaseService.TaskCollection.ReplaceOneAsync(t => t.Id == Task.Id, Task);
            await Shell.Current.GoToAsync("//tasks");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//tasks");
    }

    private async void LoadSectors()
    {
        try
        {
            var sectors = await DatabaseService.SectorsCollection.Find(_ => true).ToListAsync();

            if (sectors != null && sectors.Any())
            {
                SectorPicker.ItemsSource = sectors;
                SectorPicker.ItemDisplayBinding = new Binding("name");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load sectors: {ex.Message}", "OK");
        }
    }


    private async void LoadUsers()
    {
        try
        {
            var users = await DatabaseService.UsersCollection.Find(user => user.userRole == Role.Handyman).ToListAsync();

            if (users != null && users.Any())
            {
                UserPicker.ItemsSource = users;
                UserPicker.ItemDisplayBinding = new Binding("username");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
        }
    }

}
