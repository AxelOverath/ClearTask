using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ClearTask.Data
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Colors.Gray; // Default color for null or unknown status

            var status = value.ToString().ToLower();

            return status switch
            {
                "pending" => Colors.LightGrey,   //  Yellow for Pending
                "inprogress" => Colors.Orange,  //  Orange for In Progress
                "completed" => Colors.Green,    //  Green for Completed
                _ => Colors.Gray // Default for unknown values
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
