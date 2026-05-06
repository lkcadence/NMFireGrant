using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedImages
    {
        private string fileType;

        public Guid FileId { get; set; }
        public string FileDesc { get; set; }
        public int? SeqNum { get; set; }
        public string FileName { get; set; }

        public string FileType
        {
            get
            {
                if (String.IsNullOrWhiteSpace(fileType))
                {
                    fileType = GetFileType(FileName);
                }
                return fileType;
            }
        }

        public static string GetFileType(string fileName)
        {
            var imageType = String.Empty;
            if (!String.IsNullOrWhiteSpace(fileName))
            {
                var info = new FileInfo(fileName);
                var ext = info.Extension.Replace(".", "");
                if (!String.IsNullOrWhiteSpace(ext))
                {
                    if (ext.Equals("bmp", StringComparison.InvariantCultureIgnoreCase) ||
                        ext.Equals("gif", StringComparison.InvariantCultureIgnoreCase)  ||
                        ext.Equals("png", StringComparison.InvariantCultureIgnoreCase) ||
                        ext.Equals("tiff", StringComparison.InvariantCultureIgnoreCase) ||
                        ext.Equals("tif", StringComparison.InvariantCultureIgnoreCase))
                    {
                        imageType = "image/" + ext.ToLower();
                    }
                    else if (ext.Equals("jpg", StringComparison.InvariantCultureIgnoreCase) ||
                             ext.Equals("jpeg", StringComparison.InvariantCultureIgnoreCase))
                    {
                        imageType = "image/jpeg";
                    }
                    else if (ext.Equals("pdf", StringComparison.InvariantCultureIgnoreCase))
                    {
                        imageType = "application/pdf";
                    }
                    else if (ext.Equals("doc", StringComparison.InvariantCultureIgnoreCase) || 
                             ext.Equals("docx", StringComparison.InvariantCultureIgnoreCase))
                    {                        
                        imageType = "application/msword";
                    }
                    else if (ext.Equals("xls", StringComparison.InvariantCultureIgnoreCase) || 
                             ext.Equals("xlsx", StringComparison.InvariantCultureIgnoreCase))
                    {
                        imageType = "application/vnd.ms-excel";
                    }                                        
                }
            }

            return imageType;
        }


    }
}