using System;
using System.Globalization;
using System.Windows.Data;

namespace RevePbx.modules.history.historyValueConverters
{
    class UnreadHistoryCountToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            int unreadHistoryCount = (int) value;

            if (unreadHistoryCount == 0)
                return string.Empty;

            if (unreadHistoryCount > 9)
                return "9+";

            return unreadHistoryCount.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}