using System.Windows.Input;
using MongoDB.Driver;
using ClearTask.Models;
using ClearTask.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace ClearTask.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string identifier;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool rememberMe;

        public ICommand SignInCommand { get; }

        public LoginViewModel()
        {
            SignInCommand = new AsyncRelayCommand(SignInAsync);

            // Load saved credentials
            LoadRememberedCredentials();
        }

        private async Task SignInAsync()
        {
            if (string.IsNullOrWhiteSpace(Identifier) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            var existingUser = await CheckIfUserExists(Identifier);

            if (existingUser != null && BCrypt.Net.BCrypt.Verify(Password, existingUser.password))
            {
                // Save user details in a static storage class
                UserStorage.Username = existingUser.username;
                UserStorage.Email = existingUser.email;
                UserStorage.Password = existingUser.password;
                UserStorage.UserRole = existingUser.userRole;

                Console.WriteLine("Login successful!");

                // Handle "Remember Me"
                if (RememberMe)
                {
                    SecureStorage.SetAsync("identifier", Identifier);
                    SecureStorage.SetAsync("password", Password);
                }
                else
                {
                    SecureStorage.Remove("identifier");
                    SecureStorage.Remove("password");
                }

                // Update AppShell navigation
                (Application.Current.MainPage as AppShell)?.SetupTabs();

                await Shell.Current.GoToAsync("//tasks");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email or password is incorrect", "OK");
            }
        }

        private async Task<User> CheckIfUserExists(string identifier)
        {
            Console.WriteLine($"Searching for user: {identifier}");

            var filter = Builders<User>.Filter.Or(
                Builders<User>.Filter.Eq(u => u.email, identifier),
                Builders<User>.Filter.Eq(u => u.username, identifier)
            );

            return await DatabaseService.UsersCollection.Find(filter).FirstOrDefaultAsync();
        }

        private async void LoadRememberedCredentials()
        {
            string savedIdentifier = await SecureStorage.GetAsync("identifier");
            string savedPassword = await SecureStorage.GetAsync("password");

            if (!string.IsNullOrEmpty(savedIdentifier) && !string.IsNullOrEmpty(savedPassword))
            {
                Identifier = savedIdentifier;
                Password = savedPassword;
                RememberMe = true;
            }
        }
    }
}
