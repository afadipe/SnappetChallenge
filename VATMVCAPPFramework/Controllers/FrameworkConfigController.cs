using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VATMVCAPPFramework.Data.Entities;
using VATMVCAPPFramework.Data.IdentityModel;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.Repository;
using VATMVCAPPFramework.Utilities;
using log4net;
using System.IO;
using Newtonsoft.Json;
using VATMVCAPPFramework.Data.AppEntities;

namespace VATMVCAPPFramework.Controllers
{
    public class RootObject
    {
        public int SubmittedAnswerId { get; set; }
        public DateTime SubmitDateTime { get; set; }
        public int Correct { get; set; }
        public int Progress { get; set; }
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public string Difficulty { get; set; }
        public string Subject { get; set; }
        public string Domain { get; set; }
        public string LearningObjective { get; set; }
    }


    public class FrameworkConfigController : Controller
    {
        private readonly IRepositoryQuery<Application> _applicationQuery;
        private readonly IRepositoryQuery<ApplicationUser> _applicationUserQuery;
        private readonly IRepositoryCommand<StudentPerformance> _StudentPerformanceCommand;
        private readonly IActivityLogRepositoryCommand _activityRepo;
        private readonly ILog _log;
        public FrameworkConfigController(IActivityLogRepositoryCommand activityRepo,
            IRepositoryQuery<Application> application,
             IRepositoryCommand<StudentPerformance> StudentPerformanceCommand,
            IRepositoryQuery<ApplicationUser> applicationUser,ILog log)
        {
            _applicationQuery = application;
            _applicationUserQuery = applicationUser;
            _activityRepo = activityRepo;
            _StudentPerformanceCommand = StudentPerformanceCommand;
            _log = log;
        }
        // GET: FrameworkConfig
        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                // ProcessExcelFile();

                _log.InfoFormat("VATMVC Framework Config checked @ : {0}", DateTime.Now);
                _activityRepo.CreateActivityLog("In Framework setting checking if application portal has being configured", this.GetContollerName(), this.GetActionName(), 0, null);
                if (_applicationQuery.Count() >= 1)
                {
                    if (_applicationQuery.GetAll().FirstOrDefault().HasAdminUserConfigured)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        return RedirectToAction("Start", "FrameworkSetup", new { area = "Portal" });
                    }
                }
                else
                {
                    return RedirectToAction("Start", "FrameworkSetup", new { area = "Portal" });
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return View("Error");
            }
        }

        public void ProcessExcelFile()
        {

            //get the Json filepath  
            string file = Server.MapPath("~/JsonFile/work.json");
            //using (StreamReader r = new StreamReader(@"C:\Users\afadipe\Documents\Visual Studio 2015\Projects\SnappetChallengeApp\VATMVCAPPFramework\JsonFile\work.json"))
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                List<RootObject> items = JsonConvert.DeserializeObject<List<RootObject>>(json);
                if (items.Any())
                {
                    foreach(RootObject item in items)
                    {
                        StudentPerformance performanceItem = new StudentPerformance();
                        performanceItem.Correct = item.Correct;
                        performanceItem.DateCreated = DateTime.Now;
                        performanceItem.SubmittedAnswerId = item.SubmittedAnswerId;
                        performanceItem.SubmitDateTime = item.SubmitDateTime;
                        performanceItem.Progress = item.Progress;
                        performanceItem.UserId = item.UserId;
                        performanceItem.ExerciseId = item.ExerciseId;
                        performanceItem.Difficulty = item.Difficulty;
                        performanceItem.Progress = item.Progress;
                        performanceItem.Subject = item.Subject;
                        performanceItem.Domain = item.Domain;
                        performanceItem.LearningObjective = item.LearningObjective;
                        _StudentPerformanceCommand.Insert(performanceItem);
                        _StudentPerformanceCommand.SaveChanges();
                    }
                    
    }

                //is well

            }
        }
    }
}