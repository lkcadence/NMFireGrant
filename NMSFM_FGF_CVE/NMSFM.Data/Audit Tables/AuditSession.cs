namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuditSession
    {
        [Key]
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string ComputerName { get; set; }

        [StringLength(15)]
        public string ComputerIP { get; set; }

        [Required]
        [StringLength(50)]
        public string WindowsUser { get; set; }

        public DateTime SessionStart { get; set; }

        public DateTime? SessionEnd { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public Guid rowguid { get; set; }
    }
}
