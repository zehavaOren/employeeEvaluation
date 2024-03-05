using Entity;

namespace Service
{
    public interface ISchoolBL
    {
        List<School> GetAllSchools();
        public List<EmployeeData> GetGeneralEmployeeEvaluation(int schoolId);
        public List<EmployeeData> GetWeightedEmployeeGrade(int schoolId);
        public List<OutstandingEmployeesSchool> GetOutstandingEmployees(int schoolId);
        public OutstandingEmployeesSchool GetOutstandingEmployee(int schoolId);
    }
}