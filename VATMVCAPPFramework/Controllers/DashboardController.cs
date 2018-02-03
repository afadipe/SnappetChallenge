using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VATMVCAPPFramework.Data.AppEntities;
using VATMVCAPPFramework.Models;
using VATMVCAPPFramework.Repository.CoreRepositories;

namespace VATMVCAPPFramework.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepositoryCommand<StudentPerformance> _StudentPerformanceCommand;
        private readonly IRepositoryQuery<StudentPerformance> _StudentPerformanceQuery;

        private readonly ILog _log;
        public DashboardController(
            IRepositoryQuery<StudentPerformance> StudentPerformanceQuery,
             IRepositoryCommand<StudentPerformance> StudentPerformanceCommand, ILog log)
        {
            _StudentPerformanceQuery = StudentPerformanceQuery;
            _StudentPerformanceCommand = StudentPerformanceCommand;
            _log = log;
        }

        // GET: Dashboard
        public ActionResult Index()
        {

            //clear the tbl of any precious data.
            _StudentPerformanceQuery.ExecuteStoreprocedure("spDeleteStudentPerformances");
            ProcessExcelFile();

            return View();
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
                    foreach (RootObject item in items)
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