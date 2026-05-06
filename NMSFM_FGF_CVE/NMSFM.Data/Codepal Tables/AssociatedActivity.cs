namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssociatedActivity
    {
        public Guid AssociatedActivityId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid AssocActivityId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdate { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
