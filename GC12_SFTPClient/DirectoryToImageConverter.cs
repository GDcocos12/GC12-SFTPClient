using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GC12_SFTPClient
{
    public class DirectoryToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDirectory)
            {
                if (isDirectory)
                {
                    if (parameter is string paramString && paramString == "IsExpanded")
                    {
                        if (parameter is DependencyObject depObj)
                        {
                            TreeViewItem treeViewItem = FindParent<TreeViewItem>(depObj);
                            if (treeViewItem != null)
                            {
                                if (treeViewItem.IsExpanded)
                                {
                                    return "/Images/open_folder.png";
                                }
                                else
                                {
                                    return "/Images/close_folder.png";
                                }
                            }
                            else
                            {
                                return "/Images/close_folder.png";
                            }
                        }
                        return "/Images/close_folder.png";
                    }
                    else
                    {
                        return "/Images/close_folder.png";
                    }
                }
                else
                {
                    return "/Images/file.png";
                }
            }
            return null;
        }
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = LogicalTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}