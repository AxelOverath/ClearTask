using MongoDB.Driver;
using ClearTask.Models;
using ClearTask.Data;
using ClearTask.Views.Pc;
using ClearTask;

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

        if (existingUser != null)
        {
            Console.WriteLine($"User found: {existingUser.username}");
            Console.WriteLine($"Entered Password: {password}");
            Console.WriteLine($"Stored Hashed Password: {existingUser.password}");

            if (BCrypt.Net.BCrypt.Verify(password, existingUser.password))
            {
                Console.WriteLine("Password verification successful");

                // Store user information in UserStorage
                UserStorage.Username = existingUser.username;
                UserStorage.Email = existingUser.email;
                UserStorage.Password = existingUser.password;
                UserStorage.UserRole = existingUser.userRole;

                if (existingUser.userRole == Role.Admin)
                {

                    CheckUserAccess();
                    await Navigation.PushAsync(new SectorsOverviewPage());
                }
                else
                {
                    await Navigation.PushAsync(new TaskList());
                }
            }
            else
            {
                Console.WriteLine("Password verification failed");
                await DisplayAlert("Error", "Email or password is incorrect", "OK");
            }
        }
        else
        {
            Console.WriteLine("No user found with this email/username");
            await DisplayAlert("Error", "No user found with this email/username.", "OK");
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

    private async Task CheckUserAccess()
    {
        var currentUser = await DatabaseService.GetCurrentUserAsync();

        // Conditie voor platform en rol
        bool isPc = DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst;
        bool isAdmin = currentUser != null && currentUser.userRole == Role.Admin;

        // Wacht op het hoofdthread voor wijzigingen in de UI
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            var adminNavBar = Shell.Current.FindByName<TabBar>("AdminNavBar");
            if (adminNavBar != null)
            {
                // Zorg ervoor dat de TabBar zichtbaar wordt voor admins op pc's
                adminNavBar.IsVisible = isPc && isAdmin;
            }
        });
    }


}
