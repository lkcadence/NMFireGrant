namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_AddressParties
    {
        [Key]
        [Column(Order = 0)]
        public Guid PartyID { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(2000)]
        public string Comment { get; set; }

        public Guid? RoleTypeId { get; set; }

        [StringLength(50)]
        public string RoleType { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public Guid? StateId { get; set; }

        public Guid? CountryId { get; set; }

        //[Key]
        //[Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        public Guid? RegionId { get; set; }

        public Guid? CountyId { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        public bool? Inactive { get; set; }

        //[Key]
        //[Column(Order = 2)]
        public bool PtyInact { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public Guid? AddressTypeId { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [Key]
        [Column(Order =27)]
        public Guid? AddressPartyId { get; set; }

        [StringLength(50)]
        public string ExternalValue { get; set; }

        public bool? EmployeeType { get; set; }

        public Guid? InspectorId { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string PhoneExt { get; set; }

        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(20)]
        public string FaxExt { get; set; }

        [StringLength(20)]
        public string Pager { get; set; }

        [StringLength(20)]
        public string PagerExt { get; set; }

        [StringLength(20)]
        public string Cell { get; set; }

        [StringLength(20)]
        public string CellExt { get; set; }

        [StringLength(50)]
        public string Map { get; set; }

        [StringLength(50)]
        public string Block { get; set; }

        [StringLength(50)]
        public string Lot { get; set; }

        [StringLength(50)]
        public string TaxParcel { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
