using NMSFM.Services.Models;
using System;
using System.Collections.Generic;

namespace NMSFM.ViewModels
{
    public class DetailedFGLegacyApps
    {
        public Guid AddressId { get; set; }
        public string FiscalYear { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
    }
}
