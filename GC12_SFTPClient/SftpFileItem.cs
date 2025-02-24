using Renci.SshNet.Sftp;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GC12_SFTPClient
{
    public class SftpFileItem : INotifyPropertyChanged
    {
        public ISftpFile SftpFile { get; set; }
        public string DisplayName { get; set; }
        public ObservableCollection<SftpFileItem> Children { get; set; }
        public bool IsDirectory { get; set; }
        public bool IsRoot { get; set; }


        public SftpFileItem(ISftpFile file, bool isRoot = false)
        {
            SftpFile = file;
            IsDirectory = file.IsDirectory;
            IsRoot = isRoot;
            DisplayName = file.Name;
            Children = new ObservableCollection<SftpFileItem>();

            if (IsDirectory && (DisplayName == "." || DisplayName == ".."))
            {
                DisplayName = file.FullName;
            }

            if (IsDirectory && !IsRoot)
            {
                Children.Add(null);
            }
        }

        public SftpFileItem(string rootPath, ISftpFile rootFile)
        {
            SftpFile = rootFile;
            IsDirectory = true;
            IsRoot = true;
            DisplayName = rootPath;
            Children = new ObservableCollection<SftpFileItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}