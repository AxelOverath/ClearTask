using ClearTask.Views.Pc;

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
            }
        }

}

