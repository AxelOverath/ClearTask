using ClearTask.ViewModels;

namespace ClearTask.Views;
public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
        Shell.SetNavBarIsVisible(this, false);
    }

}
