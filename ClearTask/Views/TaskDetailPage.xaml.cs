using ClearTask.Models;
using ClearTask.Data;
using Microsoft.Maui.Controls;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MongoDB.Driver;
using CustomTag = ClearTask.Models.Tag;
namespace ClearTask.Views;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class TaskDetailPage : ContentPage
{
    public string TaskId { get; set; }
    public Task_ Task { get; set; } = new();
    public ObservableCollection<CustomTag> AvailableTags { get; set; } = new();

    public TaskDetailPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!string.IsNullOrEmpty(TaskId))
        {
            var objectId = new ObjectId(TaskId);
            Task = await DatabaseService.TaskCollection
                .Find(t => t.Id == objectId)
                .FirstOrDefaultAsync();

            if (Task != null)
            {
                // Sector en Handyman laden (indien niet null)
                if (Task.sector != ObjectId.Empty)
                    Task.actualSector = await DatabaseService.SectorCollection.Find(s => s.Id == Task.sector).FirstOrDefaultAsync();

                

                // Geselecteerde tags ophalen
                if (Task.tags != null && Task.tags.Count > 0)
                {
                    var filter = Builders<CustomTag>.Filter.In(t => t.Id, Task.tags);
                    Task.taglist = await DatabaseService.TagCollection.Find(filter).ToListAsync();
                }

                // Beschikbare tags ophalen (tags die niet geselecteerd zijn)
                var allTags = await DatabaseService.TagCollection.Find(_ => true).ToListAsync();
                AvailableTags = new ObservableCollection<CustomTag>(allTags.Except(Task.taglist ?? new List<CustomTag>()));

                OnPropertyChanged(nameof(Task));
                OnPropertyChanged(nameof(AvailableTags));
            }
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
