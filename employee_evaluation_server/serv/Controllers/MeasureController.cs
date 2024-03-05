using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Service;
using Entity;


namespace webProjeact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class MeasureController : Controller
    {
        private readonly IMeasureBL _measureBL;

        public MeasureController(IMeasureBL measureBL)
        {
            _measureBL = measureBL;
        }

        [HttpGet("GetAllMeasures")]
        public ActionResult<IEnumerable<MeasureList>> GetAllMeasures()
        {
            var measures = _measureBL.GetAllMeasures();
            return Ok(measures);
        }

        [HttpGet("GetAllMeasureGrade")]
        public ActionResult<IEnumerable<MeasureGrade>> GetAllMeasureGrade()
        {
            var measureGrades = _measureBL.GetAllMeasureGrade();
            return Ok(measureGrades);
        }

        [HttpPost("InsertOrUpdateEmployeeEvaluationMeasure")]
        public ActionResult<bool> InsertOrUpdateEmployeeEvaluationMeasure([FromBody] EmployeeEvaluationMeasure evaluationData)
        {
            bool result = _measureBL.InsertOrUpdateEmployeeEvaluationMeasure(evaluationData);
            return result;
        }


    }
}
