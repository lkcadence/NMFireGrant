using System;

namespace NMSFM.ViewModels
{
    public class OccupancyTypeViewModel
    {
        public Guid OccupancyTypeId { get; set; }

        public string OccupancyType { get; set; }

        public string OccupancyTypeCode { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}