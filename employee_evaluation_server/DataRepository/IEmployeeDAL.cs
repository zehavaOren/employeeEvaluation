using Entity;
using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;

namespace DataRepository
{
    public interface IEmployeeDAL
    {
        public List<Employee> GetAllEmployees();
        public Employee GetEmployeeById(string employeeId);
        public List<EmployeeData> GetEmployeesBySupervisorId(string supervisorId);
        public bool InsertGeneralEmployeeEvaluation(GeneralEmployeeEvaluation generalEmployeeEvaluation);
        public int CheckEmployeesInSchool(string employeeId);
        public bool UpdateGeneralEmployeeEvaluation(string employeeId);
        public bool UpdateSchoolStatuses(GeneralEmployeeEvaluation generalEmployeeEvaluation);
        public bool CheckEmployeeExists(string employeeId);
        public List<EmployeeEvaluatioStatus> GetEmployeeEvaluationStatuses();
        public List<EmployeeData> GetWeightedEmployeeScore(string superiorId);
        public List<OutstandingEmployeesSchool> GetOutstandingEmployees(string supervisorId);
        public bool UpdateOutstandingEmployee(GeneralEmployeeEvaluation evaluation);
    }
}
