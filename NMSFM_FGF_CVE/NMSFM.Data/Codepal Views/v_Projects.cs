namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Projects
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ProjectTypeId { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Complete { get; set; }

        [Key]
        [Column(Order = 4)]
        public Guid rowguid { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        public Guid? ProjectStatusId { get; set; }

        public Guid? ReportId { get; set; }

        [StringLength(50)]
        public string ProjectType { get; set; }

        [StringLength(50)]
        public string ProjectStatus { get; set; }
    }
}
