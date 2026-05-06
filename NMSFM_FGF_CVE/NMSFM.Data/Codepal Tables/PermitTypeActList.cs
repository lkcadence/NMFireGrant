namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermitTypeActList")]
    public partial class PermitTypeActList
    {
        [Key]
        public Guid PermitTypeId { get; set; }

        [StringLength(2000)]
        public string ActListText { get; set; }

        public Guid? PropConst { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
