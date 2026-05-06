namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoutingSlip
    {
        public Guid RoutingSlipId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoutingSlipName { get; set; }

        public bool Progressive { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
