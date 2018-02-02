using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using VATMVCAPPFramework.Data.Entities;

namespace VATMVCAPPFramework.Utilities
{
    public static class SerializeDeserializeExtension
    {
        public static string Serialize(this object o)
        {
            var sw = new StringWriter();
            var formatter = new LosFormatter();
            formatter.Serialize(sw, o);

            return sw.ToString();
        }

        public static object Deserialize(this string data)
        {
            if (String.IsNullOrEmpty(data))
                return null;

            var formatter = new LosFormatter();
            return formatter.Deserialize(data);
        }
    }

    public static class ExtentionUtility
    {

        public static string GetAppSetting(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                return "";
            }
        }

        public static int GetIntAppSetting(string key)
        {
            try
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings[key].ToString());
            }
            catch
            {
                return 2;
            }
        }

        public static string GeneratePreviewHTML(string htmlcode, List<EmailToken> lstToken)
        {
            try
            {
                foreach (EmailToken item in lstToken)
                {
                    htmlcode = htmlcode.Replace(item.Token, item.PreviewText);
                }
            }
            catch 
            {
                //ErrorMgt.WriteLog(ex);
            }
            return htmlcode;
        }

        public static string GetApiContollerName(this ApiController controller)
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

        }
        public static string GetApiActionName(this ApiController controller)
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();

        }
        public static string GetContollerName(this Controller controller)
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
        }
        public static string GetActionName(this Controller controller)
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
        }

        public static String EncryptID(this Int64 ID)
        {
            try
            {
                string EcryptedApplicantID = CryptoClass.EncryptPlainTextToCipherText(ID.ToString());
                EcryptedApplicantID = EcryptedApplicantID.Replace("/", "~");
                EcryptedApplicantID = EcryptedApplicantID.Replace("\\", "`");
                return EcryptedApplicantID;
            }
            catch
            {
                return null;
            }

        }
        public static String Encrypt(this string Text)
        {
            try
            {
                String Ecrypted=CryptoClass.EncryptPlainTextToCipherText(Text);
                return Ecrypted;
            }
            catch 
            {
                return null;
            }

        }
        public static Int64 DecryptID(this string ID)
        {
            try
            {
                ID = ID.Replace("~", "/");
                ID = ID.Replace("`", "\\");
                Int64 DecryptedApplicantID = Convert.ToInt64(CryptoClass.DecryptCipherTextToPlainText(ID));
                return DecryptedApplicantID;
            }
            catch 
            {
                return 0;
            }
        }



        #region MyDateConversionRegion
        public static DateTime? GetDateNullValue(Object obj)
        {
            DateTime? retVal = null;
            try
            {
                if (!(obj is DBNull))
                {
                    retVal = (DateTime)obj;
                }
                else
                {
                    retVal = null;
                }
            }
            catch { }
            return retVal;
        }

        public static DateTime GetDateValue(Object obj)
        {
            DateTime retVal = DateTime.Now;
            try
            {
                if (!(obj is DBNull))
                {
                    retVal = (DateTime)obj;
                }
            }
            catch { }
            return retVal;
        }

        public static DateTime ConvertDateValue(Object obj)
        {
            SetDateCulture();
            DateTime result;
            var succeeded = DateTime.TryParse(obj.ToString(), out result);
            if (succeeded == true)
            {
                return result;
            }
            else
            {
                return DateTime.Now;
            }
        }


        public static bool IsNumeric(string val)
        {
            Double result;
            return Double.TryParse(val, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        public static void SetDateCulture()
        {
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }

        public static DateTime ConvertDateValueWithCulture(String obj)
        {
            DateTime result;
            CultureInfo culture = new CultureInfo("en-GB");

            var succeeded = DateTime.TryParseExact(obj, "dd/MM/yyyy", culture, System.Globalization.DateTimeStyles.None, out result);
            if (succeeded == true)
            {
                return result;
            }
            else
            {
                return DateTime.Now;
            }
        }
        #endregion

    }
}