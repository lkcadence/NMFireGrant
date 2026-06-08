using NMSFM.Data;
using NMSFM.Services.Address;
using NMSFM.Services.Logging;
using NMSFM.Services.Party;
using NMSFM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NMSFM.Services.FireGrant
{
    public class FGApplicationService : IFGApplicationServices
    {
        private ICodepalWebModel cwmContext;
        private ILogging logger;
        private IAddressService addressService;

		Guid deptAddType = new Guid("43856752-8b7a-4e6f-b697-bf8acd457c16");

		public FGApplicationService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
        {
            cwmContext = codepalWebModel;
            logger = codepalLogger;
            addressService = new AddressService(cwmContext, logger);
        }

		public async Task<bool> CreateNewApplication(FGApplications model)
		{
			try
			{
				if (model != null)
				{
					var application = await cwmContext.FGApplications.SingleOrDefaultAsync(a => a.FiscalYear == model.FiscalYear && a.AddressId == model.AddressId);

					if (application != null)
					{
						return false;
					}

					application = cwmContext.FGApplications.Add(new NMSFM.Data.FGApplications());
					application.ApplicationId = model.ApplicationId;
					application.FiscalYear = model.FiscalYear;
					application.AddressId = model.AddressId;
					application.ApplicationNumber = await GetNextApplicationNumber(model.FiscalYear);
					application.DateStarted = DateTime.Now;
					application.DateSubmitted = null;
					application.AppStatus = 6;
					application.LastStatusChange = DateTime.Now;
					application.InstructionsSubmitted = model.InstructionsSubmitted;


					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							return true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to update settings '" + model.ApplicationId.ToString() + "'.", ex);
							return false;
						}
					}
					else
					{
						logger.Error("Unable to update settings '" + model.ApplicationId.ToString() + "', DbContext was not available.");
						return false;
					}
				}
				else
				{
					throw new Exception("Unable to save null application");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<bool> UpdateApplication(FGApplications model)
		{
			try
			{
				if (model != null)
				{
					var application = await cwmContext.FGApplications.SingleOrDefaultAsync(a => a.FiscalYear == model.FiscalYear && a.AddressId == model.AddressId);

					if (application == null)
					{
						return false;
					}

					if (model.DateSubmitted != null)
                    {
						application.DateSubmitted = model.DateSubmitted;
					}
					if (model.AppStatus > 0)
                    {
						if (application.AppStatus != model.AppStatus)
						{
							if (model.AppStatus == 1)
							{
								application.ApprovedBy = model.ApprovedBy;
								application.ApprovedDate = DateTime.Now;
							}
						}
						application.AppStatus = model.AppStatus;
					}
					if (model.LastStatusChange != null)
                    {
						application.LastStatusChange = model.LastStatusChange;
					}
					if (model.ApplicationNotes != null)
                    {
						application.ApplicationNotes = model.ApplicationNotes;
                    }
					application.GrantedAmount = model.GrantedAmount;
					application.StipendAmount = model.StipendAmount;
					if (model.SubmittedBy != null)
                    {
						application.SubmittedBy = model.SubmittedBy;
                    }
					

					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							return true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to update settings '" + model.ApplicationId.ToString() + "'.", ex);
							return false;
						}
					}
					else
					{
						logger.Error("Unable to update settings '" + model.ApplicationId.ToString() + "', DbContext was not available.");
						return false;
					}
				}
				else
				{
					throw new Exception("Unable to save null application");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<nm_FGApplication> GetFGApplicationAsync(Guid addressId, short FiscalYear)
		{
			nm_FGApplication result = null;
			try
			{
				result = await cwmContext.nm_FGApplications.SingleOrDefaultAsync(a => a.AddressId == addressId && a.FiscalYear == FiscalYear);
			}
			catch (Exception ex)
            {
                _ = ex;

			}

			return result;
		}

		public async Task<DetailedFGAppValidation> GetDetailedFGApplicationValidationAsync(Guid applicationId)
		{
			DetailedFGAppValidation result = new DetailedFGAppValidation();
			try
			{
				nm_FGApplication app = await cwmContext.nm_FGApplications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (app != null)
                {
					result.ApplicationId = app.ApplicationId;
					result.FiscalYear = app.FiscalYear;
					result.ApplicationNumber = app.ApplicationNumber;
					result.AppStatus = app.AppStatus;
					result.ApplicationStatus = app.ApplicationStatus;
					result.LastStatusChange = app.LastStatusChange;
					result.InstructionsSubmitted = app.InstructionsSubmitted;

					FG_App_GeneralInfo genInfo = await cwmContext.FG_App_GeneralInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (genInfo != null)
                    {
						result.GeneralInfoValid = genInfo.IsValid;
                    }
					else
                    {
						result.GeneralInfoValid = false;
                    }

					FG_App_BudgetInfo budgetInfo = await cwmContext.FG_App_BudgetInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (budgetInfo != null)
					{
						result.BudgetInfoValid = budgetInfo.IsValid;
					}
					else
					{
						result.BudgetInfoValid = false;
					}

					FG_App_CommunityInfo communityInfo = await cwmContext.FG_App_CommunityInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (communityInfo != null)
					{
						result.CommunityInfoValid = communityInfo.IsValid;
					}
					else
					{
						result.CommunityInfoValid = false;
					}

					FG_App_ResponseHistory responseHistory = await cwmContext.FG_App_ResponseHistories.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (responseHistory != null)
					{
						result.ResponseHistoryValid = responseHistory.IsValid;
					}
					else
					{
						result.ResponseHistoryValid = false;
					}

					FG_App_WaterAvailability waterAvailability = await cwmContext.FG_App_WaterAvailabilities.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (waterAvailability != null)
					{
						result.WaterAvailabilityValid = waterAvailability.IsValid;
					}
					else
					{
						result.WaterAvailabilityValid = false;
					}

					FG_App_Training trainingAvailability = await cwmContext.FG_App_Trainings.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (trainingAvailability != null)
					{
						result.TrainingValid = trainingAvailability.IsValid;
					}
					else
					{
						result.TrainingValid = false;
					}

					FG_App_Apparatus apparatus = await cwmContext.FG_App_Apparatuses.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (apparatus != null)
					{
						result.ApparatusValid = apparatus.IsValid;
					}
					else
					{
						result.ApparatusValid = false;
					}

					FG_App_Communication communication = await cwmContext.FG_App_Communications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (communication != null)
					{
						result.CommunicationEquipValid = communication.IsValid;
					}
					else
					{
						result.CommunicationEquipValid = false;
					}

					FG_App_HazardsThreats hazards = await cwmContext.FG_App_HazardsThreats.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (hazards != null)
					{
						result.HazardsThreatsValid = hazards.IsValid;
					}
					else
					{
						result.HazardsThreatsValid = false;
					}

					FG_App_PPE ppe = await cwmContext.FG_App_PPEs.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (ppe != null)
					{
						result.PPEValid = ppe.IsValid;
					}
					else
					{
						result.PPEValid = false;
					}

					FG_App_EquipmentNeeds equipmentNeeds = await cwmContext.FG_App_EquipmentNeeds.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (equipmentNeeds != null)
					{
						result.EquipmentNeedsValid = equipmentNeeds.IsValid;
					}
					else
					{
						result.EquipmentNeedsValid = false;
					}

					FG_App_FundingJustification grantFunding = await cwmContext.FG_App_FundingJustifications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (grantFunding != null)
					{
						result.GrantFundingJustificationValid = grantFunding.IsValid;
					}
					else
					{
						result.GrantFundingJustificationValid = false;
					}

					FG_App_ProjectBudget projectBudget = await cwmContext.FG_App_ProjectBudgets.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (projectBudget != null)
					{
						result.ProjectBudgetValid = projectBudget.IsValid;
					}
					else
					{
						result.ProjectBudgetValid = false;
					}

					FG_App_DocsSigs docsSigs = await cwmContext.FG_App_DocsSigs.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (docsSigs != null)
					{
						result.DocsSigsValid = docsSigs.IsValid;
					}
					else
					{
						result.DocsSigsValid = false;
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				return null;
			}

			return result;
		}

		public async Task<DetailedFGAppScores> GetDetailedFGAppScoresAsync(Guid applicationId)
		{
			DetailedFGAppScores result = new DetailedFGAppScores();
			try
			{
				nm_FGApplication app = await cwmContext.nm_FGApplications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (app != null)
				{
					result.ApplicationId = new Guid(app.ApplicationId.ToString());
					int totalScore = 0;
					FG_App_Training trainingAvailability = await cwmContext.FG_App_Trainings.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					
					if (trainingAvailability != null)
					{
						result.TrainingPoints = trainingAvailability.TrainingPoints;
						totalScore += trainingAvailability.TrainingPoints;
					}
					else
					{
						result.TrainingPoints = 0;
					}
					FG_App_GeneralInfo genInfo = await cwmContext.FG_App_GeneralInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (genInfo != null)
                    {
						result.ISORating = (genInfo.ISORating != null) ? Convert.ToInt32(genInfo.ISORating) : 0;
						totalScore += (genInfo.ISORating != null) ? Convert.ToInt32(genInfo.ISORating) : 0;
                    }
                    else
                    {
						result.ISORating = 0;
                    }
					FG_App_FundingJustification grantFunding = await cwmContext.FG_App_FundingJustifications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (grantFunding != null)
					{
						result.FinancialNeedGrade = grantFunding.FinancialNeedGrade;
						totalScore += grantFunding.FinancialNeedGrade;
						result.ProblemGrade = grantFunding.ProblemGrade;
						totalScore += grantFunding.ProblemGrade;
						result.BenefitGrade = grantFunding.BenefitGrade;
						totalScore += grantFunding.BenefitGrade;
						result.ConsequencesGrade = grantFunding.ConsequencesGrade;
						totalScore += grantFunding.ConsequencesGrade;
					}
					else
					{
						result.FinancialNeedGrade = 0;
						result.ProblemGrade = 0;
						result.BenefitGrade = 0;
						result.ConsequencesGrade = 0;
					}

					FG_App_DocsSigs docsSigs = await cwmContext.FG_App_DocsSigs.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (docsSigs != null)
					{
						result.AppCompletenessGrade = docsSigs.AppCompletenessGrade;
						totalScore += docsSigs.AppCompletenessGrade;
					}
					else
					{
						result.AppCompletenessGrade = 0;
					}
					result.TotalScore = totalScore;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				return null;
			}

			return result;
		}

		public async Task<DetailedFGAppScores> GetDetailedFGAppScoresCounselorAsync(Guid applicationId, Guid webUserId)
		{
			DetailedFGAppScores result = new DetailedFGAppScores();
			try
			{
				nm_FGApplication app = await cwmContext.nm_FGApplications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (app != null)
				{
					result.ApplicationId = new Guid(app.ApplicationId.ToString());
					int totalScore = 0;
					FG_App_Scores appScores = await cwmContext.FG_App_Scores.SingleOrDefaultAsync(a => a.ApplicationId == applicationId && a.WebUserId == webUserId);

					if (appScores != null)
					{
						result.TrainingPoints = appScores.TrainingScore;
						totalScore += appScores.TrainingScore;
						result.FinancialNeedGrade = appScores.FinancialNeedScore;
						totalScore += appScores.FinancialNeedScore;
						result.ProblemGrade = appScores.ProblemScore;
						totalScore += appScores.ProblemScore;
						result.BenefitGrade = appScores.BenefitScore;
						totalScore += appScores.BenefitScore;
						result.ConsequencesGrade = appScores.ConsequencesScore;
						totalScore += appScores.ConsequencesScore;
						result.AppCompletenessGrade = appScores.CompletenessScore;
						totalScore += appScores.CompletenessScore;
					}
					else
					{
						result.TrainingPoints = 0;
						result.FinancialNeedGrade = 0;
						result.ProblemGrade = 0;
						result.BenefitGrade = 0;
						result.ConsequencesGrade = 0;
						result.AppCompletenessGrade = 0;
					}
					result.TotalScore = totalScore;
					FG_App_GeneralInfo genInfo = await cwmContext.FG_App_GeneralInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
					if (genInfo != null)
					{
						result.ISORating = (genInfo.ISORating != null) ? Convert.ToInt32(genInfo.ISORating) : 0;
						totalScore += (genInfo.ISORating != null) ? Convert.ToInt32(genInfo.ISORating) : 0;
					}
					else
					{
						result.ISORating = 0;
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				return null;
			}

			return result;
		}

		public async Task<List<DetailedFGAppScores>> GetDetailedFGAppScoresAdminAsync(Guid applicationId)
		{
			List<DetailedFGAppScores> result = new List<DetailedFGAppScores>();
			
			try
			{
				nm_FGApplication app = await cwmContext.nm_FGApplications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (app != null)
				{
					
					int totalScore = 0;
					List<FG_App_Scores> appScores = await cwmContext.FG_App_Scores.Where(a => a.ApplicationId == applicationId).ToListAsync();

					foreach (FG_App_Scores appScore in appScores)
                    {
						DetailedFGAppScores score = new DetailedFGAppScores();
						score.ApplicationId = new Guid(app.ApplicationId.ToString());
						if (appScores != null)
						{
							score.UserName = appScore.UserName;
							score.WebUserId = appScore.WebUserId;
							score.TrainingPoints = appScore.TrainingScore;
							totalScore += appScore.TrainingScore;
							score.FinancialNeedGrade = appScore.FinancialNeedScore;
							totalScore += appScore.FinancialNeedScore;
							score.ProblemGrade = appScore.ProblemScore;
							totalScore += appScore.ProblemScore;
							score.BenefitGrade = appScore.BenefitScore;
							totalScore += appScore.BenefitScore;
							score.ConsequencesGrade = appScore.ConsequencesScore;
							totalScore += appScore.ConsequencesScore;
							score.AppCompletenessGrade = appScore.CompletenessScore;
							totalScore += appScore.CompletenessScore;
						}
						else
						{
							score.TrainingPoints = 0;
							score.FinancialNeedGrade = 0;
							score.ProblemGrade = 0;
							score.BenefitGrade = 0;
							score.ConsequencesGrade = 0;
							score.AppCompletenessGrade = 0;
						}
						score.TotalScore = totalScore;
						FG_App_GeneralInfo genInfo = await cwmContext.FG_App_GeneralInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
						if (genInfo != null)
						{
							score.ISORating = (genInfo.ISORating != null) ? Convert.ToInt32(genInfo.ISORating) : 0;
							totalScore += (genInfo.ISORating != null) ? Convert.ToInt32(genInfo.ISORating) : 0;
						}
						else
						{
							score.ISORating = 0;
						}
						result.Add(score);
						totalScore = 0;
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				return null;
			}

			return result;
		}

		public async Task<List<nm_FGApplication>> GetAllFGApplicationByFYAsync(short FiscalYear)
		{
			List<nm_FGApplication> result = null;
			try
			{
				result = await cwmContext.nm_FGApplications.Where(a => a.FiscalYear == FiscalYear).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;

			}

			return result;
		}

		public async Task<GrantYearStats> GetGrantYearStats(short FiscalYear)
        {
			GrantYearStats stats = new GrantYearStats();
            try
            {
				List<nm_FGApplication> result = null;
				result = await cwmContext.nm_FGApplications.Where(a => a.FiscalYear == FiscalYear).ToListAsync();
				stats.FiscalYear = FiscalYear;
				stats.NumApps = result.Count();
				decimal amountRequested = 0;
				decimal amountAwarded = 0;
				foreach (nm_FGApplication app in result)
                {
					amountRequested += (app.AmountRequested != null) ? (decimal)app.AmountRequested : 0;
					amountAwarded += (app.GrantedAmount != null) ? (decimal)app.GrantedAmount : 0;
                }
				stats.FundingRequested = amountRequested;
				stats.GrantsAwarded = amountAwarded;
			}
			catch (Exception ex)
            {
                _ = ex;
				
            }
			return stats;
        }

		public async Task<List<nm_FGApplicationReport>> GetFGApplicationReportAsync()
		{
			List<nm_FGApplicationReport> result = null;
			try
			{
				result = await cwmContext.nm_FGApplicationReport.ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<List<nm_FGApplication>> GetAllFGApplicationByAddressAsync(Guid addressId)
		{
			List<nm_FGApplication> result = null;
			try
			{
				result = await cwmContext.nm_FGApplications.Where(a => a.AddressId == addressId).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;

			}

			return result;
		}

		public nm_FGApplication GetFGApplication(Guid addressId, short FiscalYear)
		{
			nm_FGApplication result = null;
			try
			{
				result = cwmContext.nm_FGApplications.SingleOrDefault(a => a.AddressId == addressId && a.FiscalYear == FiscalYear);
			}
			catch (Exception ex)
            {
                _ = ex;

			}

			return result;
		}

		public async Task<FGApplications> GetFGApplicationById(Guid applicationId)
		{
			FGApplications result = null;
			try
			{
				result = await cwmContext.FGApplications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;

			}

			return result;
		}

		public async Task<List<nm_FGApplication>> GetFGApplicationByCounty(string County, int FiscalYear)
		{
			List<nm_FGApplication> result = null;
			try
			{
				result = await cwmContext.nm_FGApplications.Where(a => a.County == County && a.FiscalYear == FiscalYear && a.AppStatus == 7 && a.IndividualDept == 2).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;

			}

			return result;
		}

		public async Task<List<nm_FGApplication>> GetFGApplicationsAllAsync()
		{
			List<nm_FGApplication> result = null;

			result = await cwmContext.nm_FGApplications.Where(a => a.AddressTypeId == deptAddType && a.Inactive == false).ToListAsync();

			return result;
		}

		private async Task<string> GetNextApplicationNumber(short fiscalYear)
		{
			try
			{
				List<nm_FGApplication> result = null;
				nm_FGApplication app = null;

				result = await cwmContext.nm_FGApplications.Where(a => a.FiscalYear == fiscalYear).OrderBy(a => a.ApplicationNumber).ToListAsync();

				if (result == null || result.Count == 0)
				{
					return fiscalYear.ToString() + "-0001";
				}
				else
				{
					app = result.LastOrDefault();
					string num = app.ApplicationNumber.Substring(5, 4);
					int iNum = Convert.ToInt16(num);
					iNum = iNum + 1;
					switch (iNum.ToString().Length)
					{
						case 1:
							num = "000" + iNum.ToString();
							break;
						case 2:
							num = "00" + iNum.ToString();
							break;
						case 3:
							num = "0" + iNum.ToString();
							break;
						default:
							num = iNum.ToString();
							break;

					}
					string appnum = fiscalYear.ToString() + "-" + num;
					return appnum;
				}
			}
			catch
			{
				return fiscalYear.ToString() + "-0001";
			}
		}

		public async Task<FG_App_GeneralInfo> GetFGApplicationGeneralInfoAsync(Guid applicationId)
		{
			FG_App_GeneralInfo result = null;
			try
			{
				result = await cwmContext.FG_App_GeneralInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_GeneralInfo GetFGApplicationGeneralInfo(Guid applicationId)
		{
			FG_App_GeneralInfo result = null;
			try
			{
				result = cwmContext.FG_App_GeneralInfos.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveGeneralInformationAsync(FG_App_GeneralInfo model)
		{
			try
			{
				if (model != null)
				{
					var generalInfo = await cwmContext.FG_App_GeneralInfos.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (generalInfo == null)
					{
						generalInfo = cwmContext.FG_App_GeneralInfos.Add(new NMSFM.Data.FG_App_GeneralInfo());
						generalInfo.Id = Guid.NewGuid();
						generalInfo.DateEntered = DateTime.Now;
						generalInfo.ApplicationId = model.ApplicationId;
					}

					generalInfo.IndividualDept = model.IndividualDept;
					generalInfo.NFIRSID = model.NFIRSID;
					generalInfo.DepartmentName = model.DepartmentName;
					generalInfo.FireChiefName = model.FireChiefName;
					generalInfo.Phone = model.Phone;
					generalInfo.EmailAddress = model.EmailAddress;
					generalInfo.ISORating = model.ISORating;
					generalInfo.County = model.County;
					generalInfo.IsCityMuni = model.IsCityMuni;
					generalInfo.DeptType = model.DeptType;
					generalInfo.IsAdminDept = model.IsAdminDept;
					generalInfo.CountyDeptsCompliant = model.CountyDeptsCompliant;
					generalInfo.MainStations = model.MainStations;
					generalInfo.SubStations = model.SubStations;
					generalInfo.AdminBldgs = model.AdminBldgs;
					generalInfo.Community = model.Community;
					generalInfo.NumberOfFirefighters = model.NumberOfFirefighters;
					generalInfo.FFII_Firefighters = model.FFII_Firefighters;
					generalInfo.FFI_Firefighters = model.FFI_Firefighters;
					generalInfo.MailingAddress = model.MailingAddress;
					generalInfo.MailingCity = model.MailingCity;
					generalInfo.MailingState = model.MailingState;
					generalInfo.MailingZip = model.MailingZip;
					generalInfo.PersonCompleteApp = model.PersonCompleteApp;
					generalInfo.FireDeptMember = model.FireDeptMember;
					generalInfo.DateUpdated = DateTime.Now;
					generalInfo.UpdatedBy = model.UpdatedBy;
					generalInfo.IsValid = model.IsValid;
					generalInfo.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save general information for " + model.DepartmentName + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save general information for " + model.DepartmentName + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null application information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<FG_App_BudgetInfo> GetFGApplicationBudgetInfoAsync(Guid applicationId)
		{
			FG_App_BudgetInfo result = null;
			try
			{
				result = await cwmContext.FG_App_BudgetInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_BudgetInfo GetFGApplicationBudgetInfo(Guid applicationId)
		{
			FG_App_BudgetInfo result = null;
			try
			{
				result = cwmContext.FG_App_BudgetInfos.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveBudgetInformationAsync(FG_App_BudgetInfo model)
		{
			try
			{
				if (model != null)
				{
					var budgetInfo = await cwmContext.FG_App_BudgetInfos.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (budgetInfo == null)
					{
						budgetInfo = cwmContext.FG_App_BudgetInfos.Add(new NMSFM.Data.FG_App_BudgetInfo());
						budgetInfo.Id = Guid.NewGuid();
						budgetInfo.DateEntered = DateTime.Now;
						budgetInfo.ApplicationId = model.ApplicationId;
					}

					budgetInfo.OperatingBudget = model.OperatingBudget;
					budgetInfo.FPFDistribution = model.FPFDistribution;
					budgetInfo.StipendCarryover = model.StipendCarryover;
					budgetInfo.CarryoverBalance = model.CarryoverBalance;
					budgetInfo.CarryoverPurpose = model.CarryoverPurpose;
					budgetInfo.PerTaxes = model.PerTaxes;
					budgetInfo.PerGrants = model.PerGrants;
					budgetInfo.PerStateFMFunds = model.PerStateFMFunds;
					budgetInfo.PerDonations = model.PerDonations;
					budgetInfo.PerFundDrives = model.PerFundDrives;
					budgetInfo.PerFeeForService = model.PerFeeForService;
					budgetInfo.PerOthers = model.PerOthers;
					budgetInfo.OthersDesc = model.OthersDesc;
					budgetInfo.PerTotal = model.PerTotal;
					budgetInfo.DateUpdated = DateTime.Now;
					budgetInfo.UpdatedBy = model.UpdatedBy;
					budgetInfo.IsValid = model.IsValid;
					budgetInfo.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save budget information for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save budget information for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null budget information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGAppCommunityInfo> GetFGApplicationCommunityInfoAsync(Guid applicationId)
		{
			FG_App_CommunityInfo results = null;
			List<FG_App_AidDistricts> aidDistricts = new List<FG_App_AidDistricts>();
			DetailedFGAppCommunityInfo result = new DetailedFGAppCommunityInfo();
			try
			{
				results = await cwmContext.FG_App_CommunityInfos.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
                {
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.CommunityName = results.CommunityName;
					result.NumberOfHomes = results.NumberOfHomes;
					result.NumberOfComm = results.NumberOfComm;
					result.ResidentPopulation = results.ResidentPopulation;
					result.AidAgreements = results.AidAgreements;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					aidDistricts = await cwmContext.FG_App_AidDistricts.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
					result.AidDistricts = aidDistricts;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		private async Task<FGApplications> FindNearestPriorApplicationWithDataAsync(
			Guid addressId,
			Guid currentApplicationId,
			Func<Guid, Task<bool>> sectionHasDataAsync)
		{
			try
			{
				FGApplications currentApp = await cwmContext.FGApplications
					.SingleOrDefaultAsync(a => a.ApplicationId == currentApplicationId);
				if (currentApp == null)
				{
					return null;
				}

				List<FGApplications> priorApps = await cwmContext.FGApplications
					.Where(a => a.AddressId == addressId && a.FiscalYear < currentApp.FiscalYear)
					.OrderByDescending(a => a.FiscalYear)
					.ToListAsync();

				foreach (FGApplications candidate in priorApps)
				{
					if (await sectionHasDataAsync(candidate.ApplicationId))
					{
						return candidate;
					}
				}
			}
			catch (Exception ex)
			{
				_ = ex;
				throw ex;
			}

			return null;
		}

		private async Task<bool> HasGeneralInfoDataAsync(Guid applicationId)
		{
			return await cwmContext.FG_App_GeneralInfos.AnyAsync(a => a.ApplicationId == applicationId);
		}

		private async Task<bool> HasApparatusDataAsync(Guid applicationId)
		{
			return await cwmContext.FG_App_Apparatuses.AnyAsync(a => a.ApplicationId == applicationId)
				|| await cwmContext.FG_App_ApparatusEquipment.AnyAsync(a => a.ApplicationId == applicationId);
		}

		private async Task<bool> HasCommunityInfoDataAsync(Guid applicationId)
		{
			return await cwmContext.FG_App_CommunityInfos.AnyAsync(a => a.ApplicationId == applicationId);
		}

		private async Task<bool> HasWaterAvailabilityDataAsync(Guid applicationId)
		{
			return await cwmContext.FG_App_WaterAvailabilities.AnyAsync(a => a.ApplicationId == applicationId);
		}

		private async Task<bool> HasCommunicationDataAsync(Guid applicationId)
		{
			return await cwmContext.FG_App_Communications.AnyAsync(a => a.ApplicationId == applicationId);
		}

		private async Task<bool> HasHazardsThreatsDataAsync(Guid applicationId)
		{
			return await cwmContext.FG_App_HazardsThreats.AnyAsync(a => a.ApplicationId == applicationId);
		}

		public async Task<FGApplications> GetNearestPriorApplicationWithGeneralInfoAsync(
			Guid addressId, Guid currentApplicationId)
		{
			return await FindNearestPriorApplicationWithDataAsync(
				addressId, currentApplicationId, HasGeneralInfoDataAsync);
		}

		//Added 12/26/23
		public async Task<DetailedFGAppCommunityInfo> GetFGApplicationPriorYearCommunityInfoAsync(Guid addressId, Guid applicationId)
		{
			FG_App_CommunityInfo results = null;
			List<FG_App_AidDistricts> aidDistricts = new List<FG_App_AidDistricts>();
			DetailedFGAppCommunityInfo result = new DetailedFGAppCommunityInfo();
			try
			{
				FGApplications priorApp = await FindNearestPriorApplicationWithDataAsync(
					addressId, applicationId, HasCommunityInfoDataAsync);
				if (priorApp != null)
                {
					Guid appId = priorApp.ApplicationId;
					results = await cwmContext.FG_App_CommunityInfos.SingleOrDefaultAsync(a => a.ApplicationId == appId);
					if (results != null)
					{
						result.Id = results.Id;
						result.ApplicationId = results.ApplicationId;
						result.CommunityName = results.CommunityName;
						result.NumberOfHomes = results.NumberOfHomes;
						result.NumberOfComm = results.NumberOfComm;
						result.ResidentPopulation = results.ResidentPopulation;
						result.AidAgreements = results.AidAgreements;
						result.DateEntered = results.DateEntered;
						result.DateUpdated = results.DateUpdated;
						result.UpdatedBy = results.UpdatedBy;
						result.IsValid = results.IsValid;
						result.InvalidText = results.InvalidText;

						aidDistricts = await cwmContext.FG_App_AidDistricts.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
						result.AidDistricts = aidDistricts;
					}
				}
				
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_CommunityInfo GetFGApplicationCommunityInfo(Guid applicationId)
		{
			FG_App_CommunityInfo result = null;
			try
			{
				result = cwmContext.FG_App_CommunityInfos.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveCommunityInformationAsync(DetailedFGAppCommunityInfo model)
		{
			try
			{
				if (model != null)
				{
					bool isNew = false;
					var communityInfo = await cwmContext.FG_App_CommunityInfos.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (communityInfo == null)
					{
						isNew = true;
						communityInfo = cwmContext.FG_App_CommunityInfos.Add(new NMSFM.Data.FG_App_CommunityInfo());
						communityInfo.Id = Guid.NewGuid();
						communityInfo.DateEntered = DateTime.Now;
						communityInfo.ApplicationId = model.ApplicationId;
						
					}

					communityInfo.CommunityName = model.CommunityName;
					communityInfo.NumberOfHomes = model.NumberOfHomes;
					communityInfo.NumberOfComm = model.NumberOfComm;
					communityInfo.ResidentPopulation = model.ResidentPopulation;
					communityInfo.AidAgreements = model.AidAgreements;

					communityInfo.DateUpdated = DateTime.Now;
					communityInfo.UpdatedBy = model.UpdatedBy;
					communityInfo.IsValid = model.IsValid;
					communityInfo.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save budget information for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save budget information for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					if (retbol == true)
					{
						if (isNew)
                        {
							if (model.AidDistricts != null)
							{
								foreach (FG_App_AidDistricts aidDist in model.AidDistricts)
								{
									await SaveAidDistrict(aidDist);
								}
							}
						}
                        else
                        {
							List<FG_App_AidDistricts> existingDists = await cwmContext.FG_App_AidDistricts.Where(a => a.ApplicationId == communityInfo.ApplicationId).ToListAsync();
							bool isDelete = true;
							foreach (FG_App_AidDistricts existDist in existingDists)
                            {
								foreach (FG_App_AidDistricts newdist in model.AidDistricts)
                                {
									if (existDist.AidDistrictId.ToString() == newdist.AidDistrictId.ToString())
                                    {
										isDelete = false;
                                    }
                                }
								if (isDelete)
                                {
									cwmContext.FG_App_AidDistricts.Remove(existDist);
                                }
								isDelete = true;
                            }
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.AidDistricts != null)
							{
								foreach (FG_App_AidDistricts aidDist in model.AidDistricts)
								{
									await SaveAidDistrict(aidDist);
								}
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null budget information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SaveAidDistrict(FG_App_AidDistricts model)
        {
			try
			{
				if (model != null)
				{
					var aidDistrict = await cwmContext.FG_App_AidDistricts.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.AidDistrictId == model.AidDistrictId);
					if (aidDistrict == null)
					{
						aidDistrict = cwmContext.FG_App_AidDistricts.Add(new NMSFM.Data.FG_App_AidDistricts());
						aidDistrict.AidDistrictId = Guid.NewGuid();
						aidDistrict.ApplicationId = model.ApplicationId;
					}

					aidDistrict.Number = model.Number;
					aidDistrict.AidDistrict = model.AidDistrict;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save aid district for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save aid district for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null aid district information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<FG_App_ResponseHistory> GetFGApplicationResponseHistoryAsync(Guid applicationId)
		{
			FG_App_ResponseHistory result = null;
			try
			{
				result = await cwmContext.FG_App_ResponseHistories.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_ResponseHistory GetFGApplicationResponseHistory(Guid applicationId)
		{
			FG_App_ResponseHistory result = null;
			try
			{
				result = cwmContext.FG_App_ResponseHistories.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveResponseHistoryAsync(FG_App_ResponseHistory model)
		{
			try
			{
				if (model != null)
				{
					var responseHistory = await cwmContext.FG_App_ResponseHistories.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (responseHistory == null)
					{
						responseHistory = cwmContext.FG_App_ResponseHistories.Add(new NMSFM.Data.FG_App_ResponseHistory());
						responseHistory.Id = Guid.NewGuid();
						responseHistory.DateEntered = DateTime.Now;
						responseHistory.ApplicationId = model.ApplicationId;
					}

					responseHistory.NFIRSCurrent = model.NFIRSCurrent;
					responseHistory.ResponseStructure = model.ResponseStructure;
					responseHistory.ResponseVehicle = model.ResponseVehicle;
					responseHistory.ResponseVegitation = model.ResponseVegitation;
					responseHistory.ResponseEMS = model.ResponseEMS;
					responseHistory.ResponseRescue = model.ResponseRescue;
					responseHistory.ResponseHazardous = model.ResponseHazardous;
					responseHistory.ResponseService = model.ResponseService;
					responseHistory.ResponseGoodIntent = model.ResponseGoodIntent;
					responseHistory.ResponseFalse = model.ResponseFalse;
					responseHistory.ResponseOther = model.ResponseOther;
					responseHistory.ResponseTotal = model.ResponseTotal;
					responseHistory.AdminComments = model.AdminComments;
					responseHistory.DateUpdated = DateTime.Now;
					responseHistory.UpdatedBy = model.UpdatedBy;
					responseHistory.IsValid = model.IsValid;
					responseHistory.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save response history for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save response history for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null response history.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGWaterAvailability> GetFGApplicationWaterAvailabilityAsync(Guid applicationId)
		{
			FG_App_WaterAvailability results = null;
			List<FG_App_WaterSources> waterSources = new List<FG_App_WaterSources>();
			DetailedFGWaterAvailability result = new DetailedFGWaterAvailability();
			try
			{
				results = await cwmContext.FG_App_WaterAvailabilities.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.ComHydrantSys = results.ComHydrantSys;
					result.AvailableWaterCapacity = results.AvailableWaterCapacity;
					result.WaterOnWheelsCapacity = results.WaterOnWheelsCapacity;
					result.StationWaterCapacity = results.StationWaterCapacity;
					result.TankAtStation = results.TankAtStation;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					waterSources = await cwmContext.FG_App_WaterSources.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
					result.WaterSources = waterSources;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		//Added 12/26/23 (vwd)
		public async Task<DetailedFGWaterAvailability> GetFGApplicationPriorYearWaterAvailabilityAsync(Guid addressId, Guid applicationId)
		{
			FG_App_WaterAvailability results = null;
			List<FG_App_WaterSources> waterSources = new List<FG_App_WaterSources>();
			DetailedFGWaterAvailability result = new DetailedFGWaterAvailability();
			try
			{
				FGApplications priorApp = await FindNearestPriorApplicationWithDataAsync(
					addressId, applicationId, HasWaterAvailabilityDataAsync);
				if (priorApp != null)
				{
					Guid appId = priorApp.ApplicationId;
					results = await cwmContext.FG_App_WaterAvailabilities.SingleOrDefaultAsync(a => a.ApplicationId == appId);
					if (results != null)
					{
						result.Id = results.Id;
						result.ApplicationId = results.ApplicationId;
						result.ComHydrantSys = results.ComHydrantSys;
						result.AvailableWaterCapacity = results.AvailableWaterCapacity;
						result.WaterOnWheelsCapacity = results.WaterOnWheelsCapacity;
						result.StationWaterCapacity = results.StationWaterCapacity;
						result.TankAtStation = results.TankAtStation;
						result.DateEntered = results.DateEntered;
						result.DateUpdated = results.DateUpdated;
						result.UpdatedBy = results.UpdatedBy;
						result.IsValid = results.IsValid;
						result.InvalidText = results.InvalidText;

						waterSources = await cwmContext.FG_App_WaterSources.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
						result.WaterSources = waterSources;
					}
				}
					
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_WaterAvailability GetFGApplicationWaterAvailability(Guid applicationId)
		{
			FG_App_WaterAvailability result = null;
			try
			{
				result = cwmContext.FG_App_WaterAvailabilities.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveWaterAvailabilityAsync(DetailedFGWaterAvailability model)
		{
			try
			{
				if (model != null)
				{
					bool isNew = false;
					var waterAvailability = await cwmContext.FG_App_WaterAvailabilities.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (waterAvailability == null)
					{
						isNew = true;
						waterAvailability = cwmContext.FG_App_WaterAvailabilities.Add(new NMSFM.Data.FG_App_WaterAvailability());
						waterAvailability.Id = Guid.NewGuid();
						waterAvailability.DateEntered = DateTime.Now;
						waterAvailability.ApplicationId = model.ApplicationId;

					}

					waterAvailability.ComHydrantSys = model.ComHydrantSys;
					waterAvailability.AvailableWaterCapacity = model.AvailableWaterCapacity;
					waterAvailability.WaterOnWheelsCapacity = model.WaterOnWheelsCapacity;
					waterAvailability.StationWaterCapacity = model.StationWaterCapacity;
					waterAvailability.TankAtStation = model.TankAtStation;

					waterAvailability.DateUpdated = DateTime.Now;
					waterAvailability.UpdatedBy = model.UpdatedBy;
					waterAvailability.IsValid = model.IsValid;
					waterAvailability.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save water availability for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save water availability for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					if (retbol == true)
					{
						if (isNew)
						{
							if (model.WaterSources != null)
							{
								foreach (FG_App_WaterSources waterSource in model.WaterSources)
								{
									await SaveWaterSource(waterSource);
								}
							}
						}
						else
						{
							List<FG_App_WaterSources> existingWaterSources = await cwmContext.FG_App_WaterSources.Where(a => a.ApplicationId == waterAvailability.ApplicationId).ToListAsync();
							bool isDelete = true;
							foreach (FG_App_WaterSources existWaterSource in existingWaterSources)
							{
								foreach (FG_App_WaterSources newWaterSource in model.WaterSources)
								{
									if (existWaterSource.WaterSourceId.ToString() == newWaterSource.WaterSourceId.ToString())
									{
										isDelete = false;
									}
								}
								if (isDelete)
								{
									cwmContext.FG_App_WaterSources.Remove(existWaterSource);
								}
								isDelete = true;
							}
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.WaterSources != null)
							{
								foreach (FG_App_WaterSources waterSource in model.WaterSources)
								{
									await SaveWaterSource(waterSource);
								}
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null water source information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SaveWaterSource(FG_App_WaterSources model)
		{
			try
			{
				if (model != null)
				{
					var waterSource = await cwmContext.FG_App_WaterSources.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.WaterSourceId == model.WaterSourceId);
					if (waterSource == null)
					{
						waterSource = cwmContext.FG_App_WaterSources.Add(new NMSFM.Data.FG_App_WaterSources());
						waterSource.WaterSourceId = Guid.NewGuid();
						waterSource.ApplicationId = model.ApplicationId;
					}

					waterSource.Number = model.Number;
					waterSource.WaterSource = model.WaterSource;
					waterSource.Capacity = model.Capacity;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save water source for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save water source for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null water district information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGAppTraining> GetFGApplicationTrainingAsync(Guid applicationId)
		{
			FG_App_Training results = null;
			List<FG_App_TrainingOpportunityView> trainingOpportunitiesView = new List<FG_App_TrainingOpportunityView>();
			List<FG_App_TrainingOpportunities> trainingOpportunities = new List<FG_App_TrainingOpportunities>();
			DetailedFGAppTraining result = new DetailedFGAppTraining();
			try
			{
				results = await cwmContext.FG_App_Trainings.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.YearlyTrainingHours = results.YearlyTrainingHours;
					result.TrainingPoints = results.TrainingPoints;
					result.AdminComments = results.AdminComments;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					trainingOpportunities = await cwmContext.FG_App_TrainingOpportunities.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
					foreach (FG_App_TrainingOpportunities training in trainingOpportunities)
                    {
						FG_App_TrainingOpportunityView trainingView = new FG_App_TrainingOpportunityView();
						trainingView.TrainingId = training.TrainingId;
						trainingView.ApplicationId = training.ApplicationId;
						trainingView.Number = training.Number;
						trainingView.TrainingDetail = training.TrainingDetail;
						trainingView.TrainingDocumentName = training.TrainingDocumentName;
						trainingView.TrainingDocumentType = training.TrainingDocumentType;
						trainingOpportunitiesView.Add(trainingView);

					}

					result.TrainingOpportunities = trainingOpportunitiesView;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_Training GetFGApplicationTraining(Guid applicationId)
		{
			FG_App_Training result = null;
			try
			{
				result = cwmContext.FG_App_Trainings.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_TrainingOpportunities GetFGApplicationTrainingOpportunity(Guid trainingId)
		{
			FG_App_TrainingOpportunities result = null;
			try
			{
				result = cwmContext.FG_App_TrainingOpportunities.SingleOrDefault(a => a.TrainingId == trainingId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveTrainingAsync(DetailedFGAppTraining model)
		{
			try
			{
				if (model != null)
				{
					var training = await cwmContext.FG_App_Trainings.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (training == null)
					{
						training = cwmContext.FG_App_Trainings.Add(new NMSFM.Data.FG_App_Training());
						training.Id = Guid.NewGuid();
						training.DateEntered = DateTime.Now;
						training.ApplicationId = model.ApplicationId;

					}

					training.YearlyTrainingHours = model.YearlyTrainingHours;
					training.AdminComments = model.AdminComments;

					training.DateUpdated = DateTime.Now;
					training.UpdatedBy = model.UpdatedBy;
					training.IsValid = model.IsValid;
					training.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save training data for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save training data for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null training information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<bool> SaveTrainingOpportunity(FG_App_TrainingOpportunities model)
		{
			try
			{
				if (model != null)
				{
					var trainingOpportunity = await cwmContext.FG_App_TrainingOpportunities.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.TrainingId == model.TrainingId);
					if (trainingOpportunity == null)
					{
						trainingOpportunity = cwmContext.FG_App_TrainingOpportunities.Add(new NMSFM.Data.FG_App_TrainingOpportunities());
						trainingOpportunity.TrainingId = Guid.NewGuid();
						trainingOpportunity.ApplicationId = model.ApplicationId;
					}

					trainingOpportunity.Number = model.Number;
					trainingOpportunity.TrainingDetail = model.TrainingDetail;
					trainingOpportunity.TrainingDocumentName = model.TrainingDocumentName;
					trainingOpportunity.TrainingDocument = model.TrainingDocument;
					trainingOpportunity.TrainingDocumentType = model.TrainingDocumentType;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save training opportunity for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save training opportunity for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null training opportunity information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<bool> DeleteTrainingOpportunityAsync(Guid id)
		{
			try
			{
				if (id != null)
				{
					FG_App_TrainingOpportunities doc = await cwmContext.FG_App_TrainingOpportunities.SingleOrDefaultAsync(a => a.TrainingId == id);
					if (doc == null)
					{
						throw new Exception("Unable to delete null app document information.");
					}

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							cwmContext.FG_App_TrainingOpportunities.Remove(doc);
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to remove Training.", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to remove training, DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to delete null app document information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGApparatus> GetFGApplicationApparatusAsync(Guid applicationId)
		{
			FG_App_Apparatus results = null;
			List<FG_App_ApparatusEquipment> apparatusEquipment = new List<FG_App_ApparatusEquipment>();
			DetailedFGApparatus result = new DetailedFGApparatus();
			try
			{
				results = await cwmContext.FG_App_Apparatuses.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.ApparatusPartOfProject = results.ApparatusPartOfProject;
					result.PumpTestsConducted = results.PumpTestsConducted;
					result.ExplainNoPumpTests = results.ExplainNoPumpTests;
					result.HoseTestConducted = results.HoseTestConducted;
					result.ExplainNoHostTests = results.ExplainNoHostTests;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					apparatusEquipment = await cwmContext.FG_App_ApparatusEquipment.Where(a => a.ApplicationId == result.ApplicationId).OrderBy(a => a.Number).ToListAsync();
					result.ApparatusEquipment = apparatusEquipment;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		//Added 12/23/23 (vwd)
		public async Task<DetailedFGApparatus> GetPriorFGApplicationApparatusAsync(Guid addressId, Guid applicationId)
		{
			FG_App_Apparatus results = null;
			List<FG_App_ApparatusEquipment> apparatusEquipment = new List<FG_App_ApparatusEquipment>();
			DetailedFGApparatus result = new DetailedFGApparatus();
			try
			{
				FGApplications priorApp = await FindNearestPriorApplicationWithDataAsync(
					addressId, applicationId, HasApparatusDataAsync);
				if (priorApp != null)
                {
					Guid appId = priorApp.ApplicationId;
					results = await cwmContext.FG_App_Apparatuses.SingleOrDefaultAsync(a => a.ApplicationId == appId);
					if (results != null)
					{
						result.Id = results.Id;
						result.ApplicationId = results.ApplicationId;
						result.ApparatusPartOfProject = results.ApparatusPartOfProject;
						result.PumpTestsConducted = results.PumpTestsConducted;
						result.ExplainNoPumpTests = results.ExplainNoPumpTests;
						result.HoseTestConducted = results.HoseTestConducted;
						result.ExplainNoHostTests = results.ExplainNoHostTests;
						result.DateEntered = results.DateEntered;
						result.DateUpdated = results.DateUpdated;
						result.UpdatedBy = results.UpdatedBy;
						result.IsValid = results.IsValid;
						result.InvalidText = results.InvalidText;

						apparatusEquipment = await cwmContext.FG_App_ApparatusEquipment.Where(a => a.ApplicationId == result.ApplicationId).OrderBy(a => a.Number).ToListAsync();
						result.ApparatusEquipment = apparatusEquipment;
					}
				}
				
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_Apparatus GetFGApplicationApparatus(Guid applicationId)
		{
			FG_App_Apparatus result = null;
			try
			{
				result = cwmContext.FG_App_Apparatuses.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveApparatusAsync(DetailedFGApparatus model)
		{
			try
			{
				if (model != null)
				{
					bool isNew = false;
					var apparatus = await cwmContext.FG_App_Apparatuses.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (apparatus == null)
					{
						isNew = true;
						apparatus = cwmContext.FG_App_Apparatuses.Add(new NMSFM.Data.FG_App_Apparatus());
						apparatus.Id = Guid.NewGuid();
						apparatus.DateEntered = DateTime.Now;
						apparatus.ApplicationId = model.ApplicationId;

					}

					apparatus.ApparatusPartOfProject = model.ApparatusPartOfProject;
					apparatus.PumpTestsConducted = model.PumpTestsConducted;
					apparatus.ExplainNoPumpTests = model.ExplainNoPumpTests;
					apparatus.HoseTestConducted = model.HoseTestConducted;
					apparatus.ExplainNoHostTests = model.ExplainNoHostTests;

					apparatus.DateUpdated = DateTime.Now;
					apparatus.UpdatedBy = model.UpdatedBy;
					apparatus.IsValid = model.IsValid;
					apparatus.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save apparatus info for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save apparatus info for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					if (retbol == true)
					{
						if (isNew)
						{
							if (model.ApparatusEquipment != null)
							{
								foreach (FG_App_ApparatusEquipment apparatusEquipment in model.ApparatusEquipment)
								{
									await SaveApparatusEquipment(apparatusEquipment);
								}
							}
						}
						else
						{
							List<FG_App_ApparatusEquipment> existingApparatusEquipment = await cwmContext.FG_App_ApparatusEquipment.Where(a => a.ApplicationId == apparatus.ApplicationId).ToListAsync();
							bool isDelete = true;
							foreach (FG_App_ApparatusEquipment existApparatusEquipment in existingApparatusEquipment)
							{
								foreach (FG_App_ApparatusEquipment newApparatusEquipment in model.ApparatusEquipment)
								{
									if (existApparatusEquipment.ApparatusId.ToString() == newApparatusEquipment.ApparatusId.ToString())
									{
										isDelete = false;
									}
								}
								if (isDelete)
								{
									cwmContext.FG_App_ApparatusEquipment.Remove(existApparatusEquipment);
								}
								isDelete = true;
							}
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.ApparatusEquipment != null)
							{
								foreach (FG_App_ApparatusEquipment apparatusEquipment in model.ApparatusEquipment)
								{
									await SaveApparatusEquipment(apparatusEquipment);
								}
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null apparatus equipment information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SaveApparatusEquipment(FG_App_ApparatusEquipment model)
		{
			try
			{
				if (model != null)
				{
					var apparatusEquipment = await cwmContext.FG_App_ApparatusEquipment.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.ApparatusId == model.ApparatusId);
					if (apparatusEquipment == null)
					{
						apparatusEquipment = cwmContext.FG_App_ApparatusEquipment.Add(new NMSFM.Data.FG_App_ApparatusEquipment());
						apparatusEquipment.ApparatusId = Guid.NewGuid();
						apparatusEquipment.ApplicationId = model.ApplicationId;
					}

					apparatusEquipment.Number = model.Number;
					apparatusEquipment.ApparatusName = model.ApparatusName;
					apparatusEquipment.VehicleType = model.VehicleType;
					apparatusEquipment.Year = model.Year;
					apparatusEquipment.VIN = model.VIN;
					apparatusEquipment.License = model.License;
					apparatusEquipment.Capacity = model.Capacity;
					apparatusEquipment.GPM = model.GPM;
					apparatusEquipment.TestDate = model.TestDate;
					apparatusEquipment.Pass = model.Pass;
					apparatusEquipment.Comments = model.Comments;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save apparatus equipment for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save apparatus equipment for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null apparatus equipment information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGCommunication> GetFGApplicationCommunicationAsync(Guid applicationId)
		{
			FG_App_Communication results = null;
			List<FG_App_CommunicationEquipment> communicationEquipment = new List<FG_App_CommunicationEquipment>();
			DetailedFGCommunication result = new DetailedFGCommunication();
			try
			{
				results = await cwmContext.FG_App_Communications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.CommunicationProject = results.CommunicationProject;
					result.HandheldRadios = results.HandheldRadios;
					result.BaseStations = results.BaseStations;
					result.MobileRadios = results.MobileRadios;
					result.ApparatusWoRadio = results.ApparatusWoRadio;
					result.LawEnforcement = results.LawEnforcement;
					result.EmergencyMedical = results.EmergencyMedical;
					result.OtherFireDepts = results.OtherFireDepts;
					result.Other = results.Other;
					result.OtherDescription = results.OtherDescription;
					result.AreasNotCovered = results.AreasNotCovered;
					result.DescribeAreasNotCovered = results.DescribeAreasNotCovered;
					result.AdminComments = results.AdminComments;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					communicationEquipment = await cwmContext.FG_App_CommunicationEquipment.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
					result.CommunicationEquipment = communicationEquipment;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}
		//Added 12/26/23 (vwd)
		public async Task<DetailedFGCommunication> GetFGApplicationPriorYearCommunicationAsync(Guid addressId, Guid applicationId)
		{
			FG_App_Communication results = null;
			List<FG_App_CommunicationEquipment> communicationEquipment = new List<FG_App_CommunicationEquipment>();
			DetailedFGCommunication result = new DetailedFGCommunication();
			try
			{
				FGApplications priorApp = await FindNearestPriorApplicationWithDataAsync(
					addressId, applicationId, HasCommunicationDataAsync);
				if (priorApp != null)
				{
					Guid appId = priorApp.ApplicationId;
					results = await cwmContext.FG_App_Communications.SingleOrDefaultAsync(a => a.ApplicationId == appId);
					if (results != null)
					{
						result.Id = results.Id;
						result.ApplicationId = results.ApplicationId;
						result.CommunicationProject = results.CommunicationProject;
						result.HandheldRadios = results.HandheldRadios;
						result.BaseStations = results.BaseStations;
						result.MobileRadios = results.MobileRadios;
						result.ApparatusWoRadio = results.ApparatusWoRadio;
						result.LawEnforcement = results.LawEnforcement;
						result.EmergencyMedical = results.EmergencyMedical;
						result.OtherFireDepts = results.OtherFireDepts;
						result.Other = results.Other;
						result.OtherDescription = results.OtherDescription;
						result.AreasNotCovered = results.AreasNotCovered;
						result.DescribeAreasNotCovered = results.DescribeAreasNotCovered;
						result.AdminComments = results.AdminComments;
						result.DateEntered = results.DateEntered;
						result.DateUpdated = results.DateUpdated;
						result.UpdatedBy = results.UpdatedBy;
						result.IsValid = results.IsValid;
						result.InvalidText = results.InvalidText;

						communicationEquipment = await cwmContext.FG_App_CommunicationEquipment.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
						result.CommunicationEquipment = communicationEquipment;
					}
				}
					
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_Communication GetFGApplicatioCommunication(Guid applicationId)
		{
			FG_App_Communication result = null;
			try
			{
				result = cwmContext.FG_App_Communications.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveCommunicationAsync(DetailedFGCommunication model)
		{
			try
			{
				if (model != null)
				{
					bool isNew = false;
					var communication = await cwmContext.FG_App_Communications.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (communication == null)
					{
						isNew = true;
						communication = cwmContext.FG_App_Communications.Add(new NMSFM.Data.FG_App_Communication());
						communication.Id = Guid.NewGuid();
						communication.DateEntered = DateTime.Now;
						communication.ApplicationId = model.ApplicationId;

					}

					communication.CommunicationProject = model.CommunicationProject;
					communication.HandheldRadios = model.HandheldRadios;
					communication.BaseStations = model.BaseStations;
					communication.MobileRadios = model.MobileRadios;
					communication.ApparatusWoRadio = model.ApparatusWoRadio;
					communication.LawEnforcement = model.LawEnforcement;
					communication.EmergencyMedical = model.EmergencyMedical;
					communication.OtherFireDepts = model.OtherFireDepts;
					communication.Other = model.Other;
					communication.OtherDescription = model.OtherDescription;
					communication.AreasNotCovered = model.AreasNotCovered;
					communication.DescribeAreasNotCovered = model.DescribeAreasNotCovered;
					communication.AdminComments = model.AdminComments;

					communication.DateUpdated = DateTime.Now;
					communication.UpdatedBy = model.UpdatedBy;
					communication.IsValid = model.IsValid;
					communication.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save communication equipment for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save communication equipment for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					if (retbol == true)
					{
						if (isNew)
						{
							if (model.CommunicationEquipment != null)
							{
								foreach (FG_App_CommunicationEquipment communicationEquipment in model.CommunicationEquipment)
								{
									await SaveCommunicationEquipment(communicationEquipment);
								}
							}
						}
						else
						{
							List<FG_App_CommunicationEquipment> existingCommunicationEquipment = await cwmContext.FG_App_CommunicationEquipment.Where(a => a.ApplicationId == communication.ApplicationId).ToListAsync();
							bool isDelete = true;
							foreach (FG_App_CommunicationEquipment existCommunicationEquipment in existingCommunicationEquipment)
							{
								foreach (FG_App_CommunicationEquipment newCommunicationEquipment in model.CommunicationEquipment)
								{
									if (existCommunicationEquipment.CommunicationEquipmentId.ToString() == newCommunicationEquipment.CommunicationEquipmentId.ToString())
									{
										isDelete = false;
									}
								}
								if (isDelete)
								{
									cwmContext.FG_App_CommunicationEquipment.Remove(existCommunicationEquipment);
								}
								isDelete = true;
							}
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.CommunicationEquipment != null)
							{
								foreach (FG_App_CommunicationEquipment communicationEquipment in model.CommunicationEquipment)
								{
									await SaveCommunicationEquipment(communicationEquipment);
								}
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null communication info.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SaveCommunicationEquipment(FG_App_CommunicationEquipment model)
		{
			try
			{
				if (model != null)
				{
					var communicationEquipment = await cwmContext.FG_App_CommunicationEquipment.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.CommunicationEquipmentId == model.CommunicationEquipmentId);
					if (communicationEquipment == null)
					{
						communicationEquipment = cwmContext.FG_App_CommunicationEquipment.Add(new NMSFM.Data.FG_App_CommunicationEquipment());
						communicationEquipment.CommunicationEquipmentId = Guid.NewGuid();
						communicationEquipment.ApplicationId = model.ApplicationId;
					}

					communicationEquipment.Number = model.Number;
					communicationEquipment.CommunicationEquipment = model.CommunicationEquipment;
					communicationEquipment.CommunicationQty = model.CommunicationQty;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save communication equipment for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save communication equipment for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null communication equipment information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGAppHazardsThreats> GetFGApplicationHazardsThreatsAsync(Guid applicationId)
		{
			FG_App_HazardsThreats results = null;
			List<FG_App_HazardThreatEvents> hazardThreatEvents = new List<FG_App_HazardThreatEvents>();
			DetailedFGAppHazardsThreats result = new DetailedFGAppHazardsThreats();
			try
			{
				results = await cwmContext.FG_App_HazardsThreats.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.AdminComments = results.AdminComments;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					hazardThreatEvents = await cwmContext.FG_App_HazardThreatEvents.Where(a => a.ApplicationId == result.ApplicationId).OrderBy(a => a.Number).ToListAsync();
					result.HazardsThreats = hazardThreatEvents;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		//Added 12/26/23 (vwd)
		public async Task<DetailedFGAppHazardsThreats> GetFGApplicationPriorYearHazardsThreatsAsync(Guid addressId, Guid applicationId)
		{
			FG_App_HazardsThreats results = null;
			List<FG_App_HazardThreatEvents> hazardThreatEvents = new List<FG_App_HazardThreatEvents>();
			DetailedFGAppHazardsThreats result = new DetailedFGAppHazardsThreats();
			try
			{
				FGApplications priorApp = await FindNearestPriorApplicationWithDataAsync(
					addressId, applicationId, HasHazardsThreatsDataAsync);
				if (priorApp != null)
				{
					Guid appId = priorApp.ApplicationId;
					results = await cwmContext.FG_App_HazardsThreats.SingleOrDefaultAsync(a => a.ApplicationId == appId);
					if (results != null)
					{
						result.Id = results.Id;
						result.ApplicationId = results.ApplicationId;
						result.AdminComments = results.AdminComments;
						result.DateEntered = results.DateEntered;
						result.DateUpdated = results.DateUpdated;
						result.UpdatedBy = results.UpdatedBy;
						result.IsValid = results.IsValid;
						result.InvalidText = results.InvalidText;

						hazardThreatEvents = await cwmContext.FG_App_HazardThreatEvents.Where(a => a.ApplicationId == result.ApplicationId).OrderBy(a => a.Number).ToListAsync();
						result.HazardsThreats = hazardThreatEvents;
					}
				}
					
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_HazardsThreats GetFGApplicationHazardsThreats(Guid applicationId)
		{
			FG_App_HazardsThreats result = null;
			try
			{
				result = cwmContext.FG_App_HazardsThreats.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveHazardThreatsAsync(DetailedFGAppHazardsThreats model)
		{
			try
			{
				if (model != null)
				{
					bool isNew = false;
					var hazardsThreats = await cwmContext.FG_App_HazardsThreats.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (hazardsThreats == null)
					{
						isNew = true;
						hazardsThreats = cwmContext.FG_App_HazardsThreats.Add(new NMSFM.Data.FG_App_HazardsThreats());
						hazardsThreats.Id = Guid.NewGuid();
						hazardsThreats.DateEntered = DateTime.Now;
						hazardsThreats.ApplicationId = model.ApplicationId;

					}

					hazardsThreats.AdminComments = model.AdminComments;

					hazardsThreats.DateUpdated = DateTime.Now;
					hazardsThreats.UpdatedBy = model.UpdatedBy;
					hazardsThreats.IsValid = model.IsValid;
					hazardsThreats.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save hazard/threats for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save hazard/threats for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					if (retbol == true)
					{
						if (isNew)
						{
							if (model.HazardsThreats != null)
							{
								foreach (FG_App_HazardThreatEvents hazardThreatEvent in model.HazardsThreats)
								{
									await SaveHazardThreatEvents(hazardThreatEvent);
								}
							}
						}
						else
						{
							List<FG_App_HazardThreatEvents> existingHazardThreatEvents = await cwmContext.FG_App_HazardThreatEvents.Where(a => a.ApplicationId == hazardsThreats.ApplicationId).ToListAsync();
							bool isDelete = true;
							foreach (FG_App_HazardThreatEvents existHazardThreatEvent in existingHazardThreatEvents)
							{
								foreach (FG_App_HazardThreatEvents newHazardThreatEvent in model.HazardsThreats)
								{
									if (existHazardThreatEvent.HazardId.ToString() == newHazardThreatEvent.HazardId.ToString())
									{
										isDelete = false;
									}
								}
								if (isDelete)
								{
									cwmContext.FG_App_HazardThreatEvents.Remove(existHazardThreatEvent);
								}
								isDelete = true;
							}
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.HazardsThreats != null)
							{
								foreach (FG_App_HazardThreatEvents hazardThreatEvent in model.HazardsThreats)
								{
									await SaveHazardThreatEvents(hazardThreatEvent);
								}
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null hazards/threats info.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SaveHazardThreatEvents(FG_App_HazardThreatEvents model)
		{
			try
			{
				if (model != null)
				{
					var hazardThreat = await cwmContext.FG_App_HazardThreatEvents.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.HazardId == model.HazardId);
					if (hazardThreat == null)
					{
						hazardThreat = cwmContext.FG_App_HazardThreatEvents.Add(new NMSFM.Data.FG_App_HazardThreatEvents());
						hazardThreat.HazardId = Guid.NewGuid();
						hazardThreat.ApplicationId = model.ApplicationId;
					}

					hazardThreat.Number = model.Number;
					hazardThreat.HazardType = model.HazardType;
					hazardThreat.HazardDetail = model.HazardDetail;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save hazard for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save hazard for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null hazard information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGAppPPE> GetFGApplicationPPEAsync(Guid applicationId)
		{
			FG_App_PPE results = null;
			List<FG_App_StandardPPE> standardPPE = new List<FG_App_StandardPPE>();
			List<FG_App_StandardSCBA> standardSCBA = new List<FG_App_StandardSCBA>();
			DetailedFGAppPPE result = new DetailedFGAppPPE();
			try
			{
				results = await cwmContext.FG_App_PPEs.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.PPEPartOfProject = results.PPEPartOfProject;
					result.SCBAPartOfProject = results.SCBAPartOfProject;
					result.PPEInspected = results.PPEInspected;
					result.AdminComments = results.AdminComments;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					standardPPE = await cwmContext.FG_App_StandardPPEs.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
					result.StandardPPE = standardPPE;

					standardSCBA = await cwmContext.FG_App_StandardSCBAs.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
					result.StandardSCBA = standardSCBA;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_PPE GetFGApplicationPPE(Guid applicationId)
		{
			FG_App_PPE result = null;
			try
			{
				result = cwmContext.FG_App_PPEs.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SavePPEAsync(DetailedFGAppPPE model)
		{
			try
			{
				if (model != null)
				{
					bool isNew = false;
					var ppe = await cwmContext.FG_App_PPEs.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (ppe == null)
					{
						isNew = true;
						ppe = cwmContext.FG_App_PPEs.Add(new NMSFM.Data.FG_App_PPE());
						ppe.Id = Guid.NewGuid();
						ppe.DateEntered = DateTime.Now;
						ppe.ApplicationId = model.ApplicationId;

					}

					ppe.PPEPartOfProject = model.PPEPartOfProject;
					ppe.SCBAPartOfProject = model.SCBAPartOfProject;
					ppe.PPEInspected = model.PPEInspected;
					ppe.AdminComments = model.AdminComments;

					ppe.DateUpdated = DateTime.Now;
					ppe.UpdatedBy = model.UpdatedBy;
					ppe.IsValid = model.IsValid;
					ppe.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save ppe for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save ppe for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					
					if (retbol == true)
					{
						if (isNew)
						{
							if (model.StandardPPE != null)
							{
								foreach (FG_App_StandardPPE standardPPE in model.StandardPPE)
								{
									await SaveStandardPPE(standardPPE);
								}
							}
							if (model.StandardSCBA != null)
							{
								foreach (FG_App_StandardSCBA standardSCBA in model.StandardSCBA)
								{
									await SaveStandardSCBA(standardSCBA);
								}
							}
						}
						else
						{
							List<FG_App_StandardPPE> existingStandardPPE = await cwmContext.FG_App_StandardPPEs.Where(a => a.ApplicationId == ppe.ApplicationId).ToListAsync();
							bool isDelete = true;
							foreach (FG_App_StandardPPE existStandardPPE in existingStandardPPE)
							{
								foreach (FG_App_StandardPPE newStandardPPE in model.StandardPPE)
								{
									if (existStandardPPE.StandardComplientPPEId.ToString() == newStandardPPE.StandardComplientPPEId.ToString())
									{
										isDelete = false;
									}
								}
								if (isDelete)
								{
									cwmContext.FG_App_StandardPPEs.Remove(existStandardPPE);
								}
								isDelete = true;
							}
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.StandardPPE != null)
							{
								foreach (FG_App_StandardPPE standardPPE in model.StandardPPE)
								{
									await SaveStandardPPE(standardPPE);
								}
							}
							//--------------------------------------------------------------------
							List<FG_App_StandardSCBA> existingStandardSCBA = await cwmContext.FG_App_StandardSCBAs.Where(a => a.ApplicationId == ppe.ApplicationId).ToListAsync();
							isDelete = true;
							foreach (FG_App_StandardSCBA existStandardSCBA in existingStandardSCBA)
							{
								foreach (FG_App_StandardSCBA newStandardSCBA in model.StandardSCBA)
								{
									if (existStandardSCBA.StandardComplientSCBAId.ToString() == newStandardSCBA.StandardComplientSCBAId.ToString())
									{
										isDelete = false;
									}
								}
								if (isDelete)
								{
									cwmContext.FG_App_StandardSCBAs.Remove(existStandardSCBA);
								}
								isDelete = true;
							}
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.StandardSCBA != null)
							{
								foreach (FG_App_StandardSCBA standardSCBA in model.StandardSCBA)
								{
									await SaveStandardSCBA(standardSCBA);
								}
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null hazards/threats info.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SaveStandardPPE(FG_App_StandardPPE model)
		{
			try
			{
				if (model != null)
				{
					var standardPPE = await cwmContext.FG_App_StandardPPEs.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.StandardComplientPPEId == model.StandardComplientPPEId);
					if (standardPPE == null)
					{
						standardPPE = cwmContext.FG_App_StandardPPEs.Add(new NMSFM.Data.FG_App_StandardPPE());
						standardPPE.StandardComplientPPEId = Guid.NewGuid();
						standardPPE.ApplicationId = model.ApplicationId;
					}

					standardPPE.PPEType = model.PPEType;
					standardPPE.Year = model.Year;
					standardPPE.Quantity = model.Quantity;
					standardPPE.Age = model.Age;
					standardPPE.Condition = model.Condition;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save standard ppe for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save standard ppe for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null standard ppe information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SaveStandardSCBA(FG_App_StandardSCBA model)
		{
			try
			{
				if (model != null)
				{
					var standardSCBA = await cwmContext.FG_App_StandardSCBAs.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.StandardComplientSCBAId == model.StandardComplientSCBAId);
					if (standardSCBA == null)
					{
						standardSCBA = cwmContext.FG_App_StandardSCBAs.Add(new NMSFM.Data.FG_App_StandardSCBA());
						standardSCBA.StandardComplientSCBAId = Guid.NewGuid();
						standardSCBA.ApplicationId = model.ApplicationId;
					}

					standardSCBA.SCBAType = model.SCBAType;
					standardSCBA.Year = model.Year;
					standardSCBA.Quantity = model.Quantity;
					standardSCBA.Age = model.Age;
					standardSCBA.Condition = model.Condition;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save standard ppe for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save standard ppe for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null standard ppe information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGAppEquipmentNeeds> GetFGApplicationEquipmentNeedsAsync(Guid applicationId)
		{
			FG_App_EquipmentNeeds results = null;
			List<FG_App_ApplicationEquipment> applicationEquipment = new List<FG_App_ApplicationEquipment>();
			DetailedFGAppEquipmentNeeds result = new DetailedFGAppEquipmentNeeds();
			try
			{
				results = await cwmContext.FG_App_EquipmentNeeds.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.SpecificNeeds = results.SpecificNeeds;
					result.ISOImpacted = results.ISOImpacted;
					result.ISOImpactExplanation = results.ISOImpactExplanation;
					result.AdminComments = results.AdminComments;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					applicationEquipment = await cwmContext.FG_App_ApplicationEquipments.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
					result.ApplicationEquipment = applicationEquipment;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_EquipmentNeeds GetFGApplicationEquipmentNeeds(Guid applicationId)
		{
			FG_App_EquipmentNeeds result = null;
			try
			{
				result = cwmContext.FG_App_EquipmentNeeds.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveEquipmentNeedsAsync(DetailedFGAppEquipmentNeeds model)
		{
			try
			{
				if (model != null)
				{
					bool isNew = false;
					var equipmentNeeds = await cwmContext.FG_App_EquipmentNeeds.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (equipmentNeeds == null)
					{
						isNew = true;
						equipmentNeeds = cwmContext.FG_App_EquipmentNeeds.Add(new NMSFM.Data.FG_App_EquipmentNeeds());
						equipmentNeeds.Id = Guid.NewGuid();
						equipmentNeeds.DateEntered = DateTime.Now;
						equipmentNeeds.ApplicationId = model.ApplicationId;

					}
					equipmentNeeds.SpecificNeeds = model.SpecificNeeds;
					equipmentNeeds.ISOImpacted = model.ISOImpacted;
					equipmentNeeds.ISOImpactExplanation = model.ISOImpactExplanation;
					equipmentNeeds.AdminComments = model.AdminComments;

					equipmentNeeds.DateUpdated = DateTime.Now;
					equipmentNeeds.UpdatedBy = model.UpdatedBy;
					equipmentNeeds.IsValid = model.IsValid;
					equipmentNeeds.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save equipment needs for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to saveequipment needss for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					if (retbol == true)
					{
						if (isNew)
						{
							if (model.ApplicationEquipment != null)
							{
								foreach (FG_App_ApplicationEquipment applicationEquipment in model.ApplicationEquipment)
								{
									await SaveApplicationEquipment(applicationEquipment);
								}
							}
						}
						else
						{
							List<FG_App_ApplicationEquipment> existingApplicationEquipment = await cwmContext.FG_App_ApplicationEquipments.Where(a => a.ApplicationId == equipmentNeeds.ApplicationId).ToListAsync();
							bool isDelete = true;
							foreach (FG_App_ApplicationEquipment existApplicationEquipment in existingApplicationEquipment)
							{
								foreach (FG_App_ApplicationEquipment newApplicationEquipment in model.ApplicationEquipment)
								{
									if (existApplicationEquipment.EquipmentId.ToString() == newApplicationEquipment.EquipmentId.ToString())
									{
										isDelete = false;
									}
								}
								if (isDelete)
								{
									cwmContext.FG_App_ApplicationEquipments.Remove(existApplicationEquipment);
								}
								isDelete = true;
							}
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.ApplicationEquipment != null)
							{
								foreach (FG_App_ApplicationEquipment applicationEquipment in model.ApplicationEquipment)
								{
									await SaveApplicationEquipment(applicationEquipment);
								}
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null equipment needs info.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SaveApplicationEquipment(FG_App_ApplicationEquipment model)
		{
			try
			{
				if (model != null)
				{
					var applicationEquipment = await cwmContext.FG_App_ApplicationEquipments.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.EquipmentId == model.EquipmentId);
					if (applicationEquipment == null)
					{
						applicationEquipment = cwmContext.FG_App_ApplicationEquipments.Add(new NMSFM.Data.FG_App_ApplicationEquipment());
						applicationEquipment.EquipmentId = Guid.NewGuid();
						applicationEquipment.ApplicationId = model.ApplicationId;
					}

					applicationEquipment.Number = model.Number;
					applicationEquipment.PriorityCategory = model.PriorityCategory;
					applicationEquipment.EquipmentNeeded = model.EquipmentNeeded;
					applicationEquipment.Quantity = model.Quantity;
					applicationEquipment.Cost = model.Cost;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save application equipment for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save appllication equipment for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null application equipment.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<FG_App_FundingJustification> GetFGApplicationFundingJustificationAsync(Guid applicationId)
		{
			FG_App_FundingJustification result = null;
			try
			{
				result = await cwmContext.FG_App_FundingJustifications.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_FundingJustification GetFGApplicationFundingJustification(Guid applicationId)
		{
			FG_App_FundingJustification result = null;
			try
			{
				result = cwmContext.FG_App_FundingJustifications.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveFundingJustificationAsync(FG_App_FundingJustification model)
		{
			try
			{
				if (model != null)
				{
					var fundingJustification = await cwmContext.FG_App_FundingJustifications.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (fundingJustification == null)
					{
						fundingJustification = cwmContext.FG_App_FundingJustifications.Add(new NMSFM.Data.FG_App_FundingJustification());
						fundingJustification.Id = Guid.NewGuid();
						fundingJustification.DateEntered = DateTime.Now;
						fundingJustification.ApplicationId = model.ApplicationId;
					}

					fundingJustification.CriticalNeed = model.CriticalNeed;
					fundingJustification.FinancialNeed = model.FinancialNeed;
					fundingJustification.Problem = model.Problem;
					fundingJustification.BenefitToCommunity = model.BenefitToCommunity;
					fundingJustification.Consequences = model.Consequences;

					fundingJustification.FinancialNeedComments = model.FinancialNeedComments;
					fundingJustification.ProblemComments = model.ProblemComments;
					fundingJustification.BenefitComments = model.BenefitComments;
					fundingJustification.ConsequencesComments = model.ConsequencesComments;
					fundingJustification.FinancialNeedGrade = model.FinancialNeedGrade;
					fundingJustification.ProblemGrade = model.ProblemGrade;
					fundingJustification.BenefitGrade = model.BenefitGrade;
					fundingJustification.ConsequencesGrade = model.ConsequencesGrade;

					fundingJustification.DateUpdated = DateTime.Now;
					fundingJustification.UpdatedBy = model.UpdatedBy;
					fundingJustification.IsValid = model.IsValid;
					fundingJustification.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save budget information for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save budget information for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null budget information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<FG_App_ProjectBudget> GetFGApplicationProjectBudgetAsync(Guid applicationId)
		{
			FG_App_ProjectBudget result = null;
			try
			{
				result = await cwmContext.FG_App_ProjectBudgets.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_ProjectBudget GetFGApplicationProjectBudget(Guid applicationId)
		{
			FG_App_ProjectBudget result = null;
			try
			{
				result = cwmContext.FG_App_ProjectBudgets.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveProjectBudgetAsync(FG_App_ProjectBudget model)
		{
			try
			{
				if (model != null)
				{
					var projectBudget = await cwmContext.FG_App_ProjectBudgets.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (projectBudget == null)
					{
						projectBudget = cwmContext.FG_App_ProjectBudgets.Add(new NMSFM.Data.FG_App_ProjectBudget());
						projectBudget.Id = Guid.NewGuid();
						projectBudget.DateEntered = DateTime.Now;
						projectBudget.ApplicationId = model.ApplicationId;
					}

					projectBudget.TotalProjectCost = model.TotalProjectCost;
					projectBudget.AmountRequested = model.AmountRequested;
					projectBudget.DepartmentResponsibility = model.DepartmentResponsibility;
					projectBudget.StipendAmount = model.StipendAmount;

					projectBudget.AdminComments = model.AdminComments;

					projectBudget.DateUpdated = DateTime.Now;
					projectBudget.UpdatedBy = model.UpdatedBy;
					projectBudget.IsValid = model.IsValid;
					projectBudget.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save project budget information for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save project budget information for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null project budget information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<DetailedFGAppSigsDocs> GetFGApplicationDocsSigsAsync(Guid applicationId)
		{
			FG_App_DocsSigs results = null;
			List<FG_App_Documents> documents = new List<FG_App_Documents>();
			List<FG_AppDocListItem> documentItems = new List<FG_AppDocListItem>();
			List<FG_App_Signatures> signautures = new List<FG_App_Signatures>();
			DetailedFGAppSigsDocs result = new DetailedFGAppSigsDocs();
			try
			{
				results = await cwmContext.FG_App_DocsSigs.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.DocumentNumber = results.DocumentNumber;
					result.SignaturesCollected = results.SignaturesCollected;
					result.AppCompletenessGrade = results.AppCompletenessGrade;
					result.AdminComments = results.AdminComments;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					documents = await cwmContext.FG_App_Documents.Where(a => a.ApplicationId == result.ApplicationId).ToListAsync();
					if (documents.Any())
                    {
						foreach (FG_App_Documents doc in documents)
                        {
							FG_AppDocListItem docItem = new FG_AppDocListItem();
							docItem.DocumentId = doc.DocumentId;
							docItem.ApplicationId = doc.ApplicationId;
							docItem.DocumentName = doc.DocumentName;
							docItem.DocumentType = doc.DocumentType;
							documentItems.Add(docItem);
                        }
                    }
					result.Documents = documentItems;

					signautures = await cwmContext.FG_App_Signatures.Where(a => a.ApplicationId == result.ApplicationId && a.FromReview == false && a.FromStatus == false).ToListAsync();
					result.Signatures = signautures;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_DocsSigs GetApplicationDocsSigs(Guid applicationId)
		{
			FG_App_DocsSigs result = null;
			try
			{
				result = cwmContext.FG_App_DocsSigs.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveApplicationDocsSigsAsync(DetailedFGAppSigsDocs model)
		{
			try
			{
				if (model != null)
				{
					bool isNew = false;
					var docssigs = await cwmContext.FG_App_DocsSigs.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (docssigs == null)
					{
						isNew = true;
						docssigs = cwmContext.FG_App_DocsSigs.Add(new NMSFM.Data.FG_App_DocsSigs());
						docssigs.Id = Guid.NewGuid();
						docssigs.DateEntered = DateTime.Now;
						docssigs.ApplicationId = model.ApplicationId;

					}

					docssigs.DocumentNumber = model.DocumentNumber;
					docssigs.SignaturesCollected = model.SignaturesCollected;
					docssigs.AppCompletenessGrade = model.AppCompletenessGrade;
					docssigs.AdminComments = model.AdminComments;

					docssigs.DateUpdated = DateTime.Now;
					docssigs.UpdatedBy = model.UpdatedBy;
					docssigs.IsValid = model.IsValid;
					docssigs.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save application documents/signatures for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save application documents/signatures for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}

					if (retbol == true)
					{
						if (isNew)
						{
							//if (model.Documents != null)
							//{
							//	foreach (FG_App_Documents document in model.Documents)
							//	{
							//		await SaveApplicationDocument(document);
							//	}
							//}
							if (model.Signatures != null)
							{
								foreach (FG_App_Signatures signature in model.Signatures)
								{
									await SaveApplicationSignatures(signature);
								}
							}
						}
						else
						{
							//List<FG_App_Documents> existingDocuments = await cwmContext.FG_App_Documents.Where(a => a.ApplicationId == docssigs.ApplicationId).ToListAsync();
							bool isDelete = true;
							//foreach (FG_App_Documents existDocument in existingDocuments)
							//{
							//	foreach (FG_App_Documents newDocument in model.Documents)
							//	{
							//		if (existDocument.DocumentId.ToString() == newDocument.DocumentId.ToString())
							//		{
							//			isDelete = false;
							//		}
							//	}
							//	if (isDelete)
							//	{
							//		cwmContext.FG_App_Documents.Remove(existDocument);
							//	}
							//	isDelete = true;
							//}
							//await ((DbContext)cwmContext).SaveChangesAsync();
							//if (model.Documents != null)
							//{
							//	foreach (FG_App_Documents document in model.Documents)
							//	{
							//		await SaveApplicationDocument(document);
							//	}
							//}
							//--------------------------------------------------------------------
							List<FG_App_Signatures> existingSignatures = await cwmContext.FG_App_Signatures.Where(a => a.ApplicationId == docssigs.ApplicationId && a.FromReview == false && a.FromStatus == false).ToListAsync();
							isDelete = true;
							foreach (FG_App_Signatures existSignature in existingSignatures)
							{
								foreach (FG_App_Signatures newSignature in model.Signatures)
								{
									if (existSignature.SignatureId.ToString() == newSignature.SignatureId.ToString())
									{
										isDelete = false;
									}
								}
								if (isDelete)
								{
									cwmContext.FG_App_Signatures.Remove(existSignature);
								}
								isDelete = true;
							}
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (model.Signatures != null)
							{
								foreach (FG_App_Signatures signature in model.Signatures)
								{
									await SaveApplicationSignatures(signature);
								}
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null document/signature info.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<bool> SaveApplicationDocumentAsync(FG_App_Documents model)
		{
			try
			{
				if (model != null)
				{
					var document = await cwmContext.FG_App_Documents.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.DocumentId == model.DocumentId);
					if (document == null)
					{
						document = cwmContext.FG_App_Documents.Add(new NMSFM.Data.FG_App_Documents());
						document.DocumentId = Guid.NewGuid();
						document.ApplicationId = model.ApplicationId;
					}

					document.DocumentType = model.DocumentType;
					document.DocumentName = model.DocumentName;
					document.Document = model.Document;
					document.DocType = model.DocType;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save app document for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save app document for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null app document information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<bool> DeleteApplicationDocumentAsync(Guid id)
        {
			try
			{
				if (id != null)
				{
					FG_App_Documents doc = await cwmContext.FG_App_Documents.SingleOrDefaultAsync(a => a.DocumentId == id);
					if (doc == null)
                    {
						throw new Exception("Unable to delete null app document information.");
					}

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							cwmContext.FG_App_Documents.Remove(doc);
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save app document.", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save app document, DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to delete null app document information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<FG_App_Documents> GetApplicationDocumentByIdAsync(Guid id)
		{
			FG_App_Documents result = null;
			try
			{
				result = await cwmContext.FG_App_Documents.SingleOrDefaultAsync(a => a.DocumentId == id);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveApplicationSignatures(FG_App_Signatures model)
		{
			try
			{
				if (model != null)
				{
					var signature = await cwmContext.FG_App_Signatures.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.SignatureId == model.SignatureId);
					if (signature == null)
					{
						signature = cwmContext.FG_App_Signatures.Add(new NMSFM.Data.FG_App_Signatures());
						signature.SignatureId = Guid.NewGuid();
						signature.ApplicationId = model.ApplicationId;
					}

					signature.SignatureRole = model.SignatureRole;
					signature.PrintedName = model.PrintedName;
					signature.EmailAddress = model.EmailAddress;					
					signature.DateEntered = model.DateEntered;
					signature.EnteredBy = model.EnteredBy;
					signature.WebUserId = model.WebUserId;
					signature.LoginToken = model.LoginToken;
					signature.FromReview = model.FromReview;
					signature.FromStatus = model.FromStatus;

					if (model.Signature != null && model.Signature != "")
                    {
						signature.Signature = model.Signature;
						signature.DateSigned = model.DateSigned;
						signature.SignedBy = model.SignedBy;
					}
                    else
                    {
						signature.Signature = null;
						signature.DateSigned = null;
						signature.SignedBy = null;
                    }

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save signature for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save signature for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null signature information.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<FG_App_Signatures> GetSignatorByToken(string applicationId, string loginToken)
        {
			FG_App_Signatures signator = new FG_App_Signatures();
			Guid appId = new Guid(applicationId);
            try
            {
				signator = await cwmContext.FG_App_Signatures.SingleOrDefaultAsync(a => a.ApplicationId == appId && a.LoginToken == loginToken);
            }
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return signator;
		}

		public async Task<FG_App_Signatures> GetReviewerSignature(string applicationId)
        {
			FG_App_Signatures signator = new FG_App_Signatures();
			Guid appId = new Guid(applicationId);
			try
			{
				signator = await cwmContext.FG_App_Signatures.SingleOrDefaultAsync(a => a.ApplicationId == appId && a.FromStatus == true);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return signator;
		}

		public async Task<FG_App_Signatures> GetCounselorSignature(string applicationId)
		{
			FG_App_Signatures signator = new FG_App_Signatures();
			Guid appId = new Guid(applicationId);
			try
			{
				signator = await cwmContext.FG_App_Signatures.SingleOrDefaultAsync(a => a.ApplicationId == appId && a.FromReview == true);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return signator;
		}

		public async Task<DetailedFGAppReview> GetFGApplicationReviewAsync(Guid applicationId)
		{
			FG_App_Review results = null;
			FG_App_Signatures reviewerSignature = new FG_App_Signatures();
			List<FG_App_Signatures> appSignatures = new List<FG_App_Signatures>();
			DetailedFGAppReview result = new DetailedFGAppReview();
			try
			{
				results = await cwmContext.FG_App_Reviews.SingleOrDefaultAsync(a => a.ApplicationId == applicationId);
				if (results != null)
				{
					result.Id = results.Id;
					result.ApplicationId = results.ApplicationId;
					result.NFIRSCompliant = results.NFIRSCompliant;
					result.PumpTestCompliant = results.PumpTestCompliant;
					result.HoseTestCompliant = results.HoseTestCompliant;
					result.AckComSigs = results.AckComSigs;
					result.SpecsReceived = results.SpecsReceived;
					result.Notes = results.Notes;
					result.DateEntered = results.DateEntered;
					result.DateUpdated = results.DateUpdated;
					result.UpdatedBy = results.UpdatedBy;
					result.IsValid = results.IsValid;
					result.InvalidText = results.InvalidText;

					appSignatures = await cwmContext.FG_App_Signatures.Where(a => a.ApplicationId == result.ApplicationId && a.FromReview == true && a.FromStatus == false).ToListAsync();
					//reviewerSignature = await cwmContext.FG_App_Signatures.FirstOrDefaultAsync(a => a.ApplicationId == result.ApplicationId && a.FromReview == true && a.FromStatus == false);
					//result.ReviewerSignature = reviewerSignature;
					result.AppSignatures = appSignatures;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public FG_App_Review GetApplicationReview(Guid applicationId)
		{
			FG_App_Review result = null;
			try
			{
				result = cwmContext.FG_App_Reviews.SingleOrDefault(a => a.ApplicationId == applicationId);
			}
			catch (Exception ex)
            {
                _ = ex;
				throw ex;
			}

			return result;
		}

		public async Task<bool> SaveApplicationReviewAsync(DetailedFGAppReview model, bool fromStatus = false)
		{
			try
			{
				if (model != null)
				{
					var review = await cwmContext.FG_App_Reviews.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);
					if (review == null)
					{
						review = cwmContext.FG_App_Reviews.Add(new NMSFM.Data.FG_App_Review());
						review.Id = Guid.NewGuid();
						review.DateEntered = DateTime.Now;
						review.ApplicationId = model.ApplicationId;

					}

					if (fromStatus)
                    {
						review.NFIRSCompliant = model.NFIRSCompliant;
						review.PumpTestCompliant = model.PumpTestCompliant;
						review.HoseTestCompliant = model.HoseTestCompliant;
						review.AckComSigs = model.AckComSigs;
						review.SpecsReceived = model.SpecsReceived;
					}
                    else
                    {
						review.Notes = model.Notes;
					}
					

					review.DateUpdated = DateTime.Now;
					review.UpdatedBy = model.UpdatedBy;
					review.IsValid = model.IsValid;
					review.InvalidText = model.InvalidText;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save application review for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save application review for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}

					if (retbol == true && fromStatus == false)
					{
						if (model.ReviewerSignature != null)
						{
							await SaveApplicationSignatures(model.ReviewerSignature);
						}
                        else
                        {
							FG_App_Signatures existingSignature = await cwmContext.FG_App_Signatures.FirstOrDefaultAsync(a => a.ApplicationId == review.ApplicationId && a.FromReview == true && a.FromStatus);
							if (existingSignature != null)
							{
								cwmContext.FG_App_Signatures.Remove(existingSignature);
								await ((DbContext)cwmContext).SaveChangesAsync();
							}
						}
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null application review info.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<bool> SaveCounselorScores(DetailedFGAppScores model)
		{
			try
			{
				if (model != null)
				{

					bool isNew = false;
					var scores = await cwmContext.FG_App_Scores.SingleOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.WebUserId == model.WebUserId);
					if (scores == null)
					{
						isNew = true;
						scores = cwmContext.FG_App_Scores.Add(new NMSFM.Data.FG_App_Scores());
						scores.ScoreId = Guid.NewGuid();
						scores.DateEntered = DateTime.Now;
						scores.ApplicationId = model.ApplicationId;
						scores.WebUserId = new Guid(model.WebUserId.ToString());
						scores.UserName = model.UserName;
					}

					scores.TrainingScore = (model.TrainingPoints != 0) ? model.TrainingPoints : scores.TrainingScore;
					scores.FinancialNeedScore = (model.FinancialNeedGrade != 0) ? model.FinancialNeedGrade : scores.FinancialNeedScore;
					scores.ProblemScore = (model.ProblemGrade != 0) ? model.ProblemGrade : scores.ProblemScore;
					scores.BenefitScore = (model.BenefitGrade != 0) ? model.BenefitGrade : scores.BenefitScore;
					scores.ConsequencesScore = (model.ConsequencesGrade != 0) ? model.ConsequencesGrade : scores.ConsequencesScore;
					scores.CompletenessScore = (model.AppCompletenessGrade != 0) ? model.AppCompletenessGrade : scores.CompletenessScore;

					bool retbol = false;
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							retbol = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save training data for " + model.ApplicationId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save score data for " + model.ApplicationId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null scores.");
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<v_Addresses2> GetRemittanceAddress(bool isCity, string deptName)
		{
			v_Addresses2 result = null;
			string addressTypeId = "";

			if (isCity)
			{
				addressTypeId = "90593d74-c6fa-463b-b843-7d64fbce216f"; //FPF_Remittance (City)
				result = await cwmContext.v_Addresses2.FirstOrDefaultAsync(a => a.AddressTypeId.ToString() == addressTypeId && a.AddressCode == deptName);
			}
			else
			{
				addressTypeId = "095dd9ae-217b-456f-926f-9bc06213af7d"; //FPF_Remittance (County)
				result = await cwmContext.v_Addresses2.FirstOrDefaultAsync(a => a.AddressTypeId.ToString() == addressTypeId && a.AddressCode == deptName);
			}

			return result;
		}
	}
}

