using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NMSFM.Data;
using NMSFM.ViewModels;
using NMSFM.Services.Logging;
using NMSFM.Services.Images;
using NMSFM.Services.Party;
using NMSFM.Services.Address;
using NMSFM.Services.Account;
using NMSFM.Services.Menu;
using NMSFM.Services.FYDist;
using NMSFM.Services.CPSystem;
using NMSFM.Services.FireGrant;
using NMSFM.Services.UDF;
using Telerik.Web.UI;
using System.IO;
using System.Threading.Tasks;


namespace NMSFMFireGrantWF.Admin
{
    public partial class AdminReport : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private ISystemService systemService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;

        protected void Page_Init(object sender, EventArgs e)
        {
            var userWebModel = new UserWebModel();
            logger = new Logging();
accountService = new AccountService(userWebModel, logger);
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.fgAppService = new FGApplicationService(userContext, logger);
            }
            else
            {
                this.addressService = null;
            }
            try
            {
                if (Session["WebUserId"] == null || Convert.ToString(Session["WebUserId"]) == "")
                {
                    Response.Redirect("~/Account/Login");
                }
                if (Session["Role"] == null || Convert.ToString(Session["Role"]) == "External")
                {
                    Response.Redirect("~/Unauthorized");
                }
                //if (Session["IsWebAdmin"] == null || Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                //{
                //    Response.Redirect("~/Unauthorized");
                //}
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    //HtmlGenericControl helpdiv = new HtmlGenericControl();
                    //helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                    //FG_App_Help help = await fgService.GetFGHelpByPage("Manage Settings (Admin)");
                    //if (help != null)
                    //{
                    //    helpdiv.InnerHtml = help.HelpText;
                    //}
                    await LoadFiscalYears();
                    await LoadReport();
                }
                catch (Exception ex)
            {
                _ = ex;
                    dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                    dvError.Focus();
                }
            }
        }

        private async Task<bool> LoadFiscalYears()
        {
            try
            {
                ddlFiscalYear.Items.Clear();
                short fiscalyear = 0;
                fiscalyear = Convert.ToInt16(DateTime.Now.Year + 1);

                for (int y = 2022; y <= fiscalyear; y++)
                {
                    DropDownListItem li = new DropDownListItem();
                    li.Text = y.ToString();
                    li.Value = y.ToString();
                    ddlFiscalYear.Items.Add(li);
                }

                if (Session["FiscalYear"] != null)
                {
                    short sfy = Convert.ToInt16(Session["FiscalYear"]);
                    if (sfy == fiscalyear)
                    {
                        ddlFiscalYear.SelectedValue = fiscalyear.ToString();
                    }
                    else
                    {
                        ddlFiscalYear.SelectedValue = (fiscalyear - 1).ToString();
                    }
                }

                FGApplicationSettings appSettings = new FGApplicationSettings();
                appSettings = await fgService.GetFireGrantAppSettings(fiscalyear);
                if (appSettings != null)
                {
                    DateTime sDate = appSettings.StartDate;
                    DateTime eDate = appSettings.EndDate;
                    rdpStartDate.SelectedDate = sDate;
                    rdpEndDate.SelectedDate = eDate;
                }
                ddlFiscalYear.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
            return true;
        }

        private async Task<bool> LoadReport()
        {
            try
            {
                //short fy = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                var applications = await fgAppService.GetFGApplicationReportAsync();

                //if (txtConfNumber.Text != "")
                //{
                //    applications = applications.Where(a => a.ApplicationNumber.ToLower().Contains(txtConfNumber.Text.ToLower())).ToList();
                //}
                //if (txtDepartment.Text != "")
                //{
                //    applications = applications.Where(a => a.AddressCode.ToLower().Contains(txtDepartment.Text.ToLower())).ToList();
                //}
                //if (txtCounty.Text != "")
                //{
                //    applications = applications.Where(a => a.County.ToLower().Contains(txtCounty.Text.ToLower())).ToList();
                //}
                if (rdpStartDate.SelectedDate != null)
                {
                    applications = applications.Where(a => a.DateSubmitted >= rdpStartDate.SelectedDate || a.DateSubmitted == null).ToList();
                }
                if (rdpEndDate.SelectedDate != null)
                {
                    applications = applications.Where(a => a.DateSubmitted <= rdpEndDate.SelectedDate || a.DateSubmitted == null).ToList();
                }
                applications = applications.Where(a => a.FiscalYear == Convert.ToInt32(ddlFiscalYear.SelectedValue)).ToList();

                List<DetailedFGApplicationReport> detailedapplications = new List<DetailedFGApplicationReport>();
                if (applications != null)
                {
                    foreach (nm_FGApplicationReport appreport in applications)
                    {
                        short FiscalYear = (appreport.FiscalYear != null) ? Convert.ToInt16(appreport.FiscalYear) : Convert.ToInt16(DateTime.Now.Year);
                        Guid appId = new Guid(appreport.ApplicationId.ToString());
                        DateTime? lastReceivedGrant = await GetLastReceivedGrant(appreport.addressId, FiscalYear);
                        
                        string equipmentCategories = await GetEquipment(appId);


                        DetailedFGApplicationReport detailedapp = new DetailedFGApplicationReport();
                        detailedapp.FGApplicationIdentity = appreport.FGApplicationIdentity;
                        detailedapp.FiscalYear = appreport.FiscalYear;
                        detailedapp.addressId = appreport.addressId;
                        detailedapp.ApplicationNumber = appreport.ApplicationNumber;
                        detailedapp.DateStarted = appreport.DateStarted;
                        detailedapp.DateSubmitted = appreport.DateSubmitted;
                        detailedapp.SubmittedBy = appreport.SubmittedBy;
                        detailedapp.SubmittedByName = appreport.SubmittedByName;
                        detailedapp.AppStatus = appreport.AppStatus;
                        detailedapp.Status = appreport.Status;
                        detailedapp.LastStatusChange = appreport.LastStatusChange;
                        detailedapp.ApprovedDate = appreport.ApprovedDate;
                        detailedapp.ApprovedBy = appreport.ApprovedBy;
                        detailedapp.ApprovedByName = appreport.ApprovedByName;
                        detailedapp.GrantedAmount = (appreport.GrantedAmount != null) ? Convert.ToDecimal(appreport.GrantedAmount) : 0;
                        detailedapp.IndividualDept = (appreport.IndividualDept != null) ? Convert.ToInt32(appreport.IndividualDept) : 0; 
                        switch (detailedapp.IndividualDept)
                        {
                            case 1:
                                detailedapp.strIndividualDept = "Individual Department";
                                break;
                            case 2:
                                detailedapp.strIndividualDept = "County Wide Project";
                                break;
                            default:
                                break;
                        }
                        detailedapp.NERISID = appreport.NERISID;
                        detailedapp.DepartmentName = appreport.DepartmentName;
                        detailedapp.FireChiefName = appreport.FireChiefName;
                        detailedapp.Phone = appreport.Phone;
                        detailedapp.EmailAddress = appreport.EmailAddress;
                        detailedapp.ISORating = (appreport.ISORating != null) ? Convert.ToInt32(appreport.ISORating) : 0;
                        detailedapp.County = appreport.County;
                        detailedapp.IsCityMuni = (appreport.IsCityMuni != null) ? Convert.ToInt32(appreport.IsCityMuni) : 0;
                        switch (detailedapp.IsCityMuni)
                        {
                            case 1:
                                detailedapp.strCityMuni = "City/Municipality";
                                break;
                            case 2:
                                detailedapp.strCityMuni = "County";
                                break;
                            default:
                                break;
                        }
                        detailedapp.DeptType = (appreport.DeptType != null) ? Convert.ToInt32(appreport.DeptType) : 0;
                        switch (detailedapp.DeptType)
                        {
                            case 1:
                                detailedapp.strDeptType = "Career";
                                break;
                            case 2:
                                detailedapp.strDeptType = "Volunteer";
                                break;
                            case 3:
                                detailedapp.strDeptType = "Combined Career & Volunteer";
                                break;
                            default:
                                break;
                        }
                        detailedapp.IsAdminDept = (appreport.IsAdminDept != null) ? appreport.IsAdminDept : false;
                        detailedapp.CountyDeptsCompliant = (appreport.CountyDeptsCompliant != null) ? Convert.ToInt32(appreport.CountyDeptsCompliant) : 0;
                        switch (detailedapp.CountyDeptsCompliant)
                        {
                            case 1:
                                detailedapp.strCountyDeptsCompliant = "Yes";
                                break;
                            case 2:
                                detailedapp.strCountyDeptsCompliant = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.MainStations = (appreport.MainStations != null) ? Convert.ToInt32(appreport.MainStations) : 0;
                        detailedapp.SubStations = (appreport.SubStations != null) ? Convert.ToInt32(appreport.SubStations) : 0;
                        detailedapp.AdminBldgs = (appreport.AdminBldgs != null) ? Convert.ToInt32(appreport.AdminBldgs) : 0;
                        detailedapp.Community = (appreport.Community != null) ? Convert.ToInt32(appreport.Community) : 0;
                        switch (detailedapp.Community)
                        {
                            case 1:
                                detailedapp.strCommunity = "Urban";
                                break;
                            case 2:
                                detailedapp.strCommunity = "Rural";
                                break;
                            case 3:
                                detailedapp.strCommunity = "Sub-Urban";
                                break;
                            default:
                                break;
                        }
                        detailedapp.NumberOfFirefighters = (appreport.NumberOfFirefighters != null) ? Convert.ToInt32(appreport.NumberOfFirefighters) : 0;
                        detailedapp.FFI_Firefighters = (appreport.FFI_Firefighters != null) ? Convert.ToInt32(appreport.FFI_Firefighters) : 0;
                        detailedapp.FFII_Firefighters = (appreport.FFII_Firefighters != null) ? Convert.ToInt32(appreport.FFII_Firefighters) : 0;
                        detailedapp.MailingAddress = appreport.MailingAddress;
                        detailedapp.MailingCity = appreport.MailingCity;
                        detailedapp.MailingState = appreport.MailingState;
                        detailedapp.MailingZip = appreport.MailingZip;
                        detailedapp.PersonCompleteApp = appreport.PersonCompleteApp;
                        detailedapp.FireDeptMember = (appreport.FireDeptMember != null) ? Convert.ToInt32(appreport.FireDeptMember) : 0;
                        switch (detailedapp.FireDeptMember)
                        {
                            case 1:
                                detailedapp.strFireDeptMember = "Yes";
                                break;
                            case 2:
                                detailedapp.strFireDeptMember = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.OperatingBudget = (appreport.OperatingBudget != null) ? Convert.ToDecimal(appreport.OperatingBudget) : 0;
                        detailedapp.FPFDistribution = (appreport.FPFDistribution != null) ? Convert.ToDecimal(appreport.FPFDistribution) : 0;
                        detailedapp.StipendCarryover = (appreport.StipendCarryover != null) ? Convert.ToDecimal(appreport.StipendCarryover) : 0;
                        detailedapp.CarryoverBalance = (appreport.CarryoverBalance != null) ? Convert.ToDecimal(appreport.CarryoverBalance) : 0;
                        detailedapp.CarryoverPurpose = appreport.CarryoverPurpose = appreport.CarryoverPurpose;
                        detailedapp.PerTaxes = (appreport.PerTaxes != null) ? Convert.ToDecimal(appreport.PerTaxes / 100) : 0;
                        detailedapp.PerGrants = (appreport.PerGrants != null) ? Convert.ToDecimal(appreport.PerGrants / 100) : 0;
                        detailedapp.PerStateFMFunds = (appreport.PerStateFMFunds != null) ? Convert.ToDecimal(appreport.PerStateFMFunds / 100) : 0;
                        detailedapp.PerDonations = (appreport.PerDonations != null) ? Convert.ToDecimal(appreport.PerDonations / 100) : 0;
                        detailedapp.PerFundDrives = (appreport.PerFundDrives != null) ? Convert.ToDecimal(appreport.PerFundDrives / 100) : 0;
                        detailedapp.PerFeeForService = (appreport.PerFeeForService != null) ? Convert.ToDecimal(appreport.PerFeeForService / 100) : 0;
                        detailedapp.PerOthers = (appreport.PerOthers != null) ? Convert.ToDecimal(appreport.PerOthers / 100) : 0;
                        detailedapp.OthersDesc = appreport.OthersDesc;
                        detailedapp.PerTotal = (appreport.PerTotal != null) ? Convert.ToDecimal(appreport.PerTotal / 100) : 0;
                        detailedapp.CommunityName = appreport.CommunityName;
                        detailedapp.NumberOfHomes = (appreport.NumberOfHomes != null) ? Convert.ToInt32(appreport.NumberOfHomes) : 0;
                        detailedapp.NumberOfComm = (appreport.NumberOfComm != null) ? Convert.ToInt32(appreport.NumberOfComm) : 0;
                        detailedapp.ResidentPopulation = (appreport.ResidentPopulation != null) ? Convert.ToInt32(appreport.ResidentPopulation) : 0;
                        detailedapp.AidAgreements = (appreport.AidAgreements != null) ? Convert.ToInt32(appreport.NERISCurrent) : 0;
                        switch (detailedapp.AidAgreements)
                        {
                            case 1:
                                detailedapp.strAidAgreements = "Yes";
                                break;
                            case 2:
                                detailedapp.strAidAgreements = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.NERISCurrent = (appreport.NERISCurrent != null) ? Convert.ToInt32(appreport.NERISCurrent) : 0;
                        switch (detailedapp.NERISCurrent)
                        {
                            case 1:
                                detailedapp.strNERISCurrent = "Yes";
                                break;
                            case 2:
                                detailedapp.strNERISCurrent = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.ResponseStructure = (appreport.ResponseStructure != null) ? Convert.ToInt32(appreport.ResponseStructure) : 0;
                        detailedapp.ResponseVehicle = (appreport.ResponseVehicle != null) ? Convert.ToInt32(appreport.ResponseVehicle) : 0;
                        detailedapp.ResponseVegitation = (appreport.ResponseVegitation != null) ? Convert.ToInt32(appreport.ResponseVegitation) : 0;
                        detailedapp.ResponseEMS = (appreport.ResponseEMS != null) ? Convert.ToInt32(appreport.ResponseEMS) : 0;
                        detailedapp.ResponseRescue = (appreport.ResponseRescue != null) ? Convert.ToInt32(appreport.ResponseRescue) : 0;
                        detailedapp.ResponseHazardous = (appreport.ResponseHazardous != null) ? Convert.ToInt32(appreport.ResponseHazardous) : 0;
                        detailedapp.ResponseService = (appreport.ResponseService != null) ? Convert.ToInt32(appreport.ResponseService) : 0;
                        detailedapp.ResponseGoodIntent = (appreport.ResponseGoodIntent != null) ? Convert.ToInt32(appreport.ResponseGoodIntent) : 0;
                        detailedapp.ResponseFalse = (appreport.ResponseFalse != null) ? Convert.ToInt32(appreport.ResponseFalse) : 0;
                        detailedapp.ResponseOther = (appreport.ResponseOther != null) ? Convert.ToInt32(appreport.ResponseOther) : 0;
                        detailedapp.ResponseTotal = (appreport.ResponseTotal != null) ? Convert.ToInt32(appreport.ResponseTotal) : 0;
                        detailedapp.ComHydrantSys = (appreport.ComHydrantSys != null) ? Convert.ToInt32(appreport.ComHydrantSys) : 0;
                        switch (detailedapp.ComHydrantSys)
                        {
                            case 1:
                                detailedapp.strComHydrantSys = "Yes";
                                break;
                            case 2:
                                detailedapp.strComHydrantSys = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.AvailableWaterCapacity = (appreport.AvailableWaterCapacity != null) ? Convert.ToInt32(appreport.AvailableWaterCapacity) : 0;
                        detailedapp.WaterOnWheelsCapacity = (appreport.WaterOnWheelsCapacity != null) ? Convert.ToInt32(appreport.WaterOnWheelsCapacity) : 0;
                        detailedapp.StationWaterCapacity = (appreport.StationWaterCapacity != null) ? Convert.ToInt32(appreport.StationWaterCapacity) : 0;
                        detailedapp.TankAtStation = (appreport.TankAtStation != null) ? Convert.ToInt32(appreport.TankAtStation) : 0;
                        switch (detailedapp.TankAtStation)
                        {
                            case 1:
                                detailedapp.strTankAtStation = "Yes";
                                break;
                            case 2:
                                detailedapp.strTankAtStation = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.YearlyTrainingHours = (appreport.YearlyTrainingHours != null) ? Convert.ToInt32(appreport.YearlyTrainingHours) : 0;
                        detailedapp.NumberOfListedTrainings = (appreport.NumberOfListedTrainings != null) ? Convert.ToInt32(appreport.NumberOfListedTrainings) : 0;
                        detailedapp.ApparatusPartOfProject = (appreport.ApparatusPartOfProject != null) ? Convert.ToInt32(appreport.ApparatusPartOfProject) : 0;
                        switch (detailedapp.ApparatusPartOfProject)
                        {
                            case 1:
                                detailedapp.strApparatusPartOfProject = "Yes";
                                break;
                            case 2:
                                detailedapp.strApparatusPartOfProject = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.PumpTestsConducted = (appreport.PumpTestsConducted != null) ? Convert.ToInt32(appreport.PumpTestsConducted) : 0;
                        switch (detailedapp.PumpTestsConducted)
                        {
                            case 1:
                                detailedapp.strPumpTestsConducted = "Yes";
                                break;
                            case 2:
                                detailedapp.strPumpTestsConducted = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.ExplainNoPumpTests = appreport.ExplainNoPumpTests;
                        detailedapp.HoseTestConducted = (appreport.HoseTestConducted != null) ? Convert.ToInt32(appreport.HoseTestConducted) : 0;
                        switch (detailedapp.HoseTestConducted)
                        {
                            case 1:
                                detailedapp.strHoseTestsConducted = "Yes";
                                break;
                            case 2:
                                detailedapp.strHoseTestsConducted = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.ExplainNoHostTests = appreport.ExplainNoHostTests;
                        detailedapp.NumberOfListedApparatus = (appreport.NumberOfListedApparatus != null) ? Convert.ToInt32(appreport.HoseTestConducted) : 0;
                        detailedapp.CommunicationProject = (appreport.CommunicationProject != null) ? Convert.ToInt32(appreport.HoseTestConducted) : 0;
                        switch (detailedapp.CommunicationProject)
                        {
                            case 1:
                                detailedapp.strCommunicationProject = "Yes";
                                break;
                            case 2:
                                detailedapp.strCommunicationProject = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.HandheldRadios = (appreport.HandheldRadios != null) ? Convert.ToInt32(appreport.HandheldRadios) : 0;
                        detailedapp.BaseStations = (appreport.BaseStations != null) ? Convert.ToInt32(appreport.BaseStations) : 0;
                        detailedapp.MobileRadios = (appreport.MobileRadios != null) ? Convert.ToInt32(appreport.MobileRadios) : 0;
                        detailedapp.ApparatusWoRadio = (appreport.ApparatusWoRadio != null) ? Convert.ToInt32(appreport.ApparatusWoRadio) : 0;
                        detailedapp.LawEnforcement = (appreport.LawEnforcement != null) ? Convert.ToInt32(appreport.LawEnforcement) : 0;
                        detailedapp.EmergencyMedical = (appreport.EmergencyMedical != null) ? Convert.ToInt32(appreport.EmergencyMedical) : 0;
                        detailedapp.OtherFireDepts = (appreport.OtherFireDepts != null) ? Convert.ToInt32(appreport.OtherFireDepts) : 0;
                        detailedapp.Other = (appreport.Other != null) ? Convert.ToInt32(appreport.Other) : 0;
                        detailedapp.OtherDescription = appreport.OtherDescription;
                        detailedapp.AreasNotCovered = (appreport.AreasNotCovered != null) ? Convert.ToInt32(appreport.AreasNotCovered) : 0;
                        detailedapp.DescribeAreasNotCovered = appreport.DescribeAreasNotCovered;
                        detailedapp.NumberOfCommunicationDevicesListed = (appreport.NumberOfCommunicationDevicesListed != null) ? Convert.ToInt32(appreport.NumberOfCommunicationDevicesListed) : 0;
                        detailedapp.NumberOfHazardsThreatsListed = (appreport.NumberOfHazardsThreatsListed != null) ? Convert.ToInt32(appreport.NumberOfHazardsThreatsListed) : 0;
                        detailedapp.PPEPartOfProject = (appreport.PPEPartOfProject != null) ? Convert.ToInt32(appreport.PPEPartOfProject) : 0;
                        switch (detailedapp.PPEPartOfProject)
                        {
                            case 1:
                                detailedapp.strPPEPartOfProject = "Yes";
                                break;
                            case 2:
                                detailedapp.strPPEPartOfProject = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.PPEInspected = (appreport.PPEInspected != null) ? Convert.ToInt32(appreport.PPEInspected) : 0;
                        switch (detailedapp.PPEInspected)
                        {
                            case 1:
                                detailedapp.strPPEInspected = "Yes";
                                break;
                            case 2:
                                detailedapp.strPPEInspected = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.NumberOfPPEListed = (appreport.NumberOfPPEListed != null) ? Convert.ToInt32(appreport.NumberOfPPEListed) : 0;
                        detailedapp.NumberOfSCBAListed = (appreport.NumberOfSCBAListed != null) ? Convert.ToInt32(appreport.NumberOfSCBAListed) : 0;
                        detailedapp.SpecificNeeds = appreport.SpecificNeeds;
                        detailedapp.ISOImpacted = (appreport.ISOImpacted != null) ? Convert.ToInt32(appreport.ISOImpacted) : 0;
                        switch (detailedapp.ISOImpacted)
                        {
                            case 1:
                                detailedapp.strISOImpacted = "Yes";
                                break;
                            case 2:
                                detailedapp.strISOImpacted = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.ISOImpactExplanation = appreport.ISOImpactExplanation;
                        detailedapp.NumberOfEquipmentNeeded = (appreport.NumberOfEquipmentNeeded != null) ? Convert.ToInt32(appreport.NumberOfEquipmentNeeded) : 0;
                        detailedapp.AmountOfEquipmentNeeded = (appreport.AmountOfEquipmentNeeded != null) ? Convert.ToDecimal(appreport.AmountOfEquipmentNeeded) : 0;
                        detailedapp.TotalProjectCost = (appreport.TotalProjectCost != null) ? Convert.ToDecimal(appreport.TotalProjectCost) : 0;
                        detailedapp.AmountRequested = (appreport.AmountRequested != null) ? Convert.ToDecimal(appreport.AmountRequested) : 0;
                        detailedapp.StipendAmount = (appreport.StipendAmount != null) ? Convert.ToDecimal(appreport.StipendAmount) : 0;
                        detailedapp.NERISCompliant = appreport.NERISCompliant;
                        switch (detailedapp.NERISCompliant)
                        {
                            case 1:
                                detailedapp.strNERISCompliant = "Yes";
                                break;
                            case 2:
                                detailedapp.strNERISCompliant = "No";
                                break;
                            default:
                                break;
                        }
                        detailedapp.PumpTestCompliant = appreport.PumpTestCompliant;
                        switch (detailedapp.PumpTestCompliant)
                        {
                            case 1:
                                detailedapp.strPumpTestCompliant = "Yes";
                                break;
                            case 2:
                                detailedapp.strPumpTestCompliant = "No";
                                break;
                            default:
                                break;
                        }

                        short fiscalYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                        if (fiscalYear < 2024)
                        {
                            detailedapp.TrainingPoints = (appreport.TrainingPoints != null) ? Convert.ToInt32(appreport.TrainingPoints) : 0;
                            detailedapp.FinancialNeedGrade = (appreport.FinancialNeedGrade != null) ? Convert.ToInt32(appreport.FinancialNeedGrade) : 0;
                            detailedapp.ProblemGrade = (appreport.ProblemGrade != null) ? Convert.ToInt32(appreport.ProblemGrade) : 0;
                            detailedapp.BenefitGrade = (appreport.BenefitGrade != null) ? Convert.ToInt32(appreport.BenefitGrade) : 0;
                            detailedapp.ConsequencesGrade = (appreport.ConsequencesGrade != null) ? Convert.ToInt32(appreport.ConsequencesGrade) : 0;
                            detailedapp.AppCompletenessGrade = (appreport.AppCompletenessGrade != null) ? Convert.ToInt32(appreport.AppCompletenessGrade) : 0;
                            detailedapp.TotalScore = detailedapp.ISORating + detailedapp.TrainingPoints + detailedapp.FinancialNeedGrade + detailedapp.ProblemGrade + detailedapp.BenefitGrade + detailedapp.ConsequencesGrade + detailedapp.AppCompletenessGrade;
                        }
                        else
                        {
                            List<DetailedFGAppScores> appScores = new List<DetailedFGAppScores>();
                            appScores = await fgAppService.GetDetailedFGAppScoresAdminAsync(appId);
                            detailedapp.TrainingPoints = 0;
                            detailedapp.FinancialNeedGrade = 0;
                            detailedapp.ProblemGrade = 0;
                            detailedapp.BenefitGrade = 0;
                            detailedapp.ConsequencesGrade = 0;
                            detailedapp.AppCompletenessGrade = 0;
                            if (appScores.Count > 0)
                            {
                                foreach (DetailedFGAppScores appScore in appScores)
                                {
                                    detailedapp.TrainingPoints += appScore.TrainingPoints;
                                    detailedapp.FinancialNeedGrade += appScore.FinancialNeedGrade;
                                    detailedapp.ProblemGrade += appScore.ProblemGrade;
                                    detailedapp.BenefitGrade += appScore.BenefitGrade;
                                    detailedapp.ConsequencesGrade += appScore.ConsequencesGrade;
                                    detailedapp.AppCompletenessGrade += appScore.AppCompletenessGrade;
                                }
                                if (detailedapp.TrainingPoints > 0) { detailedapp.TrainingPoints = detailedapp.TrainingPoints / appScores.Count; }
                                if (detailedapp.FinancialNeedGrade > 0) { detailedapp.FinancialNeedGrade = detailedapp.FinancialNeedGrade / appScores.Count; }
                                if (detailedapp.ProblemGrade > 0) { detailedapp.ProblemGrade = detailedapp.ProblemGrade / appScores.Count; }
                                if (detailedapp.BenefitGrade > 0) { detailedapp.BenefitGrade = detailedapp.BenefitGrade / appScores.Count; }
                                if (detailedapp.ConsequencesGrade > 0) { detailedapp.ConsequencesGrade = detailedapp.ConsequencesGrade / appScores.Count; }
                                if (detailedapp.AppCompletenessGrade > 0) { detailedapp.AppCompletenessGrade = detailedapp.AppCompletenessGrade / appScores.Count; }
                                detailedapp.TotalScore = detailedapp.ISORating + detailedapp.TrainingPoints + detailedapp.FinancialNeedGrade + detailedapp.ProblemGrade + detailedapp.BenefitGrade + detailedapp.ConsequencesGrade + detailedapp.AppCompletenessGrade;
                            }
                        }
                        detailedapp.LastReceivedGrant = lastReceivedGrant;
                        detailedapp.PriorityCategories = equipmentCategories;
                        detailedapplications.Add(detailedapp);
                    }
                }
                rgDepartments.DataSource = detailedapplications;
                rgDepartments.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private async Task<DateTime?> GetLastReceivedGrant(Guid addressId, short FiscalYear)
        {
            try
            {
                DateTime? lastReceivedGrant = null;
                var lastApp = await fgAppService.GetAllFGApplicationByAddressAsync(addressId);
                lastApp = lastApp.Where(a => a.AppStatus == 7 && a.FiscalYear != FiscalYear).ToList();
                if (lastApp != null && lastApp.Count > 0)
                {
                    lastApp = lastApp.OrderBy(a => a.FiscalYear).ToList();
                    lastReceivedGrant = lastApp[0].DateSubmitted;
                }
                return lastReceivedGrant;
            }
            catch (Exception ex)
            {
                _ = ex;
                return null;
            }
        }

        private async Task<String> GetEquipment(Guid appId)
        {
            try
            {
                string equipmentCategories = "";
                var equipmentNeeds = await fgAppService.GetFGApplicationEquipmentNeedsAsync(appId);
                if (equipmentNeeds != null)
                {
                    var equipment = equipmentNeeds.ApplicationEquipment;
                    if (equipment != null)
                    {
                        foreach (FG_App_ApplicationEquipment equip in equipment)
                        {
                            equipmentCategories += equip.PriorityCategory + "; ";
                        }
                        equipmentCategories = equipmentCategories.Remove(equipmentCategories.Length - 2, 2);
                    }
                }
                return equipmentCategories;
            }
            catch (Exception ex)
            {
                _ = ex;
                return "Error";
            }
        }

        protected async void rgDepartments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            await LoadReport();
        }

        //protected async void rgDepartments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        //{
        //    LoadReport();
        //}

        protected void rgDepartments_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected async void ddlFiscalYear_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                short fiscalYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                FGApplicationSettings appSettings = new FGApplicationSettings();
                appSettings = await fgService.GetFireGrantAppSettings(fiscalYear);
                if (appSettings != null)
                {
                    DateTime sDate = appSettings.StartDate;
                    DateTime eDate = appSettings.EndDate;
                    rdpStartDate.SelectedDate = sDate;
                    rdpEndDate.SelectedDate = eDate;
                }
                await LoadReport();
                ddlFiscalYear.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadReport();
        }

        protected void lnkShowColumns_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RadComboBoxItem rcbItem in ddlReportColumns.Items)
                {
                    if (rgDepartments.MasterTableView.GetColumnSafe(rcbItem.Value.ToString()) != null)
                    {
                        rgDepartments.MasterTableView.GetColumnSafe(rcbItem.Value.ToString()).Visible = rcbItem.Checked;
                    }
                }
                rgDepartments.Rebind();
                
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected void ibtnExportExcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                rgDepartments.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");

                rgDepartments.ExportSettings.Excel.WorksheetName = "AdminReport";
                rgDepartments.ExportSettings.FileName = "Fire Grant Admin Report " + DateTime.Now.ToShortDateString();
                rgDepartments.ExportSettings.IgnorePaging = true;
                rgDepartments.ExportSettings.ExportOnlyData = false;
                rgDepartments.ExportSettings.OpenInNewWindow = true;
                rgDepartments.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected void ibtnExportPdf_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                rgDepartments.MasterTableView.AllowFilteringByColumn = false;
                if (!rgDepartments.ExportSettings.IgnorePaging)
                {
                    rgDepartments.Rebind();
                }
                rgDepartments.MasterTableView.ExportToPdf();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected void lnkExcelExport_Click(object sender, EventArgs e)
        {
            try
            {
                rgDepartments.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");

                rgDepartments.ExportSettings.Excel.WorksheetName = "AdminReport";
                rgDepartments.ExportSettings.FileName = "Fire Grant Admin Report " + DateTime.Now.ToShortDateString();
                rgDepartments.ExportSettings.IgnorePaging = true;
                rgDepartments.ExportSettings.ExportOnlyData = false;
                rgDepartments.ExportSettings.OpenInNewWindow = true;
                rgDepartments.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected void lnkExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                rgDepartments.MasterTableView.AllowFilteringByColumn = false;
                if (!rgDepartments.ExportSettings.IgnorePaging)
                {
                    rgDepartments.Rebind();
                }
                rgDepartments.MasterTableView.ExportToPdf();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected void rgDepartments_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (rgDepartments.IsExporting)
                FormatGridItem(e.Item);
        }

        protected void FormatGridItem(GridItem item)
        {
            //item.Style["color"] = "#eeeeee";

            if (item is GridDataItem)
            {
                item.Style["vertical-align"] = "middle";
                item.Style["text-align"] = "center";
            }

            switch (item.ItemType) //Mimic RadGrid appearance for the exported PDF file
            {
                case GridItemType.Item: item.Style["border"] = "1px solid grey"; break;
                //case GridItemType.AlternatingItem: item.Style["background-color"] = "#494949"; break;
                case GridItemType.Header: item.Style["background-color"] = "lightgray"; item.Style["border"] = "1px solid grey"; break;
                //case GridItemType.CommandItem: item.Style["background-color"] = "#000000"; break;
            }

            if (item is GridCommandItem)
            {
                item.PrepareItemStyle();  //needed to span the image over the CommandItem cells
            }
        }
    }
}





