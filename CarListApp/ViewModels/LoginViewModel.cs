using CarListApp.Helpers;
using CarListApp.Models;
using CarListApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarListApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly CarApiService carApiService;
        public LoginViewModel(CarApiService carApiService)
        {
            this.carApiService = carApiService;
        }

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [ICommand]
        async Task Login()
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayLoginMessage("Invalid username or password");
                Password = string.Empty;
            }
            else
            {
                //Call Api to attempt to login

                var loginModel = new LoginModel(username, password);
                var response = await carApiService.Login(loginModel);

                //display wellcome message
                await DisplayLoginMessage(carApiService.StatusMessage);

                if (!string.IsNullOrEmpty(response?.Token))
                {
                    //store token
                    await SecureStorage.SetAsync("Token", response.Token);
                    //build menu
                    var jsonToken = new JwtSecurityTokenHandler().ReadToken(response.Token) as JwtSecurityToken;
                    var role = jsonToken.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;

                    App.UserInfo = new UserInfo
                    {
                        Username = Username,
                        Role = role
                    };

                    //navigate to main page
                    MenuBuilder.BuildMenu();
                    await Shell.Current.GoToAsync($"{nameof(MainPage)}");
                }
                else
                {
                    await DisplayLoginMessage("Invalid username or password");

                }
            }
        }

        async Task DisplayLoginMessage(string message)
        {
            await Shell.Current.DisplayAlert("Login attempt result", message, "Ok");
            Password = string.Empty;
        }
    }

}