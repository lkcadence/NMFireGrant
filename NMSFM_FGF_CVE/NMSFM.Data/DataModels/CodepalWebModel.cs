namespace NMSFM.Data
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class CodepalWebModel : DbContext, ICodepalWebModel
	{
		public CodepalWebModel()
			: base("name=CodepalWebModel")
		{
		}

		public CodepalWebModel(string connectionString)
					: base(connectionString)
		{
		}

		public virtual DbSet<ActivityCheckList> ActivityCheckLists { get; set; }
		public virtual DbSet<ActivityInventoryItem> ActivityInventoryItems { get; set; }
		public virtual DbSet<ActivityPermit> ActivityPermits { get; set; }
		public virtual DbSet<ActivitySetting> ActivitySettings { get; set; }
		public virtual DbSet<ActivityTypeCaus> ActivityTypeCauses { get; set; }
		public virtual DbSet<ActivityTypeGrid> ActivityTypeGrids { get; set; }
		public virtual DbSet<ActivityTypeItemType> ActivityTypeItemTypes { get; set; }
		public virtual DbSet<ActivityType> ActivityTypes { get; set; }
		public virtual DbSet<Address> Addresses { get; set; }
		public virtual DbSet<AddressMap> AddressMaps { get; set; }
		public virtual DbSet<AddressParty> AddressParties { get; set; }
		public virtual DbSet<AddressSetting> AddressSettings { get; set; }
		public virtual DbSet<AddressType> AddressTypes { get; set; }
		public virtual DbSet<Agency> Agencies { get; set; }
		public virtual DbSet<AgencyActivityType> AgencyActivityTypes { get; set; }
		public virtual DbSet<Agreement> Agreements { get; set; }
		public virtual DbSet<AgreementType> AgreementTypes { get; set; }
		public virtual DbSet<ApprovalStep> ApprovalSteps { get; set; }
		public virtual DbSet<ArchiveDB> ArchiveDBs { get; set; }
		public virtual DbSet<Assignment> Assignments { get; set; }
		public virtual DbSet<AssociatedActivity> AssociatedActivities { get; set; }
		public virtual DbSet<CalcType> CalcTypes { get; set; }
		public virtual DbSet<CategoryType> CategoryTypes { get; set; }
		public virtual DbSet<Certification> Certifications { get; set; }
		public virtual DbSet<CertificationType> CertificationTypes { get; set; }
		public virtual DbSet<CheckItemCalc> CheckItemCalcs { get; set; }
		public virtual DbSet<CheckItemCalcVal> CheckItemCalcVals { get; set; }
		public virtual DbSet<CheckItem> CheckItems { get; set; }
		public virtual DbSet<CheckItemType> CheckItemTypes { get; set; }
		public virtual DbSet<CheckItemValueInspectionDetail> CheckItemValueInspectionDetails { get; set; }
		public virtual DbSet<CheckItemValue> CheckItemValues { get; set; }
		public virtual DbSet<CheckItemViolationType> CheckItemViolationTypes { get; set; }
		public virtual DbSet<CheckListActivityType> CheckListActivityTypes { get; set; }
		public virtual DbSet<ChecklistIcon> ChecklistIcons { get; set; }
		public virtual DbSet<CheckList> CheckLists { get; set; }
		public virtual DbSet<CheckListType> CheckListTypes { get; set; }
		public virtual DbSet<CheckListValue> CheckListValues { get; set; }
		public virtual DbSet<CodeVersionAgency> CodeVersionAgencies { get; set; }
		public virtual DbSet<CodeVersion> CodeVersions { get; set; }
		public virtual DbSet<Comment> Comments { get; set; }
		public virtual DbSet<ComplaintActivity> ComplaintActivities { get; set; }
		public virtual DbSet<ComplaintParty> ComplaintParties { get; set; }
		public virtual DbSet<ComplaintPermit> ComplaintPermits { get; set; }
		public virtual DbSet<Complaint> Complaints { get; set; }
		public virtual DbSet<ComplaintStatu> ComplaintStatus { get; set; }
		public virtual DbSet<ComplaintType> ComplaintTypes { get; set; }
		public virtual DbSet<County> Counties { get; set; }
		public virtual DbSet<Country> Countries { get; set; }
		public virtual DbSet<DefaultFee> DefaultFees { get; set; }
		public virtual DbSet<FeePayment> FeePayments { get; set; }
		public virtual DbSet<Fee> Fees { get; set; }
		public virtual DbSet<FeeSchedule> FeeSchedules { get; set; }
		public virtual DbSet<FeeSetting> FeeSettings { get; set; }
		public virtual DbSet<FeesPT> FeesPTs { get; set; }
		public virtual DbSet<FeeTypeCon> FeeTypeCons { get; set; }
		public virtual DbSet<FeeTypePen> FeeTypePens { get; set; }
		public virtual DbSet<FeeTypePT> FeeTypePTs { get; set; }
		public virtual DbSet<FeeTypeRR> FeeTypeRRs { get; set; }
		public virtual DbSet<FeeType> FeeTypes { get; set; }
		public virtual DbSet<File> Files { get; set; }

		//Added for NMSFM Fire Protection Fund Distribution
		public virtual DbSet<FYStatuteDist> FYStatuteDists { get; set; }
		public virtual DbSet<FYAllowableDist> FYAllowableDists { get; set; }
		public virtual DbSet<FYTotalDist> FYTotalDists { get; set; }
		public virtual DbSet<FYTotalDistCalc> FYTotalDistCalcs { get; set; }
		public virtual DbSet<FYApplication> FYApplications { get; set; }
		public virtual DbSet<FYAppData> FYAppDatas { get; set; }		
		public virtual DbSet<FYAppStation> FYAppStations { get; set; }
		public virtual DbSet<nm_FYTotalDistribution> nm_FYTotalDistributions { get; set; }
		public virtual DbSet<nm_FYTotalDistributionCalc> nm_FYTotalDistributionCalcs { get; set; }
		public virtual DbSet<nm_FYDetailedApplication> nm_FYDetailedApplications { get; set; }
		public virtual DbSet<FYInvoices> FYInvoices { get; set; }
		public virtual DbSet<nm_FYDistributionInvoice> nm_FYDistributionInvoice { get; set; }
		//END Additions for NMSFM Fire Protection Fund Distribution

		//Added for NMSFM Fire Grant Funds
		public virtual DbSet<FGApplicationSettings> FGApplicationSettings { get; set; }
		public virtual DbSet<FGApplications> FGApplications { get; set; }
		public virtual DbSet<FG_App_GeneralInfo> FG_App_GeneralInfos { get; set; }
		public virtual DbSet<FG_App_BudgetInfo> FG_App_BudgetInfos { get; set; }
		public virtual DbSet<FG_App_CommunityInfo> FG_App_CommunityInfos { get; set; }
		public virtual DbSet<FG_App_AidDistricts> FG_App_AidDistricts { get; set; }
		public virtual DbSet<FG_App_ResponseHistory> FG_App_ResponseHistories { get; set; }
		public virtual DbSet<FG_App_WaterAvailability> FG_App_WaterAvailabilities { get; set; }
		public virtual DbSet<FG_App_WaterSources> FG_App_WaterSources { get; set; }
		public virtual DbSet<FG_App_Training> FG_App_Trainings { get; set; }
		public virtual DbSet<FG_App_TrainingOpportunities> FG_App_TrainingOpportunities { get; set; }
		public virtual DbSet<FG_App_Apparatus> FG_App_Apparatuses { get; set; }
		public virtual DbSet<FG_App_ApparatusEquipment> FG_App_ApparatusEquipment { get; set; }
		public virtual DbSet<FG_App_Communication> FG_App_Communications { get; set; }
		public virtual DbSet<FG_App_CommunicationEquipment> FG_App_CommunicationEquipment { get; set; }
		public virtual DbSet<FG_App_HazardsThreats> FG_App_HazardsThreats { get; set; }
		public virtual DbSet<FG_App_HazardThreatEvents> FG_App_HazardThreatEvents { get; set; }
		public virtual DbSet<FG_App_PPE> FG_App_PPEs { get; set; }
		public virtual DbSet<FG_App_StandardPPE> FG_App_StandardPPEs { get; set; }
		public virtual DbSet<FG_App_StandardSCBA> FG_App_StandardSCBAs { get; set; }
		public virtual DbSet<FG_App_EquipmentNeeds> FG_App_EquipmentNeeds { get; set; }
		public virtual DbSet<FG_App_ApplicationEquipment> FG_App_ApplicationEquipments { get; set; }
		public virtual DbSet<FG_App_FundingJustification> FG_App_FundingJustifications { get; set; }
        public virtual DbSet<FG_App_ProjectBudget> FG_App_ProjectBudgets { get; set; }
		public virtual DbSet<FG_App_Documents> FG_App_Documents { get; set; }
		public virtual DbSet<FG_App_Signatures> FG_App_Signatures { get; set; }
		public virtual DbSet<FG_App_DocsSigs> FG_App_DocsSigs { get; set; }
		public virtual DbSet<FG_Priorities> FG_Priorities { get; set; }
		public virtual DbSet<FG_Categories> FG_Categories { get; set; }
		public virtual DbSet<FG_FDIDs> FG_FDIDs { get; set; }
		public virtual DbSet<nm_FGApplication> nm_FGApplications { get; set; }
		public virtual DbSet<nm_FGApplicationReport> nm_FGApplicationReport { get; set; }
		public virtual DbSet<FG_App_Review> FG_App_Reviews { get; set; }
		public virtual DbSet<FG_App_Help> FG_App_Helps { get; set; }
		public virtual DbSet<FG_App_Scores> FG_App_Scores { get; set; }

		//END Additions for NMSFM Fire Grant Funds
		public virtual DbSet<Group> Groups { get; set; }
		public virtual DbSet<InspectionCaus> InspectionCauses { get; set; }
		public virtual DbSet<InspectionDetail> InspectionDetails { get; set; }
		public virtual DbSet<Inspection> Inspections { get; set; }
		public virtual DbSet<InspectionType> InspectionTypes { get; set; }
		public virtual DbSet<InspectorGroup> InspectorGroups { get; set; }
		public virtual DbSet<Inspector> Inspectors { get; set; }
		public virtual DbSet<InventoryItem> InventoryItems { get; set; }
		public virtual DbSet<InventoryItemType> InventoryItemTypes { get; set; }
		public virtual DbSet<InvNarrative> InvNarratives { get; set; }
		public virtual DbSet<InvNarrativeText> InvNarrativeTexts { get; set; }
		public virtual DbSet<InvoicePayment> InvoicePayments { get; set; }
		public virtual DbSet<Invoice> Invoices { get; set; }
		public virtual DbSet<InvoiceSetting> InvoiceSettings { get; set; }
		public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
		public virtual DbSet<ItemInspectionStatu> ItemInspectionStatus { get; set; }
		public virtual DbSet<ItemLocDesc> ItemLocDescs { get; set; }
		public virtual DbSet<Item> Items { get; set; }
		public virtual DbSet<ItemSetting> ItemSettings { get; set; }
		public virtual DbSet<ItemsStatu> ItemsStatus { get; set; }
		public virtual DbSet<ItemType> ItemTypes { get; set; }
		public virtual DbSet<License> Licenses { get; set; }
		public virtual DbSet<LicenseDevice> LicenseDevices { get; set; }
		public virtual DbSet<ListViewSave> ListViewSaves { get; set; }
		public virtual DbSet<LocationBas> LocationBases { get; set; }
		public virtual DbSet<Location> Locations { get; set; }
		public virtual DbSet<LocationSetting> LocationSettings { get; set; }
		public virtual DbSet<LocationType> LocationTypes { get; set; }
		public virtual DbSet<Memorandum> Memorandums { get; set; }
		public virtual DbSet<Mileage> Mileages { get; set; }
		public virtual DbSet<Module> Modules { get; set; }
		public virtual DbSet<ModuleTypeReport> ModuleTypeReports { get; set; }
		public virtual DbSet<Note> Notes { get; set; }
		public virtual DbSet<NumberSchema> NumberSchemas { get; set; }
		public virtual DbSet<OccupancyType> OccupancyTypes { get; set; }
		public virtual DbSet<Party> Parties { get; set; }
		public virtual DbSet<PartyPriceLevel> PartyPriceLevels { get; set; }
		public virtual DbSet<PartyRole> PartyRoles { get; set; }
		public virtual DbSet<PermitCertificateType> PermitCertificateTypes { get; set; }
		public virtual DbSet<Permit> Permits { get; set; }
		public virtual DbSet<PermitSetting> PermitSettings { get; set; }
		public virtual DbSet<PermitStatu> PermitStatus { get; set; }
		public virtual DbSet<PermitSub> PermitSubs { get; set; }
		public virtual DbSet<PermitTypeActList> PermitTypeActLists { get; set; }
		public virtual DbSet<PermitType> PermitTypes { get; set; }
		public virtual DbSet<Phone> Phones { get; set; }
		public virtual DbSet<PhoneType> PhoneTypes { get; set; }
		public virtual DbSet<ProjectActivity> ProjectActivities { get; set; }
		public virtual DbSet<ProjectAddress> ProjectAddresses { get; set; }
		public virtual DbSet<ProjectInspector> ProjectInspectors { get; set; }
		public virtual DbSet<ProjectPermit> ProjectPermits { get; set; }
		public virtual DbSet<ProjectRequest> ProjectRequests { get; set; }
		public virtual DbSet<Project> Projects { get; set; }
		public virtual DbSet<ProjectSetting> ProjectSettings { get; set; }
		public virtual DbSet<ProjectStatu> ProjectStatus { get; set; }
		public virtual DbSet<ProjectType> ProjectTypes { get; set; }
		public virtual DbSet<ProjectTypesDefault> ProjectTypesDefaults { get; set; }
		public virtual DbSet<PropertyUseType> PropertyUseTypes { get; set; }
		public virtual DbSet<Region> Regions { get; set; }
		public virtual DbSet<ReportDataDefinition> ReportDataDefinitions { get; set; }
		public virtual DbSet<ReportDefinition> ReportDefinitions { get; set; }
		public virtual DbSet<ReportGroup> ReportGroups { get; set; }
		public virtual DbSet<Report> Reports { get; set; }
		public virtual DbSet<ReportSQL> ReportSQLs { get; set; }
		public virtual DbSet<ReportUserGroup> ReportUserGroups { get; set; }
		public virtual DbSet<ReportWizardDefinition> ReportWizardDefinitions { get; set; }
		public virtual DbSet<RequestSetting> RequestSettings { get; set; }
		public virtual DbSet<Resolution> Resolutions { get; set; }
		public virtual DbSet<RoleType> RoleTypes { get; set; }
		public virtual DbSet<Route> Routes { get; set; }
		public virtual DbSet<RoutingSlip> RoutingSlips { get; set; }
		public virtual DbSet<SearchDisplay> SearchDisplays { get; set; }
		public virtual DbSet<ServiceHistory> ServiceHistories { get; set; }
		public virtual DbSet<ServiceTypeActivityType> ServiceTypeActivityTypes { get; set; }
		public virtual DbSet<ServiceTypeItemType> ServiceTypeItemTypes { get; set; }
		public virtual DbSet<ServiceType> ServiceTypes { get; set; }
		public virtual DbSet<Setting> Settings { get; set; }
		public virtual DbSet<Severity> Severities { get; set; }
		public virtual DbSet<SeverityActivityType> SeverityActivityTypes { get; set; }
		public virtual DbSet<Signature> Signatures { get; set; }
		public virtual DbSet<SignatureType> SignatureTypes { get; set; }
		public virtual DbSet<SignOffItem> SignOffItems { get; set; }
		public virtual DbSet<SignOffObjectUser> SignOffObjectUsers { get; set; }
		public virtual DbSet<SignOffType> SignOffTypes { get; set; }
		public virtual DbSet<SignOffUser> SignOffUsers { get; set; }
		public virtual DbSet<SignOffUserPosting> SignOffUserPostings { get; set; }
		public virtual DbSet<SignOffValue> SignOffValues { get; set; }
		public virtual DbSet<State> States { get; set; }
		public virtual DbSet<SyncSetting> SyncSettings { get; set; }
		public virtual DbSet<SystemItem> SystemItems { get; set; }
		public virtual DbSet<SystemTemplate> SystemTemplates { get; set; }
		public virtual DbSet<Term> Terms { get; set; }
		public virtual DbSet<UDFCategoriesIcon> UDFCategoriesIcons { get; set; }
		public virtual DbSet<UserDefCategory> UserDefCategories { get; set; }
		public virtual DbSet<UserDefCategoryType> UserDefCategoryTypes { get; set; }
		public virtual DbSet<UserDefField> UserDefFields { get; set; }
		public virtual DbSet<UserDefGlobalField> UserDefGlobalFields { get; set; }
		public virtual DbSet<UserDefType> UserDefTypes { get; set; }
		public virtual DbSet<UserDefValue> UserDefValues { get; set; }
		public virtual DbSet<Version> Versions { get; set; }
		public virtual DbSet<ViolationSearchCriteriaType> ViolationSearchCriteriaTypes { get; set; }
		public virtual DbSet<ViolationType> ViolationTypes { get; set; }
		public virtual DbSet<Zip> Zips { get; set; }
		public virtual DbSet<zzPartyWebAccess> zzPartyWebAccess { get; set; }
		public virtual DbSet<zzHelp> zzHelp { get; set; }
		public virtual DbSet<v_Activities> v_Activities { get; set; }
		public virtual DbSet<v_Activities2> v_Activities2 { get; set; }
		public virtual DbSet<v_Activities3> v_Activities3 { get; set; }
		public virtual DbSet<v_ActivitiesRW> v_ActivitiesRW { get; set; }
		public virtual DbSet<v_ActivitiesTwo> v_ActivitiesTwo { get; set; }
		public virtual DbSet<v_ActivitiesUDF> v_ActivitiesUDF { get; set; }
		public virtual DbSet<v_Activity> v_Activity { get; set; }
		public virtual DbSet<v_ActivityChecklists> v_ActivityChecklists { get; set; }
		public virtual DbSet<v_ActivityComplaints> v_ActivityComplaints { get; set; }
		public virtual DbSet<v_ActivityPermits> v_ActivityPermits { get; set; }
		public virtual DbSet<v_AddLatLon> v_AddLatLon { get; set; }
		public virtual DbSet<v_Addresses> v_Addresses { get; set; }
		public virtual DbSet<v_Addresses2> v_Addresses2 { get; set; }
		public virtual DbSet<v_AddressesReport> v_AddressesReport { get; set; }
		public virtual DbSet<v_AddressesReportNA> v_AddressesReportNA { get; set; }
		public virtual DbSet<v_AddressParties> v_AddressParties { get; set; }
		public virtual DbSet<v_AddressPartyandRole> v_AddressPartyandRole { get; set; }
		public virtual DbSet<v_Agency> v_Agency { get; set; }
		public virtual DbSet<v_Agreements> v_Agreements { get; set; }
		public virtual DbSet<v_Alerts> v_Alerts { get; set; }
		public virtual DbSet<v_AlertsNR> v_AlertsNR { get; set; }
		public virtual DbSet<v_Assignments> v_Assignments { get; set; }
		public virtual DbSet<v_AssocActivities> v_AssocActivities { get; set; }
		public virtual DbSet<v_AssocActivities2> v_AssocActivities2 { get; set; }
		public virtual DbSet<v_Certifications> v_Certifications { get; set; }
		public virtual DbSet<v_CheckItemCalcValues> v_CheckItemCalcValues { get; set; }
		public virtual DbSet<v_CheckItemCalcValuesFirePump> v_CheckItemCalcValuesFirePump { get; set; }
		public virtual DbSet<v_CheckItemCalcValuesFirePumpDiesel> v_CheckItemCalcValuesFirePumpDiesel { get; set; }
		public virtual DbSet<v_CheckItemValues> v_CheckItemValues { get; set; }
		public virtual DbSet<v_CheckItemViolationTypes> v_CheckItemViolationTypes { get; set; }
		public virtual DbSet<v_CLInspDet> v_CLInspDet { get; set; }
		public virtual DbSet<v_ComplaintActivities> v_ComplaintActivities { get; set; }
		public virtual DbSet<v_ComplaintParties> v_ComplaintParties { get; set; }
		public virtual DbSet<v_ComplaintPermits> v_ComplaintPermits { get; set; }
		public virtual DbSet<v_Complaints> v_Complaints { get; set; }
		public virtual DbSet<v_Complaints_old> v_Complaints_old { get; set; }
		public virtual DbSet<v_Complaints1> v_Complaints1 { get; set; }
		public virtual DbSet<v_FailedSubCheckItems> v_FailedSubCheckItems { get; set; }
		public virtual DbSet<v_FeeBalanceSummary> v_FeeBalanceSummary { get; set; }
		public virtual DbSet<v_FeePayments> v_FeePayments { get; set; }
		public virtual DbSet<v_Fees> v_Fees { get; set; }
		public virtual DbSet<v_FeesPermits> v_FeesPermits { get; set; }
		public virtual DbSet<v_FeesRecalc> v_FeesRecalc { get; set; }
		public virtual DbSet<v_FeesReport> v_FeesReport { get; set; }
		public virtual DbSet<v_Files> v_Files { get; set; }
		public virtual DbSet<v_InspectionDetails> v_InspectionDetails { get; set; }
		public virtual DbSet<v_InspectionDetailsReports> v_InspectionDetailsReports { get; set; }
		public virtual DbSet<v_InspectionDetailsUDF> v_InspectionDetailsUDF { get; set; }
		public virtual DbSet<v_InventoryItems> v_InventoryItems { get; set; }
		public virtual DbSet<v_InvNarratives> v_InvNarratives { get; set; }
		public virtual DbSet<v_InvoicePayments> v_InvoicePayments { get; set; }
		public virtual DbSet<v_Invoices> v_Invoices { get; set; }
		public virtual DbSet<v_InvoiceSummary> v_InvoiceSummary { get; set; }
		public virtual DbSet<v_ItemGrid> v_ItemGrid { get; set; }
		public virtual DbSet<v_ItemGridAct> v_ItemGridAct { get; set; }
		public virtual DbSet<v_ItemInspectionStatus> v_ItemInspectionStatus { get; set; }
		public virtual DbSet<v_ItemList> v_ItemList { get; set; }
		public virtual DbSet<v_ItemList2> v_ItemList2 { get; set; }
		public virtual DbSet<v_Items> v_Items { get; set; }
		public virtual DbSet<v_LastActivityAtAddress> v_LastActivityAtAddress { get; set; }
		public virtual DbSet<v_LocationItemCount> v_LocationItemCount { get; set; }
		public virtual DbSet<v_Locations> v_Locations { get; set; }
		public virtual DbSet<v_LocationsCount> v_LocationsCount { get; set; }
		public virtual DbSet<v_Mileage> v_Mileage { get; set; }
		public virtual DbSet<v_ModuleAliases> v_ModuleAliases { get; set; }
		public virtual DbSet<v_OSInspectionDetailsReports> v_OSInspectionDetailsReports { get; set; }
		public virtual DbSet<v_OutstandingChecklists> v_OutstandingChecklists { get; set; }
		public virtual DbSet<v_Parties> v_Parties { get; set; }
		public virtual DbSet<v_PartyAddresses> v_PartyAddresses { get; set; }
		public virtual DbSet<v_PaymentSum> v_PaymentSum { get; set; }
		public virtual DbSet<v_PermitActivities> v_PermitActivities { get; set; }
		public virtual DbSet<v_PermitAlerts> v_PermitAlerts { get; set; }
		public virtual DbSet<v_Permits> v_Permits { get; set; }
		public virtual DbSet<v_PermitsReport> v_PermitsReport { get; set; }
		public virtual DbSet<v_PhoneList> v_PhoneList { get; set; }
		public virtual DbSet<v_Preplans> v_Preplans { get; set; }
		public virtual DbSet<v_ProjectActivitySearch> v_ProjectActivitySearch { get; set; }
		public virtual DbSet<v_ProjectAddressSearch> v_ProjectAddressSearch { get; set; }
		public virtual DbSet<v_ProjectAlerts> v_ProjectAlerts { get; set; }
		public virtual DbSet<v_ProjectInspectorSearch> v_ProjectInspectorSearch { get; set; }
		public virtual DbSet<v_ProjectPermitSearch> v_ProjectPermitSearch { get; set; }
		public virtual DbSet<v_ProjectReport> v_ProjectReport { get; set; }
		public virtual DbSet<v_ProjectRequestSearch> v_ProjectRequestSearch { get; set; }
		public virtual DbSet<v_Projects> v_Projects { get; set; }
		public virtual DbSet<v_ROInspectionDetailsReports> v_ROInspectionDetailsReports { get; set; }
		public virtual DbSet<v_SearchActivities> v_SearchActivities { get; set; }
		public virtual DbSet<v_SearchActivities2> v_SearchActivities2 { get; set; }
		public virtual DbSet<v_SearchAddresses> v_SearchAddresses { get; set; }
		public virtual DbSet<v_SearchChecklist> v_SearchChecklist { get; set; }
		public virtual DbSet<v_SearchComplaintUDFs> v_SearchComplaintUDFs { get; set; }
		public virtual DbSet<v_SearchInspectionDetails> v_SearchInspectionDetails { get; set; }
		public virtual DbSet<v_SearchInvoices> v_SearchInvoices { get; set; }
		public virtual DbSet<v_SearchItems> v_SearchItems { get; set; }
		public virtual DbSet<v_SearchItems2> v_SearchItems2 { get; set; }
		public virtual DbSet<v_SearchItemsByParty> v_SearchItemsByParty { get; set; }
		public virtual DbSet<v_SearchPartiesBC> v_SearchPartiesBC { get; set; }
		public virtual DbSet<v_SearchPermits> v_SearchPermits { get; set; }
		public virtual DbSet<v_SearchProjects> v_SearchProjects { get; set; }
		public virtual DbSet<v_ServiceHistory> v_ServiceHistory { get; set; }
		public virtual DbSet<v_Signature> v_Signature { get; set; }
		public virtual DbSet<v_SignOffCompletes> v_SignOffCompletes { get; set; }
		public virtual DbSet<v_SignOffItems> v_SignOffItems { get; set; }
		public virtual DbSet<v_SnapshotFeesReport> v_SnapshotFeesReport { get; set; }
		public virtual DbSet<v_SubCheckItemValues> v_SubCheckItemValues { get; set; }
		public virtual DbSet<v_SubInspection> v_SubInspection { get; set; }
		public virtual DbSet<v_SubInspectionList> v_SubInspectionList { get; set; }
		public virtual DbSet<v_SubInspectionsReport> v_SubInspectionsReport { get; set; }
		public virtual DbSet<v_UserDefValues> v_UserDefValues { get; set; }
		public virtual DbSet<v_ViolationSearch> v_ViolationSearch { get; set; }
		public virtual DbSet<cv_CPTKHotlist> cv_CPTKHotlist { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			try
			{
				modelBuilder.Entity<ActivityInventoryItem>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.SubTypeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.NumberLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.ReasonLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.ItemLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.PartyLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.AltPartyLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.InspectorLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.CompleteLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.StatusLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.ReLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.TabHides)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.CommentsLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.TabCheckList)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.TabFiles)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.TabChildAct)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.TabSig)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.SecondaryInspectorLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.GroupLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.PartyRoleLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.AltPartyRoleLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.SecAddressLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ActivitySetting>()
					.Property(e => e.TabInvNarr)
					.IsUnicode(false);

				modelBuilder.Entity<ActivityTypeGrid>()
					.Property(e => e.ColumnName)
					.IsUnicode(false);

				modelBuilder.Entity<ActivityTypeGrid>()
					.Property(e => e.ColumnType)
					.IsUnicode(false);

				modelBuilder.Entity<ActivityTypeGrid>()
					.Property(e => e.Externalid)
					.IsUnicode(false);

				modelBuilder.Entity<ActivityType>()
					.Property(e => e.ActivityType1)
					.IsUnicode(false);

				modelBuilder.Entity<ActivityType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<ActivityType>()
					.Property(e => e.ViolationAlias)
					.IsUnicode(false);

				modelBuilder.Entity<ActivityType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Address1)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<Address>()
					.Property(e => e.Schedule)
					.IsUnicode(false);

				modelBuilder.Entity<AddressMap>()
					.Property(e => e.Style)
					.IsUnicode(false);

				modelBuilder.Entity<AddressParty>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<AddressParty>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.AddCodeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.SubAddLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.RegionLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.CountyLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.OccTypeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.PropUseLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.CommentLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.TabHides)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.TabLocBases)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.MapLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.BlockLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.LotLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressSetting>()
					.Property(e => e.TaxParcelLabel)
					.IsUnicode(false);

				modelBuilder.Entity<AddressType>()
					.Property(e => e.AddressType1)
					.IsUnicode(false);

				modelBuilder.Entity<AddressType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Agency>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<Agreement>()
					.Property(e => e.AgreementDescription)
					.IsUnicode(false);

				modelBuilder.Entity<Agreement>()
					.Property(e => e.AgreementText)
					.IsUnicode(false);

				modelBuilder.Entity<Agreement>()
					.Property(e => e.ElevateFromPeriod)
					.IsUnicode(false);

				modelBuilder.Entity<Agreement>()
					.Property(e => e.FeeAmount)
					.HasPrecision(19, 4);

				modelBuilder.Entity<Agreement>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Agreement>()
					.Property(e => e.AgreementTotal)
					.HasPrecision(19, 4);

				modelBuilder.Entity<AgreementType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<AgreementType>()
					.Property(e => e.AgreementType1)
					.IsUnicode(false);

				modelBuilder.Entity<AgreementType>()
					.Property(e => e.DefaultFeeAmount)
					.HasPrecision(19, 4);

				modelBuilder.Entity<AgreementType>()
					.Property(e => e.DefaultText)
					.IsUnicode(false);

				modelBuilder.Entity<AgreementType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ApprovalStep>()
					.Property(e => e.ApprovalStep1)
					.IsUnicode(false);

				modelBuilder.Entity<ArchiveDB>()
					.Property(e => e.ArchiveName)
					.IsUnicode(false);

				modelBuilder.Entity<Assignment>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CalcType>()
					.Property(e => e.CalcType1)
					.IsUnicode(false);

				modelBuilder.Entity<CategoryType>()
					.Property(e => e.CategoryType1)
					.IsUnicode(false);

				modelBuilder.Entity<CategoryType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<CategoryType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Certification>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<Certification>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Certification>()
					.Property(e => e.CertNumber)
					.IsUnicode(false);

				modelBuilder.Entity<CertificationType>()
					.Property(e => e.CertificationType1)
					.IsUnicode(false);

				modelBuilder.Entity<CertificationType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<CertificationType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemCalc>()
					.Property(e => e.Label)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemCalc>()
					.Property(e => e.ConstantVal)
					.HasPrecision(18, 9);

				modelBuilder.Entity<CheckItemCalcVal>()
					.Property(e => e.Value)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItem>()
					.Property(e => e.CheckItem1)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItem>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItem>()
					.Property(e => e.DefaultValue)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItem>()
					.Property(e => e.FailValue)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemType>()
					.Property(e => e.CheckItemType1)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemValueInspectionDetail>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemValue>()
					.Property(e => e.TextValue)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemValue>()
					.Property(e => e.ResolutionText)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemValue>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CheckItemViolationType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CheckList>()
					.Property(e => e.CheckListName)
					.IsUnicode(false);

				modelBuilder.Entity<CheckList>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CheckList>()
					.Property(e => e.NFPAReport)
					.IsUnicode(false);

				modelBuilder.Entity<CheckListType>()
					.Property(e => e.CheckListType1)
					.IsUnicode(false);

				modelBuilder.Entity<CheckListType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CheckListValue>()
					.Property(e => e.CheckListValue1)
					.IsUnicode(false);

				modelBuilder.Entity<CheckListValue>()
					.Property(e => e.Externalid)
					.IsUnicode(false);

				modelBuilder.Entity<CodeVersion>()
					.Property(e => e.CodeVersion1)
					.IsUnicode(false);

				modelBuilder.Entity<CodeVersion>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<CodeVersion>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<Comment>()
					.Property(e => e.Comment1)
					.IsUnicode(false);

				modelBuilder.Entity<Complaint>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<Complaint>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Complaint>()
					.Property(e => e.ComplaintCode)
					.IsUnicode(false);

				modelBuilder.Entity<ComplaintStatu>()
					.Property(e => e.ComplaintStatus)
					.IsUnicode(false);

				modelBuilder.Entity<ComplaintStatu>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ComplaintType>()
					.Property(e => e.ComplaintType1)
					.IsUnicode(false);

				modelBuilder.Entity<ComplaintType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<ComplaintType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<County>()
					.Property(e => e.County1)
					.IsUnicode(false);

				modelBuilder.Entity<County>()
					.Property(e => e.CountyCode)
					.IsUnicode(false);

				modelBuilder.Entity<County>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Country>()
					.Property(e => e.Country1)
					.IsUnicode(false);

				modelBuilder.Entity<Country>()
					.Property(e => e.CountryCode)
					.IsUnicode(false);

				modelBuilder.Entity<Country>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<DefaultFee>()
					.Property(e => e.ReInspectionLetter)
					.IsUnicode(false);

				modelBuilder.Entity<DefaultFee>()
					.Property(e => e.EndReInspectionLetter)
					.IsUnicode(false);

				modelBuilder.Entity<DefaultFee>()
					.Property(e => e.FeeAmount)
					.IsUnicode(false);

				modelBuilder.Entity<FeePayment>()
					.Property(e => e.PaymentAmt)
					.HasPrecision(10, 2);

				modelBuilder.Entity<FeePayment>()
					.Property(e => e.PaymentType)
					.IsUnicode(false);

				modelBuilder.Entity<FeePayment>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<FeePayment>()
					.Property(e => e.ReceivedFrom)
					.IsUnicode(false);

				modelBuilder.Entity<FeePayment>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<Fee>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<Fee>()
					.Property(e => e.PaymentAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<Fee>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<Fee>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<Fee>()
					.Property(e => e.FeeBase)
					.HasPrecision(19, 4);

				modelBuilder.Entity<Fee>()
					.Property(e => e.Units)
					.HasPrecision(11, 3);

				modelBuilder.Entity<Fee>()
					.Property(e => e.FeeUOM)
					.IsUnicode(false);

				modelBuilder.Entity<Fee>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Fee>()
					.Property(e => e.FeeDesc)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSchedule>()
					.Property(e => e.FeeItem)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSchedule>()
					.Property(e => e.FeeRate)
					.HasPrecision(19, 4);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.FeeTypeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.FeeDateLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.FeeBaseLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.PerLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.CountLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.FeeAmtLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.PayDateLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.PayAmtLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.RefLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.RespPartyLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.CommentLabel)
					.IsUnicode(false);

				modelBuilder.Entity<FeeSetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<FeeTypePen>()
					.Property(e => e.InitialPenalty)
					.HasPrecision(18, 5);

				modelBuilder.Entity<FeeTypePen>()
					.Property(e => e.RatePer)
					.HasPrecision(18, 5);

				modelBuilder.Entity<FeeTypePT>()
					.Property(e => e.Percentage)
					.IsUnicode(false);

				modelBuilder.Entity<FeeTypeRR>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<FeeTypeRR>()
					.Property(e => e.AmountFrom)
					.HasPrecision(18, 5);

				modelBuilder.Entity<FeeTypeRR>()
					.Property(e => e.AmountTo)
					.HasPrecision(18, 5);

				modelBuilder.Entity<FeeTypeRR>()
					.Property(e => e.Base)
					.HasPrecision(18, 5);

				modelBuilder.Entity<FeeTypeRR>()
					.Property(e => e.AmountEvery)
					.HasPrecision(18, 5);

				modelBuilder.Entity<FeeTypeRR>()
					.Property(e => e.AmountPer)
					.HasPrecision(18, 5);

				modelBuilder.Entity<FeeTypeRR>()
					.Property(e => e.RatePer)
					.HasPrecision(18, 5);

				modelBuilder.Entity<FeeTypeRR>()
					.Property(e => e.QBInvoiceLineItemListID)
					.IsUnicode(false);

				modelBuilder.Entity<FeeType>()
					.Property(e => e.FeeType1)
					.IsUnicode(false);

				modelBuilder.Entity<FeeType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<FeeType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<FeeType>()
					.Property(e => e.QBInvoiceLineItemListID)
					.IsUnicode(false);

				modelBuilder.Entity<FeeType>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<FeeType>()
					.Property(e => e.FeeBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<File>()
					.Property(e => e.FileName)
					.IsUnicode(false);

				modelBuilder.Entity<File>()
					.Property(e => e.FileDesc)
					.IsUnicode(false);

				modelBuilder.Entity<File>()
					.Property(e => e.FilePath)
					.IsUnicode(false);

				modelBuilder.Entity<Group>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<Group>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<Group>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Group>()
					.Property(e => e.ActiveModules)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionCaus>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionCaus>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionDetail>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionDetail>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionDetail>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionDetail>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionDetail>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<Inspection>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<Inspection>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<Inspection>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Inspection>()
					.Property(e => e.ActivitySummary)
					.IsUnicode(false);

				modelBuilder.Entity<Inspection>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionType>()
					.Property(e => e.InspectionType1)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionType>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InspectionType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionType>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionType>()
					.Property(e => e.LegalFooter)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionType>()
					.Property(e => e.Recurrance)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<InspectionType>()
					.Property(e => e.ReFeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InspectorGroup>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.Login)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.Password)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.InspectorPhone)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.RCLevel)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.ActiveModules)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.SecQOne)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.SecAOne)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.SecQTwo)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.SecATwo)
					.IsUnicode(false);

				modelBuilder.Entity<Inspector>()
					.Property(e => e.Title)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PartNumber)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.InventoryItem1)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.MFGPartNumber)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.ModelNumber)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.BinLocation)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.StandardCost)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.AverageCost)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.ExchangeCost)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.UsedCost)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel1)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel2)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel3)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel4)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel5)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel6)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel7)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel8)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel9)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PriceLevel10)
					.HasPrecision(19, 4);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.SalesUOM)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItem>()
					.Property(e => e.PurchaseUOM)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItemType>()
					.Property(e => e.InvItemType)
					.IsUnicode(false);

				modelBuilder.Entity<InventoryItemType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<InvNarrative>()
					.Property(e => e.InvNarrativeName)
					.IsUnicode(false);

				modelBuilder.Entity<InvNarrativeText>()
					.Property(e => e.InvNarrativeText1)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.PaymentAmount)
					.HasPrecision(10, 2);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.PaymentType)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.Number)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.QBImportExport)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.ReceivedFrom)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.QBTransactionId)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.QBInvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.ReturnedTo)
					.IsUnicode(false);

				modelBuilder.Entity<InvoicePayment>()
					.Property(e => e.ReturnedNumber)
					.IsUnicode(false);

				modelBuilder.Entity<Invoice>()
					.Property(e => e.InvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<Invoice>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<Invoice>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Invoice>()
					.Property(e => e.QBTransactionID)
					.IsUnicode(false);

				modelBuilder.Entity<Invoice>()
					.Property(e => e.QBInvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<Invoice>()
					.Property(e => e.InvoiceBalance)
					.HasPrecision(10, 2);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.InvNumLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.InvTypeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.InvDateLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.BillToPartyLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.BillToAddLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.TermsLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.InvAmtLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.BalDueLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.TabLegDesc)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.TabHides)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.ServiceAddressLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceSetting>()
					.Property(e => e.DueDateLabel)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceType>()
					.Property(e => e.InvoiceType1)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceType>()
					.Property(e => e.Disclaimer)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceType>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceType>()
					.Property(e => e.LegalFooter)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceType>()
					.Property(e => e.MailToMethod)
					.IsUnicode(false);

				modelBuilder.Entity<InvoiceType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ItemInspectionStatu>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<ItemInspectionStatu>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Item>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<Item>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<Item>()
					.Property(e => e.Cost)
					.HasPrecision(10, 2);

				modelBuilder.Entity<Item>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<Item>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<Item>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.StatusLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.NumberLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.CostLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.DescriptionLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.InServiceLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.CommentLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.TabFiles)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.TabService)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.TabHides)
					.IsUnicode(false);

				modelBuilder.Entity<ItemSetting>()
					.Property(e => e.BarcodeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ItemsStatu>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<ItemsStatu>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ItemType>()
					.Property(e => e.ItemType1)
					.IsUnicode(false);

				modelBuilder.Entity<ItemType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<ItemType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<License>()
					.Property(e => e.Licensee)
					.IsUnicode(false);

				modelBuilder.Entity<License>()
					.Property(e => e.LicenseKey)
					.IsUnicode(false);

				modelBuilder.Entity<ListViewSave>()
					.Property(e => e.Form)
					.IsUnicode(false);

				modelBuilder.Entity<ListViewSave>()
					.Property(e => e.Tab)
					.IsUnicode(false);

				modelBuilder.Entity<LocationBas>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<LocationBas>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Location>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<Location>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<Location>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<Location>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Location>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<Location>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.DescLabel)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.LocBaseLabel)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.LocTypeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.BarcodeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.CommentLabel)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.LatLabel)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.LonLabel)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<LocationSetting>()
					.Property(e => e.TabHides)
					.IsUnicode(false);

				modelBuilder.Entity<LocationType>()
					.Property(e => e.LocationType1)
					.IsUnicode(false);

				modelBuilder.Entity<LocationType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<LocationType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Memorandum>()
					.Property(e => e.Memorandum1)
					.IsUnicode(false);

				modelBuilder.Entity<Memorandum>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<Memorandum>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Mileage>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<Mileage>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Module>()
					.Property(e => e.ModuleDesc)
					.IsUnicode(false);

				modelBuilder.Entity<Module>()
					.Property(e => e.ModuleAlias)
					.IsUnicode(false);

				modelBuilder.Entity<Note>()
					.Property(e => e.UserName)
					.IsUnicode(false);

				modelBuilder.Entity<Note>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<Note>()
					.Property(e => e.Note1)
					.IsUnicode(false);

				modelBuilder.Entity<Note>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Note>()
					.Property(e => e.ObjectRef)
					.IsUnicode(false);

				modelBuilder.Entity<NumberSchema>()
					.Property(e => e.Part1)
					.IsUnicode(false);

				modelBuilder.Entity<NumberSchema>()
					.Property(e => e.Part2)
					.IsUnicode(false);

				modelBuilder.Entity<NumberSchema>()
					.Property(e => e.Part3)
					.IsUnicode(false);

				modelBuilder.Entity<NumberSchema>()
					.Property(e => e.Part4)
					.IsUnicode(false);

				modelBuilder.Entity<NumberSchema>()
					.Property(e => e.Part5)
					.IsUnicode(false);

				modelBuilder.Entity<NumberSchema>()
					.Property(e => e.Part6)
					.IsUnicode(false);

				modelBuilder.Entity<NumberSchema>()
					.Property(e => e.CurrentNumber)
					.IsUnicode(false);

				modelBuilder.Entity<OccupancyType>()
					.Property(e => e.OccupancyType1)
					.IsUnicode(false);

				modelBuilder.Entity<OccupancyType>()
					.Property(e => e.OccupancyTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<OccupancyType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.Salutation)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.FirstName)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.MiddleInitial)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.LastName)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.Pager)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<Party>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<PartyPriceLevel>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<PartyRole>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<PermitCertificateType>()
					.Property(e => e.PermitCertificateType1)
					.IsUnicode(false);

				modelBuilder.Entity<PermitCertificateType>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<PermitCertificateType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<PermitCertificateType>()
					.Property(e => e.FooterText)
					.IsUnicode(false);

				modelBuilder.Entity<Permit>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<Permit>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<Permit>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Permit>()
					.Property(e => e.PropConst)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.IssuedToLabel)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.DurationLabel)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.CommentLabel)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.TabFiles)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.TabSig)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.TabHides)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.StatusLabel)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.EmergContLabel)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.CompleteLabel)
					.IsUnicode(false);

				modelBuilder.Entity<PermitSetting>()
					.Property(e => e.SubmittalLabel)
					.IsUnicode(false);

				modelBuilder.Entity<PermitStatu>()
					.Property(e => e.PermitStatus)
					.IsUnicode(false);

				modelBuilder.Entity<PermitStatu>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<PermitTypeActList>()
					.Property(e => e.ActListText)
					.IsUnicode(false);

				modelBuilder.Entity<PermitType>()
					.Property(e => e.PermitType1)
					.IsUnicode(false);

				modelBuilder.Entity<PermitType>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<PermitType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<PermitType>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<PermitType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Phone>()
					.Property(e => e.Phone1)
					.IsUnicode(false);

				modelBuilder.Entity<Phone>()
					.Property(e => e.Extension)
					.IsUnicode(false);

				modelBuilder.Entity<Phone>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<PhoneType>()
					.Property(e => e.PhoneType1)
					.IsUnicode(false);

				modelBuilder.Entity<PhoneType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectActivity>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectAddress>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectInspector>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectPermit>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectRequest>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Project>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<Project>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<Project>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Project>()
					.Property(e => e.ContractTotal)
					.HasPrecision(19, 4);

				modelBuilder.Entity<ProjectSetting>()
					.Property(e => e.ProjectNameLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectSetting>()
					.Property(e => e.ProjectTypeLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectSetting>()
					.Property(e => e.ProjectNumberLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectSetting>()
					.Property(e => e.StatusLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectSetting>()
					.Property(e => e.CompleteLabel)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectSetting>()
					.Property(e => e.TabFiles)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectSetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectSetting>()
					.Property(e => e.TabHides)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectStatu>()
					.Property(e => e.ProjectStatus)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectStatu>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectType>()
					.Property(e => e.ProjectType1)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectType>()
					.Property(e => e.Recurrance)
					.IsUnicode(false);

				modelBuilder.Entity<ProjectTypesDefault>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<PropertyUseType>()
					.Property(e => e.PropertyUseType1)
					.IsUnicode(false);

				modelBuilder.Entity<PropertyUseType>()
					.Property(e => e.PropertyUseTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<PropertyUseType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Region>()
					.Property(e => e.Region1)
					.IsUnicode(false);

				modelBuilder.Entity<Region>()
					.Property(e => e.RegionCode)
					.IsUnicode(false);

				modelBuilder.Entity<Region>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ReportDataDefinition>()
					.Property(e => e.ReportDataDef)
					.IsUnicode(false);

				modelBuilder.Entity<ReportDefinition>()
					.Property(e => e.ReportName)
					.IsUnicode(false);

				modelBuilder.Entity<ReportDefinition>()
					.Property(e => e.BaseReport)
					.IsUnicode(false);

				modelBuilder.Entity<ReportDefinition>()
					.Property(e => e.ReportLayout)
					.IsUnicode(false);

				modelBuilder.Entity<ReportGroup>()
					.Property(e => e.ReportGroup1)
					.IsUnicode(false);

				modelBuilder.Entity<ReportGroup>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Report>()
					.Property(e => e.ReportCode)
					.IsUnicode(false);

				modelBuilder.Entity<Report>()
					.Property(e => e.ReportTitle)
					.IsUnicode(false);

				modelBuilder.Entity<Report>()
					.Property(e => e.ReportDesc)
					.IsUnicode(false);

				modelBuilder.Entity<Report>()
					.Property(e => e.ReportFile)
					.IsUnicode(false);

				modelBuilder.Entity<Report>()
					.Property(e => e.Module)
					.IsUnicode(false);

				modelBuilder.Entity<Report>()
					.Property(e => e.TypeDef)
					.IsUnicode(false);

				modelBuilder.Entity<ReportSQL>()
					.Property(e => e.TableName)
					.IsUnicode(false);

				modelBuilder.Entity<ReportSQL>()
					.Property(e => e.SQLString)
					.IsUnicode(false);

				modelBuilder.Entity<ReportSQL>()
					.Property(e => e.KeyField)
					.IsUnicode(false);

				modelBuilder.Entity<ReportUserGroup>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ReportWizardDefinition>()
					.Property(e => e.ReportLayout)
					.IsUnicode(false);

				modelBuilder.Entity<RequestSetting>()
					.Property(e => e.CommentLabel)
					.IsUnicode(false);

				modelBuilder.Entity<RequestSetting>()
					.Property(e => e.StatusLabel)
					.IsUnicode(false);

				modelBuilder.Entity<RequestSetting>()
					.Property(e => e.TabFiles)
					.IsUnicode(false);

				modelBuilder.Entity<RequestSetting>()
					.Property(e => e.DetailHides)
					.IsUnicode(false);

				modelBuilder.Entity<RequestSetting>()
					.Property(e => e.TabHides)
					.IsUnicode(false);

				modelBuilder.Entity<Resolution>()
					.Property(e => e.Resolution1)
					.IsUnicode(false);

				modelBuilder.Entity<Resolution>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<RoleType>()
					.Property(e => e.RoleType1)
					.IsUnicode(false);

				modelBuilder.Entity<RoleType>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<RoleType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<RoutingSlip>()
					.Property(e => e.RoutingSlipName)
					.IsUnicode(false);

				modelBuilder.Entity<SearchDisplay>()
					.Property(e => e.FormName)
					.IsUnicode(false);

				modelBuilder.Entity<SearchDisplay>()
					.Property(e => e.SettingName)
					.IsUnicode(false);

				modelBuilder.Entity<SearchDisplay>()
					.Property(e => e.SettingValue)
					.IsUnicode(false);

				modelBuilder.Entity<ServiceHistory>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ServiceTypeActivityType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ServiceTypeItemType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ServiceType>()
					.Property(e => e.ServiceType1)
					.IsUnicode(false);

				modelBuilder.Entity<ServiceType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Setting>()
					.Property(e => e.PropertyField)
					.IsUnicode(false);

				modelBuilder.Entity<Setting>()
					.Property(e => e.ValueField)
					.IsUnicode(false);

				modelBuilder.Entity<Setting>()
					.Property(e => e.UserName)
					.IsUnicode(false);

				modelBuilder.Entity<Severity>()
					.Property(e => e.SeverityLevel)
					.IsUnicode(false);

				modelBuilder.Entity<Severity>()
					.Property(e => e.Message)
					.IsUnicode(false);

				modelBuilder.Entity<Severity>()
					.Property(e => e.Color)
					.IsUnicode(false);

				modelBuilder.Entity<Signature>()
					.Property(e => e.PrintedName)
					.IsUnicode(false);

				modelBuilder.Entity<SignatureType>()
					.Property(e => e.SignatureType1)
					.IsUnicode(false);

				modelBuilder.Entity<SignatureType>()
					.Property(e => e.ModuleId)
					.IsUnicode(false);

				modelBuilder.Entity<SignatureType>()
					.Property(e => e.SignatureLegalText)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffItem>()
					.Property(e => e.SignOffItemType)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffItem>()
					.Property(e => e.LabelText)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffItem>()
					.Property(e => e.Choices)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffItem>()
					.Property(e => e.DefaultAns)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffItem>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffObjectUser>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffType>()
					.Property(e => e.TabText)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffType>()
					.Property(e => e.EmailText)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffType>()
					.Property(e => e.ReEmailText)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffUser>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffUserPosting>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffValue>()
					.Property(e => e.SignOffValue1)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffValue>()
					.Property(e => e.SignOffCBValues)
					.IsUnicode(false);

				modelBuilder.Entity<SignOffValue>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<State>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<State>()
					.Property(e => e.State1)
					.IsUnicode(false);

				modelBuilder.Entity<State>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<SyncSetting>()
					.Property(e => e.Instance)
					.IsUnicode(false);

				modelBuilder.Entity<SyncSetting>()
					.Property(e => e.DB)
					.IsUnicode(false);

				modelBuilder.Entity<SyncSetting>()
					.Property(e => e.UID)
					.IsUnicode(false);

				modelBuilder.Entity<SyncSetting>()
					.Property(e => e.Pwd)
					.IsUnicode(false);

				modelBuilder.Entity<SyncSetting>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<SyncSetting>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<SyncSetting>()
					.Property(e => e.ContAddrType)
					.IsUnicode(false);

				modelBuilder.Entity<SystemItem>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<SystemItem>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<SystemTemplate>()
					.Property(e => e.SystemName)
					.IsUnicode(false);

				modelBuilder.Entity<SystemTemplate>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<Term>()
					.Property(e => e.Terms)
					.IsUnicode(false);

				modelBuilder.Entity<Term>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<Term>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefCategory>()
					.Property(e => e.Category)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefCategory>()
					.Property(e => e.AllAgency)
					.IsFixedLength();

				modelBuilder.Entity<UserDefCategory>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefCategoryType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefField>()
					.Property(e => e.FieldDesc)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefField>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefField>()
					.Property(e => e.DefaultValue)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefGlobalField>()
					.Property(e => e.GlobalFieldDesc)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefType>()
					.Property(e => e.UserDefType1)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefValue>()
					.Property(e => e.UserDefValue1)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefValue>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<UserDefValue>()
					.Property(e => e.CheckValues)
					.IsUnicode(false);

				modelBuilder.Entity<Version>()
					.Property(e => e.ProductName)
					.IsUnicode(false);

				modelBuilder.Entity<ViolationSearchCriteriaType>()
					.Property(e => e.ViolationSearchCriteria)
					.IsUnicode(false);

				modelBuilder.Entity<ViolationSearchCriteriaType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ViolationType>()
					.Property(e => e.ViolationType1)
					.IsUnicode(false);

				modelBuilder.Entity<ViolationType>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<ViolationType>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<ViolationType>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<Zip>()
					.Property(e => e.Zip1)
					.IsUnicode(false);

				modelBuilder.Entity<Zip>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities>()
					.Property(e => e.SecondaryInspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities2>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activities3>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesRW>()
					.Property(e => e.ApprovalStep)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesTwo>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.ActivityCategory)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivitiesUDF>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.PartyPhone)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.AddressExtId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.InspectorPhone)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.LegalFooter)
					.IsUnicode(false);

				modelBuilder.Entity<v_Activity>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityChecklists>()
					.Property(e => e.CheckListName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityComplaints>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityComplaints>()
					.Property(e => e.ComplaintType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityComplaints>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityComplaints>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityComplaints>()
					.Property(e => e.ComplaintCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityComplaints>()
					.Property(e => e.ComplaintStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityComplaints>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.FeeSum)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.PaymentSum)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ReleviedAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.BalanceDue)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Paid)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.PermitStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.FaxExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.PropertyUse)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ItemDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ItemBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ItemLocation)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ItemStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ItemLocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ActivityPermits>()
					.Property(e => e.ItemLocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddLatLon>()
					.Property(e => e.Latitude)
					.HasPrecision(12, 6);

				modelBuilder.Entity<v_AddLatLon>()
					.Property(e => e.Longitude)
					.HasPrecision(12, 6);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.RegionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.CountyCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Country)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.OccupancyTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.PropertyUseTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.FieldDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.UserDefValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses>()
					.Property(e => e.Schedule)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.RegionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.OccupancyTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.CountyCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.PropertyUseTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Country)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<v_Addresses2>()
					.Property(e => e.Schedule)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.RegionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.OccupancyTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.CountyCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.PropertyUseTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReport>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.RegionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.CountyCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Country)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.OccupancyTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.PropertyUseTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.FieldDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.UserDefValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.AddNumSort)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressesReportNA>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.FaxExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Pager)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.PagerExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressParties>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressPartyandRole>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AddressPartyandRole>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.AgCity)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agency>()
					.Property(e => e.AgZip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.AgreementType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.AgreementDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.AgreementText)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.ElevateFromPeriod)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.FeeAmount)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.AgreementTotal)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Agreements>()
					.Property(e => e.ProjectStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Alerts>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AlertsNR>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Assignments>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.SecondaryInspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.SecondaryInspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_AssocActivities2>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Certifications>()
					.Property(e => e.CertificationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Certifications>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_Certifications>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Certifications>()
					.Property(e => e.CertNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Certifications>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.CheckListName)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.NFPAReport)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.CheckItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.ResolutionText)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.CheckItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.Value)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.Label)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValues>()
					.Property(e => e.CheckItemCalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point0FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point0FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point0SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point0NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point0RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point0L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point0L2)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point0L3)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint0FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint0NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point1FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point1FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point1SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point1NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point1RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point1L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point1L2)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point1L3)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint1FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint1NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point2FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point2FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point2SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point2NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point2RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point2L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point2L2)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point2L3)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint2FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint2NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point3FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point3FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point3SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point3NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point3RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point3L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point3L2)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point3L3)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint3FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint3NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point4FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point4FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point4SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point4NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point4RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point4L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point4L2)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point4L3)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint4FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint4NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point5FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point5FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point5SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point5NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point5RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point5L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point5L2)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point5L3)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint5FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint5NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point6FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point6FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point6SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point6NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point6RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point6L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point6L2)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point6L3)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint6FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint6NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point7FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point7FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point7SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point7NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point7RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point7L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point7L2)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.Point7L3)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint7FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePump>()
					.Property(e => e.PrevPoint7NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point0FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point0FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point0SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point0NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point0RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point0L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint0FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint0NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point1FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point1FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point1SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point1NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point1RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point1L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint1FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint1NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point2FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point2FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point2SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point2NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point2RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point2L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint2FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint2NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point3FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point3FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point3SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point3NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point3RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point3L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint3FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint3NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point4FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point4FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point4SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point4NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point4RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point4L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint4FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint4NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point5FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point5FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point5SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point5NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point5RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point5L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint5FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint5NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point6FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point6FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point6SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point6NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point6RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point6L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint6FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint6NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point7FirePumpFlowsGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point7FirePumpOutletpsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point7SuctionPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point7NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point7RPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.Point7L1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint7FlowGPM)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemCalcValuesFirePumpDiesel>()
					.Property(e => e.PrevPoint7NetPressurepsi)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.CheckListName)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.NFPAReport)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.CheckItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.ResolutionText)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.CheckItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.Value)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.FailValue1)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemValues>()
					.Property(e => e.FailValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemViolationTypes>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemViolationTypes>()
					.Property(e => e.CategoryType)
					.IsUnicode(false);

				modelBuilder.Entity<v_CheckItemViolationTypes>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_CLInspDet>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_CLInspDet>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_CLInspDet>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_CLInspDet>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<v_CLInspDet>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintActivities>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.FaxExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.Pager)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.PagerExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintParties>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.PermitStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ComplaintPermits>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.ComplaintType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.ComplaintCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.Country)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.ComplaintStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints_old>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints_old>()
					.Property(e => e.ComplaintType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints_old>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints_old>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints_old>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints_old>()
					.Property(e => e.ComplaintCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.ComplaintType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.ComplaintCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.Country)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.ComplaintStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.Expr1)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Complaints1>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_FailedSubCheckItems>()
					.Property(e => e.CheckItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_FailedSubCheckItems>()
					.Property(e => e.ResolutionText)
					.IsUnicode(false);

				modelBuilder.Entity<v_FailedSubCheckItems>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_FailedSubCheckItems>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeeBalanceSummary>()
					.Property(e => e.FeeSum)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeeBalanceSummary>()
					.Property(e => e.PaymentSum)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_FeeBalanceSummary>()
					.Property(e => e.ReleviedAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeeBalanceSummary>()
					.Property(e => e.BalanceDue)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_FeeBalanceSummary>()
					.Property(e => e.Paid)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeePayments>()
					.Property(e => e.PaymentAmt)
					.HasPrecision(10, 2);

				modelBuilder.Entity<v_FeePayments>()
					.Property(e => e.PaymentType)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeePayments>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeePayments>()
					.Property(e => e.ReceivedFrom)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeePayments>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.PaymentAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.BalanceDue)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.FeeType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.FeeBase)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.Units)
					.HasPrecision(11, 3);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.FeeUOM)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.RespParty)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.FeeDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.InvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.FeeBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.InventoryItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.InvItemBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Fees>()
					.Property(e => e.UserName)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.PaymentAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.BalanceDue)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.FeeType)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.FeeBase)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.Units)
					.HasPrecision(11, 3);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.FeeUOM)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.RespParty)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.FeeDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.InvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesPermits>()
					.Property(e => e.FeeBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.PaymentAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.BalanceDue)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.FeeType)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.FeeBase)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.Units)
					.HasPrecision(11, 3);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.FeeUOM)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.RespParty)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.FeeDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.InvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.FeeBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.InventoryItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesRecalc>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.PaymentAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.BalanceDue)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.FeeType)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.FeeBase)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.Units)
					.HasPrecision(11, 3);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.FeeUOM)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.RespParty)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.FeeDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.InvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_FeesReport>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Files>()
					.Property(e => e.FileName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Files>()
					.Property(e => e.FileDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_Files>()
					.Property(e => e.FilePath)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.CategoryType)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.InspectionTypeIdstr)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.CheckItemValueIdstr)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.CodeVersionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.CategoryTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.ViolationTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetails>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.CategoryType)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.SeverityLevel)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.Message)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.Color)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.CodeVersionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.CategoryTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.ViolationTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.CheckItemValueIdstr)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.InspectionTypeIdstr)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsReports>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.CategoryType)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_InspectionDetailsUDF>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PartNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.InventoryItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.MFGPartNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.ModelNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.BinLocation)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.StandardCost)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.AverageCost)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.ExchangeCost)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.UsedCost)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel1)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel2)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel3)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel4)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel5)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel6)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel7)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel8)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel9)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PriceLevel10)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.InvItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.Manufacturer)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.SalesUOM)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.PurchaseUOM)
					.IsUnicode(false);

				modelBuilder.Entity<v_InventoryItems>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvNarratives>()
					.Property(e => e.InvNarrativeName)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvNarratives>()
					.Property(e => e.InvNarrativeText)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.PaymentAmount)
					.HasPrecision(10, 2);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.PaymentType)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.Number)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.QBImportExport)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.ReceivedFrom)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.QBTransactionId)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.QBInvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoicePayments>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.InvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.QBTransactionID)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.QBInvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.InvoiceBalance)
					.HasPrecision(10, 2);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.InvoiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Invoices>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoiceSummary>()
					.Property(e => e.FeeSum)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_InvoiceSummary>()
					.Property(e => e.PaymentSum)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_InvoiceSummary>()
					.Property(e => e.QBPaymentSum)
					.HasPrecision(38, 4);

				modelBuilder.Entity<v_InvoiceSummary>()
					.Property(e => e.BalanceDue)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_InvoiceSummary>()
					.Property(e => e.Paid)
					.IsUnicode(false);

				modelBuilder.Entity<v_InvoiceSummary>()
					.Property(e => e.QBPaid)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.Cost)
					.HasPrecision(10, 2);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.ActivityCategory)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.SecAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGrid>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGridAct>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGridAct>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGridAct>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGridAct>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_ItemGridAct>()
					.Property(e => e.ActivityCategory)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemGridAct>()
					.Property(e => e.SecAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemInspectionStatus>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemInspectionStatus>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.LocationBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.LocationBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ItemList2>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.Cost)
					.HasPrecision(10, 2);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Items>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_LastActivityAtAddress>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_LocationItemCount>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_LocationItemCount>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_LocationItemCount>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_LocationItemCount>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_LocationItemCount>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_LocationItemCount>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_Locations>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_LocationsCount>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_LocationsCount>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Mileage>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Mileage>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Mileage>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ModuleAliases>()
					.Property(e => e.ModuleDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_ModuleAliases>()
					.Property(e => e.ModuleAlias)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.CategoryType)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.SeverityLevel)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.Message)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.Color)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.CodeVersionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.CategoryTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.ViolationTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.CheckItemValueIdstr)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.InspectionTypeIdstr)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_OSInspectionDetailsReports>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.CheckListName)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.CheckItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.CheckItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.Value)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.ResolutionText)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.SeverityLevel)
					.IsUnicode(false);

				modelBuilder.Entity<v_OutstandingChecklists>()
					.Property(e => e.Message)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.FaxExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.Pager)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.PagerExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.Salutation)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.FirstName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.MiddleInitial)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.LastName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Parties>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.FaxExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Pager)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.PagerExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<v_PartyAddresses>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_PaymentSum>()
					.Property(e => e.PaymentSum)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_PaymentSum>()
					.Property(e => e.ReleviedAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.Fees)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitActivities>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.FeeType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.ItemDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.ItemBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.ItemLocation)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.ItemStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.ItemLocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitAlerts>()
					.Property(e => e.ItemLocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.PropConst)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.PermitStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ActListText)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ItemDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ItemBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ItemLocation)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ItemStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ItemLocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_Permits>()
					.Property(e => e.ItemLocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.PropConst)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.PermitStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.ActListText)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_PermitsReport>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_PhoneList>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_PhoneList>()
					.Property(e => e.Extension)
					.IsUnicode(false);

				modelBuilder.Entity<v_PhoneList>()
					.Property(e => e.PhoneType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_Preplans>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.ProjectType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.ProjectStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectActivitySearch>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAddressSearch>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAlerts>()
					.Property(e => e.ProjectType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAlerts>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAlerts>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAlerts>()
					.Property(e => e.ProjectStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectAlerts>()
					.Property(e => e.Recurrance)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectInspectorSearch>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectInspectorSearch>()
					.Property(e => e.InspectorPhone)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectInspectorSearch>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectPermitSearch>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectPermitSearch>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectPermitSearch>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectPermitSearch>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectPermitSearch>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectPermitSearch>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.ContractTotal)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.CheckItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.CheckListName)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.CheckItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.Value)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.ResolutionText)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.ViolationAlias)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.SevereC)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectReport>()
					.Property(e => e.SevereMessage)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectRequestSearch>()
					.Property(e => e.ComplaintType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectRequestSearch>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ProjectRequestSearch>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Projects>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Projects>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_Projects>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Projects>()
					.Property(e => e.ProjectType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Projects>()
					.Property(e => e.ProjectStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.CategoryType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.SeverityLevel)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.Message)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.Color)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.CodeVersionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.CategoryTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.ViolationTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.CheckItemValueIdstr)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.InspectionTypeIdstr)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_ROInspectionDetailsReports>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.SecondaryInspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.FeeSum)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.PaymentSum)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.ReleviedAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.BalanceDue)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.AltPartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.SecondaryInspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.FeeSum)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.PaymentSum)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.ReleviedAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.BalanceDue)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchActivities2>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.RegionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.CountyCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Country)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.OccupancyTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.PropertyUseTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchAddresses>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchChecklist>()
					.Property(e => e.CheckListName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchChecklist>()
					.Property(e => e.CheckItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchChecklist>()
					.Property(e => e.TextValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchChecklist>()
					.Property(e => e.ResolutionText)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchChecklist>()
					.Property(e => e.FailValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.ComplaintType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchComplaintUDFs>()
					.Property(e => e.Country)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ActivityCategory)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.InspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.CategoryType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.InspectionCause)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.CorrectedComments)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.CorrectiveAction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.CodeVersionCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.CategoryTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ViolationTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ExternalValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Country)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.AddressType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Map)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Block)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Lot)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.TaxParcel)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ItemDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ItemStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ItemServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ItemLocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ItemLocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ItemLocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.SecondaryInspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInspectionDetails>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.InvoiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.InvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.Terms)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.Disclaimer)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.LegalFooter)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.OriginalLegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.MailToMethod)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.FeeSum)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.PaymentSum)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.BalanceDue)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.Paid)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.QBPaymentSum)
					.HasPrecision(38, 4);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.QBPaid)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.QBInvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.QBTransactionID)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.InvoiceBalance)
					.HasPrecision(10, 2);

				modelBuilder.Entity<v_SearchInvoices>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.LocationBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.ItemTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.LocationBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.ItemTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItems2>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Status)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Comments)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.LocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.LocationBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.PropertyUseType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.Region)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.ItemTypeCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchItemsByParty>()
					.Property(e => e.AgencySubName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Email)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.AgencyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.CertificationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Code)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.CertNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Issuer)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.FaxExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Pager)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.PagerExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Salutation)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.FirstName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.MiddleInitial)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.LastName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPartiesBC>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.State)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.LegalDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.FeeSum)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.PaymentSum)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ReleviedAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.BalanceDue)
					.HasPrecision(38, 2);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Paid)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.RoleType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.InspectorName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.GroupName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.PermitStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Phone)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.PhoneExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Fax)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.FaxExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Cell)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.CellExt)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.OccupancyType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.PropertyUse)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Latitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.Longitude)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ItemDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ItemBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ItemLocation)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ItemStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ItemLocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchPermits>()
					.Property(e => e.ItemLocationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchProjects>()
					.Property(e => e.ProjectName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchProjects>()
					.Property(e => e.ProjectType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchProjects>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SearchProjects>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ServiceHistory>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ServiceHistory>()
					.Property(e => e.ExternalId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Signature>()
					.Property(e => e.PrintedName)
					.IsUnicode(false);

				modelBuilder.Entity<v_Signature>()
					.Property(e => e.SignatureType)
					.IsUnicode(false);

				modelBuilder.Entity<v_Signature>()
					.Property(e => e.ModuleId)
					.IsUnicode(false);

				modelBuilder.Entity<v_Signature>()
					.Property(e => e.SignatureLegalText)
					.IsUnicode(false);

				modelBuilder.Entity<v_SignOffItems>()
					.Property(e => e.SignOffItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SignOffItems>()
					.Property(e => e.LabelText)
					.IsUnicode(false);

				modelBuilder.Entity<v_SignOffItems>()
					.Property(e => e.Choices)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.FeeAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.PaymentAmt)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.BalanceDue)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.Comment)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.Hrs)
					.HasPrecision(8, 4);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.PermitNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.PartyName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.FeeType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.FeeBase)
					.HasPrecision(19, 4);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.Units)
					.HasPrecision(11, 3);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.FeeUOM)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.PermitType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.RespParty)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.FeeDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.InvoiceNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.ProjectNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.AddressCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<v_SnapshotFeesReport>()
					.Property(e => e.Suffix)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubCheckItemValues>()
					.Property(e => e.CheckListName)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubCheckItemValues>()
					.Property(e => e.CheckItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubCheckItemValues>()
					.Property(e => e.CheckItem)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubCheckItemValues>()
					.Property(e => e.ResolutionText)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubCheckItemValues>()
					.Property(e => e.Value)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubCheckItemValues>()
					.Property(e => e.FailValue1)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.ItemDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspection>()
					.Property(e => e.UserDefValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.InspectionNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.Description)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.Barcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.LocationBarcode)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.LocationBase)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionList>()
					.Property(e => e.Location)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionsReport>()
					.Property(e => e.ItemType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionsReport>()
					.Property(e => e.ItemDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionsReport>()
					.Property(e => e.LocationDescription)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionsReport>()
					.Property(e => e.ItemInspectionStatus)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionsReport>()
					.Property(e => e.ActivityType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionsReport>()
					.Property(e => e.ItemNumber)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionsReport>()
					.Property(e => e.ServiceType)
					.IsUnicode(false);

				modelBuilder.Entity<v_SubInspectionsReport>()
					.Property(e => e.InspectionType)
					.IsUnicode(false);

				modelBuilder.Entity<v_UserDefValues>()
					.Property(e => e.FieldDesc)
					.IsUnicode(false);

				modelBuilder.Entity<v_UserDefValues>()
					.Property(e => e.UserDefValue)
					.IsUnicode(false);

				modelBuilder.Entity<v_UserDefValues>()
					.Property(e => e.UserDefType)
					.IsUnicode(false);

				modelBuilder.Entity<v_UserDefValues>()
					.Property(e => e.Category)
					.IsUnicode(false);

				modelBuilder.Entity<v_UserDefValues>()
					.Property(e => e.AllAgency)
					.IsUnicode(true);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.ViolationType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.RefNum)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.VioExtId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.VioCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.CategoryType)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.CatCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.CatExtId)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.CodeVersion)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.BookCode)
					.IsUnicode(false);

				modelBuilder.Entity<v_ViolationSearch>()
					.Property(e => e.BookExtId)
					.IsUnicode(false);

				modelBuilder.Entity<cv_CPTKHotlist>()
					.Property(e => e.FullAddress)
					.IsUnicode(false);

				modelBuilder.Entity<cv_CPTKHotlist>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<cv_CPTKHotlist>()
					.Property(e => e.StateAbbr)
					.IsUnicode(false);

				modelBuilder.Entity<cv_CPTKHotlist>()
					.Property(e => e.Zip)
					.IsUnicode(false);

				modelBuilder.Entity<FYAllowableDist>()
					.Property(e => e.CalculationPer)
					.IsUnicode(false);

				modelBuilder.Entity<FYAllowableDist>()
					.Property(item => item.FYDistributionFactor)
					.HasPrecision(18, 15);



				modelBuilder.Entity<nm_FYTotalDistribution>()
				.Property(e => e.AddressCode)
				.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistribution>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistribution>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistribution>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistribution>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistribution>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistribution>()
					.Property(e => e.County)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistributionCalc>()
					.Property(e => e.AddressNumber)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistributionCalc>()
					.Property(e => e.Direction)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistributionCalc>()
					.Property(e => e.Address)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistributionCalc>()
					.Property(e => e.SubAddress)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistributionCalc>()
					.Property(e => e.City)
					.IsUnicode(false);

				modelBuilder.Entity<nm_FYTotalDistributionCalc>()
					.Property(e => e.County)
					.IsUnicode(false);

			}
			catch (Exception up)
			{

				throw up;
			}
		}
	}
}
