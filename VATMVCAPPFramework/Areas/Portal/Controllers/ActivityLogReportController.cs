using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.IdentityModel;
using VATMVCAPPFramework.Data.Entities;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.ViewModel;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Text.RegularExpressions;
using VATMVCAPPFramework.Data.Core;
using VATMVCAPPFramework.Utilities;

namespace VATMVCAPPFramework.Areas.Portal.Controllers
{
    [Authorize]
    public class ActivityLogReportController : Controller
    {

        private readonly IRepositoryQuery<ActivityLog> _activitylogQuery;
        private readonly IRepositoryCommand<ActivityLog> _activitylogCommand;
        private readonly IRepositoryQuery<ApplicationUser> _applicationUserQuery;
        private readonly ILog _log;

        public ActivityLogReportController(IRepositoryCommand<ActivityLog> activitylogCommand, IRepositoryQuery<ActivityLog> activitylogQuery, IRepositoryQuery<ApplicationUser> applicationUserQuery, ILog log)
        {
            _activitylogCommand = activitylogCommand;
            _activitylogQuery = activitylogQuery;
            _applicationUserQuery = applicationUserQuery;
            _log = log;
        }

        // GET: Portal/ActivityLogReport
        public async Task<ActionResult> ActivityLog()
        {
            try
            {
               
                LoadViewDataForDropDownList();
                var activitylogModel = await _activitylogQuery.StoreprocedureQuery<ActivityInfo>("SpGetAllActivityLog").ToListAsync();
                ViewData["SearchResult"] = activitylogModel;
                return View();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ActivityLog(ActivitlogSearchInfo searchvm)
        {
            try
            {
                var SelectedController = string.Empty;
                DateTime sDate = ExtentionUtility.ConvertDateValue(searchvm.SelectedStartDate);
                DateTime eDate = ExtentionUtility.ConvertDateValue(searchvm.SelectedEndDate);

                if (!string.IsNullOrEmpty(searchvm.SelectedController))
                {
                    int Textlength = searchvm.SelectedController.Length;
                    int removelength = "Controller".ToString().Length;
                    int requiredlength = Textlength - removelength;
                    string realText = searchvm.SelectedController.Substring(0, requiredlength);
                    // string realText = Regex.Replace(searchvm.SelectedController, @"\([Controller]\)", "");
                    SelectedController = realText;
                }
                var activitylogModel = await _activitylogQuery.StoreprocedureQueryFor<ActivityInfo>("SpGetActivityLog  @UserId,@Controller,@StartDate,@EndDate", new SqlParameter("UserId", searchvm.SelectedUser.GetValueOrDefault()),new SqlParameter("controller", SelectedController), new SqlParameter("StartDate", sDate), new SqlParameter("EndDate",eDate)).ToListAsync();
                ViewData["SearchResult"] = activitylogModel;
                LoadViewDataForDropDownList();
                return View("");
            }
            catch (Exception ex)
            {

                _log.Error(ex);
                return View("Error");
            }
        }


        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList();
        }
        
        public IEnumerable<SelectListItem> GetControllerNames()
        {
            var types = GetSubClasses<Controller>().Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name
            }).AsEnumerable();
            return new SelectList(types, "Value", "Text");
        }
        

        private void LoadViewDataForDropDownList()
        {
            try
            {
                ViewData["ControllerList"] = GetControllerNames();
                ViewData["UserList"] = _applicationUserQuery.GetAll().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FirstName + " " + x.MiddleName + " " + x.LastName
                }).AsEnumerable();
            }
            catch (Exception ex)
            {
                _log.Error(ex);

            }

        }

    }
}