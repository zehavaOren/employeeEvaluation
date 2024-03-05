using DataRepository;
using Entity;

namespace Service
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeDAL _dataAccessLayer;

        public EmployeeBL(IEmployeeDAL dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public List<Employee> GetAllEmployees()
        {
            return _dataAccessLayer.GetAllEmployees();
        }
        public Employee GetEmployeeById(string employeeId)
        {
            if (ValidateID(employeeId))
            {
                return _dataAccessLayer.GetEmployeeById(employeeId);
            }
            else
            {
                return null;
            }
            
        }
        public List<EmployeeData> GetEmployeesBySupervisorId(string supervisorId)
        {
            return _dataAccessLayer.GetEmployeesBySupervisorId(supervisorId);
        }
        public bool InsertGeneralEmployeeEvaluation(GeneralEmployeeEvaluation generalEmployeeEvaluation)
        {
            GeneralEmployeeEvaluation generalEmployeeEvaluationWithStatus=checkStatuc(generalEmployeeEvaluation);
            return _dataAccessLayer.InsertGeneralEmployeeEvaluation(generalEmployeeEvaluationWithStatus);
        }

        public bool ValidateID(string id)
        {
            // Check if the ID is not null and consists of exactly 9 digits
            if (!string.IsNullOrEmpty(id) && id.Length == 9 && id.All(char.IsDigit))
            {
                return true; // ID is valid
            }
            return false; // ID is invalid
        }
        //הזנת סטטוס מתאים 2 או 3
        public GeneralEmployeeEvaluation checkStatuc(GeneralEmployeeEvaluation generalEmployeeEvaluation)
        {
            int EvaluationStatusCode = _dataAccessLayer.CheckEmployeesInSchool(generalEmployeeEvaluation.EmployeeId);
            //כל העובדים במסגרת של העובד הנ"ל עודכנו- לך לעדכן את המצטיינים
            if(EvaluationStatusCode == 3)
            {
                bool updateResult= updateSchoolStatuses(generalEmployeeEvaluation);
                if(updateResult)
                {
                    if (updateOutstandingEmployeeRating(generalEmployeeEvaluation)) 
                    {
                        generalEmployeeEvaluation.EvaluationStatusCode = EvaluationStatusCode;
                        return generalEmployeeEvaluation;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                generalEmployeeEvaluation.EvaluationStatusCode = EvaluationStatusCode;
                return generalEmployeeEvaluation;
            }
            
        }
        //הזנת סטטוס למצטיינים 4
        public bool updateOutstandingEmployeeRating(GeneralEmployeeEvaluation generalEmployeeEvaluation)
        {
            bool updateRes = _dataAccessLayer.UpdateGeneralEmployeeEvaluation(generalEmployeeEvaluation.EmployeeId);
            if(updateRes)
            {
                return true;
            }
            return false;
        }
        //הזנת סטטוס לכל המסגרת
        public bool updateSchoolStatuses(GeneralEmployeeEvaluation generalEmployeeEvaluation)
        {
            bool resSchoolStatus = _dataAccessLayer.UpdateSchoolStatuses(generalEmployeeEvaluation);
            return resSchoolStatus;
        }

        public bool CheckEmployeeExists(string employeeId)
        {
            return _dataAccessLayer.CheckEmployeeExists(employeeId);
        }

        public List<EmployeeEvaluatioStatus> GetEmployeeEvaluationStatuses()
        {
            return _dataAccessLayer.GetEmployeeEvaluationStatuses();
        }
        public List<EmployeeData> GetWeightedEmployeeScore(string superiorId)
        {
            return _dataAccessLayer.GetWeightedEmployeeScore(superiorId);
        }
        public List<OutstandingEmployeesSchool> GetOutstandingEmployees(string supervisorId)
        {
            return _dataAccessLayer.GetOutstandingEmployees(supervisorId);
        }
        public bool UpdateOutstandingEmployee(GeneralEmployeeEvaluation evaluation)
        {
            return _dataAccessLayer.UpdateOutstandingEmployee(evaluation);
        }
    }
}