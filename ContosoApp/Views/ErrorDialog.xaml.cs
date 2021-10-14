using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuanLyCauDuong.Views
{
    /// <summary>
    /// Creates a dialog that gives the users a chance to save changes, discard them, 
    /// or cancel the operation that trigggered the event. 
    /// </summary>
    public sealed partial class ErrorDialog : ContentDialog
    {
        /// <summary>
        /// Initializes a new instance of the SaveChangesDialog class.
        /// </summary>
        public ErrorDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the user's choice. 
        /// </summary>
        public SaveErrorDialogResult Result { get; private set; } = SaveErrorDialogResult.Cancel;

        /// <summary>
        /// Occurs when the user chooses to cancel the operation that triggered the event.
        /// </summary>
        private void ErrorDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = SaveErrorDialogResult.Cancel;
            Hide();
        }
    }

    public enum SaveErrorDialogResult
    {
        Cancel
    }
}
