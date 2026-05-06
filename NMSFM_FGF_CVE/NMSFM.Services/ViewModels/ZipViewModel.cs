using System;

namespace NMSFM.ViewModels
{
    public class ZipViewModel
    {
        public Guid ZipId { get; set; }

        public string Zip { get; set; }

        public Guid? CountyId { get; set; }
    }
}