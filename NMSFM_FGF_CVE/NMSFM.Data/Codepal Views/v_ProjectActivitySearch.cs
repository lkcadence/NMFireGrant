namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ProjectActivitySearch
    {
        [Key]
        public Guid ProjectId { get; set; }

        public Guid? InspectionId { get; set; }

        public DateTime? InspectionDate { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [StringLength(50)]
        public string ActivityType { get; set; }

        public Guid? InspectionTypeId { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

        public bool? Complete { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        [StringLength(50)]
        public string InspectionCause { get; set; }

        public Guid? ParentInspectionId { get; set; }

        public Guid? AAgencyId { get; set; }

        public Guid? PAgencyId { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StartDate { get; set; }

        [StringLength(50)]
        public string ItemInspectionStatus { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? InspectorId { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }

        public DateTime? ProjectStartDate { get; set; }

        public DateTime? ProjectEndDate { get; set; }

        public bool? ProjectComplete { get; set; }

        [StringLength(50)]
        public string ProjectType { get; set; }

        public bool? IsSub { get; set; }

        public Guid? ProjectStatusId { get; set; }

        [StringLength(50)]
        public string ProjectStatus { get; set; }

        public Guid? ProjectTypeId { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(8000)]
        public string Comment { get; set; }
    }
}
