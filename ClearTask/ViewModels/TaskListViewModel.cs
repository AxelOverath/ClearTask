namespace ClearTask.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClearTask.Models;
public class TaskListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }

        public TaskListViewModel()
        {
            // Hardcoded list of tasks
            Tasks = new ObservableCollection<Task>
            {
                new Task
                {
                    Id = 1,
                    Title = "Fix plumbing issue",
                    Description = "Repair leaking pipe in the kitchen.",
                    Photo = "plumbing.png",
                    Tags = new List<string> { "Plumbing", "Urgent" },
                    Deadline = DateTime.Now.AddDays(3),
                    Status = TaskStatus.Pending,
                    AssignedTo = new Handyman { Name = "John Doe" },
                    Sector = new Sector { Name = "Home Repair" }
                },
                new Task
                {
                    Id = 2,
                    Title = "Paint living room",
                    Description = "Apply two coats of paint to walls.",
                    Photo = null,
                    Tags = new List<string> { "Painting", "Interior" },
                    Deadline = null,
                    Status = TaskStatus.InProgress,
                    AssignedTo = new Handyman { Name = "Jane Smith" },
                    Sector = new Sector { Name = "Interior Design" }
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
