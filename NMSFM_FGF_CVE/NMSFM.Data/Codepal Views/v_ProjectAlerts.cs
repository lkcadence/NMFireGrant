namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ProjectAlerts
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ProjectTypeId { get; set; }

        [StringLength(50)]
        public string ProjectType { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string ProjectStatus { get; set; }

        public short? ProjectFreq { get; set; }

        [StringLength(1)]
        public string Recurrance { get; set; }

        public Guid? AgencyId { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime NextProjectDate { get; set; }

        public Guid? AddressId { get; set; }
    }
}
