namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Route
    {
        public Guid RouteId { get; set; }

        public Guid RoutingSlipId { get; set; }

        public Guid InspectionTypeId { get; set; }

        public short Sequence { get; set; }

        public Guid GroupId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
