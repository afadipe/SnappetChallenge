using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VATMVCAPPFramework.Models
{
    public static class ExtenctionHelper
    {
        public static DateTime ToDateByddMMyyyy(this string Date)
        {
            return DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateHifenddMMyyyy(this string Date)
        {
            DateTime dt;
            if (DateTime.TryParseExact(Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                //valid date
            }
            else
                dt = DateTime.MinValue;
            return dt;
            //return DateTime.ParseExact(Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateByMMddyyyy(this string Date)
        {
            return DateTime.ParseExact(Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        public static string DateDisplayddMMyyyy(this DateTime Date)
        {
            return Date.ToString("dd-MM-yyyy");
        }
    }
}