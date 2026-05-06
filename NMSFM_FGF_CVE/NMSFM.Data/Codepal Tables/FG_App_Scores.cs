using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_Scores
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid ScoreId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid WebUserId { get; set; }
        public string UserName { get; set; }
        public int TrainingScore { get; set; }
        public int FinancialNeedScore { get; set; }
        public int ProblemScore { get; set; }
        public int BenefitScore { get; set; }
        public int ConsequencesScore { get; set; }
        public int CompletenessScore { get; set; }
        public DateTime DateEntered { get; set; }

    }
}
