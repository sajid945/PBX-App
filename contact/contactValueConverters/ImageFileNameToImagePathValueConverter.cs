using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using RevePbx.modules.core;

namespace RevePbx.modules.contact.contactValueConverters
{
    class ImageFileNameToImagePathValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string filepath = string.Empty;

            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                filepath = RevePbxConfiguration.GetProfilePictureFilePath(value as string);

            if (File.Exists(filepath))
                return filepath;
            else
            {
                if ((string) parameter == "group")
                    return "pack://application:,,,/resources/img/placeholders/group_placeholder.png";
                else
                    return "pack://application:,,,/resources/img/placeholders/placeholder.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}