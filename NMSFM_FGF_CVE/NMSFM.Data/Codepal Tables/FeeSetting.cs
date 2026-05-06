namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FeeSetting
    {
        [Key]
        public Guid FeeTypeId { get; set; }

        [StringLength(50)]
        public string FeeTypeLabel { get; set; }

        [StringLength(50)]
        public string FeeDateLabel { get; set; }

        [StringLength(50)]
        public string FeeBaseLabel { get; set; }

        [StringLength(50)]
        public string PerLabel { get; set; }

        [StringLength(50)]
        public string CountLabel { get; set; }

        [StringLength(50)]
        public string FeeAmtLabel { get; set; }

        [StringLength(50)]
        public string PayDateLabel { get; set; }

        [StringLength(50)]
        public string PayAmtLabel { get; set; }

        [StringLength(50)]
        public string RefLabel { get; set; }

        [StringLength(50)]
        public string RespPartyLabel { get; set; }

        [StringLength(50)]
        public string CommentLabel { get; set; }

        [StringLength(50)]
        public string DetailHides { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
