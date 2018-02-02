using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VATMVCAPPFramework.Data.AppEntities;
using VATMVCAPPFramework.Data.Entities;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.ViewModel;

namespace VATMVCAPPFramework.Controllers
{
    [RoutePrefix("api/Metric")]
    public class MetricController : ApiController
    {
        private readonly ILog _log;
        private readonly IRepositoryQuery<StudentPerformance> _armQuery;

        public MetricController(IRepositoryQuery<StudentPerformance> armQuery, ILog log)
        {
            _armQuery = armQuery;
            _log = log;
        }


        [HttpGet]
        [Route("AssignmentByStudent")]
        public async Task<IHttpActionResult> AssignmentByStudent()
        {
            try
            {
                List<StudentBIAnalysis> model = await _armQuery.ExecuteStoredProdure<StudentBIAnalysis>("spGetAssignmentByStudentBI").ToListAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("StudentCountPerAssignment")]
        public async Task<IHttpActionResult> StudentCountPerAssignment()
        {
            try
            {
                List<StudentBIAnalysisX> model = await _armQuery.ExecuteStoredProdure<StudentBIAnalysisX>("spStudnetCountPerAssignmentBI").ToListAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return BadRequest();
            }
        }





 

    }
}
