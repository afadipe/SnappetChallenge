using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace VATMVCAPPFramework.Utilities
{
    public class EmailHandler
    {

        public class MailSettings
        {
            public MailSettings()
            {
                CopyAddresses = new HashSet<String>();
            }
            public String Host { get; set; }
            public int Port { get; set; }
            public Boolean EnableSsl { get; set; }
            public SmtpDeliveryMethod DeliveryMethod { get; set; }
            public Boolean UseDefaultCredentials { get; set; }
            public SmtpCredentials Credentials { get; set; }
            public ICollection<String> CopyAddresses { get; set; }
            public String SubjectTemplate { get; set; }
        }

        public class SmtpCredentials
        {
            public String Username { get; set; }
            public String Password { get; set; }
        }

        public static bool Send(MailMessage message)
        {

            try
            {

                string _mailsetting = ConfigurationManager.AppSettings["MailSettings"].ToString();
                MailSettings _setting = JsonConvert.DeserializeObject<MailSettings>(_mailsetting);
                if (_setting.CopyAddresses.Count > 0)
                {
                    foreach (string address in _setting.CopyAddresses)
                    {
                        message.Bcc.Add(new MailAddress(address));
                    }
                }
                var smtpClient = new SmtpClient
                {
                    Host = _setting.Host,
                    Port = _setting.Port,
                    EnableSsl = _setting.EnableSsl,
                    DeliveryMethod = _setting.DeliveryMethod,
                    UseDefaultCredentials = _setting.UseDefaultCredentials,
                    Credentials = new NetworkCredential(_setting.Credentials.Username, _setting.Credentials.Password)
                };

                //SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                ////System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("ayfadipe", "aisekegbe$1");
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("ayoola01", "doctorsng01");
                //smtpClient.Credentials = credentials;

                smtpClient.Send(message);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}