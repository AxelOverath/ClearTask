using ClearTask.Data;
using ClearTask.Models;
using MongoDB.Bson;
namespace ClearTask.Views.Pc;


public partial class AddUserPage : ContentPage
{
    public AddUserPage()
    {
        InitializeComponent();
    }

    private async void OnAddUserConfirm(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text) ||
            RolePicker.SelectedItem == null)
        {
            await DisplayAlert("Fout", "Vul alle velden in en selecteer een rol.", "OK");
            return;
        }

        // Parse the selected role
        var selectedRole = Enum.Parse<Role>(RolePicker.SelectedItem.ToString());

        // Hash the password before storing
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(PasswordEntry.Text);

        // Create the new user object
        var newUser = new User
        {
            Id = ObjectId.GenerateNewId(),
            username = NameEntry.Text,
            email = EmailEntry.Text,
            password = hashedPassword, // Store the hashed password
            userRole = selectedRole
        };

        // Insert the new user into the database
        await DatabaseService.InsertUserAsync(newUser);

        // Show success message and navigate back to users page
        await DisplayAlert("Succes", "Gebruiker toegevoegd!", "OK");
        await Shell.Current.GoToAsync("users");
    }
    public async void OnBackClicked(object sender, EventArgs e)
    {
        // Navigate back to the UsersPage
        await Shell.Current.GoToAsync("..");
    }
}

