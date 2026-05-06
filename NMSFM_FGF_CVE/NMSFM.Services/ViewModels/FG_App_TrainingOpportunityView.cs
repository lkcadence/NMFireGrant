using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
    [Serializable]
    public class FG_App_TrainingOpportunityView
    {
        public Guid TrainingId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Number { get; set; }
        public string TrainingDetail { get; set; }
        public string TrainingDocumentName { get; set; }
        public string TrainingDocumentType { get; set; }
    }
}
