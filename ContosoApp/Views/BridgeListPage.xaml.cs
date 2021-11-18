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
                             bridge.Name.Contains(parameter) ||
                             bridge.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                             bridge.Investor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                             bridge.Supervisor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                         .OrderByDescending(bridge => parameters.Count(parameter =>
                             bridge.Name.StartsWith(parameter) ||
                             bridge.Investor.StartsWith(parameter)))
                         .Select(bridge => $"{bridge.Name}");
                }
            }
        }

        /// Lọc hoặc reset danh sách cầu theo từ khóa tìm kiếm.
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
        /// Reset danh sách cầu.
        /// </summary>
        private async Task ResetBridgeList()
        {
            await dispatcherQueue.EnqueueAsync(async () => await ViewModel.GetBridgeListAsync());
        }

        /// <summary>
        /// Lọc danh sách cầu dựa trên từ khóa tìm kiếm.
        /// </summary>
        private async Task FilterBridgeList(string text)
        {
            string[] parameters = text.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            var matches = ViewModel.Bridges.Where(bridge => parameters
                .Any(parameter =>
                    bridge.Name.Contains(parameter) ||
                    bridge.Investor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                    bridge.Builder.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                    bridge.Supervisor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(bridge => parameters.Count(parameter =>
                    bridge.Name.StartsWith(parameter) ||
                    bridge.Investor.StartsWith(parameter) ||
                    bridge.Builder.StartsWith(parameter) ||
                    bridge.Supervisor.StartsWith(parameter)))
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
        /// Reset danh sách cầu khi chuyển trang.
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
            else
            {
                await ResetBridgeList();
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

        /// <summary>
        /// Xóa cầu đang chọn.
        /// </summary>
        private async void DeleteBridge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deleteBridge = ViewModel.SelectedBridge.Model;
                await ViewModel.DeleteBridge(deleteBridge);
                await ResetBridgeList();
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Xóa cầu thất bại",
                    Content = $"Gặp lỗi khi xóa " +
                        $"cầu #{ViewModel.SelectedBridge.Name}:\n{ex.Message}",
                    PrimaryButtonText = "OK"
                };
                await dialog.ShowAsync();
            }
        }

        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            Frame.Navigate(typeof(BridgeDetailPage), ViewModel.SelectedBridge.Model.Id,
                    new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Chuyển đến trang thêm mới cầu.
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
        /// Chuột phải lên cầu. 
        /// </summary>
        private void DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) =>
            ViewModel.SelectedBridge = (e.OriginalSource as FrameworkElement).DataContext as BridgeViewModel;

        /// <summary>
        /// Sắp xếp data trong bảng.
        /// </summary>
        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e) =>
            (sender as DataGrid).Sort(e.Column, ViewModel.Bridges.Sort);

        private async void FilterBuidlingBridges_Click(object sender, RoutedEventArgs e)
        {
            var matches = ViewModel.CloneBridges.Where(bridge => bridge.Status == "Đang xây dựng").ToList();

            await dispatcherQueue.EnqueueAsync(() =>
            {
                ViewModel.Bridges.Clear();
                foreach (var match in matches)
                {
                    ViewModel.Bridges.Add(match);
                }
            });
        }

        private async void FilterErrorBridges_Click(object sender, RoutedEventArgs e)
        {
            var matches = ViewModel.CloneBridges.Where(bridge => bridge.Status == "Đang bảo trì").ToList();

            await dispatcherQueue.EnqueueAsync(() =>
            {
                ViewModel.Bridges.Clear();
                foreach (var match in matches)
                {
                    ViewModel.Bridges.Add(match);
                }
            });
        }

        private async void FilterWorkingBridges_Click(object sender, RoutedEventArgs e)
        {
            var matches = ViewModel.CloneBridges.Where(bridge => bridge.Status == "Hoạt động tốt").ToList();

            await dispatcherQueue.EnqueueAsync(() =>
            {
                ViewModel.Bridges.Clear();
                foreach (var match in matches)
                {
                    ViewModel.Bridges.Add(match);
                }
            });
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            /*using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                //Create a new workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet sheet = workbook.Worksheets[0];

                //Create a dataset from XML file
                DataSet customersDataSet = new DataSet();
                customersDataSet.ReadXml(Path.GetFullPath(@"../../Data/Employees.xml"));

                //Create datatable from the dataset
                DataTable dataTable = new DataTable();
                dataTable = customersDataSet.Tables[0];

                //Import data from the data table with column header, at first row and first column, 
                //and by its column type.
                sheet.ImportDataTable(dataTable, true, 1, 1, true);

                //Creating Excel table or list object and apply style to the table
                IListObject table = sheet.ListObjects.Create("Employee_PersonalDetails", sheet.UsedRange);

                table.BuiltInTableStyle = TableBuiltInStyles.TableStyleMedium14;

                //Autofit the columns
                sheet.UsedRange.AutofitColumns();

                //Save the file in the given path
                Stream excelStream = File.Create(Path.GetFullPath(@"Output.xlsx"));
                workbook.SaveAs(excelStream);
                excelStream.Dispose();
            }*/
        }
    }
}
