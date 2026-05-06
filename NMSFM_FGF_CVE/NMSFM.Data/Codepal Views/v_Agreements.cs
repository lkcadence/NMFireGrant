namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Agreements
    {
        [StringLength(100)]
        public string AgreementType { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid AgreementId { get; set; }

        [StringLength(200)]
        public string AgreementDescription { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid AgreementTypeId { get; set; }

        [StringLength(3000)]
        public string AgreementText { get; set; }

        public Guid? ElevateFromAgreementId { get; set; }

        [StringLength(15)]
        public string ElevateFromPeriod { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ProjectId { get; set; }

        [Column(TypeName = "money")]
        public decimal? FeeAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? AgreementTotal { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool AgreementComplete { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }

        public bool? ProjectComplete { get; set; }

        public Guid? ProjectStatusId { get; set; }

        [StringLength(50)]
        public string ProjectStatus { get; set; }
    }
}
