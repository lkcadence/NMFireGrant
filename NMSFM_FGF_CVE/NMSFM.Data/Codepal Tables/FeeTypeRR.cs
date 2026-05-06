namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeeTypeRR")]
    public partial class FeeTypeRR
    {
        public Guid FeeTypeRRId { get; set; }

        public Guid FeeTypeId { get; set; }

        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        public decimal AmountFrom { get; set; }

        public decimal? AmountTo { get; set; }

        public decimal Base { get; set; }

        public decimal? AmountEvery { get; set; }

        public decimal? AmountPer { get; set; }

        public decimal? RatePer { get; set; }

        public Guid? UserDefFieldId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string QBInvoiceLineItemListID { get; set; }

        public short RoundOption { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
