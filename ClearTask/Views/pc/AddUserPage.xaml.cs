using ClearTask.Data;
using ClearTask.Models;
using Microsoft.Maui.Storage;
using MongoDB.Bson;
using System;
namespace ClearTask.Views.Pc;


public partial class AddUserPage : ContentPage
{
    public AddUserPage()
    {
        InitializeComponent();
        var users = DatabaseService.GetAllUsersAsync();
    }

    private async void OnAddUserConfirm(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text) || RolePicker.SelectedItem == null)
        {
            await DisplayAlert("Fout", "Vul alle velden in en selecteer een rol.", "OK");
            return;
        }

        var selectedRole = Enum.Parse<Role>(RolePicker.SelectedItem.ToString());

        var newUser = new User
        {
            Id = ObjectId.GenerateNewId(),
            Username = NameEntry.Text,
            Email = EmailEntry.Text,
            UserRole = selectedRole
        };

        DatabaseService.InsertUserAsync(newUser);
        await DisplayAlert("Succes", "Gebruiker toegevoegd!", "OK");
        await Shell.Current.GoToAsync("users");
    }
}

