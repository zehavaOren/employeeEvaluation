using DataRepository;
using Entity;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;

namespace Service
{
    public class FileUploadBL: IFileUploadBL
    {
        private readonly IFileUploadDAL _fileUpload;

        public FileUploadBL(IFileUploadDAL fileUpload)
        {
            _fileUpload = fileUpload;
        }

        public string UploadFiles(IFormFile file)
        {
            var fullPath="";
            try
            {
                // Save files to the server
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine("C:\\Users\\estyn\\Desktop\\employee_evaluation\\employee_evaluation_server", fileName);
                        fullPath = filePath;

                        // Handle file name collision if needed
                        // Add your collision handling logic here

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    // Save file names to the database or perform any other necessary operations
                    //_fileUpload.SaveFileNamesToDatabase(filePath);
                }

                return fullPath;
            }
            catch (Exception ex)
            {
                // Log the exception
                return ("An error occurred while uploading files");
            }
        }
        public bool ProcessEmployeeExcelFile(Stream fileStream)
        {
            try
            {
                // Use EPPlus or any other library to process the Excel file
                // Perform necessary business logic operations

                // Call the corresponding method in the DAL layer
                return _fileUpload.ProcessEmployeeExcelFile(fileStream);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return false
                return false;
            }
        }
    }
}