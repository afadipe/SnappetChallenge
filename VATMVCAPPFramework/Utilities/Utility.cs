using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VATMVCAPPFramework.Data.Core;
using VATMVCAPPFramework.Data.Entities;
using VATMVCAPPFramework.Data.IdentityModel;
using VATMVCAPPFramework.Data.IdentityService;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.ViewModel;
using System.Configuration;
using System.Web.Mail;

namespace VATMVCAPPFramework.Utilities
{
    public class Utility
    {
        private readonly IRepositoryQuery<Permission> _permissionQuery;
        private readonly ApplicationRoleManager _applicationRoleManager;
        private readonly IRepositoryQuery<EmailTemplate>  _emailTemplateRepositoryQuery;
        private readonly IRepositoryQuery<EmailToken> _emailTokenRepositoryQuery;
        private readonly IRepositoryCommand<EmailLog> _emailLogRepositoryCommand;
        public Utility(IRepositoryQuery<Permission> permissionQuery,
        IRepositoryQuery<EmailTemplate> emailTemplateRepositoryQuery,
            IRepositoryQuery<EmailToken> emailTokenRepositoryQuery,
            IRepositoryCommand<EmailLog> emailLogRepositoryCommand,
        ApplicationRoleManager applicationRoleManager)
        {
            _applicationRoleManager = applicationRoleManager;
            _emailTemplateRepositoryQuery = emailTemplateRepositoryQuery;
            _emailTokenRepositoryQuery = emailTokenRepositoryQuery;
            _emailLogRepositoryCommand = emailLogRepositoryCommand;
           
        }
        public IEnumerable<SelectListItem> GetPermissions()
        {
            var types = _permissionQuery.GetAllList().Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name

                                }).AsEnumerable();
            return new SelectList(types, "Value", "Text");
        }

       
        
        public IEnumerable<SelectListItem> GetRoles()
        {
            var types = _applicationRoleManager.Roles.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Name,
                                    Text = x.Name

                                }).AsEnumerable();
            return new SelectList(types, "Value", "Text");
        }

        


        public void SendPasswordResetEmail(ApplicationUser mUser, string resetUrl)
        {
            try
            {
                EmailTemplate emailFormat = _emailTemplateRepositoryQuery.GetAllList(m=>m.Code== "F_PASSWORD").SingleOrDefault();
                List<EmailToken> tokenCol = _emailTokenRepositoryQuery.GetAllList(m => m.EmailCode == emailFormat.Code).ToList();
                foreach (var token in tokenCol)
                {
                    if (token.Token.Equals("{Name}"))
                    {
                        token.PreviewText = mUser.FirstName + " " + mUser.LastName;
                    }
                    else if (token.Token.Equals("{Email}"))
                    {
                        token.PreviewText = mUser.Email ?? string.Empty;
                    }
                    else if (token.Token.Equals("{Url}"))
                    {
                        token.PreviewText = resetUrl;
                    }
                }
                try
                {
                    EmailLog mlog = new EmailLog();
                    mlog.Receiver = mUser.Email;
                    mlog.Sender = GetAppSetting("EmailSender");
                    mlog.Subject = "Password Reset Notification";
                    mlog.MessageBody = ExtentionUtility.GeneratePreviewHTML(emailFormat.Body, tokenCol);
                    mlog.DateCreated = mlog.DateToSend = DateTime.Now;
                    mlog.IsSent = mlog.HasAttachment = false;
                    mlog.EmailAttachments = new List<EmailAttachment>();
                    _emailLogRepositoryCommand.Insert(mlog);
                    _emailLogRepositoryCommand.SaveChanges();
                }
                catch (DbEntityValidationException filterContext)
                {
                    if (typeof(DbEntityValidationException) == filterContext.GetType())
                    {
                        foreach (var validationErrors in filterContext.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                            }
                        }
                    }
                    throw;
                }

            }
            catch
            {

                throw;
            }
        }

        public void SendUsernameRequest(ApplicationUser mUser)
        {
            try
            {
                EmailTemplate emailFormat = _emailTemplateRepositoryQuery.GetAllList(m => m.Code == "USR").SingleOrDefault();
                List<EmailToken> tokenCol = _emailTokenRepositoryQuery.GetAllList(m => m.EmailCode == emailFormat.Code).ToList();
                foreach (var token in tokenCol)
                {
                    if (token.Token.Equals("{Name}"))
                    {
                        token.PreviewText = mUser.FirstName + " " + mUser.LastName;
                    }
                    else if (token.Token.Equals("{Username}"))
                    {
                        token.PreviewText = mUser.UserName;
                    }
                }
                try
                {
                    EmailLog mlog = new EmailLog();
                    mlog.Receiver = mUser.Email;
                    mlog.Sender = GetAppSetting("EmailSender");
                    mlog.Subject = "Username Request Notification";
                    mlog.MessageBody = ExtentionUtility.GeneratePreviewHTML(emailFormat.Body, tokenCol);
                    mlog.DateCreated = mlog.DateToSend = DateTime.Now;
                    mlog.IsSent = mlog.HasAttachment = false;
                    mlog.EmailAttachments = new List<EmailAttachment>();
                    _emailLogRepositoryCommand.Insert(mlog);
                    _emailLogRepositoryCommand.SaveChanges();
                }
                catch (DbEntityValidationException filterContext)
                {
                    if (typeof(DbEntityValidationException) == filterContext.GetType())
                    {
                        foreach (var validationErrors in filterContext.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                            }
                        }
                    }
                    throw;
                }

            }
            catch
            {

                throw;
            }
        }



        public void SendWelcomeAndPasswordResetEmail(ApplicationUser mUser, string resetUrl)
        {
            try
            {
                EmailTemplate emailFormat = _emailTemplateRepositoryQuery.GetAllList(m => m.Code == "Welc").SingleOrDefault();
                List<EmailToken> tokenCol = _emailTokenRepositoryQuery.GetAllList(m => m.EmailCode == emailFormat.Code).ToList();
                foreach (var token in tokenCol)
                {
                    if (token.Token.Equals("{UserName}"))
                    {
                        token.PreviewText = mUser.UserName;
                    }
                    else if (token.Token.Equals("{Email}"))
                    {
                        token.PreviewText = mUser.Email ?? string.Empty;
                    }
                    else if (token.Token.Equals("{Url}"))
                    {
                        token.PreviewText = resetUrl;
                    }
                }
                try
                {
                    EmailLog mlog = new EmailLog();
                    mlog.Receiver = mUser.Email;
                    mlog.Sender = GetAppSetting("EmailSender");
                    mlog.Subject = "Welcome to VATACADA Portal";
                    mlog.MessageBody = ExtentionUtility.GeneratePreviewHTML(emailFormat.Body, tokenCol);
                    mlog.DateCreated = mlog.DateToSend = DateTime.Now;
                    mlog.IsSent = mlog.HasAttachment = false;
                    mlog.EmailAttachments = new List<EmailAttachment>();
                    _emailLogRepositoryCommand.Insert(mlog);
                    _emailLogRepositoryCommand.SaveChanges();
                }
                catch (DbEntityValidationException filterContext)
                {
                    if (typeof(DbEntityValidationException) == filterContext.GetType())
                    {
                        foreach (var validationErrors in filterContext.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                            }
                        }
                    }
                    throw;
                }

            }
            catch
            {

                throw;
            }
        }

        public void SendWelcomeAndPasswordResetEmail11(string email,ApplicationUser mUser, string resetUrl)
        {
            try
            {
                EmailTemplate emailFormat = _emailTemplateRepositoryQuery.GetAllList(m => m.Code == "Welc").SingleOrDefault();
                List<EmailToken> tokenCol = _emailTokenRepositoryQuery.GetAllList(m => m.EmailCode == emailFormat.Code).ToList();
                foreach (var token in tokenCol)
                {
                    if (token.Token.Equals("{UserName}"))
                    {
                        token.PreviewText = mUser.UserName;
                    }
                    else if (token.Token.Equals("{Email}"))
                    {
                        token.PreviewText = mUser.Email ?? string.Empty;
                    }
                    else if (token.Token.Equals("{Url}"))
                    {
                        token.PreviewText = resetUrl;
                    }
                }
                try
                {
                    EmailLog mlog = new EmailLog();
                    mlog.Receiver = email;
                    mlog.Sender = GetAppSetting("EmailSender");
                    mlog.Subject = "Welcome to VATACADA Portal";
                    mlog.MessageBody = ExtentionUtility.GeneratePreviewHTML(emailFormat.Body, tokenCol);
                    mlog.DateCreated = mlog.DateToSend = DateTime.Now;
                    mlog.IsSent = mlog.HasAttachment = false;
                    mlog.EmailAttachments = new List<EmailAttachment>();
                    _emailLogRepositoryCommand.Insert(mlog);
                    _emailLogRepositoryCommand.SaveChanges();
                }
                catch (DbEntityValidationException filterContext)
                {
                    if (typeof(DbEntityValidationException) == filterContext.GetType())
                    {
                        foreach (var validationErrors in filterContext.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                            }
                        }
                    }
                    throw;
                }

            }
            catch
            {

                throw;
            }
        }



        public void SendWelcomeAndPasswordResetEmail2(string email,string userName, string resetUrl)
        {
            try
            {
                EmailTemplate emailFormat = _emailTemplateRepositoryQuery.GetAllList(m => m.Code == "Welc").SingleOrDefault();
                List<EmailToken> tokenCol = _emailTokenRepositoryQuery.GetAllList(m => m.EmailCode == emailFormat.Code).ToList();
                foreach (var token in tokenCol)
                {
                    if (token.Token.Equals("{UserName}"))
                    {
                        token.PreviewText = userName;
                    }
                    else if (token.Token.Equals("{Email}"))
                    {
                        token.PreviewText = email ?? string.Empty;
                    }
                    else if (token.Token.Equals("{Url}"))
                    {
                        token.PreviewText = resetUrl;
                    }
                }
                try
                {
                    EmailLog mlog = new EmailLog();
                    mlog.Receiver = email;
                    mlog.Sender = GetAppSetting("EmailSender");
                    mlog.Subject = "Welcome to VATACADA Portal";
                    mlog.MessageBody = ExtentionUtility.GeneratePreviewHTML(emailFormat.Body, tokenCol);
                    mlog.DateCreated = mlog.DateToSend = DateTime.Now;
                    mlog.IsSent = mlog.HasAttachment = false;
                    mlog.EmailAttachments = new List<EmailAttachment>();
                    _emailLogRepositoryCommand.Insert(mlog);
                    _emailLogRepositoryCommand.SaveChanges();
                }
                catch (DbEntityValidationException filterContext)
                {
                    if (typeof(DbEntityValidationException) == filterContext.GetType())
                    {
                        foreach (var validationErrors in filterContext.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                            }
                        }
                    }
                    throw;
                }

            }
            catch 
            {

                throw;
            }
        }

       
        public void SendWelcomeSchoolPasswordResetEmail(ApplicationUser mUser,string schoolName, string resetUrl)
        {
            try
            {
                EmailTemplate emailFormat = _emailTemplateRepositoryQuery.GetAllList(m => m.Code == "WelcSchool").SingleOrDefault();
                List<EmailToken> tokenCol = _emailTokenRepositoryQuery.GetAllList(m => m.EmailCode == emailFormat.Code).ToList();
                foreach (var token in tokenCol)
                {
                    if (token.Token.Equals("{UserName}"))
                    {
                        token.PreviewText = mUser.UserName;
                    }
                    else if (token.Token.Equals("{School}"))
                    {
                        token.PreviewText = schoolName ?? string.Empty;
                    }
                    else if (token.Token.Equals("{Email}"))
                    {
                        token.PreviewText = mUser.Email ?? string.Empty;
                    }
                    else if (token.Token.Equals("{Url}"))
                    {
                        token.PreviewText = resetUrl;
                    }
                }
                try
                {
                    EmailLog mlog = new EmailLog();
                    mlog.Receiver = mUser.Email;
                    mlog.Sender = GetAppSetting("EmailSender");
                    mlog.Subject = "Welcome to VATACADA Portal";
                    mlog.MessageBody = ExtentionUtility.GeneratePreviewHTML(emailFormat.Body, tokenCol);
                    mlog.DateCreated = mlog.DateToSend = DateTime.Now;
                    mlog.IsSent = mlog.HasAttachment = false;
                    mlog.EmailAttachments = new List<EmailAttachment>();
                    _emailLogRepositoryCommand.Insert(mlog);
                    _emailLogRepositoryCommand.SaveChanges();
                }
                catch (DbEntityValidationException filterContext)
                {
                    if (typeof(DbEntityValidationException) == filterContext.GetType())
                    {
                        foreach (var validationErrors in filterContext.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                            }
                        }
                    }
                    throw;
                }

            }
            catch 
            {

                throw;
            }
        }


        public void SendWelcomeSchoolCampusAdminPasswordResetEmail(ApplicationUser mUser, string schoolName, string resetUrl)
        {
            try
            {
                EmailTemplate emailFormat = _emailTemplateRepositoryQuery.GetAllList(m => m.Code == "WelcSchoolAdmin").SingleOrDefault();
                List<EmailToken> tokenCol = _emailTokenRepositoryQuery.GetAllList(m => m.EmailCode == emailFormat.Code).ToList();
                foreach (var token in tokenCol)
                {
                    if (token.Token.Equals("{UserName}"))
                    {
                        token.PreviewText = mUser.UserName;
                    }
                    else if (token.Token.Equals("{School}"))
                    {
                        token.PreviewText = mUser.Email ?? string.Empty;
                    }
                    else if (token.Token.Equals("{Email}"))
                    {
                        token.PreviewText = mUser.Email ?? string.Empty;
                    }
                    else if (token.Token.Equals("{Url}"))
                    {
                        token.PreviewText = resetUrl;
                    }
                }
                try
                {
                    EmailLog mlog = new EmailLog();
                    mlog.Receiver = mUser.Email;
                    mlog.Sender = GetAppSetting("EmailSender");
                    mlog.Subject = "Welcome to VATACADA Portal";
                    mlog.MessageBody = ExtentionUtility.GeneratePreviewHTML(emailFormat.Body, tokenCol);
                    mlog.DateCreated = mlog.DateToSend = DateTime.Now;
                    mlog.IsSent = mlog.HasAttachment = false;
                    mlog.EmailAttachments = new List<EmailAttachment>();
                    _emailLogRepositoryCommand.Insert(mlog);
                    _emailLogRepositoryCommand.SaveChanges();
                }
                catch (DbEntityValidationException filterContext)
                {
                    if (typeof(DbEntityValidationException) == filterContext.GetType())
                    {
                        foreach (var validationErrors in filterContext.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                            }
                        }
                    }
                    throw;
                }

            }
            catch
            {

                throw;
            }
        }

        public IEnumerable<SelectListItem> GetSpecificRoles(Int64[] roleIds)
        {

            var types = _applicationRoleManager.Roles.Where(e =>roleIds.Contains(e.Id)).Select(x =>
                                  new SelectListItem
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Name
                                  });

            return new SelectList(types, "Value", "Text");
        }


        public IEnumerable<SelectListItem> GetSpecificRolesStringValue(Int64[] roleIds)
        {

            var types = _applicationRoleManager.Roles.Where(e => roleIds.Contains(e.Id)).Select(x =>
                                   new SelectListItem
                                   {
                                       Value = x.Name,
                                       Text = x.Name
                                   });

            return new SelectList(types, "Value", "Text");
        }


        public string GetAppSetting(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }



        public int GetIntAppSetting(string key)
        {
            try
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings[key].ToString());
            }
            catch (Exception)
            {
                return 2;
            }
        }

        public string Upload(HttpPostedFileBase file, string foldername)
        {
            var folder = System.Web.HttpContext.Current.Server.MapPath(foldername);
            var fileName = UploadFile(file, folder);
            return fileName;
        }


        private string UploadFile(HttpPostedFileBase file, string folderName)
        {
            var fileExtension = Path.GetExtension(file.FileName);

            var fileName = Guid.NewGuid() + fileExtension;
            if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);
            var folderPath = folderName + "\\" + fileName;
            file.SaveAs(folderPath);
            return folderPath;
        }
    }
}