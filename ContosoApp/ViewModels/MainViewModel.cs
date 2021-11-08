using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Models;
using Windows.System;

namespace QuanLyCauDuong.ViewModels
{
    /// <summary>
    /// Provides data and commands accessible to the entire app.  
    /// </summary>
    public class MainViewModel : BindableBase
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        /// <summary>
        /// Creates a new MainViewModel.
        /// </summary>
        public MainViewModel() => Task.Run(GetCustomerListAsync);

        /// <summary>
        /// The collection of bridges in the list. 
        /// </summary>
        public ObservableCollection<CustomerViewModel> Customers { get; }
            = new ObservableCollection<CustomerViewModel>();

        public ObservableCollection<BridgeViewModel> Bridges { get; }
            = new ObservableCollection<BridgeViewModel>();

        public ObservableCollection<BridgeViewModel> CloneBridges { get; set; }
            = new ObservableCollection<BridgeViewModel>();

        public ObservableCollection<UserViewModel> Users { get; }
             = new ObservableCollection<UserViewModel>();

        public ObservableCollection<HistoryViewModel> Histories { get; }
            = new ObservableCollection<HistoryViewModel>();

        private CustomerViewModel _selectedCustomer;

        /// <summary>
        /// Gets or sets the selected customer, or null if no customer is selected. 
        /// </summary>
        public CustomerViewModel SelectedCustomer
        {
            get => _selectedCustomer;
            set => Set(ref _selectedCustomer, value);
        }

        private BridgeViewModel _selectedBridge;

        /// <summary>
        /// Gets or sets the selected bridge, or null if no customer is selected. 
        /// </summary>
        public BridgeViewModel SelectedBridge
        {
            get => _selectedBridge;
            set => Set(ref _selectedBridge, value);
        }

        private Bridge _selectedBridge2;

        /// <summary>
        /// Gets or sets the currently selected order.
        /// </summary>
        public Bridge SelectedBridge2
        {
            get => _selectedBridge2;
            set => Set(ref _selectedBridge2, value);
        }

        private UserViewModel _selectedUser;

        /// <summary>
        /// Gets or sets the selected bridge, or null if no customer is selected. 
        /// </summary>
        public UserViewModel SelectedUser
        {
            get => _selectedUser;
            set => Set(ref _selectedUser, value);
        }

        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Customers list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        /// <summary>
        /// Gets the complete list of customers from the database.
        /// </summary>
        public async Task GetCustomerListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var customers = await App.Repository.Customers.GetAsync();
            if (customers == null)
            {
                return;
            }

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Customers.Clear();
                foreach (var c in customers)
                {
                    Customers.Add(new CustomerViewModel(c));
                }
                IsLoading = false;
            });
        }

        /// <summary>
        /// Gets the complete list of bridges from the database.
        /// </summary>
        public async Task GetBridgeListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var bridges = await App.Repository.Bridges.GetAsync();
            if (bridges == null)
            {
                return;
            }

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Bridges.Clear();
                foreach (var c in bridges)
                {
                    Bridges.Add(new BridgeViewModel(c));
                }

                CloneBridges = Bridges;
                IsLoading = false;
            });
        }

        /// <summary>
        /// Gets the complete list of bridges from the database.
        /// </summary>
        public async Task GetBuildingBridgeListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var bridges = await App.Repository.Bridges.GetWithCustomQueryAsync();
            if (bridges == null)
            {
                return;
            }

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Bridges.Clear();
                foreach (var c in bridges)
                {
                    Bridges.Add(new BridgeViewModel(c));
                }
                IsLoading = false;
            });
        }

        /// <summary>
        /// Gets the complete list of bridges from the database.
        /// </summary>
        public async Task GetUserListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var users = await App.Repository.Users.GetAsync();
            if (users == null)
            {
                return;
            }

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Users.Clear();
                foreach (var c in users)
                {
                    Users.Add(new UserViewModel(c));
                }
                IsLoading = false;
            });
        }

        /// <summary>
        /// Gets the complete list of bridges from the database.
        /// </summary>
        public async Task GetHistoryListAsync()
        {
            await dispatcherQueue.EnqueueAsync(() => IsLoading = true);

            var histories = await App.Repository.Histories.GetAsync();
            if (histories == null)
            {
                return;
            }

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Histories.Clear();
                foreach (var c in histories)
                {
                    Histories.Add(new HistoryViewModel(c));
                }
                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified customers and reloads the customer list from the database.
        /// </summary>
        public void Sync()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                foreach (var modifiedCustomer in Customers
                    .Where(customer => customer.IsModified).Select(customer => customer.Model))
                {
                    await App.Repository.Customers.UpsertAsync(modifiedCustomer);
                }

                await GetCustomerListAsync();
                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified customers and reloads the customer list from the database.
        /// </summary>
        public void BridgeSync()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                foreach (var modifiedBridge in Bridges
                    .Where(bridge => bridge.IsModified).Select(bridge => bridge.Model))
                {
                    await App.Repository.Bridges.UpsertAsync(modifiedBridge);
                }

                await GetBridgeListAsync();
                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified customers and reloads the customer list from the database.
        /// </summary>
        public void UserSync()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                foreach (var modifiedUser in Users
                    .Where(user => user.IsModified).Select(user => user.Model))
                {
                    await App.Repository.Users.UpsertAsync(modifiedUser);
                }

                await GetUserListAsync();
                IsLoading = false;
            });
        }

        /// <summary>
        /// Saves any modified customers and reloads the customer list from the database.
        /// </summary>
        public void HistorySync()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                foreach (var modifiedHistory in Histories
                    .Where(history => history.IsModified).Select(history => history.Model))
                {
                    await App.Repository.Histories.UpsertAsync(modifiedHistory);
                }

                await GetHistoryListAsync();
                IsLoading = false;
            });
        }

        /// <summary>
        /// Deletes the specified order from the database.
        /// </summary>
        public async Task DeleteBridge(Bridge bridgeToDelete) =>
            await App.Repository.Bridges.DeleteAsync(bridgeToDelete.Id);

        /// <summary>
        /// Deletes the specified order from the database.
        /// </summary>
        public async Task DeleteUser(Models.User userToDelete) =>
            await App.Repository.Users.DeleteAsync(userToDelete.Id);
    }
}
