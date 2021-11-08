using System;
using System.Linq;
using Models;
using QuanLyCauDuong.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Services.Maps;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace QuanLyCauDuong.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BridgeDetailPage : Page
    {
        string[] Investors = { "Tổng công ty Hoàng Anh - TNHH MTV", "Công ty Tập đoàn Thăng Long", "Sở Giao thông vận tải Hà Nội", "Tập đoàn Xây Dựng cầu đường", "Tổng CTCP Xuất nhập khẩu và Xây dựng Việt Nam", "Công ty CP Ecoba", "Công ty CP xây dựng Coteccons", "Tập đoàn CBRE", "Công ty tài chính cổ phần Điện lực", "Margaret Yang", "Tổng Công ty Đầu tư phát triển cầu đường UDIC", "Tập đoàn xây dựng Delta", "Tập Đoàn TIMA", "Lotte Finance", "Nhà thầu Fecon", "Công ty TNHH BHNT Prudential Việt Nam", "Benjamin Graham", "Công ty CP Đầu tư Xây dựng Newtecons", "Công ty CP Tập đoàn đầu tư cầu đường", "Công ty CP Xây dựng Phục Hưng Holdings", "Trần Đô Thành", "Cục Quản lý đường bộ Thăng Long", "Nhà thầu Becamex", "Công ty Tập đoàn Xây Dựng Hòa Bình", "Nhà Đỏ Group", "Tổng công ty thăm dò & khai thác cầu đường", "Công ty CP Tập đoàn Hòa Phát", "Tập đoàn Vingroup" };
        string[] Designers = { "Xây Dựng Công Trình Hoàng Anh", "Xây Dựng - Công Ty Xây Dựng", "Bộ GTVT", "Xây Dựng tổng hợp Bản Việt", "Xây Lắp công trình COMA", "Xây Dựng Hiệp Phúc", "Xây Dựng Thắng Hòa Phát", "Cát Xây Dựng", "Xây Dựng CECICO579", "Kiến Trúc và xây dựng Hòa Bình", "Conteccons", "Xây dựng tổng hợp Ricons", "Vật liệu - Xây dựng Delta", "Unicons Designs", "Cường Thuận IDICO", "Fecon Designs", "Thi công xây lắp công trình Dân dụng", "Xây Dựng Số 18.3", "Quang Minh", "GoGo", "Kiến Trúc Đăng Phát", "CSAMCO", "Tcty Xây Dựng Cầu Thăng Long", "Bê Tông Becamex", "Xây Dựng Vương Trần", "Thiết Kế Xây Dựng Nhà Đỏ", "Xây Dựng Dân Dụng Nguyễn Nguyên Phát", "Hoàng Long An", "Thương Mại Hoàn Cầu" };
        string[] Builders = { "Công Ty TNHH Kỹ Thuật Xây Dựng Công Trình Hoàng Anh", "Tổng Công Ty Thăng Long CTCP", "Tổng Công Ty Xây Dựng Công Trình Giao Thông 1", "Công Ty Cổ Phần Đầu Tư Và Phát Triển Bản Việt", "Công Ty Cổ Phần Xây Lắp Và Thương Mại COMA 25", "Công Ty TNHH Xây Dựng Và Thương Mại Dịch Vụ Hiệp Phúc 4", "Công Ty TNHH Thắng Hòa Phát", "Sand Resource Investment Co..,Ltd", "Công Ty Cổ Phần Đầu Tư & Xây Dựng 579", "Công ty CP Tập đoàn xây dựng Hoà Bình", "Công ty CP xây dựng Coteccons", "Công ty CP Đầu tư Xây dựng Ricons", "Công ty TNHH Tập đoàn xây dựng Delta", "Công ty TNHH Đầu tư Xây dựng Unicons", "Công Ty Cổ Phần Đầu Tư Phát Triển Cường Thuận IDICO", "Công ty CP Fecon", "Công ty TNHH Xây dựng tổng hợp Thắng Đạt", "Công Ty Cổ Phần Đầu Tư Và Xây Dựng Số 18.3", "Công Ty TNHH Quang Minh", "Công Ty Cổ Phần Tập Đoàn Liên Doanh Hồng Thái", "Công Ty TNHH Đầu Tư Xây Dựng", "Xí Nghiệp Đầu Tư Xây Dựng Đô Thành", "Công Ty Cổ Phần Cầu 5 Thăng Long", "Công Ty Cổ Phần Đầu Tư Và Xây Dựng Bình Dương ACC", "Công Ty TNHH Xây Dựng Thương Mại Và Dịch Vụ Vương Trần", "Công Ty CP Thiết Kế Xây Dựng Thương Mại Trang Trí Nội Thất Nhà Đỏ", "Công Ty TNHH Nguyễn Nguyên Phát", "Tổng Công Ty Tư Vấn Thiết Kế Giao Thông Vận Tải", "Công Ty TNHH Xây Dựng & Môi Trường Hoàng Long An", "Công Ty TNHH Xây Dựng Dịch Vụ" };
        string[] Supervisors = { "Ứng dụng Bản đồ Việt", "Đơn Vị Tư Vấn Giám Sát TEXO", "Cục quản lý đường bộ khu vực", "Tư vấn đầu tư và thiết kế xây dựng CDC", "Tư vấn kiểm định xây dựng quốc tế ICCI", "Đơn vị Savills Việt Nam", "Giám sát Hòa Phát", "Monitoring department SRI", "Tư vấn quản lý xây dựng Delta", "Uỷ ban nhân dân tỉnh Hòa Bình", "Conteccons", "Monitoring department Ricons", "The Ascott Limited – Capitaland", "HANCIC USC", "Dịch vụ giám sát cầu đường Newtecons", "Đơn vị Fecon", "Công ty đầu tư phát triển nhà và đô thị HUD", "Cục Quản lý đường bộ", "Ứng dụng Tư vấn & Giám sát Bảo Sơn", "GoGo", "Đơn vị Vinhomes", "Công ty TNHH Artelia Việt Nam", "Công ty tư vấn giám sát xây dựng Nhà Phố Group", "Monitoring Home Red", "Giám sát xây dựng HUD", "Công ty cổ phần tư vấn xây dựng Nagecco" };
        string[] Managers = { "Đầu tư TSG Việt Nam", "Quản lý Thăng Long", "Cục quản lý đường bộ khu vực", "VietSun", "Tư vấn Quản lý Xây dựng COMA 25", "Quản lý Xây Dựng và Thương Mại Hiệp Phúc", "Công ty phát triển dự án Song Nam", "Công ty quản lý xây dựng cao cấp CBRE", "Quản lý dự án & đầu tư CECICO", "Ủy ban nhân dân tỉnh Hòa Bình", "Công ty dịch vụ tiện ích quốc tế OCS", "Ricons Group", "Quản Lý và Khai thác Pan Services", "Tập đoàn xây dựng Newtecons", "Đơn vị Fencon", "Quản Lý Xây Dựng INPLY", "Cục Quản lý đường bộ", "Quản lý Bảo Sơn", "GoGo Group", "Đơn vị Vinhomes", "Ủy ban nhân dân cấp tỉnh Bình Dương ", "DV An ninh Phương Đông STC", "Nhà đỏ Group", "Quản lý và kiểm tra HUD", "Quản lý XD & MT HLA", "Vingroup" };
        string[] Statuses = { "Đang xây dựng", "Đang bảo trì", "Hoạt động tốt" };

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public BridgeDetailPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Used to bind the UI to the data.
        /// </summary>
        public BridgeViewModel ViewModel { get; set; }

        /// <summary>
        /// Navigate to the previous page when the user cancels the creation of a new Bridge record.
        /// </summary>
        private void AddNewBridgeCanceled(object sender, EventArgs e) => Frame.GoBack();

        /// <summary>
        /// Displays the selected Bridge data.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Models.User currentUser = await App.Repository.Users.GetByEmailAsync((string)ApplicationData.Current.RoamingSettings.Values["Email"]);

            var bridgesByUserId = App.ViewModel.Bridges.Where(bridge => bridge.Model.UserId == currentUser.Id).FirstOrDefault();

            if (e.Parameter == null)
            {

                ViewModel = new BridgeViewModel
                {
                    UserId = currentUser.Id,
                    IsNewBridge = true,
                    IsInEdit = true
                };

                VisualStateManager.GoToState(this, "NewBridge", false);
            }
            else
            {
                ViewModel = App.ViewModel.Bridges.Where(
                    bridge => bridge.Model.Id == (Guid)e.Parameter).First();
            }

            ViewModel.AddNewBridgeCanceled += AddNewBridgeCanceled;
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Check whether there are unsaved changes and warn the user.
        /// </summary>
        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (ViewModel.IsModified)
            {
                // Cancel the navigation immediately, otherwise it will continue at the await call. 
                e.Cancel = true;

                void resumeNavigation()
                {
                    if (e.NavigationMode == NavigationMode.Back)
                    {
                        Frame.GoBack();
                    }
                    else
                    {
                        Frame.Navigate(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo);
                    }
                }

                var saveDialog = new SaveChangesDialog() { Title = $"Lưu thay đổi?" };
                await saveDialog.ShowAsync();
                SaveChangesDialogResult result = saveDialog.Result;

                switch (result)
                {
                    case SaveChangesDialogResult.Save:
                        await ViewModel.SaveAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.DontSave:
                        await ViewModel.RevertChangesAsync();
                        resumeNavigation();
                        break;
                    case SaveChangesDialogResult.Cancel:
                        break;
                }
            }

            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// Disconnects the AddNewBridgeCanceled event handler from the ViewModel 
        /// when the parent frame navigates to a different page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.AddNewBridgeCanceled -= AddNewBridgeCanceled;
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void BridgeSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UserControls.CollapsibleSearchBox searchBox)
            {
                searchBox.AutoSuggestBox.QuerySubmitted += BridgeSearchBox_QuerySubmitted;
                searchBox.AutoSuggestBox.TextChanged += BridgeSearchBox_TextChanged;
                searchBox.AutoSuggestBox.PlaceholderText = "Search...";
            }
        }

        /// <summary>
        /// Queries the database for a bridge result matching the search text entered.
        /// </summary>
        private async void BridgeSearchBox_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // If no search query is entered, refresh the complete list.
                if (string.IsNullOrEmpty(sender.Text))
                {
                    sender.ItemsSource = null;
                }
                else
                {
                    sender.ItemsSource = await App.Repository.Bridges.GetAsync(sender.Text);
                }
            }
        }

        /// <summary>
        /// Search by Bridge name, email, or phone number, then display results.
        /// </summary>
        private void BridgeSearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is Bridge bridge)
            {
                Frame.Navigate(typeof(BridgeDetailPage), bridge.Id);
            }
        }

        public static readonly Geopoint SeattleGeopoint = new Geopoint(new BasicGeoposition()
        {
            Latitude = 21.0434302,
            Longitude = 105.8567339
        });

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string addressToGeocode = sender.Text;

                BasicGeoposition queryHint = new BasicGeoposition();
                queryHint.Latitude = 21.0434302;
                queryHint.Longitude = 105.8567339;
                Geopoint hintPoint = new Geopoint(queryHint);

                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(addressToGeocode, hintPoint, 3);
                sender.ItemsSource = result.Locations;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            MapLocation selectedLocation = (MapLocation)args.SelectedItem;

            myMap.Center = selectedLocation.Point;
            myMap.ZoomLevel = 18;

            ViewModel.Latitude = selectedLocation.Point.Position.Latitude;
            ViewModel.Longitude = selectedLocation.Point.Position.Longitude;
            ViewModel.Location = selectedLocation.Address.FormattedAddress;
        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            myMap.Center = SeattleGeopoint;
            myMap.ZoomLevel = 18;
            SetMapStyle();
            SetMapProjection();
        }

        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage
        };

        /// <summary>
        /// Display a message to the user.
        /// This method may be called from any thread.
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            // If called from the UI thread, then update immediately.
            // Otherwise, schedule a task on the UI thread to perform the update.
            if (Dispatcher.HasThreadAccess)
            {
                UpdateStatus(strMessage, type);
            }
            else
            {
                var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UpdateStatus(strMessage, type));
            }
        }

        private void UpdateStatus(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }

            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
                StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
                StatusPanel.Visibility = Visibility.Collapsed;
            }

            // Raise an event if necessary to enable a screen reader to announce the status update.
            var peer = FrameworkElementAutomationPeer.FromElement(StatusBlock);
            if (peer != null)
            {
                peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            }
        }

        private void MyMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            var tappedGeoPosition = args.Location.Position;
            ViewModel.Latitude = tappedGeoPosition.Latitude;
            ViewModel.Longitude = tappedGeoPosition.Longitude;
            string status = "Kinh độ: " + tappedGeoPosition.Longitude + "\nVĩ độ: " + tappedGeoPosition.Latitude;
            NotifyUser(status, NotifyType.StatusMessage);
        }

        private void TrafficFlowVisible_Checked(object sender, RoutedEventArgs e)
        {
            myMap.TrafficFlowVisible = true;
        }

        private void trafficFlowVisibleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            myMap.TrafficFlowVisible = false;
        }

        private void styleCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Protect against events that are raised before we are fully initialized.
            if (myMap != null)
            {
                SetMapStyle();
            }
        }

        private void SetMapStyle()
        {
            switch (styleCombobox.SelectedIndex)
            {
                case 0:
                    myMap.Style = MapStyle.None;
                    break;
                case 1:
                    myMap.Style = MapStyle.Road;
                    break;
                case 2:
                    myMap.Style = MapStyle.Aerial;
                    break;
                case 3:
                    myMap.Style = MapStyle.AerialWithRoads;
                    break;
                case 4:
                    myMap.Style = MapStyle.Terrain;
                    break;
            }
        }

        private void projectionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Protect against events that are raised before we are fully initialized.
            if (myMap != null)
            {
                SetMapProjection();
            }
        }

        private void SetMapProjection()
        {
            switch (projectionCombobox.SelectedIndex)
            {
                case 0:
                    myMap.MapProjection = MapProjection.WebMercator;
                    break;
                case 1:
                    myMap.MapProjection = MapProjection.Globe;
                    break;
            }
        }

        private async void launchURI_Click(object sender, RoutedEventArgs e)
        {
            // The URI to launch
            var uriBing = new Uri(@"bingmaps:?cp=" + ViewModel.Latitude.ToString() + "~" + ViewModel.Longitude.ToString() + "&lvl=18");
            await Windows.System.Launcher.LaunchUriAsync(uriBing);
        }
    }
}
