namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivityInventoryItem
    {
        public Guid ActivityInventoryItemId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid InvItemId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public double Quantity { get; set; }
    }
}
