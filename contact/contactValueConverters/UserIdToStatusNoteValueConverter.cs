using System;
using System.Globalization;
using System.Windows.Data;
using RevePbx.modules.dataModel.buddy;
using RevePbx.modules.services;

namespace RevePbx.modules.contact.contactValueConverters
{
    class UserIdToStatusNoteValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            ImBuddyDataModel imBuddyDataModel = ServiceLocator.buddyService.GetBuddyByUserId(value.ToString());

            if (imBuddyDataModel == null)
                return null;

            return imBuddyDataModel.StatusNote;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}