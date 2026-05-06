namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("zzPartyWebAccess")]
    public partial class zzPartyWebAccess
    {
        [Key]
        public Guid PartyId { get; set; }

        public Guid AgencyId { get; set; }

        [StringLength(50)]
        [Required]
        public string UserName { get; set; }

        [StringLength(50)]
        [Required]
        public string Password { get; set; }

        public bool TempPW { get; set; }
        public Guid rowguid { get; set; }

        [StringLength(500)]
        public string Secq01 { get; set; }

        [StringLength(500)]
        public string Secq01A { get; set; }

        [StringLength(500)]
        public string Secq02 { get; set; }

        [StringLength(500)]
        public string Secq02A { get; set; }
        [StringLength(500)]
        public string Secq03 { get; set; }

        [StringLength(500)]
        public string Secq03A { get; set; }
        [StringLength(500)]
        public string Secq04 { get; set; }

        [StringLength(500)]
        public string Secq04A { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool? Locked { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }

    }
}
