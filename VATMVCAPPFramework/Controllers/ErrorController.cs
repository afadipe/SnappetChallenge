using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VATMVCAPPFramework.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }

        public ActionResult Error404()
        {
            //return a status code for proper seo
            Response.StatusCode = 404;

            return View();
        }

        public ActionResult Error403()
        {
            //return a status code for proper seo
            Response.StatusCode = 403;

            return View();
        }
    }
}