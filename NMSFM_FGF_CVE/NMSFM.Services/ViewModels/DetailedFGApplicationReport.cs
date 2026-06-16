using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.ViewModels
{
    public class DetailedFGApplicationReport
    {
		//Application Table
		public Guid? ApplicationId { get; set; }
		public long? FGApplicationIdentity { get; set; }
		public short? FiscalYear { get; set; }
		public Guid addressId { get; set; }
		public string ApplicationNumber { get; set; }
		public DateTime DateStarted { get; set; }
		public DateTime? DateSubmitted { get; set; }
		public Guid? SubmittedBy { get; set; }
		public string SubmittedByName { get; set; }
		public short? AppStatus { get; set; }
		public string Status { get; set; }
		public DateTime? LastStatusChange { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public Guid? ApprovedBy { get; set; }
		public string ApprovedByName { get; set; }
		public decimal? GrantedAmount { get; set; }
		//General Information
		public int? IndividualDept { get; set; }
		public string strIndividualDept { get; set; }
		public string NERISID { get; set; }
		public string DepartmentName { get; set; }
		public string FireChiefName { get; set; }
		public string Phone { get; set; }
		public string EmailAddress { get; set; }
		public int? ISORating { get; set; }
		public string County { get; set; }
		public int? IsCityMuni { get; set; }
		public string strCityMuni { get; set; }
		public int? DeptType { get; set; }
		public string strDeptType { get; set; }
		public bool? IsAdminDept { get; set; }
		public int? CountyDeptsCompliant { get; set; }
		public string strCountyDeptsCompliant { get; set; }
		public int? MainStations { get; set; }
		public int? SubStations { get; set; }
		public int? AdminBldgs { get; set; }
		public int? Community { get; set; }
		public string strCommunity { get; set; }
		public int? NumberOfFirefighters { get; set; }
		public int? FFI_Firefighters { get; set; }
		public int? FFII_Firefighters { get; set; }
		public string MailingAddress { get; set; }
		public string MailingCity { get; set; }
		public string MailingState { get; set; }
		public string MailingZip { get; set; }
		public string PersonCompleteApp { get; set; }
		public int? FireDeptMember { get; set; }
		public string strFireDeptMember { get; set; }
		//Budget Information
		public decimal? OperatingBudget { get; set; }
		public decimal? FPFDistribution { get; set; }
		public decimal? StipendCarryover { get; set; }
		public decimal? CarryoverBalance { get; set; }
		public string CarryoverPurpose { get; set; }
		public decimal? PerTaxes { get; set; }
		public decimal? PerGrants { get; set; }
		public decimal? PerStateFMFunds { get; set; }
		public decimal? PerDonations { get; set; }
		public decimal? PerFundDrives { get; set; }
		public decimal? PerFeeForService { get; set; }
		public decimal? PerOthers { get; set; }
		public string OthersDesc { get; set; }
		public decimal? PerTotal { get; set; }
		//Community Information
		public string CommunityName { get; set; }
		public int? NumberOfHomes { get; set; }
		public int? NumberOfComm { get; set; }
		public int? ResidentPopulation { get; set; }
		public int? AidAgreements { get; set; }
		public string strAidAgreements { get; set; }
		//Response History
		public int? NERISCurrent { get; set; }
		public string strNERISCurrent { get; set; }
		public int? ResponseStructure { get; set; }
		public int? ResponseVehicle { get; set; }
		public int? ResponseVegitation { get; set; }
		public int? ResponseEMS { get; set; }
		public int? ResponseRescue { get; set; }
		public int? ResponseHazardous { get; set; }
		public int? ResponseService { get; set; }
		public int? ResponseGoodIntent { get; set; }
		public int? ResponseFalse { get; set; }
		public int? ResponseOther { get; set; }
		public int? ResponseTotal { get; set; }
		//Water Availability
		public int? ComHydrantSys { get; set; }
		public string strComHydrantSys { get; set; }
		public int? AvailableWaterCapacity { get; set; }
		public int? WaterOnWheelsCapacity { get; set; }
		public int? StationWaterCapacity { get; set; }
		public int? TankAtStation { get; set; }
		public string strTankAtStation { get; set; }
		//Training
		public int? YearlyTrainingHours { get; set; }
		public int? NumberOfListedTrainings { get; set; }
		//Apparatus
		public int? ApparatusPartOfProject { get; set; }
		public string strApparatusPartOfProject { get; set; }
		public int? PumpTestsConducted { get; set; }
		public string strPumpTestsConducted { get; set; }
		public string ExplainNoPumpTests { get; set; }
		public int? HoseTestConducted { get; set; }
		public string strHoseTestsConducted { get; set; }
		public string ExplainNoHostTests { get; set; }
		public int? NumberOfListedApparatus { get; set; }
		//Communication
		public int? CommunicationProject { get; set; }
		public string strCommunicationProject { get; set; }
		public int? HandheldRadios { get; set; }
		public int? BaseStations { get; set; }
		public int? MobileRadios { get; set; }
		public int? ApparatusWoRadio { get; set; }
		public int? LawEnforcement { get; set; }
		public int? EmergencyMedical { get; set; }
		public int? OtherFireDepts { get; set; }
		public int? Other { get; set; }
		public string OtherDescription { get; set; }
		public int? AreasNotCovered { get; set; }
		public string DescribeAreasNotCovered { get; set; }
		public int? NumberOfCommunicationDevicesListed { get; set; }
		//Hazards/Threads
		public int? NumberOfHazardsThreatsListed { get; set; }
		//PPE
		public int? PPEPartOfProject { get; set; }
		public string strPPEPartOfProject { get; set; }
		public int? PPEInspected { get; set; }
		public string strPPEInspected { get; set; }
		public int? NumberOfPPEListed { get; set; }
		public int? NumberOfSCBAListed { get; set; }
		//EquipmentNeeded
		public string SpecificNeeds { get; set; }
		public int? ISOImpacted { get; set; }
		public string strISOImpacted { get; set; }
		public string ISOImpactExplanation { get; set; }
		public int? NumberOfEquipmentNeeded { get; set; }
		public decimal? AmountOfEquipmentNeeded { get; set; }
		//Project Budget
		public decimal? TotalProjectCost { get; set; }
		public decimal? AmountRequested { get; set; }
		public decimal? StipendAmount { get; set; }
		//Application Review
		public int? NERISCompliant { get; set; }
		public string strNERISCompliant { get; set; }
		public int? PumpTestCompliant { get; set; }
		public string strPumpTestCompliant { get; set; }
		//Application Scores
		public int? TrainingPoints { get; set; }
		public int? FinancialNeedGrade { get; set; }
		public int? ProblemGrade { get; set; }
		public int? BenefitGrade { get; set; }
		public int? ConsequencesGrade { get; set; }
		public int? AppCompletenessGrade { get; set; }
		public int? TotalScore { get; set; }
		//Other
		public DateTime? LastReceivedGrant { get; set; }
		public string PriorityCategories { get; set; }
		
		
		
		
	}
}
