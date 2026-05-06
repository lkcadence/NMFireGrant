namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ServiceType
    {
        public Guid ServiceTypeId { get; set; }

        [Column("ServiceType")]
        [Required]
        [StringLength(100)]
        public string ServiceType1 { get; set; }

        public Guid? ItemCategoryId { get; set; }

        public Guid? ItemTypeId { get; set; }

        public short ServiceFrequency { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool? CalcAnyDate { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
