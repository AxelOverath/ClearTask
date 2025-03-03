using ClearTask.Views.Pc;
﻿using ClearTask.Views;
using ClearTask.Data;
using ClearTask.Models;

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
            GoToLogin();
        }

        public async void GoToLogin()
        {
            // Zorg ervoor dat de navigatie op de hoofdthread gebeurt
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//Login");
            });
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
                Title = "Taken",
                ContentTemplate = new DataTemplate(typeof(TaskList)),
                Route = "tasks"
            };
            var taskcreatedtab = new ShellContent
            {
                Title = "Reported Tasks",
                ContentTemplate = new DataTemplate(typeof(ReportedTasklist)),
                Route = "ReportedTasklist"
            };
            tabBar.Items.Add(taskTab);
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

                managerSection.Items.Add(new ShellContent
                {
                    Title = "adminticketlist",
                    ContentTemplate = new DataTemplate(typeof(AdminTicketList)),
                    Route = "adminticketlist"
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

