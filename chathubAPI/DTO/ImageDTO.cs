using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.DTO
{
    public class UploadImageDTO
    {
        public string UploaderEmail { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string DataBase64 { get; set; }
    }
}
