namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SearchDisplay
    {
        public Guid SearchDisplayId { get; set; }

        [StringLength(50)]
        public string FormName { get; set; }

        [StringLength(50)]
        public string SettingName { get; set; }

        [StringLength(150)]
        public string SettingValue { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
