using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Service;
using Entity;


namespace webProjeact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class SchoolController : Controller
    {
        private readonly ISchoolBL _schoolBL;

        public SchoolController(ISchoolBL schoolBL)
        {
            _schoolBL = schoolBL;
        }
        [HttpGet("GetAllSchools")]
        public ActionResult<List<School>> GetAllSchools()
        {
            return Ok(_schoolBL.GetAllSchools());
        }

        [HttpGet("GetGeneralEmployeeEvaluation/{schoolId}")]
        public ActionResult<List<EmployeeData>> GetGeneralEmployeeEvaluation(int schoolId)
        {
            return Ok(_schoolBL.GetGeneralEmployeeEvaluation(schoolId));
        }

        [HttpGet("GetWeightedEmployeeGrade/{schoolId}")]
        public ActionResult<List<EmployeeData>> GetWeightedEmployeeGrade(int schoolId)
        {
            return Ok(_schoolBL.GetWeightedEmployeeGrade(schoolId));
        }

        [HttpGet("GetOutstandingEmployees/{schoolId}")]
        public ActionResult<List<OutstandingEmployeesSchool>> GetOutstandingEmployees(int schoolId)
        {
            return Ok(_schoolBL.GetOutstandingEmployees(schoolId));
        }

        [HttpGet("GetOutstandingEmployee/{schoolId}")]
        public ActionResult<OutstandingEmployeesSchool> GetOutstandingEmployee(int schoolId)
        {
            return Ok(_schoolBL.GetOutstandingEmployee(schoolId));
        }

    }
}
