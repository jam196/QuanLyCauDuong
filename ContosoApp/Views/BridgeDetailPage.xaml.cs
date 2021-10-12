using System;
using System.Linq;
using Models;
using QuanLyCauDuong.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace QuanLyCauDuong.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BridgeDetailPage : Page
    {
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                ViewModel = new BridgeViewModel
                {
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

                var saveDialog = new SaveChangesDialog() { Title = $"Save changes?" };
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

        /// <summary>
        /// Navigates to the order page for the Bridge.
        /// </summary>
        private void ViewOrderButton_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(OrderDetailPage), ((sender as FrameworkElement).DataContext as Order).Id,
                new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Adds a new order for the Bridge.
        /// </summary>
        private void AddOrder_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(OrderDetailPage), ViewModel.Model.Id);

        /// <summary>
        /// Sorts the data in the DataGrid.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Orders.Sort);
    }
}
