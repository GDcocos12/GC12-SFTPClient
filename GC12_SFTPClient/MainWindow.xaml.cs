using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Media;
using System.Text;

namespace GC12_SFTPClient
{
    public partial class MainWindow : Window
    {
        private SftpClient _sftpClient;
        private SftpFileItem _currentlyEditedFile;

        public MainWindow()
        {
            InitializeComponent();
            txtPort.Text = "22";
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            ConnectAndListFilesAsync();
        }

        private async Task ConnectAndListFilesAsync()
        {
            string host = txtHost.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;
            int port;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter host, username, and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtPort.Text, out port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("Invalid port number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                SetStatus("Connecting...", true);
                _sftpClient = new SftpClient(host, port, username, password);
                await Task.Run(() => _sftpClient.Connect());

                if (_sftpClient.IsConnected)
                {
                    SetStatus("Connected.", false);
                    btnConnect.IsEnabled = false;
                    btnDisconnect.IsEnabled = true;
                    btnRefresh.IsEnabled = true;
                    await ListRemoteFilesAsync();
                }
                else
                {
                    SetStatus("Connection failed.", false);
                }
            }
            catch (Exception ex)
            {
                SetStatus($"Connection error: {ex.Message}", false);
                MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _sftpClient?.Dispose();
                _sftpClient = null;
            }
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (_sftpClient != null && _sftpClient.IsConnected)
            {
                _sftpClient.Disconnect();
                _sftpClient.Dispose();
                _sftpClient = null;
                SetStatus("Disconnected.", false);
                btnConnect.IsEnabled = true;
                btnDisconnect.IsEnabled = false;
                btnUpload.IsEnabled = false;
                btnDownload.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnEdit.IsEnabled = false;
                btnRefresh.IsEnabled = false;
                btnCreate.IsEnabled = false;
                tvFiles.Items.Clear();
                HideEditArea();
            }
        }

        private async Task ListRemoteFilesAsync()
        {
            if (_sftpClient == null || !_sftpClient.IsConnected) return;

            try
            {
                SetStatus("Listing files...", true);
                string rootPath = "/";
                SftpFileItem rootItem = new SftpFileItem(rootPath, null);
                await LoadChildrenAsync(rootItem);

                tvFiles.Dispatcher.Invoke(() =>
                {
                    tvFiles.Items.Clear();
                    tvFiles.Items.Add(rootItem);
                    SetStatus("File tree loaded.", false);
                });
            }
            catch (Exception ex)
            {
                tvFiles.Dispatcher.Invoke(() =>
                {
                    SetStatus($"Error listing files: {ex.Message}", false);
                    MessageBox.Show($"Error listing files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        private async Task LoadChildrenAsync(SftpFileItem parentItem)
        {
            if (!_sftpClient.IsConnected) return;

            try
            {
                IEnumerable<ISftpFile> files;

                if (parentItem.IsRoot)
                {
                    files = await Task.Run(() => _sftpClient.ListDirectory("/"));
                }
                else
                {
                    files = await Task.Run(() => _sftpClient.ListDirectory(parentItem.SftpFile.FullName));
                }

                parentItem.Children.Clear();

                foreach (var file in files)
                {
                    if (file.Name == "." || file.Name == "..") continue;

                    var newItem = new SftpFileItem(file);
                    parentItem.Children.Add(newItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading children for {parentItem.DisplayName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            var treeViewItem = sender as TreeViewItem;
            if (treeViewItem == null) return;

            var fileItem = treeViewItem.DataContext as SftpFileItem;
            if (fileItem == null || !fileItem.IsDirectory) return;
            if (fileItem.Children.Count > 0 && fileItem.Children[0] != null) return;

            await LoadChildrenAsync(fileItem);

            treeViewItem.Dispatcher.Invoke(() =>
            {
                treeViewItem.ItemsSource = null;
                treeViewItem.ItemsSource = fileItem.Children;
            });

            e.Handled = true;
        }

        private void tvFiles_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = tvFiles.SelectedItem as SftpFileItem;

            btnDownload.IsEnabled = btnDelete.IsEnabled = (selectedItem != null && !selectedItem.IsDirectory);
            btnUpload.IsEnabled = (selectedItem != null && selectedItem.IsDirectory);
            btnCreate.IsEnabled = (selectedItem != null && selectedItem.IsDirectory);
            btnEdit.IsEnabled = (selectedItem != null && !selectedItem.IsDirectory && selectedItem.SftpFile.Length < 50 * 1024);

            HideEditArea();
        }

        private async void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (!(_sftpClient != null && _sftpClient.IsConnected)) return;

            var selectedItem = tvFiles.SelectedItem as SftpFileItem;
            if (selectedItem == null || selectedItem.IsDirectory) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = selectedItem.SftpFile.Name;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    SetStatus($"Downloading {selectedItem.SftpFile.Name}...", true);
                    long fileSize = selectedItem.SftpFile.Length;
                    progressBar.Maximum = fileSize;
                    progressBar.Value = 0;

                    using (var fileStream = File.Create(saveFileDialog.FileName))
                    {
                        await Task.Run(() => _sftpClient.DownloadFile(selectedItem.SftpFile.FullName, fileStream,
                            (bytesDownloaded) =>
                            {
                                progressBar.Dispatcher.Invoke(() => progressBar.Value = bytesDownloaded);
                            }));
                    }

                    SetStatus($"Downloaded {selectedItem.SftpFile.Name} successfully.", false);
                }
                catch (Exception ex)
                {
                    SetStatus($"Error downloading file: {ex.Message}", false);
                    MessageBox.Show($"Error downloading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (!(_sftpClient != null && _sftpClient.IsConnected)) return;

            var selectedItem = tvFiles.SelectedItem as SftpFileItem;
            if (selectedItem == null || !selectedItem.IsDirectory) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    SetStatus($"Uploading {Path.GetFileName(openFileDialog.FileName)}...", true);

                    string targetDirectory = selectedItem.SftpFile.FullName;
                    string targetPath = Path.Combine(targetDirectory, Path.GetFileName(openFileDialog.FileName)).Replace("\\", "/");

                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    ulong fileSize = (ulong)fileInfo.Length;
                    progressBar.Maximum = fileSize;
                    progressBar.Value = 0;

                    using (var fileStream = File.OpenRead(openFileDialog.FileName))
                    {
                        await Task.Run(() => _sftpClient.UploadFile(fileStream, targetPath, true,
                            (bytesUploaded) =>
                            {
                                progressBar.Dispatcher.Invoke(() => progressBar.Value = bytesUploaded);
                            }));
                    }

                    SetStatus($"Uploaded {Path.GetFileName(openFileDialog.FileName)} successfully.", false);
                    await ListRemoteFilesAsync();
                }
                catch (Exception ex)
                {
                    SetStatus($"Error uploading file: {ex.Message}", false);
                    MessageBox.Show($"Error uploading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(_sftpClient != null && _sftpClient.IsConnected)) return;

            var selectedItem = tvFiles.SelectedItem as SftpFileItem;
            if (selectedItem == null || selectedItem.IsDirectory) return;

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedItem.SftpFile.Name}?",
                                                        "Confirm Delete",
                                                        MessageBoxButton.YesNo,
                                                        MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    SetStatus($"Deleting {selectedItem.SftpFile.Name}...", true);
                    await Task.Run(() => _sftpClient.DeleteFile(selectedItem.SftpFile.FullName));
                    SetStatus($"Deleted {selectedItem.SftpFile.Name} successfully.", false);
                    await ListRemoteFilesAsync();
                }
                catch (Exception ex)
                {
                    SetStatus($"Error deleting file: {ex.Message}", false);
                    MessageBox.Show($"Error deleting file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!(_sftpClient != null && _sftpClient.IsConnected)) return;

            var selectedItem = tvFiles.SelectedItem as SftpFileItem;
            if (selectedItem == null || selectedItem.IsDirectory || selectedItem.SftpFile.Length >= 50 * 1024) return;

            try
            {
                SetStatus($"Loading {selectedItem.SftpFile.Name} for editing...", true);

                string fileContent;
                using (var memoryStream = new MemoryStream())
                {
                    await Task.Run(() => _sftpClient.DownloadFile(selectedItem.SftpFile.FullName, memoryStream));
                    memoryStream.Position = 0;
                    using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        fileContent = await reader.ReadToEndAsync();
                    }
                }

                txtEdit.Text = fileContent;
                editArea.Visibility = Visibility.Visible;
                _currentlyEditedFile = selectedItem;

                SetStatus($"Editing {selectedItem.SftpFile.Name}...", false);
            }
            catch (Exception ex)
            {
                SetStatus($"Error loading file for editing: {ex.Message}", false);
                MessageBox.Show($"Error loading file for editing: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_currentlyEditedFile == null) return;

            try
            {
                SetStatus($"Saving changes to {_currentlyEditedFile.SftpFile.Name}...", true);

                byte[] fileBytes = Encoding.UTF8.GetBytes(txtEdit.Text);
                using (var memoryStream = new MemoryStream(fileBytes))
                {
                    await Task.Run(() => _sftpClient.UploadFile(memoryStream, _currentlyEditedFile.SftpFile.FullName, true));
                }

                SetStatus($"Changes saved to {_currentlyEditedFile.SftpFile.Name}.", false);
                HideEditArea();
                await ListRemoteFilesAsync();
            }
            catch (Exception ex)
            {
                SetStatus($"Error saving changes: {ex.Message}", false);
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            HideEditArea();
        }

        private void HideEditArea()
        {
            editArea.Visibility = Visibility.Collapsed;
            txtEdit.Text = "";
            _currentlyEditedFile = null;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "GC SFTP Files (*.gcsftp)|*.gcsftp";
            if (saveFileDialog.ShowDialog() == true)
            {
                ConnectionData data = new ConnectionData(
                    txtHost.Text.Trim(),
                    txtUsername.Text.Trim(),
                    txtPassword.Password,
                    int.Parse(txtPort.Text.Trim())
                );

                try
                {
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    File.WriteAllText(saveFileDialog.FileName, json);
                    SetStatus("Connection data saved.", false);
                }
                catch (Exception ex)
                {
                    SetStatus($"Error saving file: {ex.Message}", false);
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "GC SFTP Files (*.gcsftp)|*.gcsftp";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(openFileDialog.FileName);
                    ConnectionData data = JsonConvert.DeserializeObject<ConnectionData>(json);

                    txtHost.Text = data.Host;
                    txtUsername.Text = data.Username;
                    txtPassword.Password = data.Password;
                    txtPort.Text = data.Port.ToString();
                    SetStatus("Connection data loaded.", false);
                }
                catch (Exception ex)
                {
                    SetStatus($"Error loading file: {ex.Message}", false);
                    MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (!(_sftpClient != null && _sftpClient.IsConnected)) return;

            await ListRemoteFilesAsync();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!(_sftpClient != null && _sftpClient.IsConnected)) return;

            var selectedItem = tvFiles.SelectedItem as SftpFileItem;
            if (selectedItem == null || !selectedItem.IsDirectory) return;

            var createDialog = new CreateDialog();
            createDialog.Owner = this;
            if (createDialog.ShowDialog() == true)
            {
                string itemName = createDialog.ItemName;
                bool isDirectory = createDialog.IsDirectory;

                string fullPath = Path.Combine(selectedItem.SftpFile.FullName, itemName).Replace("\\", "/");


                try
                {
                    SetStatus($"Creating {itemName}...", true);
                    if (isDirectory)
                    {
                        await Task.Run(() => _sftpClient.CreateDirectory(fullPath));
                    }
                    else
                    {
                        await Task.Run(() =>
                        {
                            using (var stream = _sftpClient.Create(fullPath))
                            {
                            }
                        });

                    }

                    SetStatus($"Created {itemName}.", false);
                    await ListRemoteFilesAsync();
                }
                catch (Exception ex)
                {
                    SetStatus($"Error creating {itemName}: {ex.Message}", false);
                    MessageBox.Show($"Error creating {itemName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void SetStatus(string text, bool showProgress)
        {
            txtStatus.Text = text;
            progressBar.Visibility = showProgress ? Visibility.Visible : Visibility.Collapsed;
            if (!showProgress)
            {
                progressBar.Value = 0;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _sftpClient?.Disconnect();
            _sftpClient?.Dispose();
            base.OnClosed(e);
        }
    }
}