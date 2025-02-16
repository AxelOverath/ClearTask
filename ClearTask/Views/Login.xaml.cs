using MongoDB.Driver;
using ClearTask.Models;
using ClearTask.Data;
using ClearTask.Views.Pc;

namespace ClearTask.Views;
public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    public async void SignIn(object sender, EventArgs e)
    {
        string identifier = identifierInput.Text?.Trim();
        string password = passwordInput.Text;

        if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Please enter both email and password.", "OK");
            return;
        }

        var existingUser = await CheckIfUserExists(identifier);

        if (existingUser != null && BCrypt.Net.BCrypt.Verify(password, existingUser.password))
        {
            UserStorage.Username = existingUser.username;
            UserStorage.Email = existingUser.email;
            UserStorage.Password = existingUser.password;
            UserStorage.UserRole = existingUser.userRole;

            Console.WriteLine("Login succesvol!");

            // Roep SetupTabs aan om de TabBar dynamisch te genereren
            (Application.Current.MainPage as AppShell)?.SetupTabs();

            // Ga naar de juiste pagina
            await Shell.Current.GoToAsync("//tasks");
        }
        else
        {
            await DisplayAlert("Error", "Email or password is incorrect", "OK");
        }
    }



    private async Task<User> CheckIfUserExists(string identifier)
    {
        // Fetch all users for debugging
        var allUsers = await DatabaseService.UsersCollection.Find(_ => true).ToListAsync();

        Console.WriteLine($"Total Users in Database: {allUsers.Count}");

        foreach (var user in allUsers)
        {
            Console.WriteLine($"User ID: {user.Id}, Username: {user.username}, Email: {user.email}, Password: {user.password}");
        }

        // Debug: Searching for the specific user
        Console.WriteLine($"Searching for user with identifier: {identifier}");

        var filter = Builders<User>.Filter.Or(
            Builders<User>.Filter.Eq(u => u.email, identifier),
            Builders<User>.Filter.Eq(u => u.username, identifier)
        );

        var userFound = await DatabaseService.UsersCollection.Find(filter).FirstOrDefaultAsync();

        if (userFound != null)
        {
            Console.WriteLine($"User found: {userFound.username}, Email: {userFound.email}");
        }
        else
        {
            Console.WriteLine("No user found in database.");
        }

        return userFound;
    }

}
