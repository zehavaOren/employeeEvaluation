using Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;

namespace DataRepository
{
    public class FileUploadDAL : IFileUploadDAL
    {
        private readonly string _connectionString;
        public FileUploadDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public string SaveFileNamesToDatabase(string fileNames)
        {
            try
            {
                // Add your database logic here to save file names
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
            return "";
        }
        public bool ProcessEmployeeExcelFile(Stream fileStream)
        {
            bool success = false;
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet

                    int rowCount = worksheet.Dimension.Rows;
                    int columnCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                    {
                        Employee employee = new Employee
                        {
                            EmployeeId = worksheet.Cells[row, 1].GetValue<string>(),
                            LastName = worksheet.Cells[row, 2].GetValue<string>(),
                            FirstName = worksheet.Cells[row, 3].GetValue<string>(),
                            JobCode = worksheet.Cells[row, 4].GetValue<int>(),
                            SchoolCode = worksheet.Cells[row, 5].GetValue<int>(),
                            SuperiorId = worksheet.Cells[row, 6].GetValue<string>(),
                            IsSchoolManager = ParseYesNo(worksheet.Cells[row, 7].GetValue<string>()),
                            IsSuperior = ParseYesNo(worksheet.Cells[row, 8].GetValue<string>()),
                            IsGeneralManager = ParseYesNo(worksheet.Cells[row, 9].GetValue<string>())

                        };
                        success = true;

                        if (EmployeeExists(employee.EmployeeId))
                        {
                            // Update employee data in the database
                            UpdateEmployee(employee);
                        }
                        else
                        {
                            // Insert new employee record into the database
                            InsertEmployee(employee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error processing Excel file: " + ex.Message);
                success = false; // Set success to false if an error occurs
            }

            return success;
        }
        private bool ParseYesNo(string value)
        {
            return !string.IsNullOrEmpty(value) && value.Trim().Equals("כן", StringComparison.OrdinalIgnoreCase);
        }
        private bool EmployeeExists(string employeeId)
        {
            // Implement logic to check if employee exists in the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Employee WHERE employee_id = @EmployeeId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private void UpdateEmployee(Employee employee)
        {
            // Implement logic to update employee data in the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Employee SET last_name = @LastName, first_name = @FirstName, job_code = @JobCode, school_code = @SchoolCode, " +
                               "superior_id = @SuperiorId, is_school_manager = @IsSchoolManager, is_superior = @IsSuperior, is_general_manager = @IsGeneralManager " +
                               "WHERE employee_id = @EmployeeId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@JobCode", employee.JobCode);
                    command.Parameters.AddWithValue("@SchoolCode", employee.SchoolCode);
                    command.Parameters.AddWithValue("@SuperiorId", employee.SuperiorId);
                    command.Parameters.AddWithValue("@IsSchoolManager", employee.IsGeneralManager);
                    command.Parameters.AddWithValue("@IsSuperior", employee.IsSuperior);
                    command.Parameters.AddWithValue("@IsGeneralManager", employee.IsGeneralManager);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        private void InsertEmployee(Employee employee)
        {
            // Implement logic to insert new employee record into the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Employee (employee_id, last_name, first_name, job_code, school_code, superior_id, is_school_manager, is_superior, is_general_manager) " +
                               "VALUES (@EmployeeId, @LastName, @FirstName, @JobCode, @SchoolCode, @SuperiorId, @IsSchoolManager, @IsSuperior, @IsGeneralManager)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@JobCode", employee.JobCode);
                    command.Parameters.AddWithValue("@SchoolCode", employee.SchoolCode);
                    command.Parameters.AddWithValue("@SuperiorId", employee.SuperiorId);
                    command.Parameters.AddWithValue("@IsSchoolManager", employee.IsGeneralManager);
                    command.Parameters.AddWithValue("@IsSuperior", employee.IsSuperior);
                    command.Parameters.AddWithValue("@IsGeneralManager", employee.IsGeneralManager);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
