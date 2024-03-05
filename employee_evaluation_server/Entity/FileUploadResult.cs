using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public partial class FileUploadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public FileUploadResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
