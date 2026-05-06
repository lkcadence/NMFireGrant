namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Inspector
    {
        public Guid InspectorId { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(500)]
        public string Password { get; set; }

        public bool Admin { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string InspectorPhone { get; set; }

        [Column(TypeName = "image")]
        public byte[] Signature { get; set; }

        public bool? LoggedIn { get; set; }

        public bool Madmin { get; set; }

        public Guid? GroupId { get; set; }

        public bool? Inactive { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public bool? CodeExempt { get; set; }

        public bool GlobalUser { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(500)]
        public string RCLevel { get; set; }

        [StringLength(50)]
        public string ActiveModules { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool DisablePWChange { get; set; }

        [StringLength(250)]
        public string SecQOne { get; set; }

        [StringLength(100)]
        public string SecAOne { get; set; }

        [StringLength(250)]
        public string SecQTwo { get; set; }

        [StringLength(100)]
        public string SecATwo { get; set; }

        [StringLength(200)]
        public string Title { get; set; }
    }
}
