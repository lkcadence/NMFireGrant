namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_FailedSubCheckItems
    {
        [Key]
        public Guid InspectionId { get; set; }

        public short? SeqNum { get; set; }

        [StringLength(1000)]
        public string CheckItem { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Corrected { get; set; }

        [StringLength(200)]
        public string ResolutionText { get; set; }

        public byte? BooleanValue { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        [StringLength(100)]
        public string CodeVersion { get; set; }
    }
}
