using System;
using System.Linq;
using System.Threading.Tasks;
using QuanLyCauDuong.ViewModels;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace QuanLyCauDuong.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BridgeListPage : Page
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public BridgeListPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the app-wide ViewModel instance.
        /// </summary>
        public MainViewModel ViewModel => App.ViewModel;

        /// <summary>
        /// Initializes the AutoSuggestBox portion of the search box.
        /// </summary>
        private void BridgeSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (BridgeSearchBox != null)
            {
                BridgeSearchBox.AutoSuggestBox.QuerySubmitted += BridgeSearchBox_QuerySubmitted;
                BridgeSearchBox.AutoSuggestBox.TextChanged += BridgeSearchBox_TextChanged;
                BridgeSearchBox.AutoSuggestBox.PlaceholderText = "Tìm cầu...";
            }
        }

        /// <summary>
        /// Updates the search box items source when the user changes the search text.
        /// </summary>
        private async void BridgeSearchBox_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // If no search query is entered, refresh the complete list.
                if (String.IsNullOrEmpty(sender.Text))
                {
                    await dispatcherQueue.EnqueueAsync(async () =>
                        await ViewModel.GetBridgeListAsync());
                    sender.ItemsSource = null;
                }
                else
                {
                    string[] parameters = sender.Text.Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    sender.ItemsSource = ViewModel.Bridges
                        .Where(bridge => parameters.Any(parameter =>
                            bridge.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                            bridge.Investor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                            bridge.Supervisor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                            bridge.Company.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                        .OrderByDescending(bridge => parameters.Count(parameter =>
                            bridge.Manager.StartsWith(parameter) ||
                            bridge.Location.StartsWith(parameter) ||
                            bridge.Designer.StartsWith(parameter) ||
                            bridge.Company.StartsWith(parameter)))
                        .Select(bridge => $"{bridge.Name} {bridge.Investor}");
                }
            }
        }

        /// Filters or resets the customer list based on the search text.
        /// </summary>
        private async void BridgeSearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.QueryText))
            {
                await ResetBridgeList();
            }
            else
            {
                await FilterBridgeList(args.QueryText);
            }
        }

        /// <summary>
        /// Resets the customer list.
        /// </summary>
        private async Task ResetBridgeList()
        {
            await dispatcherQueue.EnqueueAsync(async () =>
                await ViewModel.GetBridgeListAsync());
        }

        /// <summary>
        /// Filters the customer list based on the search text.
        /// </summary>
        private async Task FilterBridgeList(string text)
        {
            string[] parameters = text.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            var matches = ViewModel.Bridges.Where(bridge => parameters
                .Any(parameter =>
                    bridge.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                    bridge.Investor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                    bridge.Supervisor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                    bridge.Company.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(bridge => parameters.Count(parameter =>
                    bridge.Manager.StartsWith(parameter) ||
                    bridge.Location.StartsWith(parameter) ||
                    bridge.Designer.StartsWith(parameter) ||
                    bridge.Company.StartsWith(parameter)))
                .ToList();

            await dispatcherQueue.EnqueueAsync(() =>
            {
                ViewModel.Bridges.Clear();
                foreach (var match in matches)
                {
                    ViewModel.Bridges.Add(match);
                }
            });
        }

        /// <summary>
        /// Resets the bridge list when leaving the page.
        /// </summary>
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await ResetBridgeList();
        }

        /// <summary>
        /// Applies any existing filter when navigating to the page.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BridgeSearchBox.AutoSuggestBox.Text))
            {
                await FilterBridgeList(BridgeSearchBox.AutoSuggestBox.Text);
            }
        }

        /// <summary>
        /// Menu flyout click control for selecting a bridge and displaying details.
        /// </summary>
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedBridge != null)
            {
                Frame.Navigate(typeof(BridgeDetailPage), ViewModel.SelectedBridge.Model.Id,
                    new DrillInNavigationTransitionInfo());
            }
        }

        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            Frame.Navigate(typeof(BridgeDetailPage), ViewModel.SelectedBridge.Model.Id,
                    new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Navigates to a blank bridge details page for the user to fill in.
        /// </summary>
        private void CreateBridge_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(BridgeDetailPage), null, new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Reverts all changes to the row if the row has changes but a cell is not currently in edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape &&
                ViewModel.SelectedBridge != null &&
                ViewModel.SelectedBridge.IsModified &&
                !ViewModel.SelectedBridge.IsInEdit)
            {
                (sender as DataGrid).CancelEdit(DataGridEditingUnit.Row);
            }
        }

        /// <summary>
        /// Selects the tapped customer. 
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedBridge = (e.OriginalSource as FrameworkElement).DataContext as CustomerViewModel;

        /// <summary>
        /// Opens the order detail page for the user to create an order for the selected customer.
        /// </summary>
        private void AddOrder_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(OrderDetailPage), ViewModel.SelectedBridge.Model.Id);

        /// <summary>
        /// Sorts the data in the DataGrid.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Bridges.Sort);
    }
}
