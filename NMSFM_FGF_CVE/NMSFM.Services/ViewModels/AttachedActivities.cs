using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedActivities
    {
        public Guid InspectionId { get; set; }
        public string InspectionCause { get; set; }
        public string InspectionType { get; set; }
        public string InspectionNumber { get; set; }
        public string InspectorName { get; set; }
        public string PartyName { get; set; }
        public bool Inactive { get; set; }
        public string ActivityType { get; set; }
        public DateTime InspectionDate { get; set; }
        public string Comment { get; set; }
        public Guid? AddressId { get; set; }
        public string FullAddress { get; set; }
        public bool Complete { get; set; }
        public string ItemInspectionStatus { get; set; }
    }
}