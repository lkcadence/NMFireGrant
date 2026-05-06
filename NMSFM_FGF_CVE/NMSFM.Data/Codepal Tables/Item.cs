namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        public Guid ItemId { get; set; }

        public Guid ItemTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Barcode { get; set; }

        public Guid? StatusId { get; set; }

        public Guid? AddressId { get; set; }

        public decimal? Cost { get; set; }

        [StringLength(2500)]
        public string Comments { get; set; }

        [StringLength(100)]
        public string ItemNumber { get; set; }

        public Guid rowguid { get; set; }

        public DateTime? InServiceDate { get; set; }

        public Guid? ItemCategoryId { get; set; }

        public DateTime? NextServiceDate { get; set; }

        public Guid? ServiceTypeId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool Inactive { get; set; }

        public Guid? ActivityTypeId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public Guid? InvItemId { get; set; }

        public Guid? LocationBaseId { get; set; }

        public Guid? LocationId { get; set; }
    }
}
