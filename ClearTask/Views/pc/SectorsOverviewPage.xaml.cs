using ClearTask.Data;
using ClearTask.Models;
using ClearTask.Views.pc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClearTask.Views.Pc;

public partial class SectorsOverviewPage : ContentPage
{
    public ObservableCollection<Sector> Sectors { get; set; } = new();

    public SectorsOverviewPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadSectors();
        DatabaseService.SectorUpdated += async () => await LoadSectors();
    }

    private async Task LoadSectors()
    {
        try
        {
            var allSectors = await DatabaseService.SectorsCollection.Find(_ => true).ToListAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Sectors = new ObservableCollection<Sector>(allSectors);
                OnPropertyChanged(nameof(Sectors));
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij laden van sectoren: {ex.Message}");
        }
    }

    private async void OnAddSectorClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddSectorPage());
    }

    public async void DeleteSectorClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ObjectId sectorId)
        {
            var confirmation = await DisplayAlert("Bevestigen", $"Weet je zeker dat je deze sector wilt verwijderen?", "Ja", "Nee");

            if (confirmation)
            {
                await Data.DatabaseService.DeleteSector(sectorId);

                await DisplayAlert("Succes", "Sector verwijderd.", "OK");
            }
        }
    }

    private async void EditSectorName(object sender, EventArgs e)
    {
        Button button = sender as Button;
        await Navigation.PushAsync(new EditSectorPage(button.CommandParameter.ToString()));
    }
    private async void OverviewHandyman(object sender, EventArgs e)
    {
        Button button = sender as Button;
        await Navigation.PushAsync(new HandymanOverviewPage(button.CommandParameter.ToString()));
    }
}