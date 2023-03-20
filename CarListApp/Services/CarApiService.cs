using CarListApp.Models;
using CarListApp.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CarListApp.Services
{
    public class CarApiService
    {
        HttpClient _httpClient;
        public static string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8010" : "http://localhost:8010";
        public string StatusMessage;

        public CarApiService()
        {
            _httpClient = new()
            {
                BaseAddress = new Uri(BaseAddress)
            };
        }

        public async Task<List<Car>> GetCars()
        {
            try
            {
                await SetAuthToken();
                var response = await _httpClient.GetStringAsync("/cars");
                return JsonConvert.DeserializeObject<List<Car>>(response);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }

            return null;
        }

        public async Task<Car> GetCar(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"/cars/{id}");
                return JsonConvert.DeserializeObject<Car>(response);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data";
            }
            return null;
        }

        public async Task AddCar(Car car)
        {
            try
            {
                if (car == null)
                {
                    throw new Exception("Invalid Car Record");
                }

                var response = await _httpClient.PostAsJsonAsync("/cars", car);
                response.EnsureSuccessStatusCode();
                StatusMessage = "Insert succesfull";
            }
            catch (Exception)
            {
                StatusMessage = "Failed to add data";
            }
        }

        public async Task DeleteCar(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/cars/{id}");
                response.EnsureSuccessStatusCode();
                StatusMessage = "Delete succesfull";
            }
            catch (Exception)
            {
                StatusMessage = "Failed to delete data";
            }
        }

        public async Task UpdateCar(int id, Car car)
        {
            try
            {
                if (car == null || car.Id == 0)
                {
                    throw new Exception("Invalid Car Record");
                }

                var response = await _httpClient.PutAsJsonAsync($"/cars/{id}", car);
                response.EnsureSuccessStatusCode();
                StatusMessage = "Update succesful";
            }
            catch (Exception)
            {
                StatusMessage = "Failed to update data";
            }
        }

        public async Task<AuthResponseModel> Login(LoginModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/login", loginModel);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<AuthResponseModel>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                StatusMessage = "Failed to login successfully.";
                return default;
            }
        }

        public async Task SetAuthToken()
        {
            var token = await SecureStorage.GetAsync("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
