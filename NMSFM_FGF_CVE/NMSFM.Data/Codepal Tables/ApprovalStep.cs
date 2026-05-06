namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ApprovalStep
    {
        public Guid ApprovalStepId { get; set; }

        [Column("ApprovalStep")]
        [Required]
        [StringLength(50)]
        public string ApprovalStep1 { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
