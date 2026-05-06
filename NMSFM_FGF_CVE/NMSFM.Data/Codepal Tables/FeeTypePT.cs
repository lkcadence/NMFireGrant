namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeeTypePT")]
    public partial class FeeTypePT
    {
        public Guid FeeTypePTId { get; set; }

        public Guid BaseFeeTypeId { get; set; }

        public Guid? FeeTypeId { get; set; }

        [Required]
        [StringLength(20)]
        public string Percentage { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
