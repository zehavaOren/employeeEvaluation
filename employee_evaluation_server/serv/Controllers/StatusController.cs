using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Service;
using Entity;
using System.Collections.Generic;


namespace webProjeact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class StatusController : Controller
    {
        private readonly IStatusBL _statusBL;

        public StatusController(IStatusBL statusBL)
        {
            _statusBL = statusBL;
        }

        [HttpGet("GetAllStatuses")]
        public ActionResult<IEnumerable<EvaluationStatus>> GetAllStatuses()
        {
            var measures = _statusBL.GetAllStatuses();
            return Ok(measures);
        }

        [HttpGet("GetOutstandingEmployeeStatusCodes/{schoolCode}")]
        public ActionResult<Dictionary<string, int>> GetOutstandingEmployeeStatusCodes(int schoolCode)
        {
            Dictionary<string, int> data = _statusBL.GetOutstandingEmployeeStatusCodes(schoolCode);
            var outstandingEmployeeStatus = data.Select(kv =>
            new OutstandingEmployeeStatus { EmployeeId = kv.Key, StatusCode = kv.Value }).ToArray();
            return Ok(outstandingEmployeeStatus);
        }
        [HttpGet("GetUpdatedGradesForOutstandingEmployee/{schoolCode}")]
        public ActionResult<List<int?>> GetUpdatedGradesForOutstandingEmployee(int schoolCode)
        {
            return Ok(_statusBL.GetUpdatedGradesForOutstandingEmployee(schoolCode));
        }

    }
}
public class OutstandingEmployeeStatus
{
    public string EmployeeId { get; set; }
    public int StatusCode { get; set; }
}