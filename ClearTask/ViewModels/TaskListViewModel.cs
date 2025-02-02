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
                Tags = new List<Tag>
                {
                    new Tag("Plumbing", "Related to plumbing issues"),
                },
                Deadline = DateTime.Now.AddDays(3),
                Status = TaskStatus.Pending,
                AssignedTo = new Handyman { Username = "JohnDoe" },
                Sector = new Sector(1,"Campus Kaai")
            },
            new Task
            {
                Id = 2,
                Title = "Paint living room",
                Description = "Apply two coats of paint to walls.",
                Photo = null,
                Tags =  new List<Tag>
                {
                    new Tag("Plumbing", "Related to plumbing issues"),
                    new Tag("Electricity", "Needs immediate attention")
                },
                Deadline = DateTime.Now.AddDays(7), // Default to a week if null was intended
                Status = TaskStatus.InProgress,
                AssignedTo = new Handyman { Username = "JaneSmith" },
                Sector = new Sector(1,"Campus Kaai")
            }
        };
    }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
