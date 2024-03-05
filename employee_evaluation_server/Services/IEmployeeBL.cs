using DataRepository;
using Entity;

namespace Service
{
    public interface IEmployeeBL
    {

        public List<Employee> GetAllEmployees();
        public Employee GetEmployeeById(string employeeId);
        public List<EmployeeData> GetEmployeesBySupervisorId(string supervisorId);
        public bool InsertGeneralEmployeeEvaluation(GeneralEmployeeEvaluation generalEmployeeEvaluation);
        public bool ValidateID(string id);
        public bool CheckEmployeeExists(string employeeId);
        public List<EmployeeEvaluatioStatus> GetEmployeeEvaluationStatuses();
        public List<EmployeeData> GetWeightedEmployeeScore(string superiorId);
        public List<OutstandingEmployeesSchool> GetOutstandingEmployees(string supervisorId);
        public bool UpdateOutstandingEmployee(GeneralEmployeeEvaluation evaluation);
    }
}