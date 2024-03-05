using DataRepository;
using Entity;

namespace Service
{
    public class MeasureBL : IMeasureBL
    {
        private readonly IMeasureDAL _measureDAL;

        public MeasureBL(IMeasureDAL measureDAL)
        {
            _measureDAL = measureDAL;
        }
        public List<MeasureList> GetAllMeasures()
        {
            return _measureDAL.GetAllMeasures();
        }
        public List<MeasureGrade> GetAllMeasureGrade()
        {
            return _measureDAL.GetAllMeasureGrade();
        }
        public bool InsertOrUpdateEmployeeEvaluationMeasure(EmployeeEvaluationMeasure evaluationData)
        {
            return _measureDAL.InsertOrUpdateEmployeeEvaluationMeasure(evaluationData);
        }


    }
}