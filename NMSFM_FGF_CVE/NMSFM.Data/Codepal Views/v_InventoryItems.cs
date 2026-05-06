namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_InventoryItems
    {
        [Key]
        [Column(Order = 0)]
        public Guid InvItemId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid InvItemTypeId { get; set; }

        [StringLength(50)]
        public string PartNumber { get; set; }

        [StringLength(50)]
        public string InventoryItem { get; set; }

        [StringLength(50)]
        public string MFGPartNumber { get; set; }

        public Guid? ManufacturerId { get; set; }

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

        [Key]
        [Column(Order = 2)]
        public Guid rowguid { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime DateUpdated { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime DateInserted { get; set; }

        [StringLength(50)]
        public string InvItemType { get; set; }

        [StringLength(150)]
        public string Manufacturer { get; set; }

        [StringLength(50)]
        public string AgencyName { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool WebViewable { get; set; }

        [StringLength(100)]
        public string Barcode { get; set; }

        [StringLength(30)]
        public string SalesUOM { get; set; }

        [StringLength(30)]
        public string PurchaseUOM { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string AgencySubName { get; set; }
    }
}
