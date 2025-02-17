using ClearTask.Data;
using ClearTask.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace ClearTask.Views.Pc;

[QueryProperty(nameof(TagId), "tagId")]
public partial class TagEditPage : ContentPage
{
    public string TagId { get; set; }
    public ClearTask.Models.Tag CurrentTag { get; set; }

    public TagEditPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!string.IsNullOrEmpty(TagId))
        {
            var objectId = new ObjectId(TagId);
            CurrentTag = await DatabaseService.TagCollection.Find(t => t.Id == objectId).FirstOrDefaultAsync();

            if (CurrentTag != null)
            {
                BindingContext = CurrentTag;
            }
        }
    }

    public async void OnSaveClicked(object sender, EventArgs e)
    {
        if (CurrentTag != null)
        {
            await DatabaseService.TagCollection.ReplaceOneAsync(t => t.Id == CurrentTag.Id, CurrentTag);

            DatabaseService.TriggerTagUpdatedEvent(); // Zorgt ervoor dat andere schermen ook updaten

            await Shell.Current.GoToAsync(".."); // Terug naar vorige pagina
        }
    }

    public async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(".."); // Terug naar vorige pagina
    }

    public async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (CurrentTag != null)
        {
            var confirmation = await DisplayAlert("Bevestigen", "Weet je zeker dat je deze tag wilt verwijderen?", "Ja", "Nee");

            if (confirmation)
            {
                await DatabaseService.TagCollection.DeleteOneAsync(t => t.Id == CurrentTag.Id);

                await DisplayAlert("Succes", "Tag verwijderd.", "OK");
                
                DatabaseService.TriggerTagUpdatedEvent();

                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
