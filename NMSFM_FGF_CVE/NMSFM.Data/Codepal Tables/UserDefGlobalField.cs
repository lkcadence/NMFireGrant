namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserDefGlobalField
    {
        [Key]
        public Guid GlobalId { get; set; }

        [StringLength(3000)]
        public string GlobalFieldDesc { get; set; }

        public Guid? AddressId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
