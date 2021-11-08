using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Models;
using Microsoft.Toolkit.Uwp;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

namespace QuanLyCauDuong.ViewModels
{
    /// <summary>
    /// Cung cấp một trình bao bọc có thể liên kết cho lớp mô hình Bridge, đóng gói các dịch vụ khác nhau để giao diện người dùng truy cập.
    /// </summary>
    public class BridgeViewModel : BindableBase, IEditableObject
    {
        /* private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();*/

        /// <summary>
        /// Khởi tạo một phiên bản mới của lớp BridgeViewModel bao bọc một đối tượng Bridge.
        /// </summary>
        public BridgeViewModel(Bridge model = null) => Model = model ?? new Bridge();
        public HistoryViewModel ViewModelHistory { get; set; }

        private Bridge _model;

        /// <summary>
        /// Lấy hoặc đặt đối tượng Bridge.
        /// </summary>
        public Bridge Model
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
        public Guid UserId
        {
            get => Model.UserId;
            set
            {
                if (value != Model.UserId)
                {
                    Model.UserId = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }

        /// <summary>
        /// Lấy tên cầu.
        /// </summary>
        public string Name
        {
            get => Model?.Name;
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
        /// Get/set thông tin chủ đầu tư cầu.
        /// </summary>
        public string Investor
        {
            get => Model?.Investor;
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
        /// Get/set thông tin tổng đầu tư cầu.
        /// </summary>
        public string TotalInvestment
        {
            get => Model?.TotalInvestment.ToString();
            set
            {
                if (value != Model?.TotalInvestment.ToString())
                {
                    Model.TotalInvestment = float.Parse(value);
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalInvestment));
                }
            }
        }

        /// <summary>
        /// Get/set thông tin thời gian bắt đầu xây cầu.
        /// </summary>
        public DateTimeOffset? StartTime
        {
            get => Model?.StartTime;
            set
            {
                if (value != Model?.StartTime)
                {
                    Model.StartTime = (DateTimeOffset)value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }

        /// <summary>
        /// Get/set thông tin đơn vị thi công cầu.
        /// </summary>
        public string Builder
        {
            get => Model?.Builder;
            set
            {
                if (value != Model.Builder)
                {
                    Model.Builder = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Builder));
                }
            }
        }

        /// <summary>
        /// Get/set tải trọng thiết kế.
        /// </summary>
        public string DesignLoad
        {
            get => Model?.DesignLoad.ToString();
            set
            {
                if (value != Model.DesignLoad.ToString())
                {
                    Model.DesignLoad = float.Parse(value);
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DesignLoad));
                }
            }
        }

        /// <summary>
        /// Get/set thời gian kết thúc xây cầu.
        /// </summary>
        public DateTimeOffset? EndTime
        {
            get => Model?.EndTime;
            set
            {
                if (value != Model?.EndTime)
                {
                    Model.EndTime = (DateTimeOffset)value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }

        /// <summary>
        /// Get/set đơn vị thiết kế cầu.
        /// </summary>
        public string Designer
        {
            get => Model?.Designer;
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
        /// Gets or sets the bridge's phone number. 
        /// </summary>
        public string Supervisor
        {
            get => Model?.Supervisor;
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
            get => Model?.Manager;
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
            get => Model?.Status;
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
        /// Gets or sets the bridge's location. 
        /// </summary>
        public string Location
        {
            get => Model?.Location;
            set
            {
                if (value != Model?.Location)
                {
                    Model.Location = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's location. 
        /// </summary>
        public double Latitude
        {
            get => Model.Latitude;
            set
            {
                if (value != Model?.Latitude)
                {
                    Model.Latitude = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bridge's location. 
        /// </summary>
        public double Longitude
        {
            get => Model.Longitude;
            set
            {
                if (value != Model?.Longitude)
                {
                    Model.Longitude = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Lấy CreatedAt.
        /// </summary>
        public DateTime CreatedAt
        {
            get => Model.CreatedAt;
            /*set
            {
                if (value != Model.CreatedAt)
                {
                    Model.CreatedAt = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CreatedAt));
                }
            }*/
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

            String content = "Người dùng " + ApplicationData.Current.RoamingSettings.Values["Email"] + " đã sửa thông tin " + Name + " trong hệ thống";

            if (IsNewBridge)
            {
                IsNewBridge = false;
                App.ViewModel.Bridges.Add(this);

                content = "Người dùng " + ApplicationData.Current.RoamingSettings.Values["Email"] + " đã thêm thông tin " + Name + " vào hệ thống";
            }

            ViewModelHistory = new HistoryViewModel
            {
                IsNewHistory = IsNewBridge,
                Content = content,
                IsInEdit = true
            };
            await ViewModelHistory.SaveAsync();

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