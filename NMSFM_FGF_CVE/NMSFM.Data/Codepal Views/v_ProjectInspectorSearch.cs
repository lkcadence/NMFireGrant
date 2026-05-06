namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ProjectInspectorSearch
    {
        [Key]
        public Guid ProjectId { get; set; }

        public Guid? InspectorId { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(50)]
        public string InspectorPhone { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
