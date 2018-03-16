using System;
using System.Globalization;
using System.Windows.Data;
using RevePbx.modules.dataModel.history;
using RevePbx.modules.services;

namespace RevePbx.modules.history.historyValueConverters
{
    class UserIdToLastMessageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userIdOrGroupId = value as string;

            if (userIdOrGroupId == null)
                return null;

            HistoryDataModel history =
                ServiceLocator.historyContainerService.GetLastHistoryByUserIdOrGroupId(userIdOrGroupId);

            if (history == null) return null;
            return history.Text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}