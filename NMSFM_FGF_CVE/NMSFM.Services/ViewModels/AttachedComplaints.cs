using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedComplaints
    {
        public Guid ComplaintId { get; set; }
        public string ComplaintType { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string ComplaintStatus { get; set; }
        public string PartyName { get; set; }
        public string Comment { get; set; }
    }
}