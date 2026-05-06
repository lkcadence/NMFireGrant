namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Agreement
    {
        public Guid AgreementId { get; set; }

        [StringLength(200)]
        public string AgreementDescription { get; set; }

        public Guid AgreementTypeId { get; set; }

        [StringLength(3000)]
        public string AgreementText { get; set; }

        public Guid? ElevateFromAgreementId { get; set; }

        [StringLength(15)]
        public string ElevateFromPeriod { get; set; }

        public Guid ProjectId { get; set; }

        [Column(TypeName = "money")]
        public decimal? FeeAmount { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid rowguid { get; set; }

        [Column(TypeName = "money")]
        public decimal? AgreementTotal { get; set; }

        public bool AgreementComplete { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
