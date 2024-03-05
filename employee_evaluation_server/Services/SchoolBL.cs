using DataRepository;
using Entity;

namespace Service
{
    public class SchoolBL : ISchoolBL
    {
        private readonly ISchoolDAL _schoolDAL;

        public SchoolBL(ISchoolDAL schoolDAL)
        {
            _schoolDAL = schoolDAL;
        }
        public List<School> GetAllSchools()
        {
            return _schoolDAL.GetAllSchools();
        }
        public List<EmployeeData> GetGeneralEmployeeEvaluation(int schoolId)
        {
            return _schoolDAL.GetGeneralEmployeeEvaluation(schoolId);
        }
        public List<EmployeeData> GetWeightedEmployeeGrade(int schoolId)
        {
            return _schoolDAL.GetWeightedEmployeeGrade(schoolId);
        }
        public List<OutstandingEmployeesSchool> GetOutstandingEmployees(int schoolId)
        {
            return _schoolDAL.GetOutstandingEmployees(schoolId);
        }
        public OutstandingEmployeesSchool GetOutstandingEmployee(int schoolId)
        {
            return _schoolDAL.GetOutstandingEmployee(schoolId);
        }

    }
}