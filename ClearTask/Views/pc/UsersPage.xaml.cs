using ClearTask.Data;
using ClearTask.Models;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClearTask.Views.Pc;

public partial class UsersPage : ContentPage
{
    public ObservableCollection<User> Users { get; set; } = new();
    public ObservableCollection<User> FilteredUsers { get; set; } = new();

    public List<string> Roles { get; set; } = new() { "Alle", "Admin", "Manager", "Handyman", "Employee" };

    private string selectedRole = "Alle"; // Standaardwaarde

    public ICommand UserClickedCommand { get; private set; }

    private Role CurrentUserRole = UserStorage.UserRole; // Simulatie van ingelogde gebruiker (moet uit je auth-systeem komen)

    public UsersPage()
    {
        InitializeComponent();
        BindingContext = this;

        LoadUsers();
        DatabaseService.UserUpdated += async () => await LoadUsers();
        UserClickedCommand = new Microsoft.Maui.Controls.Command<User>(OnUserClicked);

        SetRoleVisibility(); // Controleer de rol en pas UI aan
    }

    private void SetRoleVisibility()
    {
        var currentUser = UserStorage.UserRole; // Haal de ingelogde gebruiker op
        if (currentUser != null)
        {
            if (currentUser == Role.Admin)
            {
                IsRoleFilterVisible = true;
            }
            else
            {
                IsRoleFilterVisible = false;
                selectedRole = "Handyman"; // Managers mogen alleen Handymen zien
            }
        }
    }

    private void OnRoleFilterChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem != null)
        {
            selectedRole = picker.SelectedItem.ToString() ?? "Alle";
            FilterUsers(SearchBar.Text, selectedRole);
        }
    }

    private async Task LoadUsers()
    {
        try
        {
            var allUsers = await DatabaseService.UsersCollection.Find(_ => true).ToListAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Users = new ObservableCollection<User>(allUsers);

                // Filter Handymen als de ingelogde gebruiker een Manager is
                if (CurrentUserRole == Role.Manager)
                {
                    FilteredUsers = new ObservableCollection<User>(Users.Where(user => user.userRole == Role.Handyman));
                }
                else
                {
                    FilteredUsers = new ObservableCollection<User>(Users);
                }

                OnPropertyChanged(nameof(Users));
                OnPropertyChanged(nameof(FilteredUsers));
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij laden van gebruikers: {ex.Message}");
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        FilterUsers(e.NewTextValue, selectedRole);
    }

    private void FilterUsers(string searchText, string role)
    {
        searchText = searchText?.ToLower() ?? "";
        var filteredList = Users
            .Where(user =>
                (string.IsNullOrEmpty(searchText) || user.username.ToLower().Contains(searchText)) &&
                (selectedRole == "Alle" || user.userRole.ToString() == selectedRole) &&
                (UserStorage.UserRole != Role.Manager || user.userRole == Role.Handyman)
            )
            .ToList();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            FilteredUsers.Clear();
            foreach (var user in filteredList)
            {
                FilteredUsers.Add(user);
            }
        });
    }

    private bool _isRoleFilterVisible;
    public bool IsRoleFilterVisible
    {
        get => _isRoleFilterVisible;
        set
        {
            _isRoleFilterVisible = value;
            OnPropertyChanged(nameof(IsRoleFilterVisible));
        }
    }

    private async void OnUserClicked(User user)
    {
        if (user != null)
        {
            await Shell.Current.GoToAsync($"{nameof(EditUserPage)}?userId={user.Id}");
        }
    }

    private async void OnAddUserClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("addUser");
    }
}
