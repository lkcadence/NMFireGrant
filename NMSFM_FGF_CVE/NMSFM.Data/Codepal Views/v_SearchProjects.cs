namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SearchProjects
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(50)]
        public string ProjectType { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime StartDate { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Complete { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid? PTAgencyId { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? ProjectTypeId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
