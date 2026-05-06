using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    public partial class FG_App_BudgetInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public decimal OperatingBudget { get; set; }
        public decimal FPFDistribution { get; set; }
        public decimal StipendCarryover { get; set; }
        public decimal CarryoverBalance { get; set; }
        public string CarryoverPurpose { get; set; }
        public decimal PerTaxes { get; set; }
        public decimal PerGrants { get; set; }
        public decimal PerStateFMFunds { get; set; }
        public decimal PerDonations { get; set; }
        public decimal PerFundDrives { get; set; }
        public decimal PerFeeForService { get; set; }
        public decimal PerOthers { get; set; }
        public string OthersDesc { get; set; }
        public decimal PerTotal { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
    }
}
