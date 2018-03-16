using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RevePbx.modules.history.historyValueConverters
{
    class UnreadHistoryCountToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            int unreadHistoryCount = (int) value;

            if (unreadHistoryCount == 0)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}