using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Application.Helpers
{
    public class CommonHelper
    {
        public static DateTime ConvertStringToDate(string dateString)
        {
            DateTime dateTime;

            if (DateTime.TryParseExact(
                dateString, 
                "yyyy-MM-dd", 
                System.Globalization.CultureInfo.InvariantCulture, 
                System.Globalization.DateTimeStyles.None, 
                out dateTime)
            ) {
                return dateTime;
            }
            else
            {
                throw new ArgumentException("Date string is not in expected format (yyyy-MM-dd).");
            }
        }

        public static string ConvertDateToString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
