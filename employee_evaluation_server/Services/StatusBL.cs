using DataRepository;
using Entity;

namespace Service
{
    public class StatusBL : IStatusBL
    {
        private readonly IStatusDAL _statusDAL;

        public StatusBL(IStatusDAL statusDAL)
        {
            _statusDAL = statusDAL;
        }
        public List<EvaluationStatus> GetAllStatuses()
        {
            return _statusDAL.GetAllStatuses();
        }
        public Dictionary<string, int> GetOutstandingEmployeeStatusCodes(int schoolCode)
        {
            return _statusDAL.GetOutstandingEmployeeStatusCodes(schoolCode);
        }
        public List<int?> GetUpdatedGradesForOutstandingEmployee(int schoolCode)
        {
            return _statusDAL.GetUpdatedGradesForOutstandingEmployee(schoolCode);
        }

    }
}