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
    /// Cung cấp một trình bao bọc có thể liên kết cho lớp mô hình User, đóng gói các dịch vụ khác nhau để giao diện người dùng truy cập.
    /// </summary>
    public class UserViewModel : BindableBase, IEditableObject
    {
        /* private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();*/

        /// <summary>
        /// Khởi tạo một phiên bản mới của lớp UserViewModel bao bọc một đối tượng User.
        /// </summary>
        public UserViewModel(Models.User model = null) => Model = model ?? new Models.User();

        private Models.User _model;

        /// <summary>
        /// Gets or sets the user's ID.
        /// </summary>
        public Guid Id
        {
            get => Model.Id;
            set
            {
                if (Model.Id != value)
                {
                    Model.Id = value;
                    OnPropertyChanged();
                    IsModified = true;
                }
            }
        }

        /// <summary>
        /// Lấy hoặc đặt đối tượng User.
        /// </summary>
        public Models.User Model
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
        /// Lấy tên cầu.
        /// </summary>
        public string Email
        {
            get => Model?.Email;
            set
            {
                if (value != Model.Email)
                {
                    Model.Name = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        /// <summary>
        /// Lấy tên cầu.
        /// </summary>
        public string Avatar
        {
            get => Model?.Avatar;
            set
            {
                if (value != Model.Avatar)
                {
                    Model.Avatar = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Avatar));
                }
            }
        }

        /// <summary>
        /// Lấy tên cầu.
        /// </summary>
        public string Role
        {
            get => Model?.Role;
            set
            {
                if (value != Model.Role)
                {
                    Model.Role = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Role));
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

        private bool _isNewUser;

        /// <summary>
        /// Gets or sets a value that indicates whether this is a new User.
        /// </summary>
        public bool IsNewUser
        {
            get => _isNewUser;
            set => Set(ref _isNewUser, value);
        }

        private bool _isInEdit = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the User data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => _isInEdit;
            set => Set(ref _isInEdit, value);
        }

        /// <summary>
        /// Saves User data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;
            if (IsNewUser)
            {
                IsNewUser = false;
                App.ViewModel.Users.Add(this);
            }

            await App.Repository.Users.UpsertAsync(Model);
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the User data.
        /// </summary>
        public event EventHandler AddNewUserCanceled;

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public async Task CancelEditsAsync()
        {
            if (IsNewUser)
            {
                AddNewUserCanceled?.Invoke(this, EventArgs.Empty);
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
                await RefreshUserAsync();
                IsModified = false;
            }
        }

        /// <summary>
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;

        /// <summary>
        /// Get user by email.
        /// </summary>
        public async Task<Models.User> GetUserByEmailAsync(String email)
        {
            return await App.Repository.Users.GetByEmailAsync(email);
        }

        /// <summary>
        /// Reloads all of the customer data.
        /// </summary>
        public async Task RefreshUserAsync()
        {
            /*RefreshOrders();*/
            Model = await App.Repository.Users.GetAsync(Model.Id);
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