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

namespace NMSFM.Services.FYDist
{
	public class FPFService : IFPFService
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


		public FPFService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			addressService = new AddressService(cwmContext, logger);
		}

		public async Task<FYAllowableDist> GetAllowableDistAsync(short year)
		{
			FYAllowableDist result = null;
			try
			{
				result = await cwmContext.FYAllowableDists.SingleOrDefaultAsync(f => f.Year == year);

				if (result == null)
				{
					await CreateNewAllowableDistributionAsync(year);

					result = await cwmContext.FYAllowableDists.SingleOrDefaultAsync(f => f.Year == year);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Allowable Dist for Year '" + year.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<IEnumerable<nm_FYTotalDistribution>> GetTotalDistAsync(short year)
		{
			List<nm_FYTotalDistribution> result = new List<nm_FYTotalDistribution>();
			try
			{
				result = await cwmContext.nm_FYTotalDistributions.Where(f => f.Year == year).OrderBy(f => f.AddressCode).ToListAsync();

				if (result.Count() == 0)
				{
					CreateNewTotalDistribution(year);

					result = await cwmContext.nm_FYTotalDistributions.Where(f => f.Year == year).OrderBy(f => f.AddressCode).ToListAsync();
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Total Dist for Year '" + year.ToString() + "'.", ex);
			}
			return result;
		}

		//vwd added 02.05.2021 to get district distribution
		public async Task<nm_FYTotalDistribution> GetTotalDistAsync(short year, Guid addressId)
		{
			nm_FYTotalDistribution result = new nm_FYTotalDistribution();
			try
			{
				result = await cwmContext.nm_FYTotalDistributions.SingleOrDefaultAsync(f => f.Year == year && f.AddressId == addressId);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Total Dist for Year '" + year.ToString() + "'.", ex);
			}
			return result;
		}

		

		public async Task<IEnumerable<nm_FYTotalDistributionCalc>> GetTotalDistCalcsAsync(short year, bool showInactive = false)
		{
			List<nm_FYTotalDistributionCalc> result = null;
			try
			{
				
				result = await cwmContext.nm_FYTotalDistributionCalcs.Where(f => f.Year == year).OrderBy(f => f.AddressCode).ToListAsync();

				//Added Code Below to check for new addresses and recalc if new addresses added. (vwd - 01/05/2020)
				//Guid addressTypeId = new Guid("43856752-8b7a-4e6f-b697-bf8acd457c16");
				//var addresses = cwmContext.Addresses.Where(a => a.AddressTypeId == addressTypeId && a.Inactive == showInactive).ToList();
				//if (addresses.Count != result.Count)
				//{
				//	short yy = Convert.ToInt16(Convert.ToInt32(year) - 1);
				//	await RecalcFYStatuteDistributionAsync(year: yy);
				//	result = await cwmContext.nm_FYTotalDistributionCalcs.Where(f => f.Year == year).OrderBy(f => f.AddressCode).ToListAsync();

				//}
				//End Add

				if (!showInactive)
				{
					result = result.Where(f => f.Inactive == false).ToList();
				}

				foreach (nm_FYTotalDistributionCalc item in result)
				{

					item.MainCalcTotalRnd = (item.MainStationCount * item.MainCalcAmountRnd);
					item.AdminCalcTotalRnd = (item.AdminBldgCount * item.MainCalcAmountRnd);
					item.MainAdmCalcTotalRnd = ((item.MainStationCount + item.AdminBldgCount) * item.MainCalcAmountRnd);
					item.SubCalcTotalRnd = (item.SubStationCount * item.SubCalcAmountRnd);

					item.FireFundDist = (((item.MainStationCount + item.AdminBldgCount) * item.MainCalcAmountRnd) + (item.SubStationCount * item.SubCalcAmountRnd));

					item.FYTotalDistribution = item.FireFundDist - item.NMFAAmount;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Total Dist for Year '" + year.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<List<FYStatuteDist>> GetStatuteDistsAsync(short year)
		{
			List<FYStatuteDist> result = null;
			try
			{
				bool yearExists = await cwmContext.FYStatuteDists.AnyAsync(s => s.Year == year);



				if (!yearExists)
				{
					CreateNewStatuteDiststributions(year);
				}
				result = await cwmContext.FYStatuteDists.Where(s => s.Year == year).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Total Dist for Year '" + year.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<decimal> GetStatuteDistAsync(short year, int isoClass, bool sub = false)
		{
			decimal result = 0M;
			try
			{
				bool yearExists = await cwmContext.FYStatuteDists.AnyAsync(s => s.Year == year);

				if (yearExists)
				{
					if (sub)
					{
						result = (await cwmContext.FYStatuteDists.FirstAsync(s => s.Year == year && s.ISOClass == isoClass)).SSBaseAmount;
					}
					else
					{
						result = (await cwmContext.FYStatuteDists.FirstAsync(s => s.Year == year && s.ISOClass == isoClass)).MSBaseAmount;
					}

				}
				else
				{
					if (sub)
					{
						result = (await cwmContext.FYStatuteDists.FirstAsync(s => s.Year == (year - 1) && s.ISOClass == isoClass)).SSBaseAmount;
					}
					else
					{
						result = (await cwmContext.FYStatuteDists.FirstAsync(s => s.Year == (year - 1) && s.ISOClass == isoClass)).MSBaseAmount;
					}

				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Statute Dist for Year '" + year.ToString() + "' ans ISOClass " + isoClass + ".", ex);
			}
			return result;
		}

		public async Task<List<FYTotalDistCalc>> GetFYTotalDistCalcsAsync(short year, bool refresh = false)
		{
			List<FYTotalDistCalc> result = null;
			try
			{

				bool yearExists = await cwmContext.FYTotalDistCalcs.AnyAsync(s => s.Year == year);

				if (!yearExists)
				{
					await CreateNewFYTotalDiststributionCalcsAsync(year, refresh);


					result = await cwmContext.FYTotalDistCalcs.Where(s => s.Year == year).ToListAsync();
				}
				else if (refresh)
				{
					result = await cwmContext.FYTotalDistCalcs.Where(s => s.Year == year).ToListAsync();
					foreach (FYTotalDistCalc fyTotalDistCalc in result)
					{
						fyTotalDistCalc.MainCount = Convert.ToInt16(await GetStationCountTotalsAsync("Main", fyTotalDistCalc.ISOClass) + await GetStationCountTotalsAsync("Admin", fyTotalDistCalc.ISOClass));
						fyTotalDistCalc.SubCount = await GetStationCountTotalsAsync("Subs", fyTotalDistCalc.ISOClass);

					}

				}
				else
				{
					result = await cwmContext.FYTotalDistCalcs.Where(s => s.Year == year).ToListAsync();
				}
				//await RecalcFYStatuteDistributionAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Total Dist for Year '" + year.ToString() + "'.", ex);
			}
			return result;
		}

		//public async Task<FYTotal> GetTotalsAsync(short year)
		//{
		//	FYTotal result = null;

		//	result = await cwmContext.FYTotals.FirstOrDefaultAsync(s => s.Year == year);

		//	if (result == null)
		//	{
		//		result = new FYTotal();
		//		result.Year = year;
		//	}

		//	return result;
		//}

		public async Task<List<string>> GetYearListAsync()
		{
			List<string> result = new List<string>();
			result = await cwmContext.FYAllowableDists.OrderBy(f => f.Year).Select(a => a.Year.ToString()).Distinct().ToListAsync();
			return result;
		}

		private void CreateNewTotalDistribution(short year)
		{
			//Guid addressTypeId = new Guid("43856752-8b7a-4e6f-b697-bf8acd457c16");
			var addresses = cwmContext.Addresses.Where(a => a.AddressTypeId == deptAddType && a.Inactive == false).ToList();
			foreach (Data.Address address in addresses)
			{

				var fyTotalDist = cwmContext.FYTotalDists.Add(new FYTotalDist());
				fyTotalDist.Year = year;
				fyTotalDist.AddressId = address.AddressId;

			}
			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to create New Total Distribution Records.", ex);
				}
			}
			else
			{
				logger.Error("Unable to create New Total Distribution Records., DbContext was not available.");
			}
		}

		public void CreateNewStatuteDiststributions(short year)
		{

			var statusDists = cwmContext.FYStatuteDists.Where(f => f.Year == (year - 1));
			foreach (FYStatuteDist statuteDist in statusDists)
			{
				var fystatuteDist = cwmContext.FYStatuteDists.Add(new FYStatuteDist());
				fystatuteDist.ISOClass = statuteDist.ISOClass;
				fystatuteDist.MSBaseAmount = statuteDist.MSBaseAmount;
				fystatuteDist.SSBaseAmount = statuteDist.SSBaseAmount;
				fystatuteDist.Year = year;

			}
			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to create New Total Distribution Records.", ex);
				}
			}
			else
			{
				logger.Error("Unable to create New Total Distribution Records., DbContext was not available.");
			}
		}

		private async Task CreateNewFYTotalDiststributionCalcsAsync(short year, bool refresh = false)
		{
			try
			{


				var totalDistCalcs = await cwmContext.FYTotalDistCalcs.Where(f => f.Year == (year - 1)).ToListAsync();
				if (totalDistCalcs.Count() == 0)
				{
					for (int i = 1; i <= 10; i++)
					{
						var fyTotalDistCalc = cwmContext.FYTotalDistCalcs.Add(new FYTotalDistCalc());
						fyTotalDistCalc.ISOClass = (short)i;
						fyTotalDistCalc.Year = year;
						fyTotalDistCalc.MainCount = Convert.ToInt16(await GetStationCountTotalsAsync("Main", i) + await GetStationCountTotalsAsync("Admin", i));
						fyTotalDistCalc.SubCount = await GetStationCountTotalsAsync("Subs", i);
						fyTotalDistCalc.MainGrowthAmount = 0;
						fyTotalDistCalc.MainCalcAmount = 0;
						fyTotalDistCalc.MainCalcAmountRnd = 0;
						fyTotalDistCalc.MainCalcTotal = 0;
						fyTotalDistCalc.MainCalcTotalRnd = 0;

						fyTotalDistCalc.SubGrowthAmount = 0;
						fyTotalDistCalc.SubCalcAmount = 0;
						fyTotalDistCalc.SubCalcAmountRnd = 0;
						fyTotalDistCalc.SubCalcTotal = 0;
						fyTotalDistCalc.SubCalcTotalRnd = 0;
					}
				}
				else
				{
					foreach (FYTotalDistCalc totalDistCalc in totalDistCalcs)
					{
						var fyTotalDistCalc = cwmContext.FYTotalDistCalcs.Add(new FYTotalDistCalc());
						fyTotalDistCalc.ISOClass = totalDistCalc.ISOClass;
						fyTotalDistCalc.Year = year;
						fyTotalDistCalc.MainCount = Convert.ToInt16(await GetStationCountTotalsAsync("Main", (int)totalDistCalc.ISOClass) + await GetStationCountTotalsAsync("Admin", (int)totalDistCalc.ISOClass));
						fyTotalDistCalc.SubCount = await GetStationCountTotalsAsync("Subs", (int)totalDistCalc.ISOClass);
						fyTotalDistCalc.MainGrowthAmount = 0;
						fyTotalDistCalc.MainCalcAmount = 0;
						fyTotalDistCalc.MainCalcAmountRnd = 0;
						fyTotalDistCalc.MainCalcTotal = 0;
						fyTotalDistCalc.MainCalcTotalRnd = 0;

						fyTotalDistCalc.SubGrowthAmount = 0;
						fyTotalDistCalc.SubCalcAmount = 0;
						fyTotalDistCalc.SubCalcAmountRnd = 0;
						fyTotalDistCalc.SubCalcTotal = 0;
						fyTotalDistCalc.SubCalcTotalRnd = 0;
					}
				}
				if (cwmContext is DbContext)
				{
					try
					{
						((DbContext)cwmContext).SaveChanges();
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create New Total Distribution Records.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create New Total Distribution Records., DbContext was not available.");
				}
			}
			catch (Exception ex2)
			{

				logger.Error("Unable to create New Total Distribution Records.", ex2);
			}
		}

		private async Task CreateNewAllowableDistributionAsync(short year)
		{

			var allowableDists = await cwmContext.FYAllowableDists.SingleOrDefaultAsync(f => f.Year == (year - 1));

			var fyAllowableDist = cwmContext.FYAllowableDists.Add(new FYAllowableDist());

			fyAllowableDist.Year = year;
			await RecalcFYStatuteDistributionAsync(year: year);
			fyAllowableDist.FYStatuteDistribution = allowableDists.FYStatuteDistribution;


			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to create New Allowable Distribution Record.", ex);
				}
			}
			else
			{
				logger.Error("Unable to create New Allowable Distribution Record., DbContext was not available.");
			}
		}

		public async Task<v_AddressParties> GetFPFApplicationAsync(Guid partyId, Guid addressId)
		{
			v_AddressParties result = null;

			result = await cwmContext.v_AddressParties.FirstOrDefaultAsync(a => a.PartyID == partyId && a.AddressId == addressId && (a.AddressTypeId == deptAddType || a.AddressTypeId == AdminAddType));

			return result;
		}

		public v_AddressParties GetFGFApplicationAddress(Guid partyId, Guid addressId)
        {
			v_AddressParties result = null;

			result = cwmContext.v_AddressParties.FirstOrDefault(a => a.PartyID == partyId && a.AddressId == addressId && (a.AddressTypeId == deptAddType || a.AddressTypeId == AdminAddType));

			return result;
		}

		public async Task<v_AddressParties> GetFPFApplicationAddressAsync(Guid addressId)
		{
			v_AddressParties result = null;

			result = await cwmContext.v_AddressParties.FirstOrDefaultAsync(a => a.AddressId == addressId && a.AddressTypeId == deptAddType);

			return result;
		}

		public async Task<List<v_AddressParties>> GetFPFApplicationsAsync(Guid partyId)
		{
			List<v_AddressParties> result = null;

			result = await cwmContext.v_AddressParties.Where(a => a.PartyID == partyId && a.AddressTypeId == deptAddType && a.Inactive == false).ToListAsync();

			return result;
		}

		public async Task<List<v_Addresses2>> GetFPFApplicationsAllAsync()
		{
			List<v_Addresses2> result = null;

			result = await cwmContext.v_Addresses2.Where(a => a.AddressTypeId == deptAddType && a.Inactive == false).ToListAsync();

			return result;
		}

		public async Task<List<v_AddressParties>> GetTreasurerAddresses()
		{
			List<v_AddressParties> result = null;
			string addressTypeId = "f294d4f7-b769-4a4c-b4a7-e5f218e79f99";

			result = await cwmContext.v_AddressParties.Where(a => a.AddressTypeId.ToString() == addressTypeId).ToListAsync();

			return result;
		}

		public async Task<v_Addresses2> GetTreasurerAddress(bool isCity, string cityCounty)
		{
			v_Addresses2 result = null;
			string addressTypeId = "f294d4f7-b769-4a4c-b4a7-e5f218e79f99";

			if (isCity)
            {
				result = await cwmContext.v_Addresses2.FirstOrDefaultAsync(a => a.AddressTypeId.ToString() == addressTypeId && a.City == cityCounty && a.AddressCode.Contains("CTY"));
			}
            else
            {
				result = await cwmContext.v_Addresses2.FirstOrDefaultAsync(a => a.AddressTypeId.ToString() == addressTypeId && a.County == cityCounty && a.AddressCode.Contains("CNT"));
			}

			return result;
		}

		public async Task<List<v_Addresses2>> GetStationListAsync(string stationType, Guid parentAddressId)
		{
			List<v_Addresses2> result = new List<v_Addresses2>();
			Guid addTypeId = Guid.Empty;

			switch (stationType)
			{
				case "Main":
					addTypeId = mainAddType;
					result = await cwmContext.v_Addresses2.Where(a => a.AddressId == parentAddressId || (a.AddressTypeId == addTypeId && a.ParentAddressId == parentAddressId)).OrderBy(a => a.AddressCode).ToListAsync();
					break;
				case "Admin":
					addTypeId = AdminAddType;
					result = await cwmContext.v_Addresses2.Where(a => a.AddressTypeId == addTypeId && a.ParentAddressId == parentAddressId).OrderBy(a => a.AddressCode).ToListAsync();
					break;
				case "Subs":
					addTypeId = SubAddType;
					result = await cwmContext.v_Addresses2.Where(a => a.AddressTypeId == addTypeId && a.ParentAddressId == parentAddressId).OrderBy(a => a.AddressCode).ToListAsync();
					break;
			}
			

			return result;
		}

		public async Task<List<FYAppStation>> GetExistingStationListAsync(string stationType, Guid departAddressId, short year)
		{
			List<FYAppStation> result = new List<FYAppStation>();
			Guid addTypeId = Guid.Empty;
			switch (stationType)
			{
				case "Main":
					addTypeId = mainAddType;
					result = await cwmContext.FYAppStations.Where(a => a.AddressId == departAddressId || (a.AddressTypeId == addTypeId && a.DepartmentAddressId == departAddressId) && a.Year == year).OrderBy(a => a.AddressCode).ToListAsync();
					break;
				case "Admin":
					addTypeId = AdminAddType;
					result = await cwmContext.FYAppStations.Where(a => a.AddressTypeId == addTypeId && a.DepartmentAddressId == departAddressId && a.Year == year).OrderBy(a => a.AddressCode).ToListAsync();
					break;
				case "Subs":
					addTypeId = SubAddType;
					result = await cwmContext.FYAppStations.Where(a => a.AddressTypeId == addTypeId && a.DepartmentAddressId == departAddressId && a.Year == year).OrderBy(a => a.AddressCode).ToListAsync();
					break;
			}
			

			return result;
		}

		public async Task<short> GetStationCountTotalsAsync(string stationType, int ISOClass)
		{
			short result = 0;
			Guid addTypeId = Guid.Empty;

			Guid getGuid;
			//AddressType addType;
			switch (stationType)
			{
				case "Main":
					getGuid = mainGuid;
					break;
				case "Admin":
					getGuid = adminGuid;
					break;
				case "Subs":
					getGuid = subGuid;
					break;
				default:
					getGuid = subGuid;
					//addType = await cwmContext.AddressTypes.FirstOrDefaultAsync(a => a.AddressType1.Contains(stationType) && a.AgencyId == agencyId);
					//if (addType != null)
					//{
					//	addTypeId = addType.AddressTypeId;
					//}
					//var adds2 = await cwmContext.v_Addresses2.Where(a => a.AddressTypeId == addTypeId && a.Inactive == false).Select(a => a.AddressId).ToListAsync();

					//var cnt2 = (await cwmContext.v_UserDefValues.Where(a => (a.UserDefFieldId== isoGuid && a.UserDefValue == ISOClass.ToString()) && adds2.Contains(a.RecordId ?? Guid.Empty)).Select(u => u.UserDefValueId).Distinct().ToListAsync()).Count();
					//result = Convert.ToInt16(cnt2);
					////result = Convert.ToInt16((await cwmContext.v_Addresses2.Where(a => a.AddressTypeId == addTypeId && a.Inactive == false).ToListAsync()).Count());
					break;
					//case "Subs":
					//	addType = await cwmContext.AddressTypes.FirstOrDefaultAsync(a => a.AddressType1.Contains("Subs") && a.AgencyId == agencyId);
					//	if (addType != null)
					//	{
					//		addTypeId = addType.AddressTypeId;
					//	}
					//	result = Convert.ToInt16((await cwmContext.v_Addresses2.Where(a => a.AddressTypeId == addTypeId && a.Inactive == false).ToListAsync()).Count());
					//	break;
			}
			var adds = await cwmContext.Addresses.Where(a => a.Inactive == false && a.AddressTypeId == deptAddType).Select(a => a.AddressId).ToListAsync();

			var udfs = await cwmContext.UserDefValues.Where(u => u.UserDefFieldId == isoGuid && u.UserDefValue1 == ISOClass.ToString() && adds.Contains(u.RecordId)).Select(u => u.RecordId).ToListAsync();
			//var adds = await cwmContext.v_Addresses2.Where(a => (a.AddressTypeId == deptAddType && a.Inactive == false) && udfs.Contains(a.AddressId)).Select(a => a.AddressId).ToListAsync();

			try
			{
				var cnt = (await cwmContext.UserDefValues.Where(a => a.UserDefFieldId == getGuid && udfs.Contains(a.RecordId)).Select(u => u.UserDefValue1).ToListAsync());

				foreach (var item in cnt)
				{
					result += Convert.ToInt16(item);
				}

			}
			catch (Exception ex)
            {
                _ = ex;

				logger.Error("Unable to get Station Count " + stationType + ".", ex);
			}

			return result;
		}


		public async Task<bool> SaveAllowableDistAsync(DetailedFYAllowableDist model)
		{
			bool result = false;

			var allowDist = await cwmContext.FYAllowableDists.FirstOrDefaultAsync(f => f.FYAllowableDistId == model.FYAllowableDistId);
			var allowDistm2 = await cwmContext.FYAllowableDists.FirstOrDefaultAsync(f => f.Year == (short)(model.Year - 2));
			if (allowDist != null)
			{
				allowDist.CalculationPer = model.CalculationPer;
				allowDistm2.FYAuditedRevenue = model.Prev2FYRevenue;
				allowDist.PERA = model.PERA;
				allowDist.FYAllowedDistribution = model.FYAllowableDistribution;
				allowDist.NMFAFYPayment = model.NMFAFYPayment;
				allowDist.FYDistributionFactor = (allowDist.FYAllowedDistribution - allowDist.FYStatuteDistribution) / allowDist.FYStatuteDistribution;

				if (cwmContext is DbContext)
				{
					try
					{
						((DbContext)cwmContext).SaveChanges();
						await RecalcFYStatuteDistributionAsync(year: model.Year);
						result = true;

					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create New Allowable Distribution Record.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create New Allowable Distribution Record., DbContext was not available.");
				}
			}

			return result;

		}

		public async Task<bool> SaveDistributionCalculationsAsync(DetailedFYCalculatedDist model)
		{
			bool result = false;
			try
			{
				foreach (FYTotalDistCalc item in model.TotalDistCalcs)
				{
					FYTotalDistCalc curTotDistCalc = await cwmContext.FYTotalDistCalcs.FirstAsync(t => t.FYTotalDistCalcId == item.FYTotalDistCalcId);
					curTotDistCalc.MainGrowthAmount = item.MainGrowthAmount;
					curTotDistCalc.MainCalcAmount = item.MainCalcAmount;
					curTotDistCalc.MainCalcAmountRnd = item.MainCalcAmountRnd;
					curTotDistCalc.MainCount = item.MainCount;
					curTotDistCalc.MainStatuteTotal = item.MainStatuteTotal;
					curTotDistCalc.MainCalcTotal = item.MainCalcTotal;
					curTotDistCalc.MainCalcTotalRnd = item.MainCalcTotalRnd;
					curTotDistCalc.SubGrowthAmount = item.SubGrowthAmount;
					curTotDistCalc.SubCalcAmount = item.SubCalcAmount;
					curTotDistCalc.SubCalcAmountRnd = item.SubCalcAmountRnd;
					curTotDistCalc.SubCount = item.SubCount;
					curTotDistCalc.SubStatuteTotal = item.SubStatuteTotal;
					curTotDistCalc.SubCalcTotal = item.SubCalcTotal;
					curTotDistCalc.SubCalcTotalRnd = item.SubCalcTotalRnd;

				}
				FYAllowableDist fYAllowableDist = await cwmContext.FYAllowableDists.FirstAsync(a => a.Year == model.Year);
				fYAllowableDist.FYDistCalcAccepted = true;

				if (cwmContext is DbContext)
				{
					try
					{
						((DbContext)cwmContext).SaveChanges();
						result = true;
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save FYTotalDistCalcs records.", ex);
					}
				}
				else
				{
					logger.Error("Unable to save FYTotalDistCalcs records., DbContext was not available.");
				}
			}
			catch (Exception exm)
			{
				logger.Error("Unable to save FYTotalDistCalcs records.", exm);

			}

			return result;
		}

		public async Task<bool> SaveTotalDistributionsAsync(DetailedFYCalculatedDist model)
		{
			bool result = false;

			var finals = await GetTotalDistCalcsAsync(model.Year);

			foreach (nm_FYTotalDistributionCalc item in finals)
			{
				var curDist = await cwmContext.FYTotalDists.FirstOrDefaultAsync(td => td.AddressId == item.AddressId && td.Year == model.Year);

				if (curDist == null)
				{
					curDist = cwmContext.FYTotalDists.Add(new FYTotalDist());
				}
				//if (curDist != null)
				//{
				curDist.AddressId = item.AddressId;
				curDist.Year = item.Year;
				curDist.ISOClass = item.ISOClass;
				curDist.MainStationCount = item.MainStationCount;
				curDist.AdminBldgCount = item.AdminBldgCount;
				curDist.SubStationCount = item.SubStationCount;
				curDist.FireFundDist = item.FireFundDist;
				curDist.NMFAAmount = item.NMFAAmount;
				curDist.FYTotalDistribution = item.FYTotalDistribution;
				curDist.MainCalcTotalRnd = item.MainCalcTotalRnd;
				curDist.AdminCalcTotalRnd = item.AdminCalcTotalRnd;
				curDist.MainAdmCalcTotalRnd = item.MainAdmCalcTotalRnd;
				curDist.SubCalcTotalRnd = item.SubCalcTotalRnd;
				//}
				//else
				//{

				//	curDist.AddressId = item.AddressId;
				//	curDist.Year = item.Year;
				//	curDist.ISOClass = item.ISOClass;
				//	curDist.MainStationCount = item.MainStationCount;
				//	curDist.AdminBldgCount = item.AdminBldgCount;
				//	curDist.SubStationCount = item.SubStationCount;
				//	curDist.FireFundDist = item.FireFundDist;
				//	curDist.NMFAAmount = item.NMFAAmount;
				//	curDist.FYTotalDistribution = item.FYTotalDistribution;

				//}
			}

			foreach (DbEntityEntry entry in ((DbContext)cwmContext).ChangeTracker.Entries())
			{
				if (entry.Entity.GetType().Name == "nm_FYTotalDistributionCalc")
				{
					switch (entry.State)
					{
						case EntityState.Modified:
							entry.State = EntityState.Unchanged;
							break;
						default: break;
					}
				}
			}

			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
					result = true;
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to save Total Distribution Records.", ex);
				}
			}
			else
			{
				logger.Error("Unable to save Total Distribution Records., DbContext was not available.");
			}

			return result;

		}

		public async Task<bool> SaveTotalDistributionCalcs(string col, Guid addressId, string value)
		{
			Guid mainGuid = new Guid("7ad61001-cac8-4f3c-ae4e-32d28393f891");
			Guid adminGuid = new Guid("8baa0b86-f1e5-4d84-b4f9-a8219f4b11b8");
			Guid subGuid = new Guid("4f34b96d-d944-44aa-9665-d47c55cc025d");
			Guid isoGuid = new Guid("6b8517ef-9483-4b8b-8c95-5b95a6b8f579");
			Guid NMFAGuid = new Guid("6b78aaf0-36e5-4414-9e82-8eb567748291");
			Guid chosen = Guid.Empty;
			bool result = false;
			FYAllowableDist fYAllowableDist;

			try
			{
				switch (col)
				{
					case "Inactive":
						await addressService.UpdateAddressInactive(addressId, value);
						result = true;
						return result;
					//break;
					case "AdminBldgCount":
						chosen = adminGuid;
						break;
					case "MainStationCount":
						chosen = mainGuid;
						break;
					case "SubStationCount":
						chosen = subGuid;
						break;
					case "ISOClass":
						chosen = isoGuid;
						break;
					case "NMFAAmount":
						chosen = NMFAGuid;
						break;
					default:
						break;
				}

				var udfValue = await cwmContext.UserDefValues.FirstOrDefaultAsync(u => u.RecordId == addressId && u.UserDefFieldId == chosen);
				if (udfValue == null || udfValue.UserDefValueId == Guid.Empty)
				{
					udfValue = cwmContext.UserDefValues.Add(new UserDefValue());
					udfValue.UserDefValueId = Guid.NewGuid();
					udfValue.UserDefFieldId = chosen;
					udfValue.RecordId = addressId;
					udfValue.rowguid = Guid.NewGuid();
					udfValue.DateInserted = DateTime.Now;
					udfValue.DateUpdated = udfValue.DateInserted;
					udfValue.UserDefValue1 = value;
				}
				else
				{
					if (udfValue.UserDefValue1 != value)
					{
						udfValue.UserDefValue1 = value;
					}
				}

				if (cwmContext is DbContext)
				{
					try
					{
						((DbContext)cwmContext).SaveChanges();
						result = true;
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save Department Distribution Calc Records records.", ex);
					}
				}
				else
				{
					logger.Error("Unable to save Department Distribution Calc Records records., DbContext was not available.");
				}


				if (chosen == NMFAGuid)
				{
					var year = (short)(DateTime.Now.Year + 1);
					fYAllowableDist = await cwmContext.FYAllowableDists.FirstAsync(f => f.Year == year);
					fYAllowableDist.NMFAFYPayment = await RecalcNMFATotal(year);


					if (cwmContext is DbContext)
					{
						try
						{
							((DbContext)cwmContext).SaveChanges();
							result = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to update Allowable Distribution NMFA Payment value.", ex);
						}
					}
					else
					{
						logger.Error("Unable to update Allowable Distribution NMFA Payment value., DbContext was not available.");
					}
				}
			}
			catch (Exception exm)
			{

				logger.Error("Unable to save Department Distribution Calc Records records.", exm);
			}
			return result;
		}

		public async Task<bool> SaveStatuteDistAsync(DetailedFYAllowableDist model)
		{
			bool result = false;

			foreach (FYStatuteDist item in model.StatuteDists)
			{
				var curStatDist = await cwmContext.FYStatuteDists.FirstOrDefaultAsync(s => s.FYStatuteDistId == item.FYStatuteDistId);

				if (curStatDist != null)
				{
					curStatDist.MSBaseAmount = item.MSBaseAmount;
					curStatDist.SSBaseAmount = item.SSBaseAmount;
				}
				else
				{
					var fystatuteDist = cwmContext.FYStatuteDists.Add(new FYStatuteDist());
					fystatuteDist.ISOClass = item.ISOClass;
					fystatuteDist.MSBaseAmount = item.MSBaseAmount;
					fystatuteDist.SSBaseAmount = item.SSBaseAmount;
					fystatuteDist.Year = model.Year;
				}
			}

			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();

					result = await RecalcFYStatuteDistributionAsync(model.StatuteDists);

				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to save Total Distribution Records.", ex);
				}
			}
			else
			{
				logger.Error("Unable to save Total Distribution Records., DbContext was not available.");
			}
			return result;
		}

		public async Task<bool> FinalizeAsync(DetailedFYCalculatedDist model)
		{
			bool result = false;
			try
			{


				var curAllowableDists = await cwmContext.FYAllowableDists.FirstAsync(a => a.FYAllowableDistId == model.FYAllowableDistId);

				if (curAllowableDists != null)
				{
					curAllowableDists.FYActualDistribution = model.FYActualDistribution;
					curAllowableDists.FYDistToDept = model.FYDistToDept;

					if (cwmContext is DbContext)
					{
						try
						{
							((DbContext)cwmContext).SaveChanges();
							result = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save Finalized Records.", ex);
						}
					}
					else
					{
						logger.Error("Unable to save Finalized Records., DbContext was not available.");
					}
				}
			}
			catch (Exception exm)
			{

				logger.Error("Unable to save Finalized Records.", exm);
			}
			return result;
		}

		//UnFinalizeAsync
		public async Task<bool> UnFinalizeAsync(DetailedFYCalculatedDist model)
		{
			bool result = false;
			try
			{


				var curAllowableDists = await cwmContext.FYAllowableDists.FirstAsync(a => a.FYAllowableDistId == model.FYAllowableDistId);
				curAllowableDists.FYDistCalcAccepted = false;

				if (curAllowableDists != null)
				{
					curAllowableDists.FYActualDistribution = null;
					curAllowableDists.FYDistToDept = null;

					var fyTotalDists = await cwmContext.FYTotalDists.Where(a => a.Year == model.Year).ToListAsync();

					foreach (FYTotalDist item in fyTotalDists)
					{
						await Task.FromResult(cwmContext.FYTotalDists.Remove(item));
					}


					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							result = true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save Finalized Records.", ex);
						}
					}
					else
					{
						logger.Error("Unable to save Finalized Records., DbContext was not available.");
					}
				}
			}
			catch (Exception exm)
			{

				logger.Error("Unable to save Finalized Records.", exm);
			}
			return result;
		}
		public async Task<bool> RecalcFYStatuteDistributionAsync(List<FYStatuteDist> statuteDists = null, short year = 0)
		{
			bool result = false;
			try
            {
				if (statuteDists == null)
				{
					statuteDists = await GetStatuteDistsAsync(year);
				}
				else
				{
					if (year == 0)
					{
						year = statuteDists[0].Year;
					}

				}

				List<FYTotalDistCalc> totalDistCalcs = await GetFYTotalDistCalcsAsync(year, true);

				decimal mainStatuteTotal = 0;
				decimal subStatuteTotal = 0;

				decimal fyStatuteDistribution = 0;

				FYAllowableDist fYAllowableDist = await cwmContext.FYAllowableDists.FirstAsync(f => f.Year == year);

				if (totalDistCalcs.Count() > 0)
				{
					int i = 0;
					foreach (var totalDistCalc in totalDistCalcs)
					{


						totalDistCalc.MainStatuteTotal = statuteDists[i].MSBaseAmount * totalDistCalc.MainCount;

						totalDistCalc.SubStatuteTotal = statuteDists[i].SSBaseAmount * totalDistCalc.SubCount;

						mainStatuteTotal += totalDistCalc.MainStatuteTotal;

						subStatuteTotal += totalDistCalc.SubStatuteTotal;

						i++;
					}

				}
				fyStatuteDistribution = mainStatuteTotal + subStatuteTotal;

				fYAllowableDist.FYStatuteDistribution = fyStatuteDistribution;

				fYAllowableDist.FYDistributionFactor = (fYAllowableDist.FYAllowedDistribution - fyStatuteDistribution) / fyStatuteDistribution;

				if (totalDistCalcs.Count() > 0)
				{
					int i = 0;
					foreach (var totalDistCalc in totalDistCalcs)
					{
						totalDistCalc.MainStatuteTotal = statuteDists[i].MSBaseAmount * totalDistCalc.MainCount;
						totalDistCalc.SubStatuteTotal = statuteDists[i].SSBaseAmount * totalDistCalc.SubCount;
						i++;
					}

				}
			}
			catch (Exception exm)
			{

				logger.Error("Unable to save FYStatuteDistribution.1", exm);
			}
			
			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
					result = true;
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to save FYStatuteDistribution.2", ex);
				}
			}
			else
			{
				logger.Error("Unable to save FYStatuteDistribution.3, DbContext was not available.");
			}

			return result;
		}

		public async Task<DetailedApplication> SaveApplication(DetailedApplication model)
		{
			try
			{

				IPartyService partyService = new PartyService(cwmContext, logger);
				//FYApplications
				FYApplication fYApplication = await cwmContext.FYApplications.FirstOrDefaultAsync(f => f.FYApplicationsId == model.FYApplicationsId);

				if (fYApplication == null || fYApplication.FYApplicationsId == Guid.Empty)
				{
					fYApplication = cwmContext.FYApplications.Add(new Data.FYApplication());
				}

				//fYApplication.FYApplicationsId = 
				//FYApplicationsIdentity				
				if (model.NIFRSContact1 == null || (model.NIFRSContact1.PartyId == null && model.NIFRSContact1.PartyName != null && model.NIFRSContact1.PartyName.Trim() != ""))
				{

					fYApplication.NFIRSContact1Id = await partyService.CreatePartyAsync(model.NIFRSContact1);
				}
				else if (model.NIFRSContact1.PartyId != null)
				{
					fYApplication.NFIRSContact1Id = model.NIFRSContact1.PartyId.Value;
				}
				if (model.NIFRSContact2 == null || (model.NIFRSContact2.PartyId == null && model.NIFRSContact2.PartyName != null && model.NIFRSContact2.PartyName.Trim() != ""))
				{
					fYApplication.NFIRSContact2Id = await partyService.CreatePartyAsync(model.NIFRSContact2);
				}
				else if (model.NIFRSContact2.PartyId != null)
				{
					fYApplication.NFIRSContact2Id = model.NIFRSContact2.PartyId.Value;
				}
				fYApplication.Year = model.Year;
				fYApplication.AddressId = model.AddressId;
				fYApplication.ISOContendSig = model.ISOContendSig;
				fYApplication.FPFBalance = model.FPFBalance;
				fYApplication.FPFRollOverAmount = model.FPFRollOverAmount;
				fYApplication.FPFRollOverDescription = model.FPFRollOverDescription;
				fYApplication.AppDaySubmitted = model.AppDaySubmitted;
				fYApplication.AppMonthSubmitted = model.AppMonthSubmitted;
				fYApplication.GovOffElectronicSig = model.GovOffElectronicSig;
				fYApplication.ChiefElectronicSig = model.ChiefElectronicSig;
				//Added complete 04/06/2021 vwd
				fYApplication.Complete = model.Complete;
				fYApplication.CompletedBy = model.CompletedBy;

				fYApplication.ISOClass = model.ISOClass;
				fYApplication.MainStationCount = model.MainStationCount;
				fYApplication.AdminBldgCount = model.AdminBldgCount;
				fYApplication.SubStationCount = model.SubStationCount;

				fYApplication.Approved = model.Approved;
				fYApplication.ApprovedBy = model.ApprovedBy;
				fYApplication.DateApproved = model.DateApproved;

				FYAppData fYAppData = await cwmContext.FYAppDatas.FirstOrDefaultAsync(f => f.Year == model.Year);
				if (fYAppData == null || fYAppData.FYAppDataId == Guid.Empty)
				{
					fYAppData = cwmContext.FYAppDatas.Add(new Data.FYAppData());
					fYAppData.Year = model.Year;
					fYAppData.Dist1Comm = model.Dist1Comm;
					fYAppData.Dist2Comm = model.Dist2Comm;
					fYAppData.Dist3Comm = model.Dist3Comm;
					fYAppData.Dist4Comm = model.Dist4Comm;
					fYAppData.Dist5Comm = model.Dist5Comm;
					fYAppData.Governor = model.Governor;
					fYAppData.CabinetSec = model.CabinetSec;
					fYAppData.DeputyCabinetSec = model.CabinetSec;
					fYAppData.FireMarshalName = model.FireMarshalName;
					fYAppData.ChiefofStaffTitle = model.ChiefofStaffTitle;
					fYAppData.ChiefofStaffName = model.ChiefofStaffName;
					fYAppData.ApplicationDueDate = model.ApplicationDueDate;
					fYAppData.FPFRollOverSubmittalDueDate = model.FPFRollOverSubmittalDueDate;
				}

				var fyStations = await cwmContext.FYAppStations.Where(a => a.Year == model.Year && a.DepartmentAddressId == model.AddressId).ToListAsync();
				foreach (FYAppStation item in fyStations)
				{
					await Task.FromResult(cwmContext.FYAppStations.Remove(item));
				}

				if (model.MainStations != null)
				{
					foreach (StationViewModel station in model.MainStations)
					{
						if (station.FYAppStationsId == null || station.FYAppStationsId == Guid.Empty)
						{
							var fYAppStation = cwmContext.FYAppStations.Add(new Data.FYAppStation());
							fYAppStation.Year = model.Year;
							fYAppStation.DepartmentAddressId = model.AddressId;
							fYAppStation.AddressTypeId = mainAddType;
							fYAppStation.AddressId = station.AddressId;
							fYAppStation.AddressCode = station.AddressCode;
							fYAppStation.AddressNumber = station.AddressNumber;
							fYAppStation.Direction = station.Direction;
							fYAppStation.Address = station.Address;
							fYAppStation.Suffix = station.Suffix;
							fYAppStation.SubAddress = station.SubAddress;
							fYAppStation.City = station.City;
							fYAppStation.StateAbbr = station.StateAbbr;
							fYAppStation.Zip = station.Zip;
							fYAppStation.AddDesc = station.AddDesc;
						}
					}
				}
				if (model.AdminStations != null)
				{
					foreach (StationViewModel station in model.AdminStations)
					{
						if (station.FYAppStationsId == null || station.FYAppStationsId == Guid.Empty)
						{
							var fYAppStation = cwmContext.FYAppStations.Add(new Data.FYAppStation());
							fYAppStation.Year = model.Year;
							fYAppStation.DepartmentAddressId = model.AddressId;
							fYAppStation.AddressTypeId = AdminAddType;
							fYAppStation.AddressId = station.AddressId;
							fYAppStation.AddressCode = station.AddressCode;
							fYAppStation.AddressNumber = station.AddressNumber;
							fYAppStation.Direction = station.Direction;
							fYAppStation.Address = station.Address;
							fYAppStation.Suffix = station.Suffix;
							fYAppStation.SubAddress = station.SubAddress;
							fYAppStation.City = station.City;
							fYAppStation.StateAbbr = station.StateAbbr;
							fYAppStation.Zip = station.Zip;
							fYAppStation.AddDesc = station.AddDesc;
						}
					}
				}
				if (model.SubStations != null)
				{
					foreach (StationViewModel station in model.SubStations)
					{
						if (station.FYAppStationsId == null || station.FYAppStationsId == Guid.Empty)
						{
							var fYAppStation = cwmContext.FYAppStations.Add(new Data.FYAppStation());
							fYAppStation.Year = model.Year;
							fYAppStation.DepartmentAddressId = model.AddressId;
							fYAppStation.AddressTypeId = SubAddType;
							fYAppStation.AddressId = station.AddressId;
							fYAppStation.AddressCode = station.AddressCode;
							fYAppStation.AddressNumber = station.AddressNumber;
							fYAppStation.Direction = station.Direction;
							fYAppStation.Address = station.Address;
							fYAppStation.Suffix = station.Suffix;
							fYAppStation.SubAddress = station.SubAddress;
							fYAppStation.City = station.City;
							fYAppStation.StateAbbr = station.StateAbbr;
							fYAppStation.Zip = station.Zip;
							fYAppStation.AddDesc = station.AddDesc;
						}
					}
				}

				//model.Complete = true;
				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();

					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save FPF Application Record.", ex);
						model.Complete = false;
					}
				}
				else
				{
					logger.Error("Unable to save FPF Application Record., DbContext was not available.");
					model.Complete = false;
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unable to save FPF Application Record.", ex);
				model.Complete = false;
			}
			return model;
		}

		public async Task<decimal> RecalcNMFATotal(short year)
		{
			decimal result = 0;

			result = await cwmContext.nm_FYTotalDistributionCalcs.Where(a => a.Year == year && a.Inactive == false).SumAsync(a => a.NMFAAmount);

			return result;
		}

		public async Task<nm_FYDetailedApplication> LoadExistingApplicationAsync(Guid partyId, Guid addressId, short year)
		{
			nm_FYDetailedApplication result = null;

			result = await cwmContext.nm_FYDetailedApplications.FirstOrDefaultAsync(a => a.PartyID == partyId && a.AddressId == addressId && a.Year == year);

			return result;
		}
		
		public async Task<List<nm_FYDetailedApplication>> LoadExistingApplicationsAsync(Guid partyId)
		{
			List<nm_FYDetailedApplication> result;
			try
			{
				result = await cwmContext.nm_FYDetailedApplications.Where(a => a.PartyID == partyId).ToListAsync();
				return result;
			}
			catch (Exception ex)
            {
                _ = ex;
				
			}
			return new List<nm_FYDetailedApplication>();		
		}

		public async Task<List<nm_FYDetailedApplication>> LoadExistingApplicationsAsync(Guid partyId, short year)
		{
			List<nm_FYDetailedApplication> result;
			try
			{
				result = await cwmContext.nm_FYDetailedApplications.Where(a => a.PartyID == partyId && a.Year == year).ToListAsync();
				return result;
			}
			catch (Exception ex)
            {
                _ = ex;

			}
			return new List<nm_FYDetailedApplication>();
		}

		public async Task<List<nm_FYDetailedApplication>> LoadExistingApplicationByAddressAsync(Guid addressId, short year)
		{
			List<nm_FYDetailedApplication> result;
			try
			{
				result = await cwmContext.nm_FYDetailedApplications.Where(a => a.AddressId == addressId && a.Year == year).ToListAsync();
				return result;
			}
			catch (Exception ex)
            {
                _ = ex;

			}
			return new List<nm_FYDetailedApplication>();
		}

		public async Task<List<nm_FYDetailedApplication>> LoadSubmittedApplications(short year)
		{
			List<nm_FYDetailedApplication> result;
			try
			{
				result = await cwmContext.nm_FYDetailedApplications.Where(a => a.Year == year).ToListAsync();
				return result;
			}
			catch (Exception ex)
            {
                _ = ex;

			}
			return new List<nm_FYDetailedApplication>();
		}
		public async Task<nm_FYDetailedApplication> LoadSubmittedApplicationAsync(Guid applicationId)
		{
			nm_FYDetailedApplication result = null;

			result = await cwmContext.nm_FYDetailedApplications.FirstOrDefaultAsync(a => a.FYApplicationsId == applicationId);

			return result;
		}

		//added 02.05.2021 vwd
		public async Task<nm_FYDistributionInvoice> LoadExistingInvoiceAsync(Guid addressId, short year, short quarter)
		{
			nm_FYDistributionInvoice result = null;

			result = await cwmContext.nm_FYDistributionInvoice.FirstOrDefaultAsync(a => a.AddressId == addressId && a.Year == year && a.Quarter == quarter);

			return result;
		}

		public async Task<FYInvoices> LoadExistingInvoiceTableAsync(Guid addressId, short year, short quarter)
		{
			FYInvoices result = null;

			result = await cwmContext.FYInvoices.FirstOrDefaultAsync(a => a.AddressId == addressId && a.Year == year && a.Quarter == quarter);

			return result;
		}

		public async Task<List<FYInvoices>> LoadSavedInvoices(short year, short quarter)
		{
			List<FYInvoices> result;
			try
			{
				result = await cwmContext.FYInvoices.Where(a => a.Year == year && a.Quarter == quarter).ToListAsync();
				return result;
			}
			catch (Exception ex)
            {
                _ = ex;

			}
			return new List<FYInvoices>();
		}

		public async Task<DetailedFYInvoice> SaveInvoice(DetailedFYInvoice model)
        {
            try
            {
				FYInvoices fYInvoice = await cwmContext.FYInvoices.FirstOrDefaultAsync(f => f.FYInvoiceId == model.FYInvoiceId);

				if (fYInvoice == null || fYInvoice.FYInvoiceId == Guid.Empty)
				{
					fYInvoice = cwmContext.FYInvoices.Add(new Data.FYInvoices());
				}
				fYInvoice.InvoiceNo = model.InvoiceNo;
				fYInvoice.Year = model.Year;
				fYInvoice.AddressId = model.AddressId;
				fYInvoice.InvoiceDate = model.InvoiceDate;
				fYInvoice.Quarter = model.Quarter;
				fYInvoice.PartyName = model.PartyName;
				fYInvoice.DateSent = model.DateSent;
				if (model.Quarter == 1)
                {
					fYInvoice.InvoiceAmount = model.FirstAllotment;
                }
				else
                {
					fYInvoice.InvoiceAmount = model.SecondAllotment;
                }
				fYInvoice.Finalize = model.Finalize;
				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();

					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save FPF Invoice.", ex);
						model.Finalize = false;
					}
				}
				else
				{
					logger.Error("Unable to save FPF Invoice., DbContext was not available.");
					model.Finalize = false;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unable to save FPF Application Record.", ex);
				model.Finalize = false;
			}
			return model;
		}
	}
}

