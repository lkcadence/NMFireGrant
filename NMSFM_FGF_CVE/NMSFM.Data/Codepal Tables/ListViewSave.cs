namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ListViewSave")]
    public partial class ListViewSave
    {
        public Guid ListViewSaveId { get; set; }

        public Guid? UserId { get; set; }

        [StringLength(50)]
        public string Form { get; set; }

        [StringLength(50)]
        public string Tab { get; set; }

        [Column(TypeName = "ntext")]
        public string ListViewData { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
