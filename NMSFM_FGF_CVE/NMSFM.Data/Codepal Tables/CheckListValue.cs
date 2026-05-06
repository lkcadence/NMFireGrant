namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CheckListValue
    {
        public Guid CheckListValueId { get; set; }

        public Guid CheckListId { get; set; }

        public Guid InspectionId { get; set; }

        [Column("CheckListValue")]
        [Required]
        [StringLength(3000)]
        public string CheckListValue1 { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string Externalid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
