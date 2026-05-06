namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LocationSetting
    {
        [Key]
        public Guid LocationTypeId { get; set; }

        [StringLength(50)]
        public string DescLabel { get; set; }

        [StringLength(50)]
        public string LocBaseLabel { get; set; }

        [StringLength(50)]
        public string LocTypeLabel { get; set; }

        [StringLength(50)]
        public string BarcodeLabel { get; set; }

        [StringLength(50)]
        public string CommentLabel { get; set; }

        [StringLength(50)]
        public string LatLabel { get; set; }

        [StringLength(50)]
        public string LonLabel { get; set; }

        [StringLength(50)]
        public string DetailHides { get; set; }

        [StringLength(50)]
        public string TabHides { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
