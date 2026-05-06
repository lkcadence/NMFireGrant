namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PermitSetting
    {
        [Key]
        public Guid PermitTypeId { get; set; }

        [StringLength(50)]
        public string IssuedToLabel { get; set; }

        [StringLength(50)]
        public string DurationLabel { get; set; }

        [StringLength(50)]
        public string CommentLabel { get; set; }

        [StringLength(50)]
        public string TabFiles { get; set; }

        [StringLength(50)]
        public string TabSig { get; set; }

        [StringLength(50)]
        public string DetailHides { get; set; }

        [StringLength(50)]
        public string TabHides { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(50)]
        public string StatusLabel { get; set; }

        [StringLength(50)]
        public string EmergContLabel { get; set; }

        [StringLength(50)]
        public string CompleteLabel { get; set; }

        [StringLength(50)]
        public string SubmittalLabel { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
