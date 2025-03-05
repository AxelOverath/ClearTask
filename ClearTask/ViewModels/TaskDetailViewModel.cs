using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClearTask.Models;
using ClearTask.Data; // Ensure this namespace contains DatabaseService
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ClearTask.ViewModels
{
    public class TaskDetailViewModel : INotifyPropertyChanged
    {
        private Task_ _task;
        public Task_ Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged();
            }
        }

        private string _assignedUserName;
        public string AssignedUserName
        {
            get => _assignedUserName;
            set
            {
                _assignedUserName = value;
                OnPropertyChanged();
            }
        }

        private bool _isAssignedUserVisible;
        public bool IsAssignedUserVisible
        {
            get => _isAssignedUserVisible;
            set
            {
                _isAssignedUserVisible = value;
                OnPropertyChanged();
            }
        }

        private string _sectorName;
        public string SectorName
        {
            get => _sectorName;
            set
            {
                _sectorName = value;
                OnPropertyChanged();
            }
        }

        private bool _isSectorVisible;
        public bool IsSectorVisible
        {
            get => _isSectorVisible;
            set
            {
                _isSectorVisible = value;
                OnPropertyChanged();
            }
        }

        public TaskDetailViewModel(Task_ task)
        {
            Task = task;

            // For Assigned User
            if (Task.assignedTo != ObjectId.Empty)
            {
                IsAssignedUserVisible = true;
                LoadAssignedUserNameAsync();
            }
            else
            {
                IsAssignedUserVisible = false;
            }

            // For Sector
            if (Task.sector != ObjectId.Empty)
            {
                IsSectorVisible = true;
                LoadSectorNameAsync();
            }
            else
            {
                IsSectorVisible = false;
            }
        }

        private async void LoadAssignedUserNameAsync()
        {
            try
            {
                var user = await DatabaseService.UsersCollection
                    .Find(u => u.Id == Task.assignedTo)
                    .FirstOrDefaultAsync();

                AssignedUserName = user != null ? user.username : string.Empty;
            }
            catch (Exception)
            {
                AssignedUserName = "Unknown User";
            }
        }

        private async void LoadSectorNameAsync()
        {
            try
            {
                var sector = await DatabaseService.SectorCollection
                    .Find(s => s.Id == Task.sector)
                    .FirstOrDefaultAsync();

                SectorName = sector != null ? sector.name : string.Empty;
            }
            catch (Exception)
            {
                SectorName = "Unknown Sector";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
