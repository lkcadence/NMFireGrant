namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserDefCategory
    {
        public Guid UserDefCategoryId { get; set; }

        public Guid? ModuleId { get; set; }

        public Guid? ModuleTypeId { get; set; }

        public bool AllModuleTypes { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        public Guid rowguid { get; set; }

        public short? SeqNum { get; set; }

        public bool? ActPrint { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(3)]
        public string AllAgency { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool? WebViewable { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }
    }
}
