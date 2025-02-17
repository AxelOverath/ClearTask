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

    public UsersPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadUsers();
        DatabaseService.UserUpdated += async () => await LoadUsers();
        UserClickedCommand = new Microsoft.Maui.Controls.Command<User>(OnUserClicked);
    }

    private async Task LoadUsers()
    {
        try
        {
            var allUsers = await DatabaseService.UsersCollection.Find(_ => true).ToListAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Users = new ObservableCollection<User>(allUsers);
                FilteredUsers = new ObservableCollection<User>(allUsers);
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

    private void OnRoleFilterChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        selectedRole = picker.SelectedItem?.ToString() ?? "Alle";
        FilterUsers(SearchBar.Text, selectedRole);
    }

    private void FilterUsers(string searchText, string role)
    {
        searchText = searchText?.ToLower() ?? "";
        var filteredList = Users
            .Where(user => (string.IsNullOrEmpty(searchText) || user.username.ToLower().Contains(searchText)) &&
                           (role == "Alle" || user.userRole.ToString() == role))
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
