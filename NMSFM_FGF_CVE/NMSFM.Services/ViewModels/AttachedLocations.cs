using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedLocations
    {
        public Guid LocationId { get; set; }
        public string Description { get; set; }
        public string LocationType { get; set; }
        public int ItemCount { get; set; }
        public string LocationBase { get; set; }
    }
}