using ClearTask.Data;
using ClearTask.Models;
using ClearTask.Views.pc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClearTask.Views.Pc;

public partial class HandymanOverviewPage : ContentPage
{
	public ObservableCollection<User> HandymanSector { get; set; }
    public ObservableCollection<User> AllHandyman { get; set; }
    public ObjectId SectorId { get; set; }

    public ObservableCollection<Sector> Sectors { get; set; } = new();

    private User _selectedHandyman;
    public User SelectedHandyman
    {
        get => _selectedHandyman;
        set
        {
            _selectedHandyman = value;
            OnPropertyChanged(nameof(SelectedHandyman));
            OnPropertyChanged(nameof(SelectedHandymanUsername));
        }
    }

    public string SelectedHandymanUsername => SelectedHandyman?.username;

    public HandymanOverviewPage(String Id)
	{
		InitializeComponent();
        SectorId = ObjectId.Parse(Id);
        BindingContext = this;
        LoadHandymanSector();
        LoadHandyman();
        DatabaseService.SectorUpdated += async () => await LoadHandymanSector();
        DatabaseService.SectorUpdated += async () => await LoadHandyman();
    }

    private async Task LoadHandymanSector()
    {
        try
        {
            var Sector = await DatabaseService.GetSectorById(SectorId);
            var HandymanFromSector = await DatabaseService.GetHandymanFromSectorByIds(Sector.handymanIds);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                HandymanSector = new ObservableCollection<User>(HandymanFromSector);
                OnPropertyChanged(nameof(HandymanSector));
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij laden van sectoren: {ex.Message}");
        }
    }

    private async Task LoadHandyman()
    {
        try
        {
            var allHandyman = await DatabaseService.GetAllHandymen();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                AllHandyman = new ObservableCollection<User>(allHandyman);
                OnPropertyChanged(nameof(AllHandyman));
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij laden van sectoren: {ex.Message}");
        }
    }

    private async void DeleteHandymanClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ObjectId HandymanId)
        {
            await DatabaseService.DeleteHandymanSector(SectorId, HandymanId);
            await DisplayAlert("Succes", "Handyman Removed!", "OK");
        }
    }

    private async void OnAddHandymanClicked(object sender, EventArgs e)
    {
        ObjectId IdHandymanToAdd = new ObjectId();
        foreach (var item in AllHandyman)
        {
            if (item.username == SelectedHandymanUsername)
            {
                IdHandymanToAdd = item.Id;
            }
        }
        await DatabaseService.AddHandymanSector(SectorId, IdHandymanToAdd);
        await DisplayAlert("Succes", "Handyman Added!", "OK");

    }

}