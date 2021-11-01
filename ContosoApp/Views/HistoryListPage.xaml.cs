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
    public sealed partial class HistoryListPage : Page
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public HistoryListPage()
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
        private void HistorySearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (HistorySearchBox != null)
            {
                HistorySearchBox.AutoSuggestBox.QuerySubmitted += HistorySearchBox_QuerySubmitted;
                HistorySearchBox.AutoSuggestBox.TextChanged += HistorySearchBox_TextChanged;
                HistorySearchBox.AutoSuggestBox.PlaceholderText = "Tìm hoạt động...";
            }
        }

        /// <summary>
        /// Updates the search box items source when the user changes the search text.
        /// </summary>
        private async void HistorySearchBox_TextChanged(AutoSuggestBox sender,
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
                        await ViewModel.GetHistoryListAsync());
                    sender.ItemsSource = null;
                }
                else
                {
                    string[] parameters = sender.Text.Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    sender.ItemsSource = ViewModel.Histories
                        .Where(history => parameters.Any(parameter =>
                            history.Content.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                        .OrderByDescending(history => parameters.Count(parameter =>
                            history.Content.StartsWith(parameter)))
                        .Select(history => $"{history.Content}");
                }
            }
        }

        /// Filters or resets the customer list based on the search text.
        /// </summary>
        private async void HistorySearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.QueryText))
            {
                await ResetHistoryList();
            }
            else
            {
                await FilterHistoryList(args.QueryText);
            }
        }

        /// <summary>
        /// Resets the customer list.
        /// </summary>
        private async Task ResetHistoryList()
        {
            await dispatcherQueue.EnqueueAsync(async () => await ViewModel.GetHistoryListAsync());
        }

        /// <summary>
        /// Filters the customer list based on the search text.
        /// </summary>
        private async Task FilterHistoryList(string text)
        {
            string[] parameters = text.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            var matches = ViewModel.Histories.Where(history => parameters
                .Any(parameter =>
                    history.Content.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(history => parameters.Count(parameter =>
                    history.Content.StartsWith(parameter)))
                .ToList();

            await dispatcherQueue.EnqueueAsync(() =>
            {
                ViewModel.Histories.Clear();
                foreach (var match in matches)
                {
                    ViewModel.Histories.Add(match);
                }
            });
        }

        /// <summary>
        /// Resets the history list when leaving the page.
        /// </summary>
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await ResetHistoryList();
        }

        /// <summary>
        /// Applies any existing filter when navigating to the page.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(HistorySearchBox.AutoSuggestBox.Text))
            {
                await FilterHistoryList(HistorySearchBox.AutoSuggestBox.Text);
            }
            else
            {
                await ResetHistoryList();
            }
        }
    }
}
