using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;


namespace NMSFM.ViewModels
{
    public class DetailedFGAppTraining
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int YearlyTrainingHours { get; set; }
        public int TrainingPoints { get; set; }
        public string AdminComments { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
        public List<FG_App_TrainingOpportunityView> TrainingOpportunities { get; set; }
    }
}
