using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Models;
using Microsoft.Toolkit.Uwp;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace QuanLyCauDuong.ViewModels
{
    /// <summary>
    /// Cung cấp một trình bao bọc có thể liên kết cho lớp mô hình Bridge, đóng gói các dịch vụ khác nhau để giao diện người dùng truy cập.
    /// </summary>
    public class HistoryViewModel : BindableBase, IEditableObject
    {
        /* private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();*/

        /// <summary>
        /// Khởi tạo một phiên bản mới của lớp BridgeViewModel bao bọc một đối tượng Bridge.
        /// </summary>
        public HistoryViewModel(History model = null) => Model = model ?? new History();

        private History _model;

        /// <summary>
        /// Lấy hoặc đặt đối tượng Bridge.
        /// </summary>
        public History Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Lấy tên cầu.
        /// </summary>
        public string Content
        {
            get => Model?.Content;
            set
            {
                if (value != Model.Content)
                {
                    Model.Content = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Content));
                }
            }
        }


        /// <summary>
        /// Lấy CreatedAt.
        /// </summary>
        public DateTime CreatedAt
        {
            get => Model.CreatedAt;
            set
            {
                if (value != Model.CreatedAt)
                {
                    Model.CreatedAt = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CreatedAt));
                }
            }
        }


        /// <summary>
        /// Lấy UpdatedAt.
        /// </summary>
        public DateTime UpdatedAt
        {
            get => Model.UpdatedAt;
            set
            {
                if (value != Model.UpdatedAt)
                {
                    Model.UpdatedAt = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UpdatedAt));
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

        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that indicates whether to show a progress bar. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private bool _isNewHistory;

        /// <summary>
        /// Gets or sets a value that indicates whether this is a new bridge.
        /// </summary>
        public bool IsNewHistory
        {
            get => _isNewHistory;
            set => Set(ref _isNewHistory, value);
        }

        private bool _isInEdit = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the History data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => _isInEdit;
            set => Set(ref _isInEdit, value);
        }

        /// <summary>
        /// Saves History data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;
            if (IsNewHistory)
            {
                IsNewHistory = false;
                App.ViewModel.Histories.Add(this);
            }

            await App.Repository.Histories.UpsertAsync(Model);
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
            if (IsNewHistory)
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
                await RefreshHistoryAsync();
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
        public async Task RefreshHistoryAsync()
        {
            Model = await App.Repository.Histories.GetAsync(Model.Id);
        }

        private async void DisplayNoWifiDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "Có lỗi xảy ra",
                Content = "Check your connection and try again.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

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