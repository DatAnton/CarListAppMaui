using CarListApp.ViewModels;

namespace CarListApp.Views;

public partial class CarDetailsPage : ContentPage
{
    private readonly CarDetailsViewModel carDetailsViewModel;

    public CarDetailsPage(CarDetailsViewModel carDetailsViewModel)
	{
		InitializeComponent();
		BindingContext = carDetailsViewModel;
        this.carDetailsViewModel = carDetailsViewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {

        base.OnNavigatedTo(args);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await carDetailsViewModel.GetCarData();
    }
}