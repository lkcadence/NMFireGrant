namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ComplaintParties
    {
        [Key]
        [Column(Order = 0)]
        public Guid ComplaintPartyId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ComplaintId { get; set; }

        public Guid? PartyID { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string PhoneExt { get; set; }

        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(20)]
        public string FaxExt { get; set; }

        [StringLength(20)]
        public string Cell { get; set; }

        [StringLength(20)]
        public string CellExt { get; set; }

        [StringLength(20)]
        public string Pager { get; set; }

        [StringLength(20)]
        public string PagerExt { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(2000)]
        public string Comment { get; set; }

        public bool? Inactive { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string AgencyName { get; set; }

        public Guid? RoleTypeId { get; set; }

        [StringLength(50)]
        public string RoleType { get; set; }

        public bool? EmployeeType { get; set; }

        public Guid? InspectorId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
