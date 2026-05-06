using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class PropertyUseTypeViewModel
    {
        public Guid PropertyUseTypeId { get; set; }

        public string PropertyUseType { get; set; }

        public string PropertyUseTypeCode { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}