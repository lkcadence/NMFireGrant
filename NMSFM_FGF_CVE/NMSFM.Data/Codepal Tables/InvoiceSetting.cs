namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoiceSetting
    {
        [Key]
        public Guid InvoiceTypeId { get; set; }

        [StringLength(50)]
        public string InvNumLabel { get; set; }

        [StringLength(50)]
        public string InvTypeLabel { get; set; }

        [StringLength(50)]
        public string InvDateLabel { get; set; }

        [StringLength(50)]
        public string BillToPartyLabel { get; set; }

        [StringLength(50)]
        public string BillToAddLabel { get; set; }

        [StringLength(50)]
        public string TermsLabel { get; set; }

        [StringLength(50)]
        public string InvAmtLabel { get; set; }

        [StringLength(50)]
        public string BalDueLabel { get; set; }

        [StringLength(50)]
        public string TabLegDesc { get; set; }

        [StringLength(50)]
        public string DetailHides { get; set; }

        [StringLength(50)]
        public string TabHides { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(50)]
        public string ServiceAddressLabel { get; set; }

        [StringLength(50)]
        public string DueDateLabel { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
