using ClearTask.Data;
using ClearTask.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClearTask.Views.Pc;

public partial class TagOverviewPage : ContentPage
{
    public ObservableCollection<Tag> Tags { get; set; }
    public ICommand OnTagSelectedCommand { get; private set; } // Voeg de command toe

    public TagOverviewPage()
    {
        InitializeComponent();
        Tags = new ObservableCollection<Tag>();
        OnTagSelectedCommand = new Command<Tag>(async (tag) => await OpenTagEditPage(tag)); // Command instellen
        BindingContext = this;
        LoadTags();
        // Luister naar het event en update de UI als een tag verandert
        DatabaseService.TagUpdated += async () => await LoadTags();
    }

    private async Task LoadTags()
    {
        var tags = await DatabaseService.GetTagsAsync(); // Tags ophalen van database
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Tags.Clear();
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        });
    }

    private async Task OpenTagEditPage(Tag selectedTag)
    {
        if (selectedTag != null)
        {
            Console.WriteLine($"Navigating to TagEditPage with ID: {selectedTag.Id}"); // Debug
            await Shell.Current.GoToAsync($"{nameof(TagEditPage)}?tagId={selectedTag.Id}");
        }
    }

    private async void OnCreateNewTagClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("tagcreate");
    }
}
