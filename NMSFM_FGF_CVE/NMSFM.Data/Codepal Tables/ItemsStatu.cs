namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemsStatu
    {
        [Key]
        public Guid StatusId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool WebViewable { get; set; }

        public bool Inactive { get; set; }

        public Guid rowguid { get; set; }
    }
}
