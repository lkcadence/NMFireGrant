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
    public class FGService : IFGService
    {
		private ICodepalWebModel cwmContext;
		private ILogging logger;
		private IAddressService addressService;
		private List<string> imageSuffixes = new List<string> { ".bmp", ".gif", ".jpeg", ".png", ".tiff", ".tif", ".jpg", ".pdf", ".doc", ".docx", ".xls", ".xlsx" };

		private Guid NFIRSCont1RoleId = new Guid("d89fe814-de10-4bd7-9bf4-27e7011956b9");
		private Guid NFIRSCont2RoleId = new Guid("bdbe3cef-b8ea-4364-aa1c-0f66a1353d36");
		private Guid mainAddType = new Guid("4d8edb54-c7f1-41e6-925f-074d2f9719f5");
		private Guid SubAddType = new Guid("0b12d3c5-ea00-46ac-8b40-0e6993ddcc1e");
		private Guid AdminAddType = new Guid("456680a9-4e3b-44dc-9a3a-d596b1bff605");
		//Guid agencyId = new Guid(HttpContext.Current.Session["AgencyId"].ToString());

		Guid deptAddType = new Guid("43856752-8b7a-4e6f-b697-bf8acd457c16");
		Guid mainGuid = new Guid("7ad61001-cac8-4f3c-ae4e-32d28393f891");
		Guid adminGuid = new Guid("8baa0b86-f1e5-4d84-b4f9-a8219f4b11b8");
		Guid subGuid = new Guid("4f34b96d-d944-44aa-9665-d47c55cc025d");
		Guid isoGuid = new Guid("6b8517ef-9483-4b8b-8c95-5b95a6b8f579");

		Guid respPartyRoleId = new Guid("ba5f97d0-10d6-4fdd-8881-bc30b0e083af");
		Guid respFGPartyRoleId = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");

		public FGService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			addressService = new AddressService(cwmContext, logger);
		}

		public async Task<FGApplicationSettings> GetFireGrantAppSettings(short year)
		{
			FGApplicationSettings result = null;
			try
			{
				result = await cwmContext.FGApplicationSettings.SingleOrDefaultAsync(f => f.FiscalYear == year);

				if (result == null)
				{
					await CreateNewFireGrantSettings(year);

					result = await cwmContext.FGApplicationSettings.SingleOrDefaultAsync(f => f.FiscalYear == year);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant Settings for Year '" + year.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<List<FG_Categories>> GetFGCategories()
		{
			List<FG_Categories> result = null;
			try
			{
				result = await cwmContext.FG_Categories.ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant Categories.", ex);
			}

			return result;
		}

		public async Task<List<FG_FDIDs>> GetFG_FDIDs()
		{
			List<FG_FDIDs> result = null;
			try
			{
				result = await cwmContext.FG_FDIDs.ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant FDIDs.", ex);
			}

			return result;
		}
		public async Task<FG_FDIDs> GetFG_FDID(int fdid)
		{
			FG_FDIDs result = null;
			try
			{
				result = await cwmContext.FG_FDIDs.FirstOrDefaultAsync(a => a.FDID == fdid.ToString());
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Department ID.", ex);
			}

			return result;
		}

		private async Task<FG_FDIDs> GetFGFDIDByDepartment(string department)
		{
			FG_FDIDs result = null;
			try
			{
				result = await cwmContext.FG_FDIDs.FirstOrDefaultAsync(a => a.FireDepartment == department);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Department ID.", ex);
			}

			return result;
		}

		public async Task<bool> SaveFDIDAsync(FG_FDIDs model)
		{
			try
			{
				if (model != null)
				{
					var fdid = await cwmContext.FG_FDIDs.SingleOrDefaultAsync(a => a.FDID == model.FDID);
					if (fdid != null)
					{
						throw new Exception("Category Already Exists");
					}

					fdid = cwmContext.FG_FDIDs.Add(new NMSFM.Data.FG_FDIDs());
					fdid.FDID = model.FDID;
					fdid.FireDepartment = model.FireDepartment;
					fdid.Inactive = model.Inactive;

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
							logger.Error("Unable to create FIDI for '" + model.FireDepartment.ToString() + "'.", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to create FDID '" + model.FireDepartment.ToString() + "', DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null FDID.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<bool> UpdateFDIDAsync(FG_FDIDs model)
		{
			try
			{
				if (model != null)
				{
					var fdid = await cwmContext.FG_FDIDs.SingleOrDefaultAsync(a => a.FDID == model.FDID);
					if (fdid == null)
					{
						logger.Error("Unable to update FDID for '" + model.FireDepartment.ToString() + "', FDID was not found.");
						return false;
					}


					fdid.FireDepartment = model.FireDepartment;
					fdid.Inactive = model.Inactive;

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
							logger.Error("Unable to update FDID for '" + model.FireDepartment.ToString() + "'.", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to update FDID for '" + model.FDID.ToString() + "', DbContext was not available.");
						retbol = false;
					}

					return retbol;
				}
				else
				{
					throw new Exception("Unable to update null FDID.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<FG_FDIDs> IsFDIDValid(int fdid)
		{
			FG_FDIDs result = null;
			string strFDID = fdid.ToString();
			if (strFDID.Length == 4) { strFDID = "0" + strFDID; }
			try
			{
				result = await cwmContext.FG_FDIDs.FirstOrDefaultAsync(a => a.FDID == strFDID);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant FDIDs.", ex);
			}

			return result;
		}

		public async Task<FG_Categories> GetFGCategory(int categoryId)
		{
			FG_Categories result = null;
			try
			{
				result = await cwmContext.FG_Categories.FirstOrDefaultAsync(a => a.CategoryId == categoryId);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant Category.", ex);
			}

			return result;
		}

		private async Task<FG_Categories> GetFGCategoryByName(string name)
		{
			FG_Categories result = null;
			try
			{
				result = await cwmContext.FG_Categories.FirstOrDefaultAsync(a => a.CategoryName == name);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant Category.", ex);
			}

			return result;
		}

		public async Task<bool> SaveCategoryAsync(DetailedFGCategory model)
        {
			try
			{
				if (model != null)
				{
					var category = await cwmContext.FG_Categories.SingleOrDefaultAsync(a => a.CategoryId == model.CategoryId);
					if (category != null)
					{
						return false;
					}

					category = await cwmContext.FG_Categories.SingleOrDefaultAsync(a => a.CategoryName == model.CategoryName);
					if (category != null)
					{
						throw new Exception("Category Already Exists");
					}

					category = cwmContext.FG_Categories.Add(new NMSFM.Data.FG_Categories());
					category.CategoryName = model.CategoryName;
					category.Inactive = model.Inactive;
					category.DateCreated = DateTime.Now;

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
							logger.Error("Unable to create category '" + model.CategoryName.ToString() + "'.", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to create category '" + model.CategoryName.ToString() + "', DbContext was not available.");
						retbol = false;
					}

					if (retbol == true)
                    {
						int catId = 0;
						FG_Categories newcat = await GetFGCategoryByName(model.CategoryName);
						if (newcat != null)
						{
							catId = newcat.CategoryId;
						}

						if (model.Priorities != null)
                        {
							foreach (FG_Priorities priority in model.Priorities)
                            {
								priority.CategoryId = catId;
								await SavePriority(priority);
                            }
                        }

					}
					

					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null category.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<bool> UpdateCategoryAsync(DetailedFGCategory model)
		{
			try
			{
				if (model != null)
				{
					var category = await cwmContext.FG_Categories.SingleOrDefaultAsync(a => a.CategoryId == model.CategoryId);
					if (category == null)
					{
						logger.Error("Unable to update category '" + model.CategoryName.ToString() + "', Category was not found.");
						return false;
					}

					
					category.CategoryName = model.CategoryName;
					category.Inactive = model.Inactive;
					category.DateModified = DateTime.Now;

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
							logger.Error("Unable to update category '" + model.CategoryName.ToString() + "'.", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to update category '" + model.CategoryName.ToString() + "', DbContext was not available.");
						retbol = false;
					}

					if (retbol == true)
					{
						int catId = model.CategoryId;

						if (model.Priorities != null)
						{
							foreach (FG_Priorities priority in model.Priorities)
							{
								var isPriority = await cwmContext.FG_Priorities.SingleOrDefaultAsync(a => a.CategoryId == model.CategoryId && a.PriorityId == priority.PriorityId);
								if (isPriority == null)
                                {
									priority.CategoryId = catId;
									await SavePriority(priority);
								}
                                else
                                {
									priority.CategoryId = catId;
									await UpdatePriority(priority);
								}
								
							}
						}
					}


					return retbol;
				}
				else
				{
					throw new Exception("Unable to update null category.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> SavePriority(FG_Priorities model)
        {
			try
			{
				if (model != null)
				{
					var priority = await cwmContext.FG_Priorities.SingleOrDefaultAsync(a => a.CategoryId == model.CategoryId && a.PriorityName == model.PriorityName);

					if (priority != null)
					{
						return false;
					}

					priority = cwmContext.FG_Priorities.Add(new NMSFM.Data.FG_Priorities());
					priority.CategoryId = model.CategoryId;
					priority.PriorityName = model.PriorityName;
					priority.Inactive = model.Inactive;
					priority.DateCreated = DateTime.Now;

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
							logger.Error("Unable to create priority '" + model.PriorityName.ToString() + "'.", ex);
							return false;
						}
					}
					else
					{
						logger.Error("Unable to create priority '" + model.PriorityName.ToString() + "', DbContext was not available.");
						return false;
					}
				}
				else
				{
					throw new Exception("Unable to save null priority");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		private async Task<bool> UpdatePriority(FG_Priorities model)
		{
			try
			{
				if (model != null)
				{
					var priority = await cwmContext.FG_Priorities.SingleOrDefaultAsync(a => a.CategoryId == model.CategoryId && a.PriorityId == model.PriorityId);

					if (priority == null)
					{
						logger.Error("Unable to update priority '" + priority.PriorityName.ToString() + "'.  The priority could not be located in the database.");
						return false;
					}

					priority.CategoryId = model.CategoryId;
					priority.PriorityName = model.PriorityName;
					priority.Inactive = model.Inactive;
					priority.DateModified = DateTime.Now;

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
							logger.Error("Unable to update priority '" + model.PriorityName.ToString() + "'.", ex);
							return false;
						}
					}
					else
					{
						logger.Error("Unable to update priority '" + model.PriorityName.ToString() + "', DbContext was not available.");
						return false;
					}
				}
				else
				{
					throw new Exception("Unable to update null priority");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

		public async Task<List<FG_Priorities>> GetFGPriorities(int categoryId)
		{
			List<FG_Priorities> result = null;
			try
			{
				result = await cwmContext.FG_Priorities.Where(a => a.CategoryId == categoryId).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant Priorities.", ex);
			}

			return result;
		}

		private async Task<bool> CreateNewFireGrantSettings(short year)
		{
			var fgAppSettings = cwmContext.FGApplicationSettings.Add(new FGApplicationSettings());
			fgAppSettings.FiscalYear = year;
			fgAppSettings.StartDate = Convert.ToDateTime("7/1/" + (year).ToString());
			fgAppSettings.EndDate = Convert.ToDateTime("9/30/" + (year).ToString());
			fgAppSettings.MaxGrantAmount = 400000;
			fgAppSettings.DefaultPageContent = noDefaultPageContent();
			fgAppSettings.DefaultPageHeader = noDefaultPageHeader();
			fgAppSettings.PumpTestStatute = defaultPumpTestStatute();
			fgAppSettings.HoseTestStatute = defaulHoseTestStatute();
			fgAppSettings.EligibilityRequirementsText = GetDefaultEligibilityRequirements();
			fgAppSettings.eSignatureLegalText = "The New Mexico State Fire Marshal Fire Grant Council will now be accepting electronic signatures on submitted Fire Grant Applications. Each signer agrees that this Fire Grant Application may be electronically signed, and that any electronic signatures appearing on this Fire Grant Application are the same as handwritten signatures for the purposes of validity, enforceability, and admissibility.";
			fgAppSettings.faCertifiationText = "all information in this application is true and correct to the best of my knowledge.";

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
					logger.Error("Unable to create New Allowable Distribution Record.", ex);
					return false;
				}
			}
			else
			{
				logger.Error("Unable to create New Allowable Distribution Record., DbContext was not available.");
				return false;
			}
		}

		private string noDefaultPageHeader()
        {
			string defaultPageHeader = "<h1>FY - Online Fire Protection Grant Application ( OFPGA )</h1><p class='lead'>*For questions and technical support regarding the application, departments should initially contact their SFMO Fire Support Inspector.</p>";
			return defaultPageHeader;

		}

		private string noDefaultPageContent()
		{
			string defaultPageContent = "";
			defaultPageContent += "<h2>Welcome to the Online New Mexico Fire Protection Grant Application</h2>";
			defaultPageContent += "<p>This web-app is in response to feedback from many New Mexico departments for a more user - friendly process.Your continued patience and understanding is appreciated as we work to improve the process and serve you better.</p>";
			defaultPageContent += "<p>To begin, enter your department’s five digit NFIRS FDID number in both the NFIRS FDID number field and the Password field. Upon logon, in the General Information page, you will be prompted to provide an email address and may change the password. Please note: Only one application per department will be accepted; therefore only one email address and password per department will be recognized.</p>";
			defaultPageContent += "<p style='text-decoration:underline'>Please read the eligibility requirements on the Welcome Page carefully before completing the application.</p>";
			defaultPageContent += "<p>To assist in tracking completion of the application, the status is shown in then gray shaded area to the right of the application. A green checkmark <img src='Content/images/tick.png' /> tick indicates the section has been opened and started. Due to the varied responses, however, It does not necessarily indicate that the section is complete. A red cross <img src='Content/images/cross.png' /> indicates that there is required information that has not been completed. A circle <img src='Content/images/round.png' /> with an empty in the center indicates the section has not yet been started.</p>";
			defaultPageContent += "<p>Should you have technical questions or experience problems navigating through the application, click on the Technical Support link located in the gray shaded banner at the top of each page, describe the problem, and click SEND. Your question will be answered within 2 business days.</p>";
			defaultPageContent += "<p>Should you have questions specific to the content requirements of the application, click on the SFMO Fire Services Support Team link located in the gray shaded banner at the top of each page, describe the question, and click SEND. Your question will be answered within 2 business days.</p>";
			defaultPageContent += "<p>You are now ready to begin entering the NM Fire Protection Grant application.</p>";
			defaultPageContent += "<p><a class='btn btn-default' href='https://www.nmdhsem.org/state-firemarshal/fire-grant-council/#grant' target='_blank'>New Mexico State Fire Marshal Fire Grant Council &raquo;</a></p";
			return defaultPageContent;
		}

		private string defaultPumpTestStatute()
        {
			string pumpTestStatute = "<p>All rated fire pumps shall undergo annual pump tests to ensure proper function and firefighter safety; evidence must be provided that apparatus pump tests are conducted on each apparatus with rated fire pumps by documenting results in the Pump Test Data Log below.</p>";
			pumpTestStatute += "<ul><li>All annual pump tests shall be in accordance with NFPA 1901 and the Insurance Service Office (ISO) requirements.</li><li>";
			pumpTestStatute += "A notarized Affidavit signed by the Fire Chief must be uploaded with the application. The Affidavit is to verify that three years of pump test records exist for each ";
			pumpTestStatute += "apparatus with a rated fire pump, are on file with the department and are available for SFMO inspection upon request. A .pdf file of the Affidavit is available on the ";
			pumpTestStatute += "Grant website and must be uploaded with the application. Note: Notary signature and seal must be clear and legible. <span style='font - weight:bold'><u>Falsified affidavits may result in forfeiture of funds and future grant consideration.</u></span></li>";
			pumpTestStatute += "<li><strong>Pump Test Affidavit should be uploaded in the ‘Signatures and Supporting documents’ tab in the Step 3</strong></li>";
			return pumpTestStatute;
        }

		private string defaulHoseTestStatute()
		{
			string hoseTestStatute = "<p style='font-weight: bold'>10.25.10.10 PERIODIC REQUIREMENTS:</p>";
			hoseTestStatute += "<p>A. Each fire department shall complete a monthly fire report utilizing the national fire incident reporting system. This report shall be filed with the state fire marshal’s office by the 10th day of each month";
			hoseTestStatute += "following the month for which the report is prepared, (e.g., the report for January is due by February 10th). Each fire department shall identify and file with the fire marshal’s office, as a minimum, one representative responsible to";
			hoseTestStatute += "comply with the reporting requirements.</p>";
			return hoseTestStatute;
		}

		public String GetDefaultEligibilityRequirements()
        {
			String HTML = "";
			String BASEURI = HttpContext.Current.Server.MapPath(@"~/Admin/Documents/");
			String SRC = String.Format("{0}{1}", BASEURI, "DefaultEligibilityText.txt");

			if (System.IO.File.Exists(SRC))
            {
				HTML = System.IO.File.ReadAllText(SRC);
			}
			return HTML;
		}

		public async Task<bool> UpdateFireGrantMainSettings(FGApplicationSettings fgSettings)
		{
			var fgAppSettings = await cwmContext.FGApplicationSettings.SingleOrDefaultAsync(a => a.AppSettingsId == fgSettings.AppSettingsId);

			if (fgAppSettings == null)
            {
				logger.Error("Unable to update settings '" + fgSettings.AppSettingsId.ToString() + "'.  The settings could not be located in the database.");
				return false;
			}
			
			fgAppSettings.FiscalYear = fgSettings.FiscalYear;
			fgAppSettings.StartDate = fgSettings.StartDate;
			fgAppSettings.EndDate = fgSettings.EndDate;
			fgAppSettings.MaxGrantAmount = fgSettings.MaxGrantAmount;
			if (fgSettings.EligibilityDocument != null)
            {
				fgAppSettings.EligibilityDocument = fgSettings.EligibilityDocument;
            }
			if (fgSettings.EligibilityDocumentName != null)
			{
				fgAppSettings.EligibilityDocumentName = fgSettings.EligibilityDocumentName;
			}
			if (fgSettings.ApplicationInstructions != null && fgSettings.ApplicationInstructions != "")
            {
				fgAppSettings.ApplicationInstructions = fgSettings.ApplicationInstructions;
            }
			if (fgSettings.DefaultPageHeader != null && fgSettings.DefaultPageHeader != "")
			{
				fgAppSettings.DefaultPageHeader = fgSettings.DefaultPageHeader;
			}
			if (fgSettings.DefaultPageContent != null && fgSettings.DefaultPageContent != "")
			{
				fgAppSettings.DefaultPageContent = fgSettings.DefaultPageContent;
			}
			if (fgSettings.PumpTestStatute != null && fgSettings.PumpTestStatute != "")
            {
				fgAppSettings.PumpTestStatute = fgSettings.PumpTestStatute;
            }
			if (fgSettings.HoseTestStatute != null && fgSettings.HoseTestStatute != "")
			{
				fgAppSettings.HoseTestStatute = fgSettings.HoseTestStatute;
			}
			if (fgSettings.eSignatureLegalText != null)
            {
				fgAppSettings.eSignatureLegalText = fgSettings.eSignatureLegalText;
            }
			if (fgSettings.EligibilityRequirementsText != null && fgSettings.EligibilityRequirementsText != "")
            {
				fgAppSettings.EligibilityRequirementsText = fgSettings.EligibilityRequirementsText;
            }
			if (fgSettings.faCertifiationText != null && fgSettings.faCertifiationText != "")
            {
				fgAppSettings.faCertifiationText = fgSettings.faCertifiationText;
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
					logger.Error("Unable to update settings '" + fgSettings.AppSettingsId.ToString() + "'.", ex);
					return false;
				}
			}
			else
			{
				logger.Error("Unable to update settings '" + fgSettings.AppSettingsId.ToString() + "', DbContext was not available.");
				return false;
			}
		}

		public async Task<List<v_AddressParties>> GetFGDepartmentsAsync(Guid partyId)
		{
			List<v_AddressParties> result = null;

			result = await cwmContext.v_AddressParties.Where(a => a.PartyID == partyId && a.AddressTypeId == deptAddType && a.Inactive == false).ToListAsync();

			return result;
		}

		public async Task<v_AddressParties> GetFGDepartmentByPartyAddAsync(Guid addressId, Guid partyId)
		{
			v_AddressParties result = null;

			result = await cwmContext.v_AddressParties.FirstOrDefaultAsync(a => a.AddressId == addressId && a.PartyID == partyId);

			return result;
		}

		public async Task<v_AddressParties> GetFGDepartmentByIdAsync(Guid addressId)
		{
			v_AddressParties result = null;

			result = await cwmContext.v_AddressParties.FirstOrDefaultAsync(a => a.AddressId == addressId);

			return result;
		}

		public async Task<List<v_Addresses2>> GetFGDepartmentsAllAsync()
		{
			List<v_Addresses2> result = null;

			result = await cwmContext.v_Addresses2.Where(a => a.AddressTypeId == deptAddType && a.Inactive == false).ToListAsync();

			return result;
		}

		public async Task<List<FG_App_Help>> GetFGAllHelp()
		{
			List<FG_App_Help> result = null;
			try
			{
				result = await cwmContext.FG_App_Helps.ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant Categories.", ex);
			}

			return result;
		}
		public async Task<FG_App_Help> GetFGHelpByPage(string page, string section = "")
		{
			FG_App_Help result = null;
			try
			{
				if (section == "")
                {
					result = await cwmContext.FG_App_Helps.FirstOrDefaultAsync(a => a.Page == page && (a.Section == "" || a.Section == null));
				}
                else
                {
					result = await cwmContext.FG_App_Helps.FirstOrDefaultAsync(a => a.Page == page && a.Section == section);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant Category.", ex);
			}

			return result;
		}

		public async Task<FG_App_Help> GetFGHelpById(Guid Id)
		{
			FG_App_Help result = null;
			try
			{
				result = await cwmContext.FG_App_Helps.FirstOrDefaultAsync(a => a.HelpId == Id);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Grant Category.", ex);
			}

			return result;
		}

		public async Task<bool> SavHelpText(FG_App_Help model)
		{
			try
			{
				if (model != null)
				{
					var help = await cwmContext.FG_App_Helps.SingleOrDefaultAsync(a => a.HelpId == model.HelpId);
					if (help == null)
					{
						help = cwmContext.FG_App_Helps.Add(new NMSFM.Data.FG_App_Help());
						help.HelpId = Guid.NewGuid();
					}

					help.Page = model.Page;
					help.Section = model.Section;
					help.Number = model.Number;
					if (model.Image != null)
                    {
						help.Image = model.Image;
                    }
					help.HelpText = model.HelpText;
					help.Inactive = model.Inactive;
					help.AdminOnly = model.AdminOnly;

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
							logger.Error("Unable to save application review for " + model.HelpId + ".", ex);
							retbol = false;
						}
					}
					else
					{
						logger.Error("Unable to save application review for " + model.HelpId + ", DbContext was not available.");
						retbol = false;
					}
					return retbol;
				}
				else
				{
					throw new Exception("Unable to save null category.");
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error(ex.Message.ToString());
				return false;
			}
		}

	}
}

