using ClearTask.Data;
using ClearTask.Models;
using Microsoft.Maui.Storage;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ClearTask.Views.Pc;

[QueryProperty(nameof(UserId), "userId")]
public partial class EditUserPage : ContentPage
{
    public string UserId { get; set; }
    public User CurrentUser { get; set; }

    // Extra properties for UI control
    public List<Role> Roles { get; set; } = new List<Role> { Role.Admin, Role.Manager, Role.Handyman, Role.Employee };

    private bool _isManager;
    public bool IsManager
    {
        get => _isManager;
        set
        {
            _isManager = value;
            OnPropertyChanged(nameof(IsManager));
        }
    }

    private bool _isAdmin;
    public bool IsAdmin
    {
        get => _isAdmin;
        set
        {
            _isAdmin = value;
            OnPropertyChanged(nameof(IsAdmin));
        }
    }

    private bool _canDeleteUser;
    public bool CanDeleteUser
    {
        get => _canDeleteUser;
        set
        {
            _canDeleteUser = value;
            OnPropertyChanged(nameof(CanDeleteUser));
        }
    }

    private bool _isEditable;
    public bool IsEditable
    {
        get => _isEditable;
        set
        {
            _isEditable = value;
            OnPropertyChanged(nameof(IsEditable));
        }
    }

    public EditUserPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        var loggedInUserRole = UserStorage.UserRole;

        IsManager = loggedInUserRole == Role.Manager;
        IsAdmin = loggedInUserRole == Role.Admin;

        if (!string.IsNullOrEmpty(UserId))
        {
            var objectId = new ObjectId(UserId);

            // ✅ Fetch user data properly
            var userFromDb = await DatabaseService.UsersCollection.Find(u => u.Id == objectId).FirstOrDefaultAsync();

            if (userFromDb != null)
            {
                CurrentUser = userFromDb; 
                OnPropertyChanged(nameof(CurrentUser)); 

                
                BindingContext = this;

                
                IsEditable = IsAdmin;
                CanDeleteUser = IsAdmin;

                
                RolePicker.ItemsSource = new List<Role> { Role.Admin, Role.Manager, Role.Handyman, Role.Employee };
                RolePicker.SelectedItem = CurrentUser.userRole;
            }
        }
    }

    public async void OnSaveClicked(object sender, EventArgs e)
    {
        if (CurrentUser != null)
        {
            // Als het wachtwoord is gewijzigd, versleutel het
            if (!string.IsNullOrEmpty(CurrentUser.password) && CurrentUser.password != "defaultValue")
            {
                CurrentUser.password = BCrypt.Net.BCrypt.HashPassword(CurrentUser.password);
            }

            // Update de gebruiker in de database
            await DatabaseService.UsersCollection.ReplaceOneAsync(u => u.Id == CurrentUser.Id, CurrentUser);

            DatabaseService.TriggerUserUpdatedEvent();

            await Shell.Current.GoToAsync("..");
        }
    }

    public async void OnBackClicked(object sender, EventArgs e)
    {
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
