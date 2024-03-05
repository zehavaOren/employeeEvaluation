using Entity;
using Microsoft.AspNetCore.Http;

namespace Service
{
    public interface IFileUploadBL
    {
        public string UploadFiles(IFormFile file);
        public bool ProcessEmployeeExcelFile(Stream fileStream);
    }
}