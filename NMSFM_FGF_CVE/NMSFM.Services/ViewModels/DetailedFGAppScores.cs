using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.ViewModels
{
    public class DetailedFGAppScores
    {
        public Guid ApplicationId { get; set; }
        public Guid? WebUserId { get; set; }
        public string UserName { get; set; }
        public int ISORating { get; set; }
        public int TrainingPoints { get; set; }
        public int FinancialNeedGrade { get; set; }
        public int ProblemGrade { get; set; }
        public int BenefitGrade { get; set; }
        public int ConsequencesGrade { get; set; }
        public int AppCompletenessGrade { get; set; }
        public int TotalScore { get; set; }
    }
}
