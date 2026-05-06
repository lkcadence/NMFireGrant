using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class AttachedMaps
    {
        public Guid AddressId { get; set; }
        public byte[] MapData { get; set; }
        public int Zoom { get; set; }
        public string Style { get; set; }
        public Boolean LatLon { get; set; }
        public Guid rowguid { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }
    }
}