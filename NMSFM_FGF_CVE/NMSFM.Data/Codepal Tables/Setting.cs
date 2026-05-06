namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Setting
    {
        [Key]
        public Guid SettingsId { get; set; }

        [StringLength(200)]
        public string PropertyField { get; set; }

        [StringLength(3000)]
        public string ValueField { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
