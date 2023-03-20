using CarListApp.Models;
using CarListApp.Services;

namespace CarListApp;

public partial class App : Application
{
	public static UserInfo UserInfo;
    public static CarDatabaseService CarService { get; private set; }

	public App(CarDatabaseService carService)
	{
		InitializeComponent();

		MainPage = new AppShell();
		CarService = carService;
	}
}
