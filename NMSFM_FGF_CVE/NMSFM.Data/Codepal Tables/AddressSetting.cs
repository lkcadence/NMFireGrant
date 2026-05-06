namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AddressSetting
    {
        [Key]
        public Guid AddressTypeId { get; set; }

        [StringLength(50)]
        public string AddCodeLabel { get; set; }

        [StringLength(50)]
        public string SubAddLabel { get; set; }

        [StringLength(50)]
        public string RegionLabel { get; set; }

        [StringLength(50)]
        public string CountyLabel { get; set; }

        [StringLength(50)]
        public string OccTypeLabel { get; set; }

        [StringLength(50)]
        public string PropUseLabel { get; set; }

        [StringLength(50)]
        public string CommentLabel { get; set; }

        [StringLength(50)]
        public string DetailHides { get; set; }

        [StringLength(50)]
        public string TabHides { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(50)]
        public string TabLocBases { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        [StringLength(50)]
        public string MapLabel { get; set; }

        [StringLength(50)]
        public string BlockLabel { get; set; }

        [StringLength(50)]
        public string LotLabel { get; set; }

        [StringLength(50)]
        public string TaxParcelLabel { get; set; }
    }
}
