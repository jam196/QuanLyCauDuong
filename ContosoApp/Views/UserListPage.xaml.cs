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
    public sealed partial class UserListPage : Page
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public UserListPage()
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
        private void UserSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserSearchBox != null)
            {
                UserSearchBox.AutoSuggestBox.QuerySubmitted += UserSearchBox_QuerySubmitted;
                UserSearchBox.AutoSuggestBox.TextChanged += UserSearchBox_TextChanged;
                UserSearchBox.AutoSuggestBox.PlaceholderText = "Tìm người dùng...";
            }
        }

        /// <summary>
        /// Updates the search box items source when the user changes the search text.
        /// </summary>
        private async void UserSearchBox_TextChanged(AutoSuggestBox sender,
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
                        await ViewModel.GetUserListAsync());
                    sender.ItemsSource = null;
                }
                else
                {
                    string[] parameters = sender.Text.Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    sender.ItemsSource = ViewModel.Users
                        .Where(user => parameters.Any(parameter =>
                            user.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                            user.Email.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                        .OrderByDescending(user => parameters.Count(parameter =>
                            user.Name.StartsWith(parameter) ||
                            user.Email.StartsWith(parameter)))
                        .Select(user => $"{user.Name} {user.Email}");
                }
            }
        }

        /// Filters or resets the customer list based on the search text.
        /// </summary>
        private async void UserSearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.QueryText))
            {
                await ResetUserList();
            }
            else
            {
                await FilterUserList(args.QueryText);
            }
        }

        /// <summary>
        /// Resets the customer list.
        /// </summary>
        private async Task ResetUserList()
        {
            await dispatcherQueue.EnqueueAsync(async () => await ViewModel.GetUserListAsync());
        }

        /// <summary>
        /// Filters the customer list based on the search text.
        /// </summary>
        private async Task FilterUserList(string text)
        {
            string[] parameters = text.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            var matches = ViewModel.Users.Where(user => parameters
                .Any(parameter =>
                    user.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                    user.Email.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(user => parameters.Count(parameter =>
                    user.Name.StartsWith(parameter) ||
                    user.Email.StartsWith(parameter)))
                .ToList();

            await dispatcherQueue.EnqueueAsync(() =>
            {
                ViewModel.Users.Clear();
                foreach (var match in matches)
                {
                    ViewModel.Users.Add(match);
                }
            });
        }

        /// <summary>
        /// Resets the user list when leaving the page.
        /// </summary>
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await ResetUserList();
        }

        /// <summary>
        /// Applies any existing filter when navigating to the page.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UserSearchBox.AutoSuggestBox.Text))
            {
                await FilterUserList(UserSearchBox.AutoSuggestBox.Text);
            }
            else
            {
                await ResetUserList();
            }
        }

        /// <summary>
        /// Menu flyout click control for selecting a user and displaying details.
        /// </summary>
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedUser != null)
            {
                Frame.Navigate(typeof(UserDetailPage), ViewModel.SelectedUser.Model.Id,
                    new DrillInNavigationTransitionInfo());
            }
        }

        /// <summary>
        /// Deletes the currently selected order.
        /// </summary>
        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deleteUser = ViewModel.SelectedUser.Model;
                await ViewModel.DeleteUser(deleteUser);
                await ResetUserList();
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Xóa cầu thất bại",
                    Content = $"Gặp lỗi khi xóa " +
                        $"cầu #{ViewModel.SelectedUser.Name}:\n{ex.Message}",
                    PrimaryButtonText = "OK"
                };
                await dialog.ShowAsync();
            }
        }

        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            Frame.Navigate(typeof(UserDetailPage), ViewModel.SelectedUser.Model.Id,
                    new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Navigates to a blank User details page for the user to fill in.
        /// </summary>
        private void CreateUser_Click(object sender, RoutedEventArgs e) =>
            Frame.Navigate(typeof(UserDetailPage), null, new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Reverts all changes to the row if the row has changes but a cell is not currently in edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape &&
                ViewModel.SelectedUser != null &&
                ViewModel.SelectedUser.IsModified &&
                !ViewModel.SelectedUser.IsInEdit)
            {
                (sender as DataGrid).CancelEdit(DataGridEditingUnit.Row);
            }
        }

        /// <summary>
        /// Selects the tapped User. 
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedUser = (e.OriginalSource as FrameworkElement).DataContext as UserViewModel;

        /// <summary>
        /// Sorts the data in the DataGrid.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Users.Sort);
    }
}
