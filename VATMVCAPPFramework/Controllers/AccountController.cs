using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VATMVCAPPFramework.Models;
using VATMVCAPPFramework.Data.IdentityModel;
using VATMVCAPPFramework.Data.IdentityService;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.Utilities;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;
using VATMVCAPPFramework.Data.IdentityService.Extensions;
using log4net;
using VATMVCAPPFramework.Data.Core;
using VATMVCAPPFramework.Data.Constant;
using VATMVCAPPFramework.Data.Entities;
using VATMVCAPPFramework.ViewModel;
using System.Text;
using VATMVCAPPFramework.Repository;
using System.Web.UI;

namespace VATMVCAPPFramework.Controllers
{
    [Authorize]
    [OutputCache(Location = OutputCacheLocation.None, Duration=0)]
    public class AccountController : BaseController
    {
        //private ApplicationSignInManager SignInManager;
        //private ApplicationUserManager UserManager;
        private readonly IRepositoryQuery<ApplicationUser> _applicationUser;
        private readonly IRepositoryCommand<ApplicationUser> _applicationUserCommand;
        private readonly IRepositoryCommand<ApplicationUserPasswordHistory> _passwordCommand;
        private readonly ILog _log;
        private readonly Utility _utility;
        private readonly IActivityLogRepositoryCommand _activityRepo;
        public AccountController(IRepositoryQuery<Data.IdentityModel.ApplicationUser>  applicationUser,
            IRepositoryCommand<Data.IdentityModel.ApplicationUser> applicationUserCommand,
            IActivityLogRepositoryCommand activityRepo,
            IRepositoryCommand<ApplicationUserPasswordHistory> passwordCommand, Utility utility, ILog log)
        {

            // ApplicationUserManager userManager, ApplicationSignInManager signInManager,

            //UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //SignInManager =  HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _utility = utility;
            _applicationUser = applicationUser;
            _applicationUserCommand = applicationUserCommand;
            _passwordCommand = passwordCommand;
            _activityRepo = activityRepo;
            // _organizer = organizer;
            _log = log;
        }
        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return SignInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set 
        //    { 
        //        SignInManager = value; 
        //    }
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return UserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        UserManager = value;
        //    }
        //}

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser usermodel = UserManager.Users.FirstOrDefault(m => m.UserName.Trim() == model.Email.Trim());
            if (usermodel != null)
            {
                if (usermodel.IsActive)
                {
                    // string USERPERMISSION = SetUserPermissions(usermodel.Id);
                    var userIdentity = await UserManager.CreateIdentityAsync(usermodel, DefaultAuthenticationTypes.ApplicationCookie);
                    userIdentity.AddClaim(new Claim("FullName", usermodel.FullName));
                    userIdentity.AddClaim(new Claim("Email", usermodel.Email));
                    userIdentity.AddClaim(new Claim("DateCreated", usermodel.DateCreated.ToString("dd/MM/yyyy")));
                    //userIdentity.AddClaim(new Claim("UserPermission", USERPERMISSION));
                    var listIdentity = new List<ClaimsIdentity>();
                    listIdentity.Add(userIdentity);
                    ClaimsPrincipal c = new ClaimsPrincipal(listIdentity);
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, userIdentity);
                }
                else
                {
                    ModelState.AddModelError("", "Your Account is Inactive ,kindly contact the school for support.");
                    return View(model);
                }
               
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:

                    if (usermodel != null)
                    {
                        if (User.Identity.GetFullName() == string.Empty)
                        {
                            await usermodel.GenerateUserIdentityAsync(UserManager);
                        }
                        if (usermodel.IsFirstLogin)
                        {
                            return RedirectToAction("SetFirstlogin", new { code = usermodel.Id.EncryptID() });
                        }
                    }

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }



        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            CreateViewBagParams();
            return PartialView();
        }


        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            CreateViewBagParams();
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            long UserId = User.Identity.GetUserId<Int64>();
            if (HasNewPasswordBeenUsedRecently(UserId, model.NewPassword))
            {
                ModelState.AddModelError("A previous password can't be used as your new password", "Kindly provide a new password this password ");
                ViewBag.ErrMsg = "A previous password can't be used as your new password, Kindly provide a new password this password ";
                return PartialView(model);

            }
            else
            {

                var result = await UserManager.ChangePasswordAsync(UserId, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    ApplicationUserPasswordHistory passwordModel = new ApplicationUserPasswordHistory();
                    passwordModel.UserId = UserId;
                    passwordModel.DateCreated = DateTime.Now;
                    passwordModel.HashPassword = ExtentionUtility.Encrypt(model.NewPassword);
                    _passwordCommand.Insert(passwordModel);
                    _passwordCommand.SaveChanges();

                    TempData["MESSAGE"] = "Password change successfully";
                    ModelState.Clear();
                    return Json(new { success = true });
                }
                else
                {
                    AddErrors(result);
                    return PartialView(model);
                }
                
            }
        }

        public ActionResult SetFirstlogin(string code)
        {
            try
            {
                SetFirstPasswordViewModel model = new SetFirstPasswordViewModel();
                model.code = code;
                return View(model);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetFirstlogin(SetFirstPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                long UserId = model.code.DecryptID();
                if (HasNewPasswordBeenUsedRecently(UserId, model.Password))
                {
                    //return false;
                    ModelState.AddModelError("A previous password can't be used as your new password", "Kindly provide a new password this password ");
                    return View(model);

                }
                else
                {

                    string code = await UserManager.GeneratePasswordResetTokenAsync(UserId);
                    var result = await UserManager.ResetPasswordAsync(UserId, code,model.Password);
                    if (result.Succeeded)
                    {
                        ApplicationUserPasswordHistory passwordModel = new ApplicationUserPasswordHistory();
                        passwordModel.UserId = UserId;
                        passwordModel.DateCreated = DateTime.Now;
                        passwordModel.HashPassword = ExtentionUtility.Encrypt(model.Password);
                        _passwordCommand.Insert(passwordModel);
                        _passwordCommand.SaveChanges();

                        ApplicationUser xmodel = UserManager.FindById(UserId);
                        xmodel.IsFirstLogin = false;
                        UserManager.Update(xmodel);
                        return RedirectToAction("SetPasswordConfirmation", "Account");
                    }
                    AddErrors(result);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View("Error");
            }
        }

        public ActionResult SetPasswordConfirmation()
        {
            return View();
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }


        private string SetUserPermissions(long userId)
        {
            try
            {
                var mUserPremissionRolemodel = _applicationUser.StoreprocedureQueryFor<UserPremissionAndRole>(AppConstant.FetchUserPermissionAndRole  + " @UserId", new SqlParameter("UserId", userId)).ToList();

                string userPermissions = "";
                if (mUserPremissionRolemodel != null)
                {
                    int i = 0;
                    foreach (var item in mUserPremissionRolemodel)
                    {
                        i = i + 1;
                        if (i == 0)
                        {
                            userPermissions = item.PermissionCode + ",";
                        }
                        else
                        {
                            userPermissions = userPermissions + item.PermissionCode + ",";
                        }
                    }
                }
                return userPermissions;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return string.Empty;
            }
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //UserManager
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(long userId, string code)
        {
            if (userId.ToString() == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            //|| !(await UserManager.IsEmailConfirmedAsync(user.Id))
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null )
                {
                    ModelState.AddModelError("Invalid Email Adddress", "The Specified email address does not exist!");
                    // Don't reveal that the user does not exist or is not confirmed
                    return View(model);
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string portalUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
                
                string callbackUrl = string.Format("Account/ResetPassword?userCode={0}&code={1}",user.Id.EncryptID(),code);
                string mPre = portalUrl + callbackUrl;
                _utility.SendPasswordResetEmail(user, mPre);


                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [AllowAnonymous]
        public ActionResult UsernameRequest()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UsernameRequest(ForgotPasswordViewModel model)
        {
            //|| !(await UserManager.IsEmailConfirmedAsync(user.Id))
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Invalid Email Adddress", "The Specified email address does not exist!");
                    // Don't reveal that the user does not exist or is not confirmed
                    return View(model);
                }
                
                _utility.SendUsernameRequest(user);
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("UsernameRequestConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult UsernameRequestConfirmation()
        {
            return View();
        }


        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string userCode,string code)
        {
            if (Session["ResetPasswordError"]!= null)
            {
                var error = Session["ResetPasswordError"].ToString();
                ModelState.AddModelError("", error);
            }
            if (code == null)
                return View("Error");
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.userCode = userCode;
            model.Code = code;
            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            model.Code=model.Code;
            model.userCode=model.userCode;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Int64 UserId=model.userCode.DecryptID();
            var user = await UserManager.FindByIdAsync(UserId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            //check here if password exist b4
            if (HasNewPasswordBeenUsedRecently(user.Id, model.Password))
            {
               
                //return false;
                Session["ResetPasswordError"] = "Kindly provide a new password this password has being used previously";
                ModelState.AddModelError("A previous password can't be used as your new password", "Kindly provide a new password this password has being used previously");
                return RedirectToAction("ResetPassword", "Account", new { area = "", userCode =model.userCode, Code =model.Code });
               // return View(model);

            }
            else
            {

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                var result = await UserManager.ResetPasswordAsync(user.Id,code, model.Password);
                if (result.Succeeded)
                {
                    ApplicationUserPasswordHistory passwordModel = new ApplicationUserPasswordHistory();
                    passwordModel.UserId = user.Id;
                    passwordModel.DateCreated = DateTime.Now;
                    passwordModel.HashPassword = ExtentionUtility.Encrypt(model.Password);
                    _passwordCommand.Insert(passwordModel);
                    _passwordCommand.SaveChanges();

                    ApplicationUser xmodel = UserManager.FindById(user.Id);
                    xmodel.IsFirstLogin = false;
                    UserManager.Update(xmodel);

                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        public bool HasNewPasswordBeenUsedRecently(Int64 userId, string proposedNewPassword)
        {
            int numberOfRecentPasswordsToKeep = ExtentionUtility.GetIntAppSetting("NoOfPasswordCheck"); 
            //first delete old record
            _applicationUser.ExecuteStoreprocedure("spPasswordHistoryDeleteNonRecentPasswords   @UserId,@numberOfRecentPasswordsToKeep", new SqlParameter("UserId", userId), new SqlParameter("numberOfRecentPasswordsToKeep", numberOfRecentPasswordsToKeep));
            //Now get recent password details
            var hashedPasswordDetails = _applicationUser.ExecuteStoredProdure<HashedPasswordDetails>("spPasswordHistorySelect  @UserId,@numberOfRecentPasswordsToKeep", new SqlParameter("UserId", userId), new SqlParameter("numberOfRecentPasswordsToKeep", numberOfRecentPasswordsToKeep));
            foreach (HashedPasswordDetails passwordDetails in hashedPasswordDetails)
            {
                var encodePassword = ExtentionUtility.Encrypt(proposedNewPassword);
                if (ProposedNewPasswordMatchesAPreviousPassword(passwordDetails, encodePassword))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ProposedNewPasswordMatchesAPreviousPassword(HashedPasswordDetails passwordDetails, string encodePassword)
        {
            return encodePassword == passwordDetails.HashPassword;
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (string.IsNullOrEmpty(userId.ToString()))
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }


        [Authorize]
        public async Task<ActionResult> MyProfile()
        {
            try
            {
                long UserId = User.Identity.GetUserId<Int64>();
                ApplicationUser usermodel = await _applicationUser.GetAsync(UserId);
                UserViewModel userdataModel = UserViewModel.EntityToModels(usermodel);
                if (userdataModel == null)
                {
                    return HttpNotFound();
                }
                return View(userdataModel);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View("Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MyProfile(UserViewModel model)
        {
            long UserId = User.Identity.GetUserId<Int64>();
            try
            {
                if (ModelState.IsValid)
                {
                    var emailExist = _applicationUser.GetAllList(m => m.Email.ToLower().Trim() == model.Email.ToLower().Trim() && m.Id != model.Id).ToList();
                    if (emailExist.Any())
                    {
                        ModelState.AddModelError("", "email address already exist");
                        ViewBag.ErrMsg = "email address already exist";
                        return View(model);
                    }

                    //checking if username does not exist b4
                    var usernameExist = _applicationUser.GetAllList(m => m.UserName.ToLower().Trim() == model.UserName.ToLower().Trim() && m.Id != model.Id).ToList();
                    if (usernameExist.Any())
                    {
                        ModelState.AddModelError("", "username already exist");
                        ViewBag.ErrMsg = "username already exist";
                        return View(model);
                    }

                    ApplicationUser usermodel = await _applicationUser.GetAsync(model.Id);
                    //if (usermodel != null)
                    {
                        usermodel.FirstName = model.FirstName;
                        usermodel.LastName = model.LastName;
                        usermodel.MiddleName = model.MiddleName;
                        usermodel.MobileNumber = model.MobileNumber;
                        usermodel.PhoneNumber = model.PhoneNumber;
                        //if (!string.IsNullOrEmpty(model.DOB))
                        //{
                        //    usermodel.DOB = ExtentionUtility.ConvertDateValue(model.DOB);
                        //}
                       
                        //usermodel.Address = model.Address;
                        await _applicationUserCommand.UpdateAsync(usermodel);
                        await _applicationUserCommand.SaveChangesAsync();
                        _activityRepo.CreateActivityLog(string.Format("Updtae User Id:{0} with Name :{1}", usermodel.Id, (usermodel.LastName + " " + usermodel.FirstName)), this.GetContollerName(), this.GetActionName(), usermodel.Id, null);


                        ViewBag.Msg = "User Profile was successfully updated";
                        ModelState.Clear();
                        return View(model);
                    }
                }
                else
                {
                    StringBuilder errorMsg = new StringBuilder();

                    foreach (var modelError in ModelState.Values.SelectMany(modelState => modelState.Errors))
                    {
                        errorMsg.AppendLine(modelError.ErrorMessage);
                        ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                    }
                    ViewBag.ErrMsg = errorMsg.ToString();
                    return View(model);
                }
              //  return View(model);

            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View("Error");
            }
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UserManager != null)
                {
                    UserManager.Dispose();
                    //UserManager = null;
                }

                if (SignInManager != null)
                {
                    SignInManager.Dispose();
                    //SignInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard", new { area = "APPPortal" });

        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}