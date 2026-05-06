namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ActivityTypeCauses")]
    public partial class ActivityTypeCaus
    {
        [Key]
        public Guid ActivityTypeCauseId { get; set; }

        public Guid ActivityTypeId { get; set; }

        public Guid InspectionCauseId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdate { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
