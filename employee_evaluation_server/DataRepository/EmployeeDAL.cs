using Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataRepository
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private readonly string _connectionString;
        public EmployeeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string query = "SELECT * FROM employee";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            EmployeeId = reader["employee_id"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            FirstName = reader["first_name"].ToString(),
                            JobCode = Convert.ToInt32(reader["job_code"]),
                            SchoolCode = Convert.ToInt32(reader["school_code"]),
                            SuperiorId = reader["superior_id"].ToString(),
                            IsSchoolManager = Convert.ToBoolean(reader["is_school_manager"]),
                            IsSuperior = Convert.ToBoolean(reader["is_superior"]),
                            IsGeneralManager = Convert.ToBoolean(reader["is_general_manager"])
                        };

                        employees.Add(employee);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return employees;
        }

        public Employee GetEmployeeById(string employeeId)
        {
            Employee employee = null;

            string query = "SELECT * FROM employee WHERE employee_id = @EmployeeId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            EmployeeId = (string)reader["employee_id"],
                            LastName = reader["last_name"].ToString(),
                            FirstName = reader["first_name"].ToString(),
                            JobCode = Convert.ToInt32(reader["job_code"]),
                            SchoolCode = Convert.ToInt32(reader["school_code"]),
                            SuperiorId = reader["superior_id"].ToString(),
                            IsSchoolManager = Convert.ToBoolean(reader["is_school_manager"]),
                            IsSuperior = Convert.ToBoolean(reader["is_superior"]),
                            IsGeneralManager = Convert.ToBoolean(reader["is_general_manager"])
                        };
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return employee;
        }
        public List<EmployeeData> GetEmployeesBySupervisorId(string supervisorId)
        {
            List<EmployeeData> employees = new List<EmployeeData>();

            string query = @"
                        SELECT
                            e.employee_id AS EmployeeId,
                            e.last_name AS LastName,
                            e.first_name AS FirstName,
                            j.job_description AS JobName,
                            s.school_description AS SchoolName,
                            e.superior_id AS superiorId,
                            CONCAT(es.first_name, ' ', es.last_name) AS supervisor_name,
                            YEAR(GETDATE()) AS current_year,
                            COALESCE(ge.evaluation_status_code, 1) AS evaluation_status_code
                        FROM
                            employee e
                        LEFT JOIN
                            employee es ON es.employee_id = e.superior_id
                        LEFT JOIN
                            job j ON e.job_code = j.job_code
                        LEFT JOIN
                            school s ON e.school_code = s.school_code
                        LEFT JOIN
                            general_employee_evaluation ge ON ge.employee_id = e.employee_id
                        WHERE
                            e.superior_id = @SupervisorId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SupervisorId", supervisorId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EmployeeData employee = new EmployeeData
                        {
                            EmployeeId = reader["EmployeeId"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            JobName = reader["JobName"].ToString(),
                            SchoolName = reader["SchoolName"].ToString(),
                            SuperiorId = reader["superiorId"].ToString(),
                            SupervisorName = reader["supervisor_name"].ToString(),
                            CurrentYear = Convert.ToInt32(reader["current_year"]),
                            EvaluationStatusCode = Convert.ToInt32(reader["evaluation_status_code"])
                    };

                        employees.Add(employee);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return employees;
        }

        public bool InsertGeneralEmployeeEvaluation(GeneralEmployeeEvaluation generalEmployeeEvaluation)
        {
            string query = @"INSERT INTO general_employee_evaluation 
                             (employee_id, evaluation_year, weighted_measure_grade, 
                              general_evaluation, evaluation_document_1, evaluation_document_2,
                              evaluation_document_3, outstanding_employee_rating, unique_initiative,
                              Reason_selected_rating, participate_rating_decision, evaluation_status_code) 
                             VALUES 
                             (@EmployeeId, @EvaluationYear, @WeightedMeasureGrade, 
                              @GeneralEvaluation, @EvaluationDocument1, @EvaluationDocument2,
                              @EvaluationDocument3, @OutstandingEmployeeRating, @UniqueInitiative,
                              @ReasonSelectedRating, @ParticipateRatingDecision, @EvaluationStatusCode)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                // Add parameters
                command.Parameters.AddWithValue("@EmployeeId", generalEmployeeEvaluation.EmployeeId);
                command.Parameters.AddWithValue("@EvaluationYear", generalEmployeeEvaluation.EvaluationYear);
                command.Parameters.AddWithValue("@WeightedMeasureGrade", generalEmployeeEvaluation.WeightedMeasureGrade);
                command.Parameters.AddWithValue("@GeneralEvaluation", generalEmployeeEvaluation.GeneralEvaluation);

                // Nullable parameters
                AddNullableParameter(command, "@EvaluationDocument1", generalEmployeeEvaluation.EvaluationDocument1);
                AddNullableParameter(command, "@EvaluationDocument2", generalEmployeeEvaluation.EvaluationDocument2);
                AddNullableParameter(command, "@EvaluationDocument3", generalEmployeeEvaluation.EvaluationDocument3);
                AddNullableParameter(command, "@OutstandingEmployeeRating", generalEmployeeEvaluation.OutstandingEmployeeRating);
                AddNullableParameter(command, "@UniqueInitiative", generalEmployeeEvaluation.UniqueInitiative);
                AddNullableParameter(command, "@ReasonSelectedRating", generalEmployeeEvaluation.ReasonSelectedRating);
                AddNullableParameter(command, "@ParticipateRatingDecision", generalEmployeeEvaluation.ParticipateRatingDecision);
                AddNullableParameter(command, "@EvaluationStatusCode", generalEmployeeEvaluation.EvaluationStatusCode);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    // Handle exception
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

        // Helper method to add nullable parameters
        private void AddNullableParameter(SqlCommand command, string parameterName, object value)
        {
            if (value != null)
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }
        //עדכון סטטוס ל-2 או 3
        public int CheckEmployeesInSchool(string employeeId)
        {
            int result = 0; // 0: Unknown, 2: Not all employees found, 3: All employees found

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Query to select all employees with the same school code as the provided employee
                    string query = "SELECT employee_id FROM employee WHERE school_code = (SELECT school_code FROM employee WHERE employee_id = @EmployeeId)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    HashSet<string> employeeSet = new HashSet<string>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["employee_id"].ToString();
                            employeeSet.Add(id);
                        }
                    }

                    // Check if all employees except the provided employee exist in the general_employee_evaluation table
                    foreach (string empId in employeeSet)
                    {
                        if (empId != employeeId)
                        {
                            query = "SELECT COUNT(*) FROM general_employee_evaluation WHERE employee_id = @EmployeeId";
                            command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@EmployeeId", empId);
                            int count = Convert.ToInt32(command.ExecuteScalar());

                            if (count == 0)
                            {
                                result = 2; // Not all employees found
                                break;
                            }
                        }
                    }

                    if (result != 2)
                        result = 3; // All employees found
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return result;
        }
        //עדכון סטטוס למצטיינים
        public bool UpdateGeneralEmployeeEvaluation(string employeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Query to fetch the school code of the provided employee
                    string query = @"SELECT e.school_code
                             FROM employee e
                             WHERE e.employee_id = @EmployeeId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    string schoolCode = command.ExecuteScalar()?.ToString();

                    if (schoolCode != null)
                    {
                        // Query to fetch the top 5 employees with the highest weighted_measure_grade within the same school
                        query = @"UPDATE general_employee_evaluation
                                SET evaluation_status_code = 4
                                WHERE employee_id IN (
                                    SELECT employee_id
                                    FROM (
                                        SELECT TOP 5 WITH TIES g.employee_id, g.weighted_measure_grade
                                        FROM general_employee_evaluation g
                                        INNER JOIN employee e ON g.employee_id = e.employee_id
                                        WHERE e.school_code = @SchoolCode
                                        ORDER BY g.weighted_measure_grade DESC
                                    ) AS TopFive

                          )";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@SchoolCode", schoolCode);
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0; // Return true if rows were affected
                    }
                    else
                    {
                        // No school code found for the employee
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool UpdateSchoolStatuses(GeneralEmployeeEvaluation generalEmployeeEvaluation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Retrieve the school code of the provided employee
                    string querySchoolCode = @"SELECT school_code FROM employee WHERE employee_id = @EmployeeId";
                    SqlCommand commandSchoolCode = new SqlCommand(querySchoolCode, connection);
                    commandSchoolCode.Parameters.AddWithValue("@EmployeeId", generalEmployeeEvaluation.EmployeeId);
                    string schoolCode = commandSchoolCode.ExecuteScalar()?.ToString();

                    if (schoolCode != null)
                    {
                        // Update the evaluation_status_code for employees in the same school
                        string queryUpdate = @"UPDATE general_employee_evaluation
                                       SET evaluation_status_code = 3
                                       WHERE employee_id IN (
                                           SELECT employee_id
                                           FROM employee
                                           WHERE school_code = @SchoolCode
                                       )";

                        SqlCommand commandUpdate = new SqlCommand(queryUpdate, connection);
                        commandUpdate.Parameters.AddWithValue("@SchoolCode", schoolCode);

                        int rowsAffected = commandUpdate.ExecuteNonQuery();

                        return rowsAffected > 0; // Return true if rows were affected
                    }
                    else
                    {
                        // No school code found for the employee
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        //בדיקת ציונים עודכנו
        public bool CheckEmployeeExists(string employeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Query to check if the employee_id exists in the table
                    string query = "SELECT COUNT(*) FROM general_employee_evaluation WHERE employee_id = @EmployeeId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    int count = (int)command.ExecuteScalar();
                    return count > 0; // Returns true if the employee_id exists, false otherwise
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Return false in case of any exception or error
            }
        }
        //קבלת הסטטוסים
        public List<EmployeeEvaluatioStatus> GetEmployeeEvaluationStatuses()
        {
            List<EmployeeEvaluatioStatus> evaluationStatuses = new List<EmployeeEvaluatioStatus>();

            string query = @"SELECT ge.employee_id, ge.evaluation_status_code
                                FROM general_employee_evaluation ge";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EmployeeEvaluatioStatus status = new EmployeeEvaluatioStatus();
                        status.EmployeeId = reader["employee_id"].ToString();
                        status.StatusCode = Convert.ToInt32(reader["evaluation_status_code"]);
                        evaluationStatuses.Add(status);
                    }

                    reader.Close();
                }
            }

            return evaluationStatuses;
        }

        public List<EmployeeData> GetWeightedEmployeeScore(string superiorId)
        {
            List<EmployeeData> employeeDataList = new List<EmployeeData>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL query
                string query = @"
                    SELECT
                        e.employee_id as EmployeeId,
                        e.last_name as LastName,
                        e.first_name as FirstName,
                        j.job_description as JobName,
                        s.school_description as SchoolName,
                        e.superior_id as SuperiorId,
                        CONCAT(es.first_name, ' ', es.last_name) AS SupervisorName,
                        YEAR(GETDATE()) AS CurrentYear,
                        ge.weighted_measure_grade as WeightedMeasureGrade
                    FROM
                        employee e
                    LEFT JOIN
                        general_employee_evaluation ge on e.employee_id=ge.employee_id
                    LEFT JOIN
                        employee es ON es.employee_id = e.superior_id
                    LEFT JOIN
                        job j ON e.job_code = j.job_code
                    LEFT JOIN
                        school s ON e.school_code = s.school_code
                    WHERE
                        e.superior_id = @SuperiorId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SuperiorId", superiorId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    EmployeeData employeeData = new EmployeeData();
                    employeeData.EmployeeId = reader["EmployeeId"].ToString();
                    employeeData.LastName = reader["LastName"].ToString();
                    employeeData.FirstName = reader["FirstName"].ToString();
                    employeeData.JobName = reader["JobName"].ToString();
                    employeeData.SchoolName = reader["SchoolName"].ToString();
                    employeeData.SuperiorId = reader["SuperiorId"].ToString();
                    employeeData.SupervisorName = reader["SupervisorName"] == DBNull.Value ? null : reader["SupervisorName"].ToString();
                    employeeData.CurrentYear = Convert.ToInt32(reader["CurrentYear"]);
                    employeeData.WeightedMeasureGrade = reader["WeightedMeasureGrade"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["WeightedMeasureGrade"]);

                    employeeDataList.Add(employeeData);
                }

                reader.Close();
            }

            return employeeDataList;
        }
        //קבלת העובדים המצטיינים
        public List<OutstandingEmployeesSchool> GetOutstandingEmployees(string supervisorId)
        {
            List<OutstandingEmployeesSchool> outstandingEmployees = new List<OutstandingEmployeesSchool>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT
                    e.employee_id as EmployeeId,
                    e.last_name as LastName,
                    e.first_name as FirstName,
                    j.job_description as JobName,
                    e.school_code as SchoolCode,
                    s.school_description as SchoolName,
                    e.superior_id as SuperiorId,
                    CONCAT(es.first_name, ' ', es.last_name) AS SupervisorName,
                    YEAR(GETDATE()) AS CurrentYear,
                    g.weighted_measure_grade as WeightedMeasureGrade,
                    g.general_evaluation as GeneralEvaluation,
                    g.evaluation_document_1 as EvaluationDocument1,
                    g.evaluation_document_2 as EvaluationDocument2,
                    g.evaluation_document_3 as EvaluationDocument3,
                    g.evaluation_status_code as EvaluationStatusCode
                FROM
                    employee e
                LEFT JOIN
                    employee es ON es.employee_id = e.superior_id
                LEFT JOIN
                    job j ON e.job_code = j.job_code
                LEFT JOIN
                    school s ON e.school_code = s.school_code
                INNER JOIN
                    general_employee_evaluation g ON g.employee_id = e.employee_id
                WHERE
                    e.school_code = (SELECT school_code FROM employee WHERE employee_id = @SupervisorId)
                    AND g.evaluation_status_code IN (4, 5, 6)
                ORDER BY g.weighted_measure_grade DESC";


                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SupervisorId", supervisorId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OutstandingEmployeesSchool employee = new OutstandingEmployeesSchool();
                            employee.EmployeeId = reader["EmployeeId"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.JobName = reader["JobName"].ToString();
                            employee.SchoolCode = Convert.ToInt32(reader["SchoolCode"]);
                            employee.SchoolName = reader["SchoolName"].ToString();
                            employee.SuperiorId = reader["SuperiorId"].ToString();
                            employee.SupervisorName = reader["SupervisorName"].ToString();
                            employee.CurrentYear = Convert.ToInt32(reader["CurrentYear"]);
                            employee.WeightedMeasureGrade = Convert.ToInt32(reader["WeightedMeasureGrade"]);
                            employee.GeneralEvaluation = reader["GeneralEvaluation"].ToString();
                            employee.EvaluationDocument1 = reader["EvaluationDocument1"].ToString();
                            employee.EvaluationDocument2 = reader["EvaluationDocument2"].ToString();
                            employee.EvaluationDocument3 = reader["EvaluationDocument3"].ToString();
                            employee.EvaluationStatusCode= Convert.ToInt32(reader["EvaluationStatusCode"]);

                            outstandingEmployees.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return outstandingEmployees;
        }
        //עדכון ציון לעובדים המצטיינים
        public bool UpdateOutstandingEmployee(GeneralEmployeeEvaluation evaluation)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = @"UPDATE [dbo].[general_employee_evaluation] 
                                            SET [outstanding_employee_rating] = @Rating,
                                                [unique_initiative] = @Initiative,
                                                [Reason_selected_rating] = @Reason,
                                                [participate_rating_decision] = @Participate,
                                                [evaluation_status_code] = 
                                                    CASE 
                                                        WHEN @Rating = 1 THEN 5 
                                                        ELSE 6 
                                                    END
                                            WHERE [employee_id] = @EmployeeId";

                    command.Parameters.AddWithValue("@Rating", evaluation.OutstandingEmployeeRating);
                    command.Parameters.AddWithValue("@Initiative", evaluation.UniqueInitiative);
                    command.Parameters.AddWithValue("@Reason", evaluation.ReasonSelectedRating);
                    command.Parameters.AddWithValue("@Participate", evaluation.ParticipateRatingDecision);
                    command.Parameters.AddWithValue("@EmployeeId", evaluation.EmployeeId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        transaction.Commit();
                        success = true;
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating employee evaluation: " + ex.Message);
                    transaction.Rollback();
                }
            }

            return success;
        }

    }
}

