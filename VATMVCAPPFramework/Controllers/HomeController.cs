using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using log4net;
using VATMVCAPPFramework.Data.IdentityModel;
using VATMVCAPPFramework.Data.IdentityService;
using VATMVCAPPFramework.Data.Entities;
using System.Data;
using VATMVCAPPFramework.Data.ConnectionFactory;

namespace VATMVCAPPFramework.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILog _log;
        
        public HomeController(ILog log)
        {
            _log = log;
        }

       // [Authorize]
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View();
                //return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        [Authorize]
        public async Task<ActionResult> MyProfile()
        {

            if (User.Identity.IsAuthenticated)
            {
                Int64 UserId = User.Identity.GetUserId<Int64>();
                try
                {
                    ApplicationUser usermodel = await UserManager.FindByIdAsync(UserId);
                    return View(usermodel);
                }
                catch (Exception exp)
                {
                    _log.Error(exp);
                    return View("Error");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}