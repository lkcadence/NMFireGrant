namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemLocDesc
    {
        public Guid ItemLocDescId { get; set; }

        [Column("ItemLocDesc")]
        [StringLength(50)]
        public string ItemLocDesc1 { get; set; }

        public bool IsItem { get; set; }

        public Guid rowguid { get; set; }

        public Guid? TypeId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }
    }
}
