namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ModuleAliases
    {
        [Key]
        [Column(Order = 0)]
        public Guid ModuleId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ModuleDesc { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string ModuleAlias { get; set; }

        public Guid? AgencyId { get; set; }
    }
}
