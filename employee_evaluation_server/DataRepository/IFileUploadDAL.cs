
namespace DataRepository
{
    public interface IFileUploadDAL
    {
        public string SaveFileNamesToDatabase(string fileNames);
        public bool ProcessEmployeeExcelFile(Stream fileStream);
    }
}