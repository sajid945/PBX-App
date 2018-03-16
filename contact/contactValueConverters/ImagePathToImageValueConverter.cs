using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RevePbx.modules.contact.contactValueConverters
{
    class ImagePathToImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string filepath = value as string;
            if (filepath == null) return null;

            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.UriSource = new Uri(filepath);
            bitmap.EndInit();

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}