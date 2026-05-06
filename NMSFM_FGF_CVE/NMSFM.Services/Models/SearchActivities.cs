using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;


namespace NMSFM.Services.Models
{
    class SearchActivities
    {
        public Guid InspectionId { get; set; }
        public Guid ActivityTypeId { get; set; }
        public Guid AddressId { get; set; }
        public bool Inactive { get; set; }
        public string AddressType { get; set; }
        public Guid AddressTypeId { get; set; }
        public string AddressCode { get; set; }
        public string AddressNumber { get; set; }
        public string Direction { get; set; }
        public string Address { get; set; }
        public string SubAddress { get; set; }
        public string City { get; set; }
        public string Suffix { get; set; }
        public string StateAbbr { get; set; }
        public string Zip { get; set; }
        public string Comment { get; set; }
        public string Party { get; set; }
        public string Region { get; set; }
        public string County { get; set; }
        public string Occupancy { get; set; }
        public string Property { get; set; }
        public string BarCode { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectorName { get; set; }
        public string InspectionNumber { get; set; }
        public string ItemInspectionStatus { get; set; }
        public bool? ActInactive { get; set; }
        public bool? TypeInactive { get; set; }
        public string GroupName { get; set; }
        public Guid? ParentInspectionId { get; set; }
        public Guid? ACAgencyId { get; set; }
        public Guid? InspAgencyId { get; set; }
    }
}
