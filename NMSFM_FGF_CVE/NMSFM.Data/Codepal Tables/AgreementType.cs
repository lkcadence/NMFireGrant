namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AgreementType
    {
        public Guid AgreementTypeId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [Column("AgreementType")]
        [Required]
        [StringLength(100)]
        public string AgreementType1 { get; set; }

        public Guid ActivityTypeId { get; set; }

        public Guid? FeeTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? DefaultFeeAmount { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(3000)]
        public string DefaultText { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
