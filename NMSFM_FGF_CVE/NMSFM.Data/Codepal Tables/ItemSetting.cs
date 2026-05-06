namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemSetting
    {
        [Key]
        public Guid ItemTypeId { get; set; }

        [StringLength(50)]
        public string StatusLabel { get; set; }

        [StringLength(50)]
        public string NumberLabel { get; set; }

        [StringLength(50)]
        public string CostLabel { get; set; }

        [StringLength(50)]
        public string DescriptionLabel { get; set; }

        [StringLength(50)]
        public string InServiceLabel { get; set; }

        [StringLength(50)]
        public string CommentLabel { get; set; }

        [StringLength(50)]
        public string TabFiles { get; set; }

        [StringLength(50)]
        public string TabService { get; set; }

        [StringLength(50)]
        public string DetailHides { get; set; }

        [StringLength(50)]
        public string TabHides { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(50)]
        public string BarcodeLabel { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
