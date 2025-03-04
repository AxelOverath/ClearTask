using ClearTask.Data;
using ClearTask.Views.Pc;
using MongoDB.Bson;

namespace ClearTask.Views.pc;

public partial class AddSectorPage : ContentPage
{
	public AddSectorPage()
	{
		InitializeComponent();
	}
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if(string.IsNullOrWhiteSpace(SectorName.Text)) 
        {
            await DisplayAlert("Fout", "Give a right sector name", "OK");
            return;
        }

        var newSector = new Sector
        {
            Id = ObjectId.GenerateNewId(),
            name = SectorName.Text,
            taskIds = new List<ObjectId>(),
            handymanIds = new List<ObjectId>()
        };

        await DatabaseService.AddSector(newSector);

        await DisplayAlert("Succes", "Sector Added!", "OK");

        await Navigation.PushAsync(new SectorsOverviewPage());
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}