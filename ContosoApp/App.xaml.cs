using QuanLyCauDuong.Views;
using QuanLyCauDuong.ViewModels;
using Repository;
using Repository.Rest;
using Repository.Sql;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Controls;
using System;

namespace QuanLyCauDuong
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Gets the app-wide MainViewModel singleton instance.
        /// </summary>
        public static MainViewModel ViewModel { get; } = new MainViewModel();

        /// <summary>
        /// Pipeline for interacting with backend service or database.
        /// </summary>
        public static IContosoRepository Repository { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() => InitializeComponent();

        /// <summary>
        /// Invoked when the application is launched normally by the end user.
        /// </summary>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Load the database.
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(
                "data_source", out object dataSource))
            {
                switch (dataSource.ToString())
                {
                    case "Rest": UseRest(); break;
                    default: UseSqlite(); break;
                }
            }
            else
            {
                UseSqlite();
            }

            // Prepare the app shell and window content.
            AppShell shell = Window.Current.Content as AppShell ?? new AppShell();
            shell.Language = ApplicationLanguages.Languages[0];
            Window.Current.Content = shell;

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                Models.User currentUser = await App.Repository.Users.GetByEmailAsync((string)ApplicationData.Current.RoamingSettings.Values["Email"]);

                if (currentUser != null)
                {
                    shell.AppFrame.Navigate(typeof(BridgeListPage), null, new SuppressNavigationTransitionInfo());
                }
                else
                {
                    shell.AppFrame.Navigate(typeof(ExportPage), null, new SuppressNavigationTransitionInfo());
                }
            }

            Window.Current.Activate();
        }

        /// <summary>
        /// Configures the app to use the Sqlite data source. If no existing Sqlite database exists, 
        /// loads a demo database filled with fake data so the app has content.
        /// </summary>
        public static void UseSqlite()
        {
            string demoDatabasePath = Package.Current.InstalledLocation.Path + @"\Assets\Contoso.db";
            string databasePath = ApplicationData.Current.LocalFolder.Path + @"\Contoso.db";
            if (!File.Exists(databasePath))
            {
                File.Copy(demoDatabasePath, databasePath);
            }
            var dbOptions = new DbContextOptionsBuilder<ContosoContext>().UseSqlite(
                "Data Source=" + databasePath);
            Repository = new SqlContosoRepository(dbOptions);
        }

        /// <summary>
        /// Configures the app to use the REST data source. For convenience, a read-only source is provided. 
        /// You can also deploy your own copy of the REST service locally or to Azure. See the README for details.
        /// </summary>
        public static void UseRest() =>
            Repository = new RestContosoRepository("https://customers-orders-api-prod.azurewebsites.net/api/");
    }
}