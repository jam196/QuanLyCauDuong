﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Models;
using Microsoft.Toolkit.Uwp;
using Windows.System;

namespace QuanLyCauDuong.ViewModels
{
    /// <summary>
    /// Provides a bindable wrapper for the Bridge model class, encapsulating various services for access by the UI.
    /// </summary>
    public class BridgeViewModel : BindableBase, IEditableObject
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        /// <summary>
        /// Initializes a new instance of the BridgeViewModel class that wraps a Bridge object.
        /// </summary>
        public BridgeViewModel(Bridge model = null) => Model = model ?? new Bridge();

        private Bridge _model;

        /// <summary>
        /// Gets or sets the underlying Bridge object.
        /// </summary>
        public Bridge Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    /*RefreshOrders();
*/
                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's Name.
        /// </summary>
        public string Name
        {
            get => Model.Name;
            set
            {
                if (value != Model.Name)
                {
                    Model.Name = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's Investor.
        /// </summary>
        public string Investor
        {
            get => Model.Investor;
            set
            {
                if (value != Model.Investor)
                {
                    Model.Investor = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Investor));
                }
            }
        }

        /// <summary>
        /// Gets the bridge full (first + last) name.
        /// </summary>
        /*public string Name => IsNewBridge && string.IsNullOrEmpty(Name)
            && string.IsNullOrEmpty(Investor) ? "Thêm cầu" : $"{Name} {LastName}";*/

        /// <summary>
        /// Gets or sets the customer's address.
        /// </summary>
        public string Designer
        {
            get => Model.Designer;
            set
            {
                if (value != Model.Designer)
                {
                    Model.Designer = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's company.
        /// </summary>
        public string Company
        {
            get => Model.Builder;
            set
            {
                if (value != Model.Builder)
                {
                    Model.Builder = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's phone number. 
        /// </summary>
        public string Supervisor
        {
            get => Model.Supervisor;
            set
            {
                if (value != Model.Supervisor)
                {
                    Model.Supervisor = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's email. 
        /// </summary>
        public string Manager
        {
            get => Model.Manager;
            set
            {
                if (value != Model.Manager)
                {
                    Model.Manager = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's email. 
        /// </summary>
        public string Status
        {
            get => Model.Status;
            set
            {
                if (value != Model.Status)
                {
                    Model.Status = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's email. 
        /// </summary>
        public string Location
        {
            get => Model.Location;
            set
            {
                if (value != Model.Location)
                {
                    Model.Location = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the underlying model has been modified. 
        /// </summary>
        /// <remarks>
        /// Used when sync'ing with the server to reduce load and only upload the models that have changed.
        /// </remarks>
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets the collection of the bridge's orders.
        /// </summary>
        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();

        private Order _selectedOrder;

        /// <summary>
        /// Gets or sets the currently selected order.
        /// </summary>
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that indicates whether to show a progress bar. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private bool _isNewBridge;

        /// <summary>
        /// Gets or sets a value that indicates whether this is a new bridge.
        /// </summary>
        public bool IsNewBridge
        {
            get => _isNewBridge;
            set => Set(ref _isNewBridge, value);
        }

        private bool _isInEdit = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the bridge data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => _isInEdit;
            set => Set(ref _isInEdit, value);
        }

        /// <summary>
        /// Saves bridge data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;
            if (IsNewBridge)
            {
                IsNewBridge = false;
                App.ViewModel.Bridges.Add(this);
            }

            await App.Repository.Bridges.UpsertAsync(Model);
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the bridge data.
        /// </summary>
        public event EventHandler AddNewBridgeCanceled;

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public async Task CancelEditsAsync()
        {
            if (IsNewBridge)
            {
                AddNewBridgeCanceled?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                await RevertChangesAsync();
            }
        }

        /// <summary>
        /// Discards any edits that have been made, restoring the original values.
        /// </summary>
        public async Task RevertChangesAsync()
        {
            IsInEdit = false;
            if (IsModified)
            {
                await RefreshBridgeAsync();
                IsModified = false;
            }
        }

        /// <summary>
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;

        /// <summary>
        /// Reloads all of the customer data.
        /// </summary>
        public async Task RefreshBridgeAsync()
        {
            /*RefreshOrders();*/
            Model = await App.Repository.Bridges.GetAsync(Model.Id);
        }

        /// <summary>
        /// Resets the customer detail fields to the current values.
        /// </summary>
        /*public void RefreshOrders() => Task.Run(LoadOrdersAsync);*/

        /*/// <summary>
        /// Loads the order data for the customer.
        /// </summary>
        public async Task LoadOrdersAsync()
        {
            await dispatcherQueue.EnqueueAsync(() =>
            {
                IsLoading = true;
            });

            var orders = await App.Repository.Orders.GetForCustomerAsync(Model.Id);

            await dispatcherQueue.EnqueueAsync(() =>
            {
                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }

                IsLoading = false;
            });
        }*/

        /// <summary>
        /// Called when a bound DataGrid control causes the customer to enter edit mode.
        /// </summary>
        public void BeginEdit()
        {
            // Not used.
        }

        /// <summary>
        /// Called when a bound DataGrid control cancels the edits that have been made to a customer.
        /// </summary>
        public async void CancelEdit() => await CancelEditsAsync();

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a customer.
        /// </summary>
        public async void EndEdit() => await SaveAsync();
    }
}