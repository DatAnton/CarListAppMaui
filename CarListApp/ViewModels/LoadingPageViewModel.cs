using CarListApp.Helpers;
using CarListApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarListApp.ViewModels
{
    public partial class LoadingPageViewModel : BaseViewModel
    {
        public LoadingPageViewModel()
        {
            CheckUserLoginDetails();
        }

        private async void CheckUserLoginDetails()
        {
            //retrieve token from internal storage

            var token = await SecureStorage.GetAsync("Token");

            if(string.IsNullOrEmpty(token))
            {
                await GoToLoginPage();
            }
            else
            {
                //eveluate token and decide if valid
                var jsonToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow)
                {
                    SecureStorage.Remove("Token");
                    await GoToLoginPage();
                }
                else
                {
                    var role = jsonToken.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;
                    var email = jsonToken.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;

                    App.UserInfo = new UserInfo
                    {
                        Username = email,
                        Role = role
                    };

                    MenuBuilder.BuildMenu();
                    await GoToMainPage();
                }
            }
        }

        private async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true);
        }

        private async Task GoToMainPage()
        { 
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
