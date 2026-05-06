namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Module
    {
        public Guid ModuleId { get; set; }

        [Required]
        [StringLength(50)]
        public string ModuleDesc { get; set; }

        [StringLength(50)]
        public string ModuleAlias { get; set; }

        public Guid rowguid { get; set; }

        public Guid? AgencyId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
