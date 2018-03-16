using System;
using System.Globalization;
using System.Windows.Data;
using RevePbx.modules.services;

namespace RevePbx.modules.history.historyValueConverters
{
    class UserIdToNameValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userIdOrGroupId = value as string;
            if (userIdOrGroupId == null) return string.Empty;

            if ((string) parameter == "group")
            {
                var group = ServiceLocator.groupService.GetGroupByGroupId(userIdOrGroupId);
                if (group != null) return group.GroupName;
            }
            else
            {
                var buddy = ServiceLocator.buddyService.GetBuddyByUserId(userIdOrGroupId);
                if (buddy != null) return buddy.Name;
            }

            return "No name";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}