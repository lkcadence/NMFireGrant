namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ProjectReport
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Complete { get; set; }

        [Column(TypeName = "money")]
        public decimal? ContractTotal { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool ContractComplete { get; set; }

        public bool? PrimaryParty { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        public DateTime? InspectionDate { get; set; }

        public DateTime? Expr1 { get; set; }

        public DateTime? Expr2 { get; set; }

        public Guid? InspectorId { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(1000)]
        public string CheckItem { get; set; }

        public short? SeqNum { get; set; }

        public short? CheckListOrder { get; set; }

        [StringLength(50)]
        public string CheckListName { get; set; }

        [StringLength(50)]
        public string CheckItemType { get; set; }

        [StringLength(3000)]
        public string Value { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Corrected { get; set; }

        [StringLength(2000)]
        public string ResolutionText { get; set; }

        public DateTime? Expr3 { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        [StringLength(100)]
        public string CodeVersion { get; set; }

        [StringLength(50)]
        public string ViolationAlias { get; set; }

        public int? Sequence { get; set; }

        [StringLength(20)]
        public string SevereC { get; set; }

        [StringLength(500)]
        public string SevereMessage { get; set; }

        public bool? SevereBlack { get; set; }

        public Guid? InspectionId { get; set; }

        public Guid? CurrentInspectionId { get; set; }

        public Guid? InspectedPartyId { get; set; }
    }
}
