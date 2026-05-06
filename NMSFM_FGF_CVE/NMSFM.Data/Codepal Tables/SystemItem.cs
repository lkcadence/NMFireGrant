namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemItem
    {
        public Guid SystemItemId { get; set; }

        public Guid rowguid { get; set; }

        public Guid SystemTemplateId { get; set; }

        public Guid ItemTypeId { get; set; }

        public int Count { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public Guid StatusId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }
    }
}
