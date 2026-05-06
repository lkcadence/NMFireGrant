namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SyncSetting
    {
        [StringLength(50)]
        public string Instance { get; set; }

        [StringLength(50)]
        public string DB { get; set; }

        [StringLength(50)]
        public string UID { get; set; }

        [StringLength(50)]
        public string Pwd { get; set; }

        [StringLength(50)]
        public string AddressType { get; set; }

        [StringLength(50)]
        public string RoleType { get; set; }

        [StringLength(50)]
        public string ContAddrType { get; set; }

        public byte? UpdateDirection { get; set; }

        [Key]
        public Guid SyncSettingsId { get; set; }

        public bool? FHSQL { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
