using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IO;
using NMSFM.Data;
using NMSFM.ViewModels;
using NMSFM.Services.Logging;
using NMSFM.Services.Images;
using NMSFM.Services.Party;
using NMSFM.Services.Address;
using NMSFM.Services.Account;
using NMSFM.Services.Menu;
using NMSFM.Services.FYDist;
using NMSFM.Services.FireGrant;
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Flow.Model;

namespace NMSFMFireGrantWF.Application.Reporting
{
    public partial class ApplicationPrint : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
// private IAccountService accountService; // legacy field, currently unused
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;
        private IUDFService udfService;

        protected void Page_Init(object sender, EventArgs e)
        {
            logger = new Logging();
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fpfService = new FPFService(userContext, logger);
                this.fgAppService = new FGApplicationService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.udfService = new UDFService(userContext, logger);

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
                if (Session["Role"] == null)
                {
                    Response.Redirect("~/Unauthorized");
                }
                if (Session["Department"] == null)
                {
                    if (Session["Role"].ToString() == "Internal")
                    {
                        Response.Redirect("~/Admin/Home");
                    }
                    else if (Session["Role"].ToString() == "External")
                    {
                        Response.Redirect("~/User/Home");
                    }
                    else
                    {
                        Response.Redirect("~/Unauthorized");
                    }
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        FGApplications app = new FGApplications();
                        app = await fgAppService.GetFGApplicationById(appIdGuid);
                        short fiscalYear = app.FiscalYear;
                        FGApplicationSettings appSettings = new FGApplicationSettings();
                        appSettings = await fgService.GetFireGrantAppSettings(fiscalYear);
                        if (appSettings.EligibilityRequirementsText != null)
                        {
                            dvEligibilityRequirements.InnerHtml = appSettings.EligibilityRequirementsText;
                        }
                        else
                        {
                            dvEligibilityRequirements.InnerHtml = fgService.GetDefaultEligibilityRequirements();
                        }
                        spStartDate.InnerHtml = appSettings.StartDate.ToShortDateString();
                        spEndDate.InnerHtml = appSettings.EndDate.ToShortDateString();
                        spFiscalYear.InnerHtml = "FY" + appSettings.FiscalYear.ToString();
                        spcertification.InnerHtml = appSettings.faCertifiationText;
                        FG_App_GeneralInfo genInfo = new FG_App_GeneralInfo();
                        genInfo = await fgAppService.GetFGApplicationGeneralInfoAsync(appIdGuid);
                        FG_App_BudgetInfo budgetInfo = new FG_App_BudgetInfo();
                        budgetInfo = fgAppService.GetFGApplicationBudgetInfo(appIdGuid);
                        DetailedFGAppCommunityInfo communityInfo = new DetailedFGAppCommunityInfo();
                        communityInfo = await fgAppService.GetFGApplicationCommunityInfoAsync(appIdGuid);
                        FG_App_ResponseHistory responseHistory = new FG_App_ResponseHistory();
                        responseHistory = await fgAppService.GetFGApplicationResponseHistoryAsync(appIdGuid);
                        DetailedFGWaterAvailability waterAvailability = new DetailedFGWaterAvailability();
                        waterAvailability = await fgAppService.GetFGApplicationWaterAvailabilityAsync(appIdGuid);
                        DetailedFGAppTraining training = new DetailedFGAppTraining();
                        training = await fgAppService.GetFGApplicationTrainingAsync(appIdGuid);
                        DetailedFGApparatus apparatus = new DetailedFGApparatus();
                        apparatus = await fgAppService.GetFGApplicationApparatusAsync(appIdGuid);
                        DetailedFGCommunication communication = new DetailedFGCommunication();
                        communication = await fgAppService.GetFGApplicationCommunicationAsync(appIdGuid);
                        DetailedFGAppHazardsThreats hazardsThreats = new DetailedFGAppHazardsThreats();
                        hazardsThreats = await fgAppService.GetFGApplicationHazardsThreatsAsync(appIdGuid);
                        DetailedFGAppPPE ppe = new DetailedFGAppPPE();
                        ppe = await fgAppService.GetFGApplicationPPEAsync(appIdGuid);
                        DetailedFGAppEquipmentNeeds equipmentNeeds = new DetailedFGAppEquipmentNeeds();
                        equipmentNeeds = await fgAppService.GetFGApplicationEquipmentNeedsAsync(appIdGuid);
                        FG_App_FundingJustification fundingJustification = new FG_App_FundingJustification();
                        fundingJustification = await fgAppService.GetFGApplicationFundingJustificationAsync(appIdGuid);
                        FG_App_ProjectBudget projectBudget = new FG_App_ProjectBudget();
                        projectBudget = await fgAppService.GetFGApplicationProjectBudgetAsync(appIdGuid); DetailedFGAppSigsDocs docsSigs = new DetailedFGAppSigsDocs();
                        docsSigs = await fgAppService.GetFGApplicationDocsSigsAsync(appIdGuid);
                        
                        if (genInfo != null && genInfo.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadDepartment();
                            LoadGeneralInfoData(genInfo);
                        }
                        else
                        {
                            LoadDepartment();
                        }
                        if (budgetInfo != null && budgetInfo.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadBudgetInfoData(budgetInfo);
                        }
                        if (communityInfo != null && communityInfo.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadCommunityInfoData(communityInfo);
                        }
                        if (responseHistory != null && responseHistory.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadResponseHistory(responseHistory);
                        }
                        if (waterAvailability != null && waterAvailability.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadWaterAvailability(waterAvailability);
                        }
                        if (training != null && training.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadTraining(training);
                        }
                        if (apparatus != null && apparatus.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadApparatusInfo(apparatus);
                        }
                        if (communication != null && communication.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadCommunication(communication);
                        }
                        if (hazardsThreats != null && hazardsThreats.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadHazadsThreats(hazardsThreats);
                        }
                        if (ppe != null && ppe.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadPPE(ppe);
                        }
                        if (equipmentNeeds != null && equipmentNeeds.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadEquipmentNeeds(equipmentNeeds);
                        }
                        if (fundingJustification != null && fundingJustification.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadFundingJustification(fundingJustification);
                        }
                        if (projectBudget != null && projectBudget.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadProjectBudget(projectBudget);
                        }
                        if (docsSigs != null && docsSigs.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadDocsSigs(docsSigs);
                        }
                    }
                    else
                    {
                        LoadDepartment();
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private async void LoadDepartment()
        {
            try
            {
                var department = new v_AddressParties();
                Guid deptId = new Guid(Session["Department"].ToString());
                department = await fgService.GetFGDepartmentByIdAsync(deptId);

                if (department != null)
                {
                    tdDepartmentName.InnerHtml = department.AddressCode;
                    spDepartmentName2.InnerHtml = department.AddressCode;
                    string addressDesc = "";

                    if (department.AddressNumber != null && department.AddressNumber != "")
                    {
                        addressDesc += department.AddressNumber;
                    }
                    if (department.Direction != null)
                    {
                        addressDesc += " " + department.Direction;
                    }
                    if (department.Address != null)
                    {
                        addressDesc += " " + department.Address;
                    }
                    if (department.Suffix != null)
                    {
                        addressDesc += " " + department.Suffix;
                    }
                    string County = (await addressService.GetCountyListAsync()).First(c => c.CountyId == department.CountyId).County1;
                    tdCounty.InnerHtml = County;
                    spCounty2.InnerHtml = County;
                    tdAddress.InnerHtml = addressDesc.Trim();
                    tdCity.InnerHtml = department.City;
                    tdState.InnerHtml = department.State;
                    tdZipCode.InnerHtml = department.Zip;


                    LoadDepartmentUDFs(deptId);
                }
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        private async void LoadDepartmentUDFs(Guid departmentId)
        {
            Guid mainGuid = new Guid("7ad61001-cac8-4f3c-ae4e-32d28393f891");
            Guid adminGuid = new Guid("8baa0b86-f1e5-4d84-b4f9-a8219f4b11b8");
            Guid subGuid = new Guid("4f34b96d-d944-44aa-9665-d47c55cc025d");
            Guid isoGuid = new Guid("6b8517ef-9483-4b8b-8c95-5b95a6b8f579");

            try
            {
                string iSO = await udfService.GetUDFValueAsync(isoGuid, departmentId);
                if (iSO == "")
                {
                    iSO = "0";
                }
                tdISO.InnerHtml = iSO;  //Header Info Agency UDFs

                string MC = await udfService.GetUDFValueAsync(mainGuid, departmentId);
                if (MC == "")
                {
                    MC = "0";
                }
                tdMainStations.InnerHtml = MC;  //Header Info Agency UDFs

                string AC = await udfService.GetUDFValueAsync(adminGuid, departmentId);
                if (AC == "")
                {
                    AC = "0";
                }
                tdAdmin.InnerHtml = AC;  //Header Info Agency UDFs

                string SC = await udfService.GetUDFValueAsync(subGuid, departmentId);
                if (SC == "")
                {
                    SC = "0";
                }
                tdSubstations.InnerHtml = SC;  //Header Info Agency UDFs
            }
            catch (Exception ex)
            {
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }

        }

        private void LoadGeneralInfoData(FG_App_GeneralInfo model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    //dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                }
                if (model.IndividualDept == 1)
                {
                    tdGrantRequestType.InnerHtml = "Individual Department";
                    trCountyApp.Visible = false;
                }
                else if (model.IndividualDept == 2)
                {
                    tdGrantRequestType.InnerHtml = "County Wide Project";
                    trCountyApp.Visible = true;
                    if (model.CountyDeptsCompliant == 1)
                    {
                        tdAllNERISCompliant.InnerHtml = "Yes";
                    }
                    else if (model.CountyDeptsCompliant == 2)
                    {
                        tdAllNERISCompliant.InnerHtml = "No";
                    }
                }
                if (model.NERISID != "") { tdFireDeptId.InnerHtml = model.NERISID; }
                if (model.DepartmentName != "") { tdDepartmentName.InnerHtml = model.DepartmentName; spDepartmentName.InnerHtml = model.DepartmentName; }
                if (model.FireChiefName != "") { tdFireCheifName.InnerHtml = model.FireChiefName; }
                if (model.Phone != "") { tdPhone.InnerHtml = model.Phone; }
                if (model.EmailAddress != "") { tdEmail.InnerHtml = model.EmailAddress; }
                if (model.IsCityMuni == 1)
                {
                    tdDepartmentType.InnerHtml = "City/Municipality";
                }
                else if (model.IsCityMuni == 2)
                {
                    tdDepartmentType.InnerHtml = "County";
                }
                if (model.DeptType == 1) { tdOrganizationType.InnerHtml = "Career"; }
                else if (model.DeptType == 2) { tdOrganizationType.InnerHtml = "Volunteer"; }
                else if (model.DeptType == 3) { tdOrganizationType.InnerHtml = "Combined Career & Volunteer"; }
                if (model.IsAdminDept) { tdOrganizationType.InnerHtml = tdOrganizationType.InnerHtml + " (Administation)"; };
                if (model.Community == 1) { tdCommunityType.InnerHtml = "Urban"; }
                else if (model.Community == 2) { tdCommunityType.InnerHtml = "Rural"; }
                else if (model.DeptType == 3) { tdCommunityType.InnerHtml = "Suburban"; }
                if (model.NumberOfFirefighters != null) { tdFireFighters.InnerHtml = model.NumberOfFirefighters.ToString(); }
                if (model.FFI_Firefighters != null) { tdFFI.InnerHtml = model.FFI_Firefighters.ToString(); }
                if (model.FFII_Firefighters != null) { tdFFII.InnerHtml = model.FFII_Firefighters.ToString(); }
                if (model.MailingAddress != "") { tdAddress.InnerHtml = model.MailingAddress; }
                if (model.MailingCity != "") { tdCity.InnerHtml = model.MailingCity; }
                if (model.MailingState != "") { tdState.InnerHtml = model.MailingState; }
                if (model.MailingZip != "") { tdZipCode.InnerHtml = model.MailingZip; }
                if (model.PersonCompleteApp != "") { tdPersonCompletingApp.InnerHtml = model.PersonCompleteApp; }
                if (model.FireDeptMember == 1)
                {
                    tdFireDepartmentMember.InnerHtml = "Yes";
                }
                else if (model.FireDeptMember == 2)
                {
                    tdFireDepartmentMember.InnerHtml = "No";
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private void LoadBudgetInfoData(FG_App_BudgetInfo model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    //dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidInnerHtml + "</div>";
                }
                tdOperatingBudget.InnerHtml = model.OperatingBudget.ToString("C");
                tdFPFDistribution.InnerHtml = model.FPFDistribution.ToString("C");
                tdStipendCarryover.InnerHtml = model.StipendCarryover.ToString("C");
                tdCarryoverBalance.InnerHtml = model.CarryoverBalance.ToString("C");
                tdCarryoverPurpose.InnerHtml = model.CarryoverPurpose.ToString();
                tdPerTaxes.InnerHtml = (model.PerTaxes > 0) ? (model.PerTaxes / 100).ToString("P") : 0.ToString("P");
                tdPerGrants.InnerHtml = (model.PerGrants > 0) ? (model.PerGrants / 100).ToString("P") : 0.ToString("P");
                tdPerStateFMFunds.InnerHtml = (model.PerStateFMFunds > 0) ? (model.PerStateFMFunds / 100).ToString("P") : 0.ToString("P");
                tdPerDonations.InnerHtml = (model.PerDonations > 0) ? (model.PerDonations / 100).ToString("P") : 0.ToString("P");
                tdPerFundDrives.InnerHtml = (model.PerFundDrives > 0) ? (model.PerFundDrives / 100).ToString("P") : 0.ToString("P");
                tdPerFeeForService.InnerHtml = (model.PerFeeForService > 0) ? (model.PerFeeForService / 100).ToString("P") : 0.ToString("P");
                tdPerOthers.InnerHtml = (model.PerOthers > 0) ? (model.PerOthers / 100).ToString("P") : 0.ToString("P");
                tdPerOthersDesc.InnerHtml = model.OthersDesc.ToString();
            }
            catch (Exception ex)
            {
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private void LoadCommunityInfoData(DetailedFGAppCommunityInfo model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    //if (model.InvalidText != null)
                    //{
                    //    dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                    //}
                    //else
                    //{
                    //    dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                    //}
                    
                }
                tdCommunityProtected.InnerHtml = model.CommunityName;
                tdHomesProtected.InnerHtml = model.NumberOfHomes.ToString();
                tdCommercialBuildings.InnerHtml = model.NumberOfComm.ToString();
                tdPopulation.InnerHtml = model.ResidentPopulation.ToString();
                if (model.AidAgreements == 1)
                {
                    tdAgreements.InnerHtml = "Yes";
                }
                else if (model.AidAgreements == 2)
                {
                    tdAgreements.InnerHtml = "No";
                }
                if (model.AidDistricts != null)
                {
                    string aidDistricts = "";
                    foreach (FG_App_AidDistricts district in model.AidDistricts)
                    {
                        aidDistricts += "<tr><td class='rowData'>" + district.Number.ToString() + "</td><td  colspan='3' class='rowData'>" + district.AidDistrict.ToString() + "</td></tr>";
                    }
                    ltrAidDistricts.Text = aidDistricts;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                //throw ex;
            }
        }

        private void LoadResponseHistory(FG_App_ResponseHistory model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    //dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidInnerHtml + "</div>";
                }
                if (model.NERISCurrent == 1) { tdNERISCurrent.InnerHtml = "Yes"; }
                if (model.NERISCurrent == 2) { tdNERISCurrent.InnerHtml = "No"; }
                spStructureFire.InnerHtml = model.ResponseStructure.ToString();
                spVehiclefire.InnerHtml = model.ResponseVehicle.ToString();
                spVegitationFire.InnerHtml = model.ResponseVegitation.ToString();
                spEMS.InnerHtml = model.ResponseEMS.ToString();
                spRescue.InnerHtml = model.ResponseRescue.ToString();
                spHazardous.InnerHtml = model.ResponseHazardous.ToString();
                spService.InnerHtml = model.ResponseService.ToString();
                spGoodIntent.InnerHtml = model.ResponseGoodIntent.ToString();
                spFalse.InnerHtml = model.ResponseFalse.ToString();
                spOther.InnerHtml = model.ResponseOther.ToString();
                spTotalCalls.InnerHtml = model.ResponseTotal.ToString();
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadWaterAvailability(DetailedFGWaterAvailability model)
        {
            try
            {
                //if (model.IsValid == false)
                //{
                //    if (model.InvalidText != null)
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                //    }
                //    else
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                //    }
                //}
                if (model.ComHydrantSys == 1) { tdCommunityHydrant.InnerHtml = "Yes"; }
                if (model.ComHydrantSys == 2) { tdCommunityHydrant.InnerHtml = "No"; }
                tdAvailableWater.InnerHtml = model.AvailableWaterCapacity.ToString();
                tdWaterOnWheels.InnerHtml = model.WaterOnWheelsCapacity.ToString();
                tdWaterAtStation.InnerHtml = model.StationWaterCapacity.ToString();
                if (model.TankAtStation == 1) { tdStorageTankAtStation.InnerHtml = "Yes"; }
                if (model.TankAtStation == 2) { tdStorageTankAtStation.InnerHtml = "No"; }
                if (model.WaterSources != null)
                {
                    string waterSources = "";
                    foreach (FG_App_WaterSources waterSource in model.WaterSources)
                    {
                        waterSources += "<tr><td  class='rowHeader'>" + waterSource.Number.ToString() + "</td><td  colspan='2' class='rowData'>" + waterSource.WaterSource.ToString() + "</td><td>" + waterSource.Capacity + "</td></tr>";
                    }
                    ltrWaterSources.Text = waterSources;
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadTraining(DetailedFGAppTraining model)
        {
            try
            {
                //if (model.IsValid == false)
                //{
                //    if (model.InvalidText != null)
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                //    }
                //    else
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                //    }
                //}
                tdTrainingHours.InnerHtml = model.YearlyTrainingHours.ToString();

                if (model.TrainingOpportunities != null)
                {
                    string trainings = "";
                    foreach (FG_App_TrainingOpportunityView trainingOps in model.TrainingOpportunities)
                    {
                        string trainingDocName = (trainingOps.TrainingDocumentName != null) ? trainingOps.TrainingDocumentName.ToString() : "";
                        trainings += "<tr><td colspan='2' class='rowHeader'>" + trainingOps.TrainingDetail.ToString() + "</td><td  colspan='2' class='rowData'>" + trainingDocName + "</td></tr>";
                    }
                    ltrTrainings.Text = trainings;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadApparatusInfo(DetailedFGApparatus model)
        {
            try
            {
                //if (model.IsValid == false)
                //{
                //    if (model.InvalidText != null)
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                //    }
                //    else
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                //    }
                //}
                if (model.ApparatusPartOfProject == 1) { tdApparatusPartOfProject.InnerHtml = "Yes"; tbApparatusPart.Visible = true; tbApparatusList.Visible = true; }
                if (model.ApparatusPartOfProject == 2) { tdApparatusPartOfProject.InnerHtml = "No"; tbApparatusPart.Visible = false; tbApparatusList.Visible = false; }
                if (model.PumpTestsConducted == 1)
                {
                    tdPumpTestsConducted.InnerHtml = "Yes";
                }
                else if (model.PumpTestsConducted == 2)
                {
                    tdPumpTestsConducted.InnerHtml = "No";
                    trNoPumpTests.Visible = true;
                    tdNoPumpTestsExplanation.InnerHtml = model.ExplainNoPumpTests.ToString();
                }

                if (model.HoseTestConducted == 1)
                {
                    tdHoseTestsConducted.InnerHtml = "Yes";
                }
                else if (model.HoseTestConducted == 2)
                {
                    trNoHoseTests.Visible = true;
                    tdHoseTestsConducted.InnerHtml = "No";
                    tdNoHoseTestsExplanation.InnerHtml = model.ExplainNoHostTests.ToString();
                }

                if (model.ApparatusEquipment != null)
                {
                    string apparatusEquip = "";
                    foreach (FG_App_ApparatusEquipment appEquip in model.ApparatusEquipment)
                    {
                        string appLicense = (appEquip.License != null) ? appEquip.License.ToString() : "";
                        string vin = (appEquip.VIN != null) ? appEquip.VIN.ToString() : "";
                        string type = (appEquip.VehicleType != null) ? appEquip.VehicleType.ToString() : "";
                        string testDate = (appEquip.TestDate != null) ? Convert.ToDateTime(appEquip.TestDate).ToShortDateString() : "";
                        string comments = (appEquip.Comments != null) ? appEquip.Comments.ToString() : "";
                        string passFail = "N/A";
                        if (appEquip.Pass != null)
                        {
                            passFail = (Convert.ToBoolean(appEquip.Pass)) ? "Pass" : "Fail";
                        }
                        apparatusEquip += "<tr><td class='rowData'>" + appEquip.Number.ToString() + "</td>";
                        apparatusEquip += "<td class='rowData'>" + vin + "</td>";
                        apparatusEquip += "<td class='rowData'>" + type + "</td>";
                        apparatusEquip += "<td class='rowData'>" + appEquip.Year + "</td>";
                        apparatusEquip += "<td class='rowData'>" + appLicense + "</td>";
                        apparatusEquip += "<td class='rowData'>" + appEquip.Capacity + "</td>";
                        apparatusEquip += "<td class='rowData'>" + appEquip.GPM + "</td>";
                        apparatusEquip += "<td class='rowData'>" + testDate + "</td>";
                        apparatusEquip += "<td class='rowData'>" + passFail + "</td>";
                        apparatusEquip += "<td class='rowData'>" + comments + "</td></tr>";
                    }
                    ltrApparatusDetails.Text = apparatusEquip;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadCommunication(DetailedFGCommunication model)
        {
            try
            {
                //if (model.IsValid == false)
                //{
                //    if (model.InvalidText != null)
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                //    }
                //    else
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                //    }
                //}
                if (model.CommunicationProject == 1) { tdCommunicationEquipmentPartOfProject.InnerHtml = "Yes"; IsPart.Visible = true; }
                if (model.CommunicationProject == 2) { tdCommunicationEquipmentPartOfProject.InnerHtml = "No"; IsPart.Visible = false; }

                tdHandheldRadios.InnerHtml = model.HandheldRadios.ToString();
                tdBaseStations.InnerHtml = model.BaseStations.ToString();
                tdMobileRadios.InnerHtml = model.MobileRadios.ToString();

                if (model.ApparatusWoRadio == 1) { tdApparatusWithoutRadio.InnerHtml = "Yes"; }
                if (model.ApparatusWoRadio == 2) { tdApparatusWithoutRadio.InnerHtml = "No"; }

                if (model.LawEnforcement == 1) { tdLawEnforcement.InnerHtml = "Yes"; }
                if (model.LawEnforcement == 2) { tdLawEnforcement.InnerHtml = "No"; }

                if (model.EmergencyMedical == 1) { tdEmergencyMedical.InnerHtml = "Yes"; }
                if (model.EmergencyMedical == 2) { tdEmergencyMedical.InnerHtml = "No"; }

                if (model.OtherFireDepts == 1) { tdOtherDepartments.InnerHtml = "Yes"; }
                if (model.OtherFireDepts == 2) { tdOtherDepartments.InnerHtml = "No"; }

                if (model.Other == 1) { 
                    tdOtherInterop.InnerHtml = "Yes"; 
                }
                if (model.Other == 2) { 
                    tdOtherInterop.InnerHtml = "No";
                    
                }
                tdOtherInteropDesc.InnerHtml = (model.OtherDescription != null) ? model.OtherDescription.ToString() : "";

                if (model.AreasNotCovered == 1) {
                    tdOtherJurisdictions.InnerHtml = "Yes";
                    trOtherJurisdictionsNotCovered.Visible = true;
                    tdOtherJurisdictionsNotCovered.InnerHtml = (model.DescribeAreasNotCovered != null) ? model.DescribeAreasNotCovered.ToString() : "";
                }
                if (model.AreasNotCovered == 2) {
                    tdOtherJurisdictions.InnerHtml = "No"; 
                }

                if (model.CommunicationEquipment != null)
                {
                    string commEquip = "";
                    foreach (FG_App_CommunicationEquipment communicationEquipment in model.CommunicationEquipment)
                    {
                        commEquip += "<tr><td class='rowHeader'>" + communicationEquipment.Number.ToString() + "</td><td  colspan='2' class='rowData'>" + communicationEquipment.CommunicationEquipment.ToString() + "</td><td class='rowData'>" + communicationEquipment.CommunicationQty.ToString() + "</td></tr>";
                    }
                    ltrCommunicationEquipment.Text = commEquip;
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadHazadsThreats(DetailedFGAppHazardsThreats model)
        {
            try
            {
                //if (model.IsValid == false)
                //{
                //    if (model.InvalidText != null)
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                //    }
                //    else
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                //    }
                //}
                if (model.HazardsThreats != null)
                {
                    string strHazThreats = "";
                    foreach (FG_App_HazardThreatEvents hazards in model.HazardsThreats)
                    {
                        strHazThreats += "<tr><td colspan='1' class='rowHeader'>" + hazards.HazardType.ToString() + "</td><td  colspan='3' class='rowData'>" + hazards.HazardDetail.ToString() + "</td></tr>";
                    }
                    ltrHazards.Text = strHazThreats;
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadPPE(DetailedFGAppPPE model)
        {
            try
            {
                //if (model.IsValid == false)
                //{
                //    if (model.InvalidText != null)
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                //    }
                //    else
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                //    }
                //}
                if (model.PPEPartOfProject == 1) { tdPPEIsPart.InnerHtml = "Yes"; tbPPEPart.Visible = true; }
                if (model.PPEPartOfProject == 2) { tdPPEIsPart.InnerHtml = "No"; tbPPEPart.Visible = false; }

                if (model.SCBAPartOfProject == 1) { tdSCBAIsPart.InnerHtml = "Yes"; tbSCBAPart.Visible = true; }
                if (model.SCBAPartOfProject == 2) { tdSCBAIsPart.InnerHtml = "No"; tbSCBAPart.Visible = false; }

                if (model.PPEInspected == 1) { tdPPEInspected.InnerHtml = "Yes"; }
                if (model.PPEInspected == 2) { tdPPEInspected.InnerHtml = "No"; }

                if (model.StandardPPE != null)
                {
                    string strPPE = "";
                    foreach (FG_App_StandardPPE ppe in model.StandardPPE)
                    {
                        strPPE += "<tr><td class='rowHeader'>" + ppe.Year.ToString() + "</td><td class='rowData'>" + ppe.Quantity.ToString() + "</td><td class='rowData'>" + ppe.Age.ToString() + "</td><td class='rowData'>" + ppe.Condition.ToString() + "</td></tr>";
                    }
                    ltrPPE.Text = strPPE;
                }

                if (model.StandardSCBA != null)
                {
                    string strSCBA = "";
                    foreach (FG_App_StandardSCBA scba in model.StandardSCBA)
                    {
                        strSCBA += "<tr><td class='rowHeader'>" + scba.Year.ToString() + "</td><td class='rowData'>" + scba.Quantity.ToString() + "</td><td class='rowData'>" + scba.Age.ToString() + "</td><td class='rowData'>" + scba.Condition.ToString() + "</td></tr>";
                    }
                    ltrSCBA.Text = strSCBA;
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadEquipmentNeeds(DetailedFGAppEquipmentNeeds model)
        {
            try
            {
                //if (model.IsValid == false)
                //{
                //    if (model.InvalidText != null)
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                //    }
                //    else
                //    {
                //        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                //    }
                //}
                tdWhatPurchased.InnerHtml = model.SpecificNeeds;
                if (model.ISOImpacted == 1) { tdISOAffected.InnerHtml = "Yes"; trISOChangeExp.Visible = true; }
                if (model.ISOImpacted == 2) { tdISOAffected.InnerHtml = "No"; trISOChangeExp.Visible = false; }
                tdISOChangeExp.InnerHtml = (model.ISOImpactExplanation != null) ? model.ISOImpactExplanation.ToString() : "";

                if (model.ApplicationEquipment != null)
                {
                    string strEquipment = "";
                    decimal totalCost = 0;
                    foreach (FG_App_ApplicationEquipment equipment in model.ApplicationEquipment)
                    {
                        strEquipment += "<tr><td class='rowData'>" + equipment.Number.ToString() + "</td><td class='rowData'>" + equipment.PriorityCategory.ToString();
                        strEquipment += "</td ><td class='rowData'>" + equipment.EquipmentNeeded.ToString() + "</td><td class='rowData'>" + equipment.Quantity.ToString() + "</td><td class='rowData'>" + equipment.Cost.ToString("C") + "</td></tr>";
                        totalCost += equipment.Cost;
                    }
                    strEquipment += "<tr><td colspan='5' class='rowData' style='text-align:right; padding-right:1em'>Total: " + totalCost.ToString("C") + "</td></tr>";
                    ltrEquipment.Text = strEquipment;
                    ltrProjectBudgetEquipment.Text = strEquipment;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadFundingJustification(FG_App_FundingJustification model)
        {
            try
            {
                //if (model.IsValid == false)
                //{
                //    dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                //}
                if (model.CriticalNeed == 1) { tdCriticalNeed.InnerHtml = "Yes"; }
                if (model.CriticalNeed == 2) { tdCriticalNeed.InnerHtml = "Yes"; }
                spFinancialNeed.InnerHtml = model.FinancialNeed.ToString();
                spProblem.InnerHtml = model.Problem.ToString();
                spBenefit.InnerHtml = model.BenefitToCommunity.ToString();
                spConsequences.InnerHtml = model.Consequences.ToString();

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadProjectBudget(FG_App_ProjectBudget model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    //if (model.InvalidText != null)
                    //{
                    //    dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                    //}
                    //else
                    //{
                    //    dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                    //}
                }
                tdProjectCost.InnerHtml = model.TotalProjectCost.ToString("C");
                tdAmountRequested.InnerHtml = model.AmountRequested.ToString("C");
                tdDepartmentResponsibility.InnerHtml = (model.TotalProjectCost - model.AmountRequested).ToString("C");
                tdStipendAmountRequested.InnerHtml = model.StipendAmount.ToString("C");
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadDocsSigs(DetailedFGAppSigsDocs model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    //if (model.InvalidText != null)
                    //{
                    //    dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                    //}
                    //else
                    //{
                    //    dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                    //}
                }

                FG_App_Signatures fasig = new FG_App_Signatures();
                FG_App_Signatures fcsig = new FG_App_Signatures();

                fasig = model.Signatures.FirstOrDefault(a => a.SignatureRole == "Fiscal Agent");
                fcsig = model.Signatures.FirstOrDefault(a => a.SignatureRole == "Fire Chief");

                if (fasig != null)
                {
                    dvFAName.InnerHtml = fasig.PrintedName;
                    dvFATitle.InnerHtml = "Fiscal Agent";
                    tdFASignature.InnerHtml = fasig.Signature + " / Fiscal Agent";
                    tdFADate.InnerHtml = (fasig.DateSigned != null) ? Convert.ToDateTime(fasig.DateSigned.ToString()).ToShortDateString() : "";
                    spFiscalAgentTitleName.InnerHtml = fasig.PrintedName;
                    spFiscalAgentSignature2.InnerHtml = fasig.Signature;
                    spFiscalAgentSignatureDate.InnerHtml = (fasig.DateSigned != null) ? Convert.ToDateTime(fasig.DateSigned.ToString()).ToShortDateString() : "";

                    spFireChiefName.InnerHtml = fcsig.PrintedName;
                    spFireChiefSignature.InnerHtml = fcsig.Signature;
                    spFireChiefSignatureDate.InnerHtml = (fcsig.DateSigned != null) ? Convert.ToDateTime(fcsig.DateSigned.ToString()).ToShortDateString() : "";
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Application/Instructions");
        }

        protected void btnSavePDF_Click(object sender, EventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                var sbHead = new StringBuilder();
                dvApplication.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                appHead.RenderControl(new HtmlTextWriter(new StringWriter(sbHead)));

                string head = "<style>body {max-width: 800px;margin:25px;}h1 {font-size:1em;font-weight:bold;}.sectionTable {margin-left:23px;clear: both; border: 1px solid #000000; border-right: 1px solid #000000; width: 95%; margin-top: 10px; border-collapse: collapse; text-transform: none; font-family:Arial, Helvetica, sans-serif !important; font-size:15px !important;}.sectionTitle {text-transform: uppercase; padding: 5px; border: 1px solid #000000;}.rowHeader {border: 1px solid #000000; padding: 5px; line-height: 140%; vertical-align: middle; width: 60%;}.rowData {border: 1px solid #000000;text-transform:none; padding: 5px; line-height: 140%; vertical-align: middle;}.rowDataRight {border: 1px solid #000000;text-transform:none; padding: 5px; line-height: 140%; vertical-align: middle; text-align:right;}.rowFundJustification {text-transform:none; padding: 5px; line-height: 140%; vertical-align: top;}.listHeader {border: 1px solid #000000; padding: 6px;background:#ccc; line-height: 150%;}.listHeaderTd {border: 1px solid #000000; padding: 6px;background:#ccc; line-height: 150%;}</style>";
                string body = sb.ToString();
                string htmlContent = "<!DOCTYPE html><html><head>" + head + "</head><body>" + body + "</body></html>";

                htmlContent = htmlContent.Replace("<div style='page-break-before:always'></div>", "<b>[PAGEBREAK]</b>");

                Telerik.Windows.Documents.Flow.FormatProviders.Html.HtmlFormatProvider htmlProvider = new Telerik.Windows.Documents.Flow.FormatProviders.Html.HtmlFormatProvider();
                // Create a document instance from the content. 
                RadFlowDocument document = htmlProvider.Import(htmlContent);

                foreach (var section in document.Sections)
                {
                    //section.PageSize = new System.Windows.Size(1600, 2000);
                    section.PageMargins = new Telerik.Windows.Documents.Primitives.Padding(25, 25, 25, 25);
                }

                InsertPageBreak(document);

                Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider pdfProvider = new Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider();

                // Export the document. The different overloads enables you to export to a byte[] or to a Stream. 
                byte[] pdfBytes = pdfProvider.Export(document);

                string contentType = "pdf";
                string fileName = "NMSFM Fire Grant Application (" + tdDepartmentName.InnerText + "_" + spFiscalYear.InnerText + ").pdf";
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = contentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        private void InsertPageBreak(RadFlowDocument document)
        {
            foreach (var fieldCharacter in document.EnumerateChildrenOfType<Paragraph>().ToArray())
            {
                foreach (var inline in fieldCharacter.Inlines.ToList())
                {
                    if (((inline is InlineBase)) && ((inline as Run) != null) && (((Run)inline).Text == "[PAGEBREAK]"))
                    {
                        var index = fieldCharacter.Inlines.IndexOf(inline);
                        var breakPage = new Break(document);
                        breakPage.BreakType = BreakType.PageBreak;

                        fieldCharacter.Inlines.Insert(index, breakPage);
                        fieldCharacter.Inlines.RemoveAt(index + 1);
                    }
                }
            }
        }
    }
}






