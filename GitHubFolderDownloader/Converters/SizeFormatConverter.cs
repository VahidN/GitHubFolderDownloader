using System;
using System.Windows.Data;
using GitHubFolderDownloader.Toolkit;

namespace GitHubFolderDownloader.Converters
{
    public class SizeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            var size = (long)value;
            return FilesInfo.FormatSize(size);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
