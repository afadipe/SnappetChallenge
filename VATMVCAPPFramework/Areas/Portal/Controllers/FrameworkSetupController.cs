using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VATMVCAPPFramework.ViewModel;
using VATMVCAPPFramework.Utilities;
using VATMVCAPPFramework.Data.Entities;
using VATMVCAPPFramework.Data.IdentityModel;
using VATMVCAPPFramework.Repository;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.Data.IdentityService;
using System.Threading.Tasks;
using log4net;

namespace VATMVCAPPFramework.Areas.Portal.Controllers
{
    public class FrameworkSetupController : Controller
    {
        private FrameworkSetupViewModel _setupContract;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IRepositoryCommand<ApplicationUserPasswordHistory> _applicationUserPwdhistoryCommand;
        private readonly IRepositoryQuery<Application> _applicationQuery;
        private readonly IRepositoryCommand<Application> _applicationCommand;
        private readonly IRepositoryQuery<ApplicationUser> _applicationUserQuery;
        private readonly IRepositoryQuery<PortalVersion> _portalversionQuery;
        private readonly IActivityLogRepositoryCommand _activityRepo;
        private readonly ILog _log;

        public FrameworkSetupController(IActivityLogRepositoryCommand activityRepo, IRepositoryQuery<Application> application, IRepositoryCommand<Application> applicationCommand, IRepositoryQuery<PortalVersion> portalversion, IRepositoryQuery<ApplicationUser> applicationUser, IRepositoryCommand<ApplicationUserPasswordHistory> applicationUserPwdhistory, ILog log)
        {
            _applicationQuery = application;
            _applicationCommand = applicationCommand;
            _applicationUserQuery = applicationUser;
            _applicationUserPwdhistoryCommand = applicationUserPwdhistory;
            _portalversionQuery = portalversion;
            _activityRepo = activityRepo;
            _log = log;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _setupContract = (Request.Form["FrameworkSetupViewModel"].Deserialize() ?? TempData["FrameworkSetupViewModel"]
                                                                                       ?? new FrameworkSetupViewModel()) as FrameworkSetupViewModel;
            TryUpdateModel(_setupContract);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Result is RedirectToRouteResult)
                TempData["FrameworkSetupViewModel"] = _setupContract;
        }

        [AllowAnonymous]
        public ActionResult Start(string nextButton)
        {
            if (nextButton != null) return RedirectToAction("CurrentConfig");
            ViewBag.WelcomeMessage = "Welcome to VATEBRA's VAT Framework Technology For MVC";
            return View(_setupContract);

        }

        [AllowAnonymous]
        public ActionResult CurrentConfig(string nextButton, string backButton)
        {

            _activityRepo.CreateActivityLog("In Framework setup currentconfig", this.GetContollerName(), this.GetActionName(), 0, null);

            if (nextButton != null)
            {
                return RedirectToAction("FrameworkSetting");
            }
            if (backButton != null)
            {
                return RedirectToAction("Start");
            }
            if (!LoadDefaultSettings())
            {
                return View(_setupContract);
            }

            return View(_setupContract);

        }

        [AllowAnonymous]
        public ActionResult FrameworkSetting(string nextButton, string backButton)
        {
            ModelState.Clear();
            _activityRepo.CreateActivityLog("In Framework setting currentconfig", this.GetContollerName(), this.GetActionName(), 0, null);
            if (backButton != null)
            {
                return RedirectToAction("CurrentConfig");
            }

            if (nextButton != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(_setupContract);
                }

                if (string.IsNullOrEmpty(_setupContract.PortalSetting.PortalTitle))
                {
                    ModelState.AddModelError("", "Portal title is required");
                    return View(_setupContract);
                }

                var app = new Application { ApplicationName = _setupContract.PortalSetting.PortalTitle, Description = _setupContract.PortalSetting.PortalDescription, TermsAndConditions = _setupContract.PortalSetting.TermsAndConditionPath, HasAdminUserConfigured = false };

                if (_applicationQuery.GetAll().Any())
                {
                    Application datamodel = _applicationQuery.GetAll().FirstOrDefault();
                    app.Id = datamodel.Id;
                    datamodel.ApplicationName = app.ApplicationName;
                    datamodel.Description = app.Description;
                    datamodel.TermsAndConditions = app.TermsAndConditions;
                    _applicationCommand.Update(datamodel);
                }
                else
                {
                    _applicationCommand.Insert(app);

                }
                _applicationCommand.SaveChanges();
                if (app.Id >= 1)
                {
                    _activityRepo.CreateActivityLog("creating Framework application data",this.GetContollerName(),this.GetActionName(),0, app);
                    return RedirectToAction("FramewokAdmin");
                }
                ModelState.AddModelError("", "Unable to save framework settings due to internal error! Please try again later");
                return View(_setupContract);
            }
            var portalInfo = _applicationQuery.GetAll().Select(PortalSettingViewModel.EntityToModels).FirstOrDefault();
            if (portalInfo == null)
            {
               // ModelState.AddModelError("", "Unable to initialize portal information due to internal error! Please try again later");
                return View(_setupContract);
            }

            _setupContract.PortalSetting = portalInfo;
            return View(_setupContract);

            //add settings to DB
        }

        [AllowAnonymous]
        public async Task<ActionResult> FramewokAdmin(string nextButton, string backButton)
        {
            if (backButton != null)
            {
                return RedirectToAction("FrameworkSetting");
            }

            if (nextButton != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(_setupContract);
                }
                if ( string.Compare(_setupContract.AdminUserSetting.Password, 
                    _setupContract.AdminUserSetting.ConfirmPassword,
                        StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    ViewBag.ErrMsg = "Password and confirm password must be equal";
                  // ModelState.AddModelError("","Password and confirm password must be equal");
                    return View(_setupContract);
                }


                var user = new ApplicationUser
                {
                    FirstName = _setupContract.AdminUserSetting.FirstName,
                    LastName = _setupContract.AdminUserSetting.LastName,
                    MiddleName = _setupContract.AdminUserSetting.MiddleName,
                    UserName = _setupContract.AdminUserSetting.UserName,
                    Email = _setupContract.AdminUserSetting.Email,
                    MobileNumber = _setupContract.AdminUserSetting.MobileNumber,
                    PhoneNumber = _setupContract.AdminUserSetting.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    DateCreated = DateTime.Now,
                    IsFirstLogin=false
                };
                var result = await UserManager.CreateAsync(user, _setupContract.AdminUserSetting.Password);
                if (result.Succeeded)
                {

                    ApplicationUserPasswordHistory passwordModel = new ApplicationUserPasswordHistory();
                    passwordModel.UserId = user.Id;
                    passwordModel.DateCreated = DateTime.Now;
                    passwordModel.HashPassword = ExtentionUtility.Encrypt(_setupContract.AdminUserSetting.Password);
                    passwordModel.CreatedBy = user.Id;
                    _applicationUserPwdhistoryCommand.Insert(passwordModel);
                    _applicationUserPwdhistoryCommand.Save();

                    var addRoleResult = await UserManager.AddToRoleAsync(user.Id, "PortalAdmin");
                    if (addRoleResult.Succeeded)
                    {
                        Application applicationmodel = _applicationQuery.GetAll().FirstOrDefault();
                        applicationmodel.HasAdminUserConfigured = true;
                        _applicationCommand.Update(applicationmodel);
                        _applicationCommand.SaveChanges();
                        _activityRepo.CreateActivityLog("creating Framework admin user details", this.GetContollerName(), this.GetActionName(), _setupContract.AdminUserSetting.Id, _setupContract.AdminUserSetting);
                        return RedirectToAction("Login", "Account", new { area = "" });
                    }

                }else
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault().ToString());
                }
                return View(_setupContract);
            }

            var userInfo = UserManager.Users.ToList().Select(AdminUserSettingViewModel.EntityToModels).FirstOrDefault();
            if (userInfo == null)
            {
                //ModelState.AddModelError("", "Unable to initialize admin user information due to internal error! Please try again later");
                return View(_setupContract);
            }
            _setupContract.AdminUserSetting = userInfo;
            return View(_setupContract);

        }

        #region Controller Helpers

        private bool LoadDefaultSettings()
        {
            var defaultSetting = _portalversionQuery.GetAll().Select(FrameworkDefaultSettingViewModel.EntityToModels).FirstOrDefault();
            if (defaultSetting == null)
            {
               // ModelState.AddModelError("", "Unable to load default settings! Please check config file");
                return false;
            }
            if (_setupContract == null)
            {
                _setupContract = new FrameworkSetupViewModel();
            }
            _setupContract.DefaultSetting = defaultSetting;
            return true;
        }
        #endregion
    }
}