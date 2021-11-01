using QuanLyCauDuong.Views;
using System;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace QuanLyCauDuong
{
    /// <summary>
    /// The "chrome" layer of the app that provides top-level navigation with
    /// proper keyboarding navigation.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        /// <summary>
        /// Initializes a new instance of the AppShell, sets the static 'Current' reference,
        /// adds callbacks for Back requests and changes in the SplitView's DisplayMode, and
        /// provide the nav menu list with the data to display.
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                NavView.SelectedItem = BridgeListMenuItem;
            };

            // Set up custom title bar.
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Black;
            AppTitle.Text = Windows.ApplicationModel.Package.Current.DisplayName;
        }

        /// <summary>
        /// Gets the navigation frame instance.
        /// </summary>
        public Frame AppFrame => frame;

        /// <summary>
        /// Default keyboard focus movement for any unhandled keyboarding
        /// </summary>
        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case VirtualKey.Left:
                case VirtualKey.GamepadDPadLeft:
                case VirtualKey.GamepadLeftThumbstickLeft:
                case VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case VirtualKey.Right:
                case VirtualKey.GamepadDPadRight:
                case VirtualKey.GamepadLeftThumbstickRight:
                case VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case VirtualKey.Up:
                case VirtualKey.GamepadDPadUp:
                case VirtualKey.GamepadLeftThumbstickUp:
                case VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case VirtualKey.Down:
                case VirtualKey.GamepadDPadDown:
                case VirtualKey.GamepadLeftThumbstickDown:
                case VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;
            }

            if (direction != FocusNavigationDirection.None &&
                FocusManager.FindNextFocusableElement(direction) is Control control)
            {
                control.Focus(FocusState.Keyboard);
                e.Handled = true;
            }
        }

        public readonly string HomeLabel = "Dashboard";

        public readonly string BridgeListLabel = "Quản lý cầu";

        public readonly string CustomerListLabel = "Quản lý khách hàng";

        public readonly string UserListLabel = "Quản lý người dùng";

        public readonly string OrderListLabel = "Quản lý đơn hàng";

        public readonly string HistoryListLabel = "Quản lý hoạt động";

        public readonly string ExportLabel = "Xuất dữ liệu";

        /// <summary>
        /// Navigates to the page corresponding to the tapped item.
        /// </summary>
        private async void NavigationView_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            var label = args.InvokedItem as string;
            var pageType =
                args.IsSettingsInvoked ? typeof(SettingsPage) :
                label == CustomerListLabel ? typeof(CustomerListPage) :
                label == UserListLabel ? typeof(UserListPage) :
                label == HistoryListLabel ? typeof(HistoryListPage) :
                label == ExportLabel ? typeof(ExportPage) :
                label == HomeLabel ? typeof(Home) :
                label == BridgeListLabel ? typeof(BridgeListPage) :
                label == OrderListLabel ? typeof(OrderListPage) : null;

            Models.User currentUser = await App.Repository.Users.GetByEmailAsync((string)ApplicationData.Current.RoamingSettings.Values["Email"]);

            if (pageType != null && pageType != AppFrame.CurrentSourcePageType)
            {
                if (currentUser != null)
                {
                    AppFrame.Navigate(pageType);
                }
                else
                {
                    ContentDialog noWifiDialog = new ContentDialog
                    {
                        Title = "Bạn chưa đăng nhập",
                        Content = "Vui lòng đăng nhập để sử dụng chức năng này",
                        CloseButtonText = "Ok"
                    };

                    await noWifiDialog.ShowAsync();
                }
            }
        }

        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// </summary>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                /*if (e.SourcePageType == typeof(CustomerListPage))
              {
                  NavView.SelectedItem = CustomerListMenuItem;
              }*/
                if (e.SourcePageType == typeof(UserListPage))
                {
                    NavView.SelectedItem = UserListMenuItem;
                }
                else if (e.SourcePageType == typeof(Home))
                {
                    NavView.SelectedItem = HomeMenuItem;
                }
                else if (e.SourcePageType == typeof(HistoryListPage))
                {
                    NavView.SelectedItem = HistoryListMenuItem;
                }
                else if (e.SourcePageType == typeof(ExportPage))
                {
                    NavView.SelectedItem = ExportMenuItem;
                }
                /*else if (e.SourcePageType == typeof(OrderListPage))
                {
                    NavView.SelectedItem = OrderListMenuItem;
                }*/
                else if (e.SourcePageType == typeof(SettingsPage))
                {
                    NavView.SelectedItem = NavView.SettingsItem;
                }
                else if (e.SourcePageType == typeof(BridgeListPage))
                {
                    NavView.SelectedItem = BridgeListMenuItem;
                }
            }
        }

        /// <summary>
        /// Invoked when the View Code button is clicked. Launches the repo on GitHub. 
        /// </summary>
        private async void ViewCodeNavPaneButton_Tapped(object sender, TappedRoutedEventArgs e) =>
            await Launcher.LaunchUriAsync(new Uri(
                "https://github.com/jam196/QuanLyCauDuong"));

        /// <summary>
        /// Navigates the frame to the previous page.
        /// </summary>
        private void NavigationView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            if (AppFrame.CanGoBack)
            {
                AppFrame.GoBack();
            }
        }
    }
}
