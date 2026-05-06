using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.ViewModels
{
	public class DetailedFGAppValidation
	{
		public Guid? ApplicationId { get; set; }
		public short? FiscalYear { get; set; }
		public string ApplicationNumber { get; set; }
		public DateTime? DateSubmitted { get; set; }
		public short? AppStatus { get; set; }
		public string ApplicationStatus { get; set; }
		public DateTime? LastStatusChange { get; set; }
		public bool InstructionsSubmitted { get; set; }
		public bool GeneralInfoValid { get; set; }
		public bool BudgetInfoValid { get; set; }
		public bool CommunityInfoValid { get; set; }
		public bool ResponseHistoryValid { get; set; }
		public bool WaterAvailabilityValid { get; set; }
		public bool TrainingValid { get; set; }
		public bool ApparatusValid { get; set; }
		public bool CommunicationEquipValid { get; set; }
		public bool HazardsThreatsValid { get; set; }
		public bool PPEValid { get; set; }
		public bool EquipmentNeedsValid { get; set; }
		public bool GrantFundingJustificationValid { get; set; }
		public bool ProjectBudgetValid { get; set; }
		public bool DocsSigsValid { get; set; }
    }
}
