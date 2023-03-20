using CarListApp.Controls;

namespace CarListApp.Helpers
{
    public static class MenuBuilder
    {
        public static void BuildMenu()
        {
            Shell.Current.Items.Clear();
            Shell.Current.FlyoutHeader = new FlyOutHeader();

            var role = App.UserInfo.Role;
            if(role.Equals("Admin"))
            {
                var flyoutItem = new FlyoutItem
                {
                    Title = "Admin Car Management",
                    Route = nameof(MainPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = "dotnet_bot.svg",
                            Title = "Admin Page 1",
                            ContentTemplate = new DataTemplate(typeof(MainPage))
                        },
                        new ShellContent
                        {
                            Icon = "dotnet_bot.svg",
                            Title = "Admin Page 2",
                            ContentTemplate = new DataTemplate(typeof(MainPage))
                        }
                    }
                };

                if(!Shell.Current.Items.Contains(flyoutItem))
                {
                    Shell.Current.Items.Add(flyoutItem);
                }
            }
            else if(role.Equals("User"))
            {
                var flyoutItem = new FlyoutItem
                {
                    Title = "User Car Management",
                    Route = nameof(MainPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = "dotnet_bot.svg",
                            Title = "User Page 1",
                            ContentTemplate = new DataTemplate(typeof(MainPage))
                        },
                        new ShellContent
                        {
                            Icon = "dotnet_bot.svg",
                            Title = "User Page 2",
                            ContentTemplate = new DataTemplate(typeof(MainPage))
                        }
                    }
                };

                if (!Shell.Current.Items.Contains(flyoutItem))
                {
                    Shell.Current.Items.Add(flyoutItem);
                }
            }

            var logoutFlyoutItem = new FlyoutItem
            {
                Title = "Logout",
                Route = nameof(LogoutPage),
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
                Items =
                {
                    new ShellContent
                    {
                        Icon = "dotnet_bot.svg",
                        Title = "Logout",
                        ContentTemplate = new DataTemplate(typeof(LogoutPage))
                    }
                }
            };

            if (!Shell.Current.Items.Contains(logoutFlyoutItem))
            {
                Shell.Current.Items.Add(logoutFlyoutItem);
            }
        }
    }
}
