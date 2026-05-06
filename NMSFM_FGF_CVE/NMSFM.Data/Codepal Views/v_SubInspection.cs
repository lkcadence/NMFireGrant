namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SubInspection
    {
        [StringLength(75)]
        public string ItemType { get; set; }

        [StringLength(100)]
        public string ItemDescription { get; set; }

        [StringLength(200)]
        public string LocationDescription { get; set; }

        [StringLength(50)]
        public string ItemInspectionStatus { get; set; }

        public Guid? ParentInspectionId { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid InspectionId { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [StringLength(50)]
        public string ActivityType { get; set; }

        public DateTime? InspectionDate { get; set; }

        [StringLength(100)]
        public string ItemNumber { get; set; }

        [StringLength(100)]
        public string ServiceType { get; set; }

        public DateTime? NextServiceDate { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

        public Guid? ItemId { get; set; }

        public Guid? SecAddressId { get; set; }

        public Guid? InspectionTypeId { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Complete { get; set; }

        [StringLength(3000)]
        public string UserDefValue { get; set; }

        public Guid? UserDefFieldId { get; set; }
    }
}
