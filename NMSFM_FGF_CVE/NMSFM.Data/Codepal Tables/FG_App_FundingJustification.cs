using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;


namespace NMSFM.Data
{
    public partial class FG_App_FundingJustification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int CriticalNeed { get; set; }
        public string FinancialNeed { get; set; }
        public string Problem { get; set; }
        public string BenefitToCommunity { get; set; }
        public string Consequences { get; set; }
        public string FinancialNeedComments { get; set; }
        public string ProblemComments { get; set; }
        public string BenefitComments { get; set; }
        public string ConsequencesComments { get; set; }
        public int FinancialNeedGrade { get; set; }
        public int ProblemGrade { get; set; }
        public int BenefitGrade { get; set; }
        public int ConsequencesGrade { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
    }
}
