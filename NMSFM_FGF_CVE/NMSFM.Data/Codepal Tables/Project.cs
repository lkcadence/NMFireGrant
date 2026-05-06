namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        public Guid ProjectId { get; set; }

        public Guid ProjectTypeId { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool Complete { get; set; }

        public Guid rowguid { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? ProjectStatusId { get; set; }

        public Guid? ReportId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ContractTotal { get; set; }

        public bool ContractComplete { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool StopAlerts { get; set; }
    }
}
