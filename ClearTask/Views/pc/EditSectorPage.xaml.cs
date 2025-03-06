using ClearTask.Data;
using ClearTask.Models;
using ClearTask.Views.Pc;
using Microsoft.Maui.Storage;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
namespace ClearTask.Views.pc;


public partial class EditSectorPage : ContentPage
{
    public ObjectId SectorId { get; set; }
    public EditSectorPage(string Id)
	{
		InitializeComponent();
        SectorId = ObjectId.Parse(Id);
	}
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SectorName.Text))
        {
            await DisplayAlert("Fout", "Give a correct sector name", "OK");
            return;
        }

        await DatabaseService.UpdateSectorName(SectorId, SectorName.Text);

        await Navigation.PushAsync(new SectorsOverviewPage());
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}