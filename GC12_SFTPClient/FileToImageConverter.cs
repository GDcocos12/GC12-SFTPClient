using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace GC12_SFTPClient
{
    public class FileToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SftpFileItem fileItem)
            {
                if (fileItem.IsDirectory)
                {
                    return "/Images/close_folder.png";
                }
                else
                {
                    string extension = Path.GetExtension(fileItem.DisplayName).ToLowerInvariant();
                    switch (extension)
                    {
                        case ".zip":
                        case ".rar":
                        case ".7z":
                            return "/Images/archive.png";
                        case ".js":
                            return "/Images/jsfile.png";
                        default:
                            return "/Images/file.png";
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}