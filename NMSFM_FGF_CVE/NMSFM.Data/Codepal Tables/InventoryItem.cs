namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InventoryItem
    {
        [Key]
        public Guid InvItemId { get; set; }

        public Guid InvItemTypeId { get; set; }

        [StringLength(50)]
        public string PartNumber { get; set; }

        [Column("InventoryItem")]
        [StringLength(150)]
        public string InventoryItem1 { get; set; }

        public Guid? ManufacturerId { get; set; }

        [StringLength(50)]
        public string MFGPartNumber { get; set; }

        [StringLength(50)]
        public string ModelNumber { get; set; }

        [StringLength(20)]
        public string BinLocation { get; set; }

        [Column(TypeName = "money")]
        public decimal? StandardCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? AverageCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? ExchangeCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? UsedCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel1 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel2 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel3 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel4 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel5 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel6 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel7 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel8 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel9 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceLevel10 { get; set; }

        public double? QtyOnHand { get; set; }

        public double? QtyMinStock { get; set; }

        public bool? Inactive { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        [StringLength(100)]
        public string Barcode { get; set; }

        public bool WebViewable { get; set; }

        [StringLength(30)]
        public string SalesUOM { get; set; }

        [StringLength(30)]
        public string PurchaseUOM { get; set; }
    }
}
