using Entity;

namespace DataRepository
{
    public interface IStatusDAL
    {
        List<EvaluationStatus> GetAllStatuses();
        public Dictionary<string, int> GetOutstandingEmployeeStatusCodes(int schoolCode);
        public List<int?> GetUpdatedGradesForOutstandingEmployee(int schoolCode);
    }
}