using System;
using System.Collections.Generic;
using System.Text;

namespace YoulaApi.Models
{
    public class FilesUpload
    {
        public byte[] ImageData { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }
    }
}
