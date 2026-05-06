namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoleType
    {
        public Guid RoleTypeId { get; set; }

        [Column("RoleType")]
        [StringLength(50)]
        public string RoleType1 { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid? ReportId { get; set; }

        public bool? NonAdminReadOnly { get; set; }

        public bool EmployeeType { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
