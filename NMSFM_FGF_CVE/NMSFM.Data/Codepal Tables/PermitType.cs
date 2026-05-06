namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PermitType
    {
        public Guid PermitTypeId { get; set; }

        [Column("PermitType")]
        [StringLength(100)]
        public string PermitType1 { get; set; }

        public Guid? FeeTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? FeeAmt { get; set; }

        [StringLength(25)]
        public string Code { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        public Guid rowguid { get; set; }

        public short PermitFreq { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool? NoAlert { get; set; }

        public bool? PrintLD { get; set; }

        public Guid? ReportId { get; set; }

        public bool WebViewable { get; set; }

        public bool IsSub { get; set; }

        public Guid? CertReportId { get; set; }

        public Guid? LandCertReportId { get; set; }

        public Guid? ALReportId { get; set; }

        public bool EditDefFreq { get; set; }

        public Guid? DefaultStatusId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool NoEndDate { get; set; }
    }
}
