namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SubInspectionList
    {
        [Key]
        public Guid InspectionId { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        public Guid? ParentInspectionId { get; set; }

        [StringLength(75)]
        public string ItemType { get; set; }

        [StringLength(100)]
        public string ItemNumber { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Barcode { get; set; }

        public Guid? ItemId { get; set; }

        [StringLength(20)]
        public string LocationBarcode { get; set; }

        [StringLength(50)]
        public string LocationBase { get; set; }

        [StringLength(200)]
        public string LocationDescription { get; set; }

        [StringLength(323)]
        public string Location { get; set; }
    }
}
