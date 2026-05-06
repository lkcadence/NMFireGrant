namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ItemGridAct
    {
        [Key]
        [Column(Order = 0)]
        public Guid InspectionId { get; set; }

        public Guid? InspectionCauseId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string Address { get; set; }

        public Guid? InspectorId { get; set; }

        public Guid? InspectedPartyId { get; set; }

        public DateTime? InspectionDate { get; set; }

        [StringLength(100)]
        public string ActivityType { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        public decimal? Hrs { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Complete { get; set; }

        public Guid? AlternatePartyId { get; set; }

        public Guid? ParentInspectionId { get; set; }

        public Guid? ItemInspectionStatusId { get; set; }

        [StringLength(50)]
        public string ActivityCategory { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StartDate { get; set; }

        public Guid? SecondaryInspectorId { get; set; }

        public Guid? GroupId { get; set; }

        public Guid? InspectedPartyRoleTypeId { get; set; }

        public Guid? AlternatePartyRoleTypeId { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(220)]
        public string SecAddress { get; set; }

        public Guid? ItemId { get; set; }

        [Column("Suggested Replacement")]
        public Guid? Suggested_Replacement { get; set; }
    }
}
