namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_InvNarratives
    {
        [Key]
        public Guid InvNarrativeId { get; set; }

        [StringLength(100)]
        public string InvNarrativeName { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(8000)]
        public string InvNarrativeText { get; set; }
    }
}
