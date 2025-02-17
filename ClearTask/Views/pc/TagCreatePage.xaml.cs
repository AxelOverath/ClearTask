using ClearTask.Data;
using ClearTask.Models;

namespace ClearTask.Views.Pc;

public partial class TagCreatePage : ContentPage
{
    public TagCreatePage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var newTag = new Tag
        {
            name = nameEntry.Text,
            description = descriptionEntry.Text
        };

        await DatabaseService.SaveTagAsync(newTag); // Save the tag to MongoDB
        DatabaseService.TriggerTagUpdatedEvent();
        await Shell.Current.GoToAsync(".."); // Navigate back to the Tag Overview page
    }
}
