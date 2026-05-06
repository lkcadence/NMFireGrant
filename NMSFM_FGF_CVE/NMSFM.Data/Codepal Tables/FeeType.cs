namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FeeType
    {
        public Guid FeeTypeId { get; set; }

        [Column("FeeType")]
        [StringLength(50)]
        public string FeeType1 { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public Guid rowguid { get; set; }

        public bool Rate { get; set; }

        public Guid? DefaultInvoiceTypeId { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool RatedRange { get; set; }

        public bool TotalPercent { get; set; }

        public bool Penalty { get; set; }

        public Guid? ReportId { get; set; }

        [StringLength(100)]
        public string QBInvoiceLineItemListID { get; set; }

        public bool? Contract { get; set; }

        public bool Inactive { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string FeeBarcode { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool WebViewable { get; set; }

        public Guid? InvItemId { get; set; }
    }
}
