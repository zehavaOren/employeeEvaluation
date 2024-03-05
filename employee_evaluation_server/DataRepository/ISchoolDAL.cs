using Entity;

namespace DataRepository
{
    public interface ISchoolDAL
    {
        List<School> GetAllSchools();
        public List<EmployeeData> GetGeneralEmployeeEvaluation(int schoolId);
        public List<EmployeeData> GetWeightedEmployeeGrade(int schoolId);
        public List<OutstandingEmployeesSchool> GetOutstandingEmployees(int schoolId);
        public OutstandingEmployeesSchool GetOutstandingEmployee(int schoolId);
    }
}