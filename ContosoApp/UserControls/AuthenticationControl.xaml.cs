using Windows.UI.Xaml.Controls;

namespace QuanLyCauDuong.UserControls
{
    public sealed partial class AuthenticationControl : UserControl
    {
        public ViewModels.AuthenticationViewModel ViewModel { get; set; } = new ViewModels.AuthenticationViewModel();

        public AuthenticationControl()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
