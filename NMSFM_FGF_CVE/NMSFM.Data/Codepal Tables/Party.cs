namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Party")]
    public partial class Party
    {
        public Guid PartyID { get; set; }

        [StringLength(10)]
        public string Salutation { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(5)]
        public string MiddleInitial { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(5)]
        public string Suffix { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Cell { get; set; }

        [StringLength(50)]
        public string Pager { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(2000)]
        public string Comment { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool Inactive { get; set; }

        public Guid? WebUserId { get; set; }

        [StringLength(100)]
        public string QBCustomerListID { get; set; }

        public Guid? InspectorId { get; set; }

        [Column(TypeName = "image")]
        public byte[] PartyImage { get; set; }

        [Column(TypeName = "image")]
        public byte[] Signature { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public short? PriceLevel { get; set; }

        public Guid? ReportId { get; set; }
        public bool FromWeb { get; set; }

    }
}
