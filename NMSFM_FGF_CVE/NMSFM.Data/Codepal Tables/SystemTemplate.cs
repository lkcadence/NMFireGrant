namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemTemplate
    {
        public Guid SystemTemplateId { get; set; }

        public Guid rowguid { get; set; }

        public Guid ItemTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string SystemName { get; set; }

        public Guid StatusId { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
