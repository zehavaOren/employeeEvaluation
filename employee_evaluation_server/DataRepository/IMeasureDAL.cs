using Entity;
using Microsoft.Data.SqlClient;

namespace DataRepository
{
    public interface IMeasureDAL
    {
        public List<MeasureList> GetAllMeasures();
        public List<MeasureGrade> GetAllMeasureGrade();
        public bool InsertOrUpdateEmployeeEvaluationMeasure(EmployeeEvaluationMeasure evaluationData);
    }
}
