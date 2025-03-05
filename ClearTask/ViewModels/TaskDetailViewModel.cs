using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ClearTask.Models;
using ClearTask.Data; // Ensure this namespace contains DatabaseService
using MongoDB.Bson;
using MongoDB.Driver;

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
                LoadTaskImageAsync(); // Load image when task is set
            }
        }

        private byte[] _taskImageData;
        public byte[] TaskImageData
        {
            get => _taskImageData;
            set
            {
                _taskImageData = value;
                OnPropertyChanged(nameof(TaskImageSource));
            }
        }

        public ImageSource TaskImageSource => ConvertToImageSource(TaskImageData);

        private ImageSource ConvertToImageSource(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return "fallback_image.png"; // Set a default/fallback image if null
            return ImageSource.FromStream(() => new MemoryStream(imageData));
        }

        // Load Image Data from MongoDB
        private async void LoadTaskImageAsync()
        {
            try
            {
                var taskFromDb = await DatabaseService.TaskCollection
                    .Find(t => t.Id == Task.Id)
                    .FirstOrDefaultAsync();

                if (taskFromDb != null && taskFromDb.photo != null)
                {
                    TaskImageData = taskFromDb.photo;
                }
                else
                {
                    TaskImageData = null; // No image available
                }
            }
            catch (Exception)
            {
                TaskImageData = null; // Handle errors gracefully
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

            // Load Assigned User
            if (Task.assignedTo != ObjectId.Empty)
            {
                IsAssignedUserVisible = true;
                LoadAssignedUserNameAsync();
            }
            else
            {
                IsAssignedUserVisible = false;
            }

            // Load Sector
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

                AssignedUserName = user != null ? user.username : "Unknown User";
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

                SectorName = sector != null ? sector.name : "Unknown Sector";
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
