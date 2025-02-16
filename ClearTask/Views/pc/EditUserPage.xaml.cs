using ClearTask.Data;
using ClearTask.Models;
using Microsoft.Maui.Storage;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace ClearTask.Views.Pc;

[QueryProperty(nameof(UserId), "userId")]
public partial class EditUserPage : ContentPage
{
    public string UserId { get; set; }
    public User CurrentUser { get; set; }




    public EditUserPage()
    {
        InitializeComponent();
        BindingContext = this;
        

    }

    public List<Role> Roles { get; } = new() { Role.Admin, Role.Manager, Role.Handyman, Role.Employee };

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!string.IsNullOrEmpty(UserId))
        {
            var objectId = new ObjectId(UserId);
            CurrentUser = await DatabaseService.UsersCollection.Find(u => u.Id == objectId).FirstOrDefaultAsync();

            if (CurrentUser != null)
            {
                // Zet de string om naar de enum voor userRole
                CurrentUser.userRole = Enum.Parse<Role>(CurrentUser.userRole.ToString()); // Conversie van string naar enum

                // Stel de BindingContext in voor andere velden
                BindingContext = CurrentUser;

                // Stel de ItemsSource van de Picker in op de lijst van rollen
                RolePicker.ItemsSource = Roles;

                // Stel de geselecteerde rol in op de gebruiker
                RolePicker.SelectedItem = CurrentUser.userRole;
            }
        }
    }


    public async void OnSaveClicked(object sender, EventArgs e)
    {
        if (CurrentUser != null)
        {
            // Controleer of het wachtwoord is gewijzigd
            if (!string.IsNullOrEmpty(CurrentUser.password) && CurrentUser.password != "defaultValue")  // defaultValue kan je oorspronkelijke waarde zijn
            {
                // Versleutel het wachtwoord voordat je het opslaat
                CurrentUser.password = BCrypt.Net.BCrypt.HashPassword(CurrentUser.password);
            }




            // Update de gebruiker in de database
            await DatabaseService.UsersCollection.ReplaceOneAsync(u => u.Id == CurrentUser.Id, CurrentUser);

            DatabaseService.TriggerUserUpdatedEvent();

            // Ga terug naar de vorige pagina
            await Shell.Current.GoToAsync("..");
        }

    }
    public async void OnBackClicked(object sender, EventArgs e)
    {
        // Navigate back to the UsersPage
        await Shell.Current.GoToAsync("..");
    }
    public async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (CurrentUser != null)
        {
            var confirmation = await DisplayAlert("Bevestigen", "Weet je zeker dat je deze gebruiker wilt verwijderen?", "Ja", "Nee");

            if (confirmation)
            {
                await DatabaseService.UsersCollection.DeleteOneAsync(u => u.Id == CurrentUser.Id);

                await DisplayAlert("Succes", "Gebruiker verwijderd.", "OK");

                await Shell.Current.GoToAsync("..");
            }
        }
    }


}
