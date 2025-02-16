﻿using ClearTask.Views.Pc;
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
            tabBar.Items.Add(taskTab);

            // Extra tabs voor beheerders
            if (UserStorage.UserRole == Role.Admin)
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

                tabBar.Items.Add(adminSection);
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

