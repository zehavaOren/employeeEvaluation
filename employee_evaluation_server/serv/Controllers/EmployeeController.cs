using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Service;
using Entity;


namespace webProjeact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeBL _businessLogicLayer;

        public EmployeeController(IEmployeeBL businessLogicLayer)
        {
            _businessLogicLayer = businessLogicLayer;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            var employees = _businessLogicLayer.GetAllEmployees();
            return Ok(employees);

        }

        [HttpGet("GetEmployeeById/{id}")]
        public ActionResult<Employee> GetEmployeeById(string id)
        {
            var employee = _businessLogicLayer.GetEmployeeById(id);
            return Ok(employee);
        }

        [HttpGet("GetEmployeesBySupervisorId/{supervisorId}")]
        public ActionResult<IEnumerable<EmployeeData>> GetEmployeesBySupervisorId(string supervisorId)
        {
            var employees = _businessLogicLayer.GetEmployeesBySupervisorId(supervisorId);
            return Ok(employees);
        }

        [HttpPost("InsertGeneralEmployeeEvaluation")]
        public ActionResult<bool> InsertGeneralEmployeeEvaluation(GeneralEmployeeEvaluation generalEmployeeEvaluation)
        {
            var insertResult= _businessLogicLayer.InsertGeneralEmployeeEvaluation(generalEmployeeEvaluation);
            return Ok(insertResult);
        }

        [HttpGet("CheckEmployeeExists/{id}")]
        public bool CheckEmployeeExists(string id)
        {
            return _businessLogicLayer.CheckEmployeeExists(id);
        }

        [HttpGet("GetEmployeeEvaluationStatuses")]
        public ActionResult<List<EmployeeEvaluatioStatus>> GetEmployeeEvaluationStatuses()
        {
            return Ok(_businessLogicLayer.GetEmployeeEvaluationStatuses());
        }

        [HttpGet("GetWeightedEmployeeScore/{superiorId}")]
        public ActionResult<List<EmployeeData>> GetWeightedEmployeeScore(string superiorId)
        {
            return Ok(_businessLogicLayer.GetWeightedEmployeeScore(superiorId));
        }
       
        [HttpGet("GetOutstandingEmployees/{supervisorId}")]
        public ActionResult<List<OutstandingEmployeesSchool>> GetOutstandingEmployees(string supervisorId)
        {
            return Ok(_businessLogicLayer.GetOutstandingEmployees(supervisorId));
        }

        [HttpPost("UpdateOutstandingEmployee")]
        public ActionResult<bool> UpdateOutstandingEmployee(GeneralEmployeeEvaluation evaluation)
        {
            return Ok(_businessLogicLayer.UpdateOutstandingEmployee(evaluation));
        }



    }
}
