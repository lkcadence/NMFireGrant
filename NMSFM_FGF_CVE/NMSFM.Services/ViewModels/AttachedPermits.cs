using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedPermits
    {
        public Guid PermitId { get; set; }
        public string PermitType { get; set; }
        public string PermitNumber { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public Guid? AddressId { get; set; }
        public string PartyName { get; set; }
    }
}