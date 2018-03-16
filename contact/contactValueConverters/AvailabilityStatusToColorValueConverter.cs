using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using RevePbx.modules.@enum;

namespace RevePbx.modules.contact.contactValueConverters
{
    class AvailabilityStatusToColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color nullColorBrush = ((SolidColorBrush) Application.Current.FindResource("OfflineColorBrush")).Color;

            if (value == null) return nullColorBrush;

            AvailabilityStatus availabilityStatus = (AvailabilityStatus) value;

            switch (availabilityStatus)
            {
                case AvailabilityStatus.Available:
                    return ((SolidColorBrush) Application.Current.FindResource("AvailableColorBrush")).Color;
                case AvailabilityStatus.Busy:
                    return ((SolidColorBrush) Application.Current.FindResource("BusyColorBrush")).Color;
                case AvailabilityStatus.Away:
                    return ((SolidColorBrush) Application.Current.FindResource("AwayColorBrush")).Color;
                default:
                    return nullColorBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}