using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedLocationBases
    {
        public Guid LocationBaseId { get; set; }
        public string LocationBase { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }
    }
}