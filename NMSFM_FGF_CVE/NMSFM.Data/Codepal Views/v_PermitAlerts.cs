namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_PermitAlerts
    {
        [Key]
        [Column(Order = 0)]
        public Guid PermitId { get; set; }

        public Guid? PermitTypeId { get; set; }

        [StringLength(100)]
        public string PermitType { get; set; }

        public Guid? FeeTypeId { get; set; }

        [StringLength(50)]
        public string FeeType { get; set; }

        [Column(TypeName = "money")]
        public decimal? FeeAmt { get; set; }

        [StringLength(50)]
        public string PermitNumber { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? IssuedToPartyId { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public Guid? StateId { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(2500)]
        public string Comment { get; set; }

        public Guid? RecordId { get; set; }

        public short? PermitFreq { get; set; }

        public Guid? DefaultInvoiceTypeId { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public bool? NoAlert { get; set; }

        public Guid? PTAgencyId { get; set; }

        [Key]
        [Column(Order = 1)]
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
