namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Permits
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

        public Guid? RecordId { get; set; }

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

        [StringLength(15)]
        public string Zip { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Complete { get; set; }

        public Guid? ApprovalStep { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid? ContactId { get; set; }

        [StringLength(50)]
        public string PropConst { get; set; }

        public Guid? OwnerId { get; set; }

        public Guid? ContractorId { get; set; }

        public Guid? TypePropConst { get; set; }

        [StringLength(41)]
        public string Phone { get; set; }

        public bool? FromWeb { get; set; }

        public Guid? PermitStatusId { get; set; }

        public Guid? ParentPermitId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SubmittalDate { get; set; }

        [StringLength(50)]
        public string PermitStatus { get; set; }

        public Guid? IssuedToRoleTypeId { get; set; }

        public Guid? ContactRoleTypeId { get; set; }

        [StringLength(2000)]
        public string ActListText { get; set; }

        public Guid? IssuingOfficerId { get; set; }

        public Guid? ReportId { get; set; }

        public Guid? CertReportId { get; set; }

        public Guid? LandCertReportId { get; set; }

        public Guid? ALReportId { get; set; }

        public bool? EditDefFreq { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        public bool? HasFees { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool StopAlerts { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

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
