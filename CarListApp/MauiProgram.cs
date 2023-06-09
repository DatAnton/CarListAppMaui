﻿using CarListApp.Services;
using CarListApp.ViewModels;
using CarListApp.Views;

namespace CarListApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "cars.db3");

		builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<CarDatabaseService>(s, dbPath));

		builder.Services.AddTransient<CarApiService>();

		builder.Services.AddSingleton<CarListViewModel>();
		builder.Services.AddSingleton<LoadingPageViewModel>();
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<LogoutViewModel>();
		builder.Services.AddTransient<CarDetailsViewModel>();

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<LoadingPage>();
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<LogoutPage>();
		builder.Services.AddTransient<CarDetailsPage>();

		return builder.Build();
	}
}
