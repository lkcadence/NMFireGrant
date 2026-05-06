namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Fees
    {
        [Key]
        [Column(Order = 0)]
        public Guid FeeId { get; set; }
        public Guid? RecordId { get; set; }
        public DateTime? FeeDate { get; set; }
        [Column(TypeName = "money")]
        public decimal? FeeAmt { get; set; }
        [Key]
        [Column(Order = 1, TypeName = "money")]
        public decimal PaymentAmt { get; set; }
        [Column(TypeName = "money")]
        public decimal? BalanceDue { get; set; }
        public DateTime? PaymentDate { get; set; }
        [StringLength(50)]
        public string RefNum { get; set; }
        [StringLength(2500)]
        public string Comment { get; set; }
        public DateTime? InspectionDate { get; set; }
        [StringLength(50)]
        public string InspectionNumber { get; set; }
        public Guid? InspectedPartyId { get; set; }
        public decimal? Hrs { get; set; }
        [StringLength(50)]
        public string PermitNumber { get; set; }
        public Guid? IssuedToPartyId { get; set; }
        [StringLength(50)]
        public string PartyName { get; set; }
        public Guid? InspectionId { get; set; }
        public Guid? PermitId { get; set; }
        public Guid? FeeTypeId { get; set; }
        [StringLength(50)]
        public string FeeType { get; set; }
        [Column(TypeName = "money")]
        public decimal? FeeBase { get; set; }
        public decimal? Units { get; set; }
        [StringLength(50)]
        public string FeeUOM { get; set; }
        public bool? Rate { get; set; }
        public Guid? InvoiceId { get; set; }
        [StringLength(100)]
        public string PermitType { get; set; }
        public Guid? ResponsiblePartyId { get; set; }
        public Guid? ParentInspectionId { get; set; }
        [StringLength(50)]
        public string RespParty { get; set; }
        public Guid? PAgencyId { get; set; }

        public Guid? FAgencyId { get; set; }

        public bool? RatedRange { get; set; }

        public bool? TotalPercent { get; set; }

        public bool? Penalty { get; set; }

        [StringLength(250)]
        public string FeeDesc { get; set; }

        public int? FeeStatus { get; set; }

        public Guid? PaymentUserId { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? PartyID { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }

        public Guid? ProjectId { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Contract { get; set; }

        public bool? IsSub { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool ActivityComplete { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool ProjectComplete { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool PermitComplete { get; set; }

        public bool? Inactive { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string FeeBarcode { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool IsDefault { get; set; }

        public DateTime? ReCalcDate { get; set; }

        public DateTime? OriginalFeeDate { get; set; }

        public bool? WebViewable { get; set; }

        public Guid? InvItemId { get; set; }

        [StringLength(50)]
        public string InventoryItem { get; set; }

        [StringLength(100)]
        public string InvItemBarcode { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }
    }
}
