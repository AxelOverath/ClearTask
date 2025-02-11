using ClearTask.Views.Pc;
﻿using ClearTask.Views;


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
                
                GoToLogin();
            }
            
        public async void GoToLogin()
        {
            await Navigation.PushAsync(new Login());
        }

    }
}

