using CarListApp.Models;
using CarListApp.Services;
using CarListApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CarListApp.ViewModels
{
    public partial class CarListViewModel : BaseViewModel
    {
        public ObservableCollection<Car> Cars { get; private set; } = new ObservableCollection<Car>();
        private readonly CarApiService carApiService;

        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        string message = string.Empty;

        public CarListViewModel(CarApiService carApiService) 
        {
            Title = "Car List";
            AddEditButtonText = "Add car";
            //GetCarList().Wait();
            this.carApiService = carApiService;
        }

        [ObservableProperty]
        bool isRefreshing;
        [ObservableProperty]
        string make;
        [ObservableProperty]
        string model;
        [ObservableProperty]
        string vin;
        [ObservableProperty]
        string addEditButtonText;

        [ObservableProperty]
        int? updatingCarId;

        [ICommand]
        async Task ClearForm()
        {
            Make = string.Empty;
            Model = string.Empty;
            Vin = string.Empty;
            UpdatingCarId = null;
            AddEditButtonText = "Add car";
        }

        [ICommand]
        async Task GetCarList()
        {
            if(IsLoading) return;
            try
            {
                IsLoading = true;
                if(Cars.Any()) Cars.Clear();


                var cars = new List<Car>();
                if(accessType == NetworkAccess.Internet) 
                {
                    cars = await carApiService.GetCars();
                }
                else
                {
                    cars = App.CarService.GetCars();
                }

                foreach (var car in cars) Cars.Add(car);

                //string fileName = "carlist.json";
                //var serializedList = JsonSerializer.Serialize(cars);
                //File.WriteAllText(fileName, serializedList);
                //var rawText = File.ReadAllText(fileName);
                //var carsFromText = JsonSerializer.Deserialize<List<Car>>(rawText);

                //string path = FileSystem.AppDataDirectory;
                //string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to get cars: {ex}");
                await Shell.Current.DisplayAlert("Error", "Failed to retrieve list of cars", "Ok");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        [ICommand]
        async Task GetCarDetails(int id)
        {
            if(id == 0) return;

            await Shell.Current.GoToAsync($"{nameof(CarDetailsPage)}?Id={id}", true);
            //await Shell.Current.GoToAsync(nameof(CarDetailsPage), true, new Dictionary<string, object> { { nameof(Car), car } });
        }

        [ICommand]
        async Task SaveCar()
        {
            if(string.IsNullOrEmpty(Make) || string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Vin))
            {
                await Shell.Current.DisplayAlert("Invalid data", "Please provide valid data", "Ok");
                return;
            }

            var car = new Car
            {
                Make = Make,
                Model = Model,
                Vin = Vin
            };

            if(updatingCarId.HasValue)
            {
                car.Id = updatingCarId.Value;
                await carApiService.UpdateCar(updatingCarId.Value, car);
                message = carApiService.StatusMessage;
            }
            else
            {
                await carApiService.AddCar(car);
            }

            await Shell.Current.DisplayAlert("Info", carApiService.StatusMessage, "Ok");
            await ClearForm();
            await GetCarList();
        }

        [ICommand]
        async Task DeleteCar(int id)
        {
            if(id == 0)
            {
                await Shell.Current.DisplayAlert("Invalid records", "Please try again", "Ok");
            }

            await carApiService.DeleteCar(id);
            //if(result == 0)
            //{
            //    await Shell.Current.DisplayAlert("Failed", App.CarService.StatusMessage, "Ok");
            //}
            //else
            //{
            //    await Shell.Current.DisplayAlert("Info", "Record was removed successfully", "Ok");
            //    await GetCarList();
            //}
        }

        [ICommand]
        async Task SetUpdateCar(int id)
        {
            if (id == 0)
            {
                await Shell.Current.DisplayAlert("Invalid records", "Please try again", "Ok");
                return;
            }

            updatingCarId = id;
            AddEditButtonText = "Update car";
            var car = await carApiService.GetCar(id);
            if (car != null)
            {
                Make = car.Make;
                Model = car.Model;
                Vin = car.Vin;
            }
        }
    }
}
