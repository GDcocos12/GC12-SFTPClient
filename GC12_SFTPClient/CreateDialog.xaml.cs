using System.Windows;

namespace GC12_SFTPClient
{
    public partial class CreateDialog : Window
    {
        public string ItemName { get; private set; }
        public bool IsDirectory { get; private set; }

        public CreateDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            ItemName = txtName.Text.Trim();
            IsDirectory = rbDirectory.IsChecked ?? false;

            if (string.IsNullOrEmpty(ItemName))
            {
                MessageBox.Show("Please enter a name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ItemName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            {
                MessageBox.Show("Invalid characters in file/directory name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}