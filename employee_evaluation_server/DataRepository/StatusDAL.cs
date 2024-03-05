using Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Transactions;

namespace DataRepository
{
    public class StatusDAL: IStatusDAL
    {
        private readonly string _connectionString;
        public StatusDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        //קבלת רשימת הסטטוסים
        public List<EvaluationStatus> GetAllStatuses()
        {
            List<EvaluationStatus> evaluationStatuses = new List<EvaluationStatus>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT status_code, status_description FROM evaluationֹֹ_status";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EvaluationStatus status = new EvaluationStatus
                        {
                            StatusCode = Convert.ToInt32(reader["status_code"]),
                            StatusDescription = reader["status_description"].ToString()
                        };
                        evaluationStatuses.Add(status);
                    }

                    reader.Close();
                }
            }

            return evaluationStatuses;
        }
        //קבלת הסטטוסים של העובדים המצטיינים למסגרת שמתקבלת
        public Dictionary<string, int> GetOutstandingEmployeeStatusCodes(int schoolCode)
        {
            Dictionary<string, int> employeeStatusCodes = new Dictionary<string, int>();

            // Define the SQL query
            string query = @"SELECT ge.employee_id, ge.evaluation_status_code
                             FROM general_employee_evaluation ge
                             WHERE ge.evaluation_status_code IN (4, 5, 6) 
                             AND ge.employee_id IN 
                                 (SELECT e.employee_id
                                  FROM employee e
                                  WHERE e.school_code = @schoolCode)";

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@schoolCode", schoolCode);

                    try
                    {
                        // Open the connection
                        connection.Open();

                        // Execute the command and retrieve data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                // Iterate through the rows
                                while (reader.Read())
                                {
                                    // Retrieve employee_id and evaluation_status_code
                                    string employeeId = reader.GetString(0);
                                    int evaluationStatusCode = reader.GetInt32(1);

                                    // Add to the dictionary
                                    employeeStatusCodes.Add(employeeId, evaluationStatusCode);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error retrieving data: " + ex.Message);
                    }
                }
            }

            // Return the dictionary of employee status codes
            return employeeStatusCodes;
        }
        //קבלת רשימת הדירוגים שנבחור לעובדים המצטיינים
        public List<int?> GetUpdatedGradesForOutstandingEmployee(int schoolCode)
        {
            List<int?> updatedGrades = new List<int?>();

            // Define the SQL query
            string query = @"select ge.outstanding_employee_rating
                            from general_employee_evaluation ge
                            left join employee e
                            on e.employee_id=ge.employee_id
                            where e.school_code =@schoolCode
                            AND ge.evaluation_status_code IN (4, 5, 6)";

            // Create and configure the SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@schoolCode", schoolCode);

                    try
                    {
                        // Open the connection
                        connection.Open();

                        // Execute the command and retrieve data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                // Iterate through the rows
                                while (reader.Read())
                                {
                                    // Retrieve the grade and add it to the list
                                    if (!reader.IsDBNull(0))
                                    {
                                        updatedGrades.Add(reader.GetInt32(0));
                                    }
                                    else
                                    {
                                        updatedGrades.Add(null); // Add NULL for NULL values
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error retrieving updated grades: " + ex.Message);
                    }
                }
            }

            // Return the list of updated grades
            return updatedGrades;
        }
    }
}
