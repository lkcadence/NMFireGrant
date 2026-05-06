namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InspectionType
    {
        public Guid InspectionTypeId { get; set; }

        [Column("InspectionType")]
        [StringLength(100)]
        public string InspectionType1 { get; set; }

        public Guid? FeeTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? FeeAmt { get; set; }

        public short InspectFreq { get; set; }

        public short ReInspectFreq { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        public Guid rowguid { get; set; }

        public bool IsItem { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [StringLength(2000)]
        public string LegalFooter { get; set; }

        [StringLength(1)]
        public string Recurrance { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool? DefaultPass { get; set; }

        public bool? AlwaysPrintProj { get; set; }

        public Guid? ReFeeTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ReFeeAmt { get; set; }

        public bool? SuppOut { get; set; }

        public bool Inactive { get; set; }

        public bool? PrintLD { get; set; }

        public bool ItemReq { get; set; }

        public Guid? ReportId { get; set; }

        public bool WebViewable { get; set; }

        public Guid? DefReportId { get; set; }

        public Guid? SubDefReportId { get; set; }

        public Guid? CoverLetterReportId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool CauseRequired { get; set; }
    }
}
