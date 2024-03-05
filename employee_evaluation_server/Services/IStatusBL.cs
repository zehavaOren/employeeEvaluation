using Entity;

namespace Service
{
    public interface IStatusBL
    {
        List<EvaluationStatus> GetAllStatuses();
        public Dictionary<string, int> GetOutstandingEmployeeStatusCodes(int schoolCode);
        public List<int?> GetUpdatedGradesForOutstandingEmployee(int schoolCode);
    }
}