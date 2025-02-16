using ClearTask.Data;
using ClearTask.Models;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClearTask.Views.Pc;

public partial class SectorsOverviewPage : ContentPage
{
    public ObservableCollection<Sector> Sectors { get; set; } = new();
    public ICommand SectorClickedCommand { get; private set; }

    public SectorsOverviewPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadSectors();
        DatabaseService.SectorUpdated += async () => await LoadSectors();
        SectorClickedCommand = new Microsoft.Maui.Controls.Command<Sector>(OnSectorClicked);
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

    private async void OnSectorClicked(Sector sector)
    {
        if (sector != null)
        {
            await Shell.Current.GoToAsync($"{nameof(EditUserPage)}?sectorId={sector.Id}");
        }
    }

    private async void OnAddSectorClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("addsector");
    }
}