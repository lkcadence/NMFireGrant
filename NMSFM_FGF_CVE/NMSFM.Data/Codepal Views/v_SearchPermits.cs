namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SearchPermits
    {
        [Key]
        [Column(Order = 0)]
        public Guid PermitId { get; set; }

        public Guid? PermitTypeId { get; set; }

        [StringLength(50)]
        public string PermitNumber { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(2500)]
        public string Comment { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(100)]
        public string PermitType { get; set; }

        public Guid? IssuedToPartyId { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(5)]
        public string State { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        public Guid? CountyId { get; set; }

        public Guid? RegionId { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        public Guid? ApprovalStep { get; set; }

        public Guid? AgencyId { get; set; }

        public bool? AddInact { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public Guid? RTAgencyId { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "money")]
        public decimal FeeSum { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal PaymentSum { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "money")]
        public decimal ReleviedAmt { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal BalanceDue { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(15)]
        public string Paid { get; set; }

        public Guid? ParentPermitId { get; set; }

        public Guid? PermitStatusId { get; set; }

        [StringLength(50)]
        public string RoleType { get; set; }

        public Guid? IssuingOfficerId { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string PermitStatus { get; set; }

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

        [Key]
        [Column(Order = 7)]
        public bool Complete { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SubmittalDate { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(50)]
        public string PropertyUse { get; set; }

        public Guid? RoleTypeId { get; set; }

        public bool? Inactive { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }

        public bool? FromWeb { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool StopAlerts { get; set; }

        public Guid? ItemId { get; set; }

        [StringLength(100)]
        public string ItemDescription { get; set; }

        [StringLength(75)]
        public string ItemType { get; set; }

        [StringLength(50)]
        public string ItemBarcode { get; set; }

        [StringLength(200)]
        public string ItemLocation { get; set; }

        [StringLength(20)]
        public string ItemStatus { get; set; }

        [StringLength(100)]
        public string ItemNumber { get; set; }

        [StringLength(50)]
        public string ItemLocationBase { get; set; }

        [StringLength(50)]
        public string ItemLocationType { get; set; }
    }
}
