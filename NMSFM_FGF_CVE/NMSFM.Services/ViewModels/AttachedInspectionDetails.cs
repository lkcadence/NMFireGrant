using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedInspectionDetails
    {
        public Guid InspectionDetailId { get; set; }
        public string RefNum { get; set; }
        public DateTime ViolationDate { get; set; }
        public DateTime? CorrectedDate { get; set; }
        public bool Severe { get; set; }
        public bool RefOnly { get; set; }
        public string ViolationType { get; set; }
        public string Comment { get; set; }

    }
}