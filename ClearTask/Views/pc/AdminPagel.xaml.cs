namespace ClearTask.Views.Pc;

public partial class AdminPage : ContentPage
{
    public AdminPage()
    {
        Title = "Admin Panel";
        Content = new VerticalStackLayout
        {
            Padding = 20,
            Children =
                {
                    new Label { Text = "Welcome, Admin!", FontSize = 24, TextColor = Colors.Red },
                    new Button { Text = "Manage Users" },
                    new Button { Text = "View Reports" }
                }
        };
    }
}