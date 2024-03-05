using Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Transactions;

namespace DataRepository
{
    public class MeasureDAL : IMeasureDAL
    {
        private readonly string _connectionString;
        public MeasureDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<MeasureList> GetAllMeasures()
        {
            List<MeasureList> measureList = new List<MeasureList>();
            string query = @"
                        select
                            m.measure_code as measureCode,
                            m.measure_name as measureName,
                            m.measure_weight as MeasureWeight,
                            c.category_description as categoryDescription
                        from measure m
                        left join category_measure c on m.category_code= c.category_code";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        MeasureList measure = new MeasureList
                        {
                            MeasureCode = Convert.ToInt32(reader["measureCode"]),
                            MeasureName = reader["measureName"].ToString(),
                            MeasureWeight= Convert.ToInt32(reader["MeasureWeight"]),
                            categoryDescription = reader["categoryDescription"].ToString()
                        };

                        measureList.Add(measure);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return measureList;
        }
        public List<MeasureGrade> GetAllMeasureGrade()
        {
            List<MeasureGrade> measureGrades = new List<MeasureGrade>();
            string query = @"
                        SELECT 
                            grade_code as gradeCode,
                            grade_description as gradeDescription
                        FROM measure_grade";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        MeasureGrade measureGrade = new MeasureGrade
                        {
                            GradeCode = Convert.ToInt32(reader["gradeCode"]),
                            GradeDescription = reader["gradeDescription"].ToString(),
                        };

                        measureGrades.Add(measureGrade);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return measureGrades;
        }
        public bool InsertOrUpdateEmployeeEvaluationMeasure(EmployeeEvaluationMeasure evaluationData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        string query = @"INSERT INTO [dbo].[employee_evaluation_measure]
                                               ([employee_id]
                                               ,[evaluation_year]
                                               ,[grade_code]
                                               ,[measure_grade_code])
                                         VALUES
                                               (@EmployeeID,
                                                @EvaluationYear,
                                                @GradeCode,
                                                @MeasureGradeCode);";

                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@EmployeeID", evaluationData.EmployeeId);
                            command.Parameters.AddWithValue("@EvaluationYear", evaluationData.EvaluationYear);
                            command.Parameters.AddWithValue("@GradeCode", evaluationData.GradeCode);
                            command.Parameters.AddWithValue("@MeasureGradeCode", evaluationData.MeasureGradeCode);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected <= 0)
                            {
                                // Handle insertion/update failure if necessary
                                transaction.Rollback();
                                return false;
                            }
                        }

                        // If everything went well, commit the transaction
                        transaction.Commit();
                        return true; // Insertion/update successful
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (logging, error reporting, etc.)
                        Console.WriteLine("Error: " + ex.Message);
                        // Rollback the transaction if an exception occurs
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, error reporting, etc.)
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


    }
}
