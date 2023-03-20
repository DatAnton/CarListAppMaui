using CommunityToolkit.Mvvm.Input;

namespace CarListApp.ViewModels
{
    public partial class LogoutViewModel : BaseViewModel
    {
        public LogoutViewModel() 
        {
            LogOut();
        }

        [ICommand]
        async void LogOut()
        {
            SecureStorage.Remove("Token");
            App.UserInfo = null;
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}