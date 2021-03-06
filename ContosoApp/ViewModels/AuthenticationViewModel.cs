using System;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Windows.ApplicationModel.Core;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace QuanLyCauDuong.ViewModels
{
    /// <summary>
    /// Handles user authentication and getting user info from the Microsoft Graph API.
    /// </summary>
    public class AuthenticationViewModel : BindableBase
    {
        public UserViewModel UserModel { get; set; }

        /// <summary>
        /// Creates a new AuthenticationViewModel for logging users in and getting their info.
        /// </summary>
        public AuthenticationViewModel()
        {
            Task.Run(PrepareAsync);
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += BuildAccountsPaneAsync;
        }

        private string _name;

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _email;

        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private string _title;

        /// <summary>
        /// Gets or sets the user's standard title.
        /// </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private string _domain;

        /// <summary>
        /// Gets or sets the user's AAD domain.
        /// </summary>
        public string Domain
        {
            get => _domain;
            set => Set(ref _domain, value);
        }

        private BitmapImage _photo;

        /// <summary>
        /// Gets or sets the user's photo.
        /// </summary>
        public BitmapImage Photo
        {
            get => _photo;
            set => Set(ref _photo, value);
        }

        private string _errorText;

        /// <summary>
        /// Gets or sets error text to show if the login operation fails.
        /// </summary>
        public string ErrorText
        {
            get => _errorText;
            set => Set(ref _errorText, value);
        }

        private bool _showWelcome;

        /// <summary>
        /// Gets or sets whether to show the starting welcome UI. 
        /// </summary>
        public bool ShowWelcome
        {
            get => _showWelcome;
            set => Set(ref _showWelcome, value);
        }

        private bool _showLoading;

        /// <summary>
        /// Gets or sets whether to show the logging in progress UI.
        /// </summary>
        public bool ShowLoading
        {
            get => _showLoading;
            set => Set(ref _showLoading, value);
        }

        private bool _showData;

        /// <summary>
        /// Gets or sets whether to show user data UI.
        /// </summary>
        public bool ShowData
        {
            get => _showData;
            set => Set(ref _showData, value);
        }

        private bool _showError;

        /// <summary>
        /// Gets or sets whether to show the error UI.
        /// </summary>
        public bool ShowError
        {
            get => _showError;
            set => Set(ref _showError, value);
        }

        /// <summary>
        /// Prepares the login sequence. 
        /// </summary>
        public async Task PrepareAsync()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn") &&
                (bool)ApplicationData.Current.RoamingSettings.Values["IsLoggedIn"])
            {
                await SetVisibleAsync(vm => vm.ShowLoading);
                await LoginAsync();
            }
            else
            {
                await SetVisibleAsync(vm => vm.ShowWelcome);
            }
        }

        /// <summary>
        /// Logs the user in by requesting a token and using it to query the
        /// Microsoft Graph API.
        /// </summary>
        public async Task LoginAsync()
        {
            try
            {
                await SetVisibleAsync(vm => vm.ShowLoading);
                string token = await GetTokenAsync();
                if (token != null)
                {
                    ApplicationData.Current.RoamingSettings.Values["IsLoggedIn"] = true;
                    await SetUserInfoAsync(token);
                    await SetUserPhoto(token);
                    await SetVisibleAsync(vm => vm.ShowData);

                    bool isNew = false;

                    Models.User checkUserExist = await App.Repository.Users.GetByEmailAsync(Email);

                    if (checkUserExist == null)
                    {
                        isNew = true;
                    }

                    UserModel = new UserViewModel
                    {
                        IsNewUser = isNew,
                        Avatar = "",
                        Role = "user"
                    };

                    UserModel.Model.Name = Name;
                    UserModel.Model.Email = Email;

                    await UserModel.SaveAsync();
                }
                else
                {
                    await SetVisibleAsync(vm => vm.ShowError);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("is part of a key and so cannot"))
                {
                    ErrorText = ex.Message;
                    await SetVisibleAsync(vm => vm.ShowError);
                }
            }
        }

        public WebAccount account;

        /// <summary>
        /// Gets an auth token for the user, which can be used to call the Microsoft Graph API.
        /// </summary>
        private async Task<string> GetTokenAsync()
        {
            var provider = await GetAadProviderAsync();
            var request = new WebTokenRequest(provider, "User.Read",
                Repository.Constants.AccountClientId);
            request.Properties.Add("resource", "https://graph.microsoft.com");
            var result = await WebAuthenticationCoreManager.GetTokenSilentlyAsync(request);
            if (result.ResponseStatus != WebTokenRequestStatus.Success)
            {
                result = await WebAuthenticationCoreManager.RequestTokenAsync(request);
            }
            account = result.ResponseData[0].WebAccount;
            return result.ResponseStatus == WebTokenRequestStatus.Success ?
                result.ResponseData[0].Token : null;
        }

        /// <summary>
        /// Gets and processes the user's info from the Microsoft Graph API.
        /// </summary>
        private async Task SetUserInfoAsync(string token)
        {
            var users = await Windows.System.User.FindAllAsync();
            var graph = new GraphServiceClient(new DelegateAuthenticationProvider(message =>
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return Task.CompletedTask;
            }));

            var me = await graph.Me.Request().GetAsync();

            ApplicationData.Current.RoamingSettings.Values["Email"] = me.UserPrincipalName;

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
            {
                Name = me.DisplayName;
                Email = me.UserPrincipalName;
                Title = me.JobTitle;
                Domain = (string)await users[0].GetPropertyAsync(KnownUserProperties.DomainName);
            });
        }

        /// <summary>
        /// Gets and processes the user's photo from the Microsoft Graph API. 
        /// </summary>
        private async Task SetUserPhoto(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string url = "https://graph.microsoft.com/beta/me/photo/$value";
                var result = await client.GetAsync(url);
                if (!result.IsSuccessStatusCode)
                {
                    return;
                }
                using (Stream stream = await result.Content.ReadAsStreamAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal, async () =>
                        {
                            Photo = new BitmapImage();
                            await Photo.SetSourceAsync(memoryStream.AsRandomAccessStream());
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the AccountsSettingsPane with AAD login.
        /// </summary>
        private async void BuildAccountsPaneAsync(AccountsSettingsPane sender,
            AccountsSettingsPaneCommandsRequestedEventArgs args)
        {
            var deferral = args.GetDeferral();
            args.HeaderText = "Vui lòng chọn một trong các tài khoản dưới đây.";
            var command = new WebAccountProviderCommand(await GetAadProviderAsync(), async (x) =>
                await LoginAsync());
            args.WebAccountProviderCommands.Add(command);
            deferral.Complete();
        }

        private void GetTwitterTokenAsync(WebAccountProviderCommand command)
        {
            // Manually handle Twitter login here
        }

        /// <summary>
        /// Gets the Microsoft ADD login provider.
        /// </summary>
        public async Task<WebAccountProvider> GetAadProviderAsync() =>
            /*await WebAuthenticationCoreManager.FindAccountProviderAsync(
                "https://login.microsoft.com", "organizations");*/
            await WebAuthenticationCoreManager.FindAccountProviderAsync(
                "https://login.microsoft.com", "consumers");


        /// <summary>
        /// Logs the user in.
        /// </summary>
        public async void LoginClick()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn") &&
                (bool)ApplicationData.Current.RoamingSettings.Values["IsLoggedIn"])
            {
                await LoginAsync();
            }
            else
            {
                AccountsSettingsPane.Show();
            }
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        public async void LogoutClick()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn") &&
                (bool)ApplicationData.Current.RoamingSettings.Values["IsLoggedIn"])
            {
                ContentDialog SignoutDialog = new ContentDialog()
                {
                    Title = "Đăng xuất",
                    Content = "Bạn muốn đăng xuất?",
                    PrimaryButtonText = "Đăng xuất",
                    SecondaryButtonText = "Hủy"
                };

                ApplicationData.Current.LocalSettings.Values.Remove("CurrentUserProviderId");
                ApplicationData.Current.LocalSettings.Values.Remove("CurrentUserId");

                ApplicationData.Current.RoamingSettings.Values["IsLoggedIn"] = false;
                ApplicationData.Current.RoamingSettings.Values["Email"] = "null@gmail.com";
                ApplicationData.Current.RoamingSettings.Values.Remove("IsLoggedIn");
                ApplicationData.Current.RoamingSettings.Values.Remove("Email");

                ApplicationData.Current.LocalSettings.Values.Remove("CurrentUserWebAccountProviderId");
                ApplicationData.Current.LocalSettings.Values.Remove("CurrentUserWebAccountId");
                await account.SignOutAsync();

                await SetVisibleAsync(vm => vm.ShowWelcome);
                await SignoutDialog.ShowAsync();
            }
        }


        /// <summary>
        /// Shows one part of the login UI sequence and hides all the others.
        /// </summary>
        private async Task SetVisibleAsync(Expression<Func<AuthenticationViewModel, bool>> selector)
        {
            var prop = (PropertyInfo)((MemberExpression)selector.Body).Member;
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ShowWelcome = false;
                ShowLoading = false;
                ShowData = false;
                ShowError = false;
                prop.SetValue(this, true);
            });
        }
    }
}
