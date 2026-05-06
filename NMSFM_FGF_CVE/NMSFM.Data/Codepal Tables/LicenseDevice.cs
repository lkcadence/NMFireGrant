namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LicenseDevice
    {
        public Guid LicenseDeviceId { get; set; }

        [Required]
        [StringLength(100)]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(50)]
        public string DeviceName { get; set; }

        public bool? ReadOnly { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
