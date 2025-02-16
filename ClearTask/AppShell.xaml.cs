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
            
            // Routes registreren
            Routing.RegisterRoute("users", typeof(UsersPage));
                Routing.RegisterRoute("addUser", typeof(AddUserPage));
                Routing.RegisterRoute(nameof(EditUserPage), typeof(EditUserPage));
            GoToLogin();
            }
            
        public async void GoToLogin()
        {
            await Navigation.PushAsync(new Login());
        }



    }
}

