using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using VATMVCAPPFramework.Data.IdentityService;
using System.Security.Claims;
using VATMVCAPPFramework.Data.Constant;
using log4net;
using System.Web.Security;

namespace VATMVCAPPFramework.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILog _log;
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;
        protected ApplicationRoleManager _roleManager;
        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ??
                       HttpContext.GetOwinContext()
                           .Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                       HttpContext.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }

        protected ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ??
                       HttpContext.GetOwinContext()
                           .GetUserManager<ApplicationRoleManager>();
            }
            private set { _roleManager = value; }
        }

        public BaseController()
        {
        }

        //public BaseController(ApplicationSignInManager SignInManager, ApplicationUserManager UserManager, ApplicationRoleManager RoleManagerr)
        //{
        //    _signInManager = SignInManager;
        //    _userManager = UserManager;
        //    _roleManager = RoleManagerr;

        //}
        protected void CreateViewBagParams()
        {
            ViewBag.IsUpdate = false;
            ViewBag.ModalTitle = "Add";
            ViewBag.ButtonAction = "Save";
            ViewBag.PostAction = "Create";
            ViewBag.ImageProperty = "fileinput-new";
            ViewBag.ImageProperty2 = "fileinput-new";
            ViewBag.ButtonActionCss = "btn btn-primary";
            ViewBag.ButtonActionAddIcon = "fa fa-plus-circle";
            ViewBag.ButtonActionCloseIcon = "fa fa-plus-circle";
        }
        protected void EditViewBagParams()
        {
            ViewBag.IsUpdate = true;
            ViewBag.ModalTitle = "Edit";
            ViewBag.ButtonAction = "Update";
            ViewBag.PostAction = "Edit";
        }
        protected void EditViewBagParams(string imageUrl)
        {
            ViewBag.IsUpdate = true;
            ViewBag.ModalTitle = "Edit";
            ViewBag.ButtonAction = "Update";
            ViewBag.PostAction = "Edit";
            ViewBag.ImageProperty = string.IsNullOrEmpty(imageUrl) ? "fileinput-new" : "fileinput-exists";
            ViewBag.ImageProperty2 = "fileinput-exists";
        }
        public string GeneratePassword()
        {
            return Membership.GeneratePassword(8, 3);
        }
        protected long GetCurrentUserId()
        {
            return User.Identity.GetUserId<Int64>();
        }

        protected long GetLastUserId()
        {
            return UserManager.Users.ToList().Last().Id + 1;
        }

        protected void EditViewBagParams(string imageUrl, string imageUrl2)
        {
            ViewBag.IsUpdate = true;
            ViewBag.ModalTitle = "Edit";
            ViewBag.ButtonAction = "Update";
            ViewBag.PostAction = "Edit";
            ViewBag.ImageProperty = string.IsNullOrEmpty(imageUrl) ? "fileinput-new" : "fileinput-exists";
            ViewBag.ImageProperty2 = string.IsNullOrEmpty(imageUrl2) ? "fileinput-new" : "fileinput-exists";

        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

        protected bool IsUserPortalAdmin()
        {
            if (User.IsInRole(AppConstant.PortalAdminRoleName))
            {
                return true;
            }
            return false;
        }

        
    }
}