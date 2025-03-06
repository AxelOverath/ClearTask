using ClearTask.Views.Pc;
﻿using ClearTask.Views;
using ClearTask.Data;
using ClearTask.Models;
using ClearTask.ViewModels;

namespace ClearTask
{


    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("users", typeof(UsersPage));
            Routing.RegisterRoute("addUser", typeof(AddUserPage));
            Routing.RegisterRoute(nameof(EditUserPage), typeof(EditUserPage));
            Routing.RegisterRoute(nameof(TagEditPage), typeof(TagEditPage));
            Routing.RegisterRoute("tagcreate", typeof(TagCreatePage));
            Routing.RegisterRoute("tagoverview", typeof(TagOverviewPage));
            Routing.RegisterRoute("dashboard", typeof(ManagerDashboardPage));
            Routing.RegisterRoute("adminticketlist", typeof(AdminTicketList));
            Routing.RegisterRoute("ReportedTasklist", typeof(ReportedTasklist));
            Routing.RegisterRoute(nameof(TaskDetailViewModel), typeof(TaskDetailViewModel));
            Routing.RegisterRoute(nameof(TaskDetailPageEdit), typeof(TaskDetailPageEdit));
            Routing.RegisterRoute("Sectoren", typeof(SectorsOverviewPage));
            Routing.RegisterRoute("Login", typeof(Login));
            Routing.RegisterRoute("tasks", typeof(TaskList));
            Routing.RegisterRoute("MyTaskList", typeof(MyTaskList));


        }

        public void SetupTabs()
        {
            // Verwijder bestaande items
            Items.Clear();

            // Nieuwe TabBar aanmaken
            var tabBar = new TabBar();
            // Algemene tab voor taken
            var taskTab = new ShellContent
            {
                Icon = "task.png",
                ContentTemplate = new DataTemplate(typeof(TaskList)),
                Route = "tasks"
            };
            var taskcreatedtab = new ShellContent
            {
                Icon ="done.png",
                ContentTemplate = new DataTemplate(typeof(ReportedTasklist)),
                Route = "ReportedTasklist"
            };
            var mytasktab = new ShellContent
            {
                Icon = "assign.png",
                ContentTemplate = new DataTemplate(typeof(MyTaskList)),
                Route = "MyTaskList"
            };

            

            // Only add the task tab if the user is not an Employee
            if (UserStorage.UserRole != Role.Employee)
            {
                tabBar.Items.Add(taskTab);
                if(UserStorage.UserRole == Role.Handyman)
                {
                    tabBar.Items.Add(mytasktab);
                }
            }
            tabBar.Items.Add(taskcreatedtab);
            // Extra tabs voor beheerders
            if (UserStorage.UserRole == Role.Admin && (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst))
            {
                var adminSection = new ShellSection { Title = "Beheer" };

                adminSection.Items.Add(new ShellContent
                {
                    Title = "Gebruikers",
                    ContentTemplate = new DataTemplate(typeof(UsersPage)),
                    Route = "users"
                });

                adminSection.Items.Add(new ShellContent
                {
                    Title = "Tags",
                    ContentTemplate = new DataTemplate(typeof(TagOverviewPage)),
                    Route = "tagoverview"
                });

                adminSection.Items.Add(new ShellContent
                {
                    Title = "adminticketlist",
                    ContentTemplate = new DataTemplate(typeof(AdminTicketList)),
                    Route = "adminticketlist"
                });
                adminSection.Items.Add(new ShellContent
                {
                    Title = "Sector List",
                    ContentTemplate = new DataTemplate(typeof(SectorsOverviewPage)),
                    Route = "Sectoren"
                });

                tabBar.Items.Add(adminSection);
            }

            // Extra tabs voor beheerders
            if (UserStorage.UserRole == Role.Manager && (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst))
            {
                var managerSection = new ShellSection { Title = "Manage" };

                managerSection.Items.Add(new ShellContent
                {
                    Title = "dashboard",
                    ContentTemplate = new DataTemplate(typeof(ManagerDashboardPage)),
                    Route = "dashboard"
                });

                managerSection.Items.Add(new ShellContent
                {
                    Title = "Gebruikers",
                    ContentTemplate = new DataTemplate(typeof(UsersPage)),
                    Route = "users"
                });


                tabBar.Items.Add(managerSection);
            }

            // Voeg de TabBar toe
            Items.Add(tabBar);

            // Kleurinstellingen via Shell.Set
            Shell.SetTabBarBackgroundColor(this, Color.FromArgb("#288d52"));
            Shell.SetTabBarForegroundColor(this, Colors.Aquamarine);
            Shell.SetTabBarUnselectedColor(this, Colors.White);
        }

    }

}

