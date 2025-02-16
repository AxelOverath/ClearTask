using ClearTask.Data;
using ClearTask.Models;
using MongoDB.Driver;
using System.Collections.ObjectModel;

namespace ClearTask.Views.Pc;

public partial class UsersPage : ContentPage
{
    public ObservableCollection<User> Users { get; set; } = new();

    public UsersPage()
    {
        //InitializeComponent();
        
        LoadUsers();
    }

    private async void LoadUsers()
    {
        var users = await DatabaseService.GetAllUsersAsync();
        Users.Clear();
        foreach (var user in users)
        {
            Users.Add(user);
        }
    }

    private async void OnAddUserClicked(object sender, EventArgs e)
    {
       await Shell.Current.GoToAsync("addUser");
    }
}
