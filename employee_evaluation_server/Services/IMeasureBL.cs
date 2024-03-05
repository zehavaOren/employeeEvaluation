using DataRepository;
using Entity;

namespace Service
{
    public interface IMeasureBL
    {
        public List<MeasureList> GetAllMeasures();
        public List<MeasureGrade> GetAllMeasureGrade();
        public bool InsertOrUpdateEmployeeEvaluationMeasure(EmployeeEvaluationMeasure evaluationData);
    }
}