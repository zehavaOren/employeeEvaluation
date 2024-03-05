using Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Transactions;

namespace DataRepository
{
    public class SchoolDAL : ISchoolDAL
    {
        private readonly string _connectionString;
        public SchoolDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        //קבלת כל המסגרות
        public List<School> GetAllSchools()
        {
            List<School> schools = new List<School>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT [school_code], [school_description] FROM [employee_evaluation].[dbo].[school]";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                School school = new School();
                                school.SchoolCode = Convert.ToInt32(reader["school_code"]);
                                school.SchoolDescription = reader["school_description"].ToString();
                                schools.Add(school);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, e.g., log it or throw it
                Console.WriteLine("Error retrieving schools: " + ex.Message);
            }

            return schools;
        }
        public List<EmployeeData> GetGeneralEmployeeEvaluation(int schoolId)
        {
            List<EmployeeData> employeeDataList = new List<EmployeeData>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT e.employee_id, e.last_name, e.first_name, j.job_description, s.school_description, 
                               e.superior_id, CONCAT(emp.first_name, ' ', emp.last_name) AS superior_name, 
                               ge.evaluation_year, ge.weighted_measure_grade, es.status_description
                        FROM employee e
                        JOIN school s ON e.school_code = s.school_code
                        JOIN job j ON e.job_code = j.job_code
                        LEFT JOIN general_employee_evaluation ge ON e.employee_id = ge.employee_id
                        LEFT JOIN evaluationֹֹ_status es ON ge.evaluation_status_code = es.status_code
                        LEFT JOIN employee emp ON e.superior_id = emp.employee_id
                        WHERE e.school_code = @schoolId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@schoolId", schoolId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeData employeeData = new EmployeeData();
                                employeeData.EmployeeId = reader["employee_id"].ToString();
                                employeeData.LastName = reader["last_name"].ToString();
                                employeeData.FirstName = reader["first_name"].ToString();
                                employeeData.JobName = reader["job_description"].ToString();
                                employeeData.SchoolName = reader["school_description"].ToString();
                                employeeData.SuperiorId = reader["superior_id"].ToString();
                                employeeData.SupervisorName = reader["superior_name"] != DBNull.Value ? reader["superior_name"].ToString() : null;
                                employeeData.CurrentYear = reader["evaluation_year"] != DBNull.Value ? Convert.ToInt32(reader["evaluation_year"]) : (int?)null;
                                employeeData.WeightedMeasureGrade = reader["weighted_measure_grade"] != DBNull.Value ? Convert.ToInt32(reader["weighted_measure_grade"]) : (int?)null;
                                employeeData.StatusDescription = reader["status_description"] != DBNull.Value ? reader["status_description"].ToString() : null;

                                employeeDataList.Add(employeeData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, e.g., log it or throw it
                Console.WriteLine("Error retrieving employee data: " + ex.Message);
            }

            return employeeDataList;
        }
        public List<EmployeeData> GetWeightedEmployeeGrade(int schoolId)
        {
            List<EmployeeData> employeeDataList = new List<EmployeeData>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT e.employee_id, e.last_name, e.first_name, j.job_description, s.school_description, 
                               e.superior_id, CONCAT(emp.first_name, ' ', emp.last_name) AS superior_name, 
                               ge.evaluation_year, ge.weighted_measure_grade
                        FROM employee e
                        JOIN school s ON e.school_code = s.school_code
                        JOIN job j ON e.job_code = j.job_code
                        LEFT JOIN general_employee_evaluation ge ON e.employee_id = ge.employee_id
                        LEFT JOIN employee emp ON e.superior_id = emp.employee_id
                        WHERE e.school_code = @SchoolId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SchoolId", schoolId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeData employeeData = new EmployeeData();
                                employeeData.EmployeeId = reader["employee_id"].ToString();
                                employeeData.LastName = reader["last_name"].ToString();
                                employeeData.FirstName = reader["first_name"].ToString();
                                employeeData.JobName = reader["job_description"].ToString();
                                employeeData.SchoolName = reader["school_description"].ToString();
                                employeeData.SuperiorId = reader["superior_id"].ToString();
                                employeeData.SupervisorName = reader["superior_name"] != DBNull.Value ? reader["superior_name"].ToString() : null;
                                employeeData.CurrentYear = reader["evaluation_year"] != DBNull.Value ? Convert.ToInt32(reader["evaluation_year"]) : (int?)null;
                                employeeData.WeightedMeasureGrade = reader["weighted_measure_grade"] != DBNull.Value ? Convert.ToInt32(reader["weighted_measure_grade"]) : (int?)null;

                                employeeDataList.Add(employeeData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, e.g., log it or throw it
                Console.WriteLine("Error retrieving employee data: " + ex.Message);
            }

            return employeeDataList;
        }
        public List<OutstandingEmployeesSchool> GetOutstandingEmployees(int schoolId)
        {
            List<OutstandingEmployeesSchool> outstandingEmployeesList = new List<OutstandingEmployeesSchool>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT e.employee_id, e.last_name, e.first_name, j.job_description, s.school_description, 
                               CONCAT(emp.first_name, ' ', emp.last_name) AS superior_name, 
                               ge.evaluation_year, ge.weighted_measure_grade, ge.general_evaluation, ge.evaluation_document_1, 
                               ge.evaluation_document_2, ge.evaluation_document_3
                        FROM employee e
                        JOIN school s ON e.school_code = s.school_code
                        JOIN job j ON e.job_code = j.job_code
                        LEFT JOIN general_employee_evaluation ge ON e.employee_id = ge.employee_id
                        LEFT JOIN employee emp ON e.superior_id = emp.employee_id
                        WHERE e.school_code = @SchoolId
                        AND ge.evaluation_status_code IN (4, 5, 6)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SchoolId", schoolId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OutstandingEmployeesSchool outstandingEmployee = new OutstandingEmployeesSchool();
                                outstandingEmployee.EmployeeId = reader["employee_id"].ToString();
                                outstandingEmployee.LastName = reader["last_name"].ToString();
                                outstandingEmployee.FirstName = reader["first_name"].ToString();
                                outstandingEmployee.JobName = reader["job_description"].ToString();
                                outstandingEmployee.SchoolName = reader["school_description"].ToString();
                                outstandingEmployee.SupervisorName = reader["superior_name"] != DBNull.Value ? reader["superior_name"].ToString() : null;
                                outstandingEmployee.CurrentYear = reader["evaluation_year"] != DBNull.Value ? Convert.ToInt32(reader["evaluation_year"]) : (int?)null;
                                outstandingEmployee.WeightedMeasureGrade = Convert.ToInt32(reader["weighted_measure_grade"]);
                                outstandingEmployee.GeneralEvaluation = reader["general_evaluation"].ToString();
                                outstandingEmployee.EvaluationDocument1 = reader["evaluation_document_1"] != DBNull.Value ? reader["evaluation_document_1"].ToString() : null;
                                outstandingEmployee.EvaluationDocument2 = reader["evaluation_document_2"] != DBNull.Value ? reader["evaluation_document_2"].ToString() : null;
                                outstandingEmployee.EvaluationDocument3 = reader["evaluation_document_3"] != DBNull.Value ? reader["evaluation_document_3"].ToString() : null;

                                outstandingEmployeesList.Add(outstandingEmployee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, e.g., log it or throw it
                Console.WriteLine("Error retrieving outstanding employees: " + ex.Message);
            }

            return outstandingEmployeesList;
        }
        public OutstandingEmployeesSchool GetOutstandingEmployee(int schoolId)
        {
            OutstandingEmployeesSchool outstandingEmployee = new OutstandingEmployeesSchool();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                SELECT e.employee_id, e.last_name, e.first_name, j.job_description, s.school_description, 
                       CONCAT(emp.first_name, ' ', emp.last_name) AS supervisor_name, 
                       ge.evaluation_year, ge.weighted_measure_grade, ge.general_evaluation, ge.evaluation_document_1, 
                       ge.evaluation_document_2, ge.evaluation_document_3, ge.outstanding_employee_rating, 
                       ge.unique_initiative, ge.reason_selected_rating, ge.participate_rating_decision
                FROM employee e
                JOIN school s ON e.school_code = s.school_code
                JOIN job j ON e.job_code = j.job_code
                LEFT JOIN general_employee_evaluation ge ON e.employee_id = ge.employee_id
                LEFT JOIN employee emp ON e.superior_id = emp.employee_id
                WHERE e.school_code = @SchoolId
                    AND ge.evaluation_status_code = 5";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SchoolId", schoolId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                outstandingEmployee.EmployeeId = reader["employee_id"].ToString();
                                outstandingEmployee.LastName = reader["last_name"].ToString();
                                outstandingEmployee.FirstName = reader["first_name"].ToString();
                                outstandingEmployee.JobName = reader["job_description"].ToString();
                                outstandingEmployee.SchoolName = reader["school_description"].ToString();
                                outstandingEmployee.SuperiorName = reader["supervisor_name"] != DBNull.Value ? reader["supervisor_name"].ToString() : null;
                                outstandingEmployee.CurrentYear = reader["evaluation_year"] != DBNull.Value ? Convert.ToInt32(reader["evaluation_year"]) : (int?)null;
                                outstandingEmployee.WeightedMeasureGrade = reader["weighted_measure_grade"] != DBNull.Value ? Convert.ToInt32(reader["weighted_measure_grade"]) : 0;
                                outstandingEmployee.GeneralEvaluation = reader["general_evaluation"].ToString();
                                outstandingEmployee.EvaluationDocument1 = reader["evaluation_document_1"] != DBNull.Value ? reader["evaluation_document_1"].ToString() : null;
                                outstandingEmployee.EvaluationDocument2 = reader["evaluation_document_2"] != DBNull.Value ? reader["evaluation_document_2"].ToString() : null;
                                outstandingEmployee.EvaluationDocument3 = reader["evaluation_document_3"] != DBNull.Value ? reader["evaluation_document_3"].ToString() : null;
                                outstandingEmployee.OutstandingEmployeeRating = reader["outstanding_employee_rating"] != DBNull.Value ? Convert.ToInt32(reader["outstanding_employee_rating"]) : (int?)null;
                                outstandingEmployee.UniqueInitiative = reader["unique_initiative"] != DBNull.Value ? reader["unique_initiative"].ToString() : null;
                                outstandingEmployee.ReasonSelectedRating = reader["reason_selected_rating"] != DBNull.Value ? reader["reason_selected_rating"].ToString() : null;
                                outstandingEmployee.ParticipateRatingDecision = reader["participate_rating_decision"] != DBNull.Value ? reader["participate_rating_decision"].ToString() : null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving employees with evaluation status code 5: " + ex.Message);
            }

            return outstandingEmployee;
        }
    }
}
