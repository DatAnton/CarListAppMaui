using CarListApp.Models;
using CarListApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Web;

namespace CarListApp.ViewModels
{
    //[QueryProperty(nameof(Car), "Car")]
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class CarDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly CarApiService carApiService;

        [ObservableProperty]
        Car car;

        [ObservableProperty]
        int id;

        public CarDetailsViewModel(CarApiService carApiService)
        {
            this.carApiService = carApiService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Id = Convert.ToInt32(HttpUtility.UrlDecode(query[nameof(Id)].ToString()));
            //Car = carApiService.GetCar();
        }

        public async Task GetCarData()
        {
            Car = await carApiService.GetCar(Id);
        }
    }
}
