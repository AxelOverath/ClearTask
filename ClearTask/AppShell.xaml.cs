using ClearTask.Views;

namespace ClearTask
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            GoToLogin();
        }

        public async void GoToLogin()
        {
            await Navigation.PushAsync(new Login());
        }

    }
}

