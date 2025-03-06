using ClearTask.Data;
using ClearTask.Models;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearTask.Views.Pc
{
    public partial class ManagerDashboardPage : ContentPage
    {
        public List<TaskStatusData> TaskStatuses { get; set; }
        public List<SectorData> SectorCounts { get; set; }

        public ManagerDashboardPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDashboardData();
        }

        private async Task LoadDashboardData()
        {
            // Ophalen van gegevens uit de database

            // Ophalen van het aantal taken per status (Pending, InProgress, Completed)
            var taskStatuses = await GetTaskStatusesAsync();
            TaskStatuses = taskStatuses.ToList();

            // Ophalen van het aantal taken per sector
            var sectorCounts = await GetTasksBySectorAsync();
            SectorCounts = sectorCounts.ToList();

            // Update de UI met de nieuwe gegevens
            OnPropertyChanged(nameof(TaskStatuses));
            OnPropertyChanged(nameof(SectorCounts));
        }

        // Ophalen van taken per status
        private async Task<IEnumerable<TaskStatusData>> GetTaskStatusesAsync()
        {
            var taskStatusCounts = await DatabaseService.GetTaskCountByStatus();

            // Converteer naar een lijst van TaskStatusData voor binding
            return taskStatusCounts.Select(kv => new TaskStatusData
            {
                Status = kv.Key.ToString(),
                Count = kv.Value
            });
        }

        // Ophalen van taken per sector
        private async Task<IEnumerable<SectorData>> GetTasksBySectorAsync()
        {
            var sectorCounts = await DatabaseService.GetTaskCountPerSector();

            // Converteer naar een lijst van SectorData voor binding
            return sectorCounts.Select(kv => new SectorData
            {
                Sector = kv.Key,
                Count = kv.Value
            });
        }
    }

    // Model voor Taakstatus
    public class TaskStatusData
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    // Model voor Sectoren
    public class SectorData
    {
        public string Sector { get; set; }
        public int Count { get; set; }
    }
}
