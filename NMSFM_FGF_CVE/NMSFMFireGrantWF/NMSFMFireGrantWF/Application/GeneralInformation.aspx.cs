using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
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

namespace NMSFMFireGrantWF.Application
{
    public partial class GeneralInformation : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
// private IAccountService accountService; // legacy field, currently unused
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;
        private IUDFService udfService;

        RadMenu _rmStep1;
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
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("General Information (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "General Information";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);
                _rmStep1.CausesValidation = false;

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        FG_App_GeneralInfo genInfo = new FG_App_GeneralInfo();
                        genInfo = await fgAppService.GetFGApplicationGeneralInfoAsync(appIdGuid);
                        if (genInfo != null && genInfo.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            await LoadDepartment();
                            LoadGeneralInfoData(genInfo);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                        else
                        {
                            await LoadDepartment();
                        }
                    }
                    else
                    {
                        await LoadDepartment();
                    }
                    if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                    {
                        DisableControls(this);
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            } 
        }

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                DisableControls(c);
            }
            if (con is TextBox)
            {
                TextBox t = (TextBox)con;
                t.ReadOnly = true;
            }
            if (con is RadTextBox)
            {
                RadTextBox t = (RadTextBox)con;
                t.ReadOnly = true;
            }
            if (con is RadNumericTextBox)
            {
                RadNumericTextBox t = (RadNumericTextBox)con;
                t.ReadOnly = true;
            }
            if (con is RadMaskedTextBox)
            {
                RadMaskedTextBox t = (RadMaskedTextBox)con;
                t.ReadOnly = true;
            }
            else if (con is CheckBox)
            {
                CheckBox t = (CheckBox)con;
                t.Enabled = false;
            }
            else if (con is RadioButton)
            {
                RadioButton t = (RadioButton)con;
                t.Enabled = false;
            }
            else if (con is DropDownList)
            {
                DropDownList t = (DropDownList)con;
                t.Enabled = false;
            }
            btnSave.Visible = false;
        }

        private async Task<bool> LoadDepartment()
        {
            try
            {
                var department = new v_AddressParties();
                Guid deptId = new Guid(Session["Department"].ToString());
                department = await fgService.GetFGDepartmentByIdAsync(deptId);
                
                if (department != null)
                {
                    txtDepartment.Text = department.AddressCode;
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
                    txtCounty.Text = County;
                    txtAddress.Text = addressDesc.Trim();
                    txtCity.Text = department.City;
                    txtState.Text = department.State;
                    txtZip.Text = department.Zip;
                    LoadDepartmentUDFs(deptId);

                    int fYear = Convert.ToInt16(Session["FiscalYear"].ToString()) - 1;
                    Int16 sYear = Convert.ToInt16(fYear);
                    nm_FGApplication lastYearApp = new nm_FGApplication();
                    Guid addId = new Guid(department.AddressId.ToString());
                    lastYearApp = fgAppService.GetFGApplication(addId, sYear);
                    if (lastYearApp != null && lastYearApp.ApplicationId.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        FG_App_GeneralInfo genInfo = new FG_App_GeneralInfo();
                        genInfo = await fgAppService.GetFGApplicationGeneralInfoAsync(new Guid(lastYearApp.ApplicationId.ToString()));
                        if (genInfo != null && genInfo.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            txtFDID.Text = genInfo.NERISID.ToString();
                            if (genInfo.FireChiefName != "") { txtFireCheif.Text = genInfo.FireChiefName; }
                            if (genInfo.Phone != "") { txtPhone.Text = genInfo.Phone; }
                            if (genInfo.EmailAddress != "") { txtEmail.Text = genInfo.EmailAddress; }
                            if (genInfo.IndividualDept == 1)
                            {
                                rbIndividual.Checked = true;
                            }
                            else if (genInfo.IndividualDept == 2)
                            {
                                rbCountyWide.Checked = true;
                                if (genInfo.CountyDeptsCompliant == 1)
                                {
                                    rbCountyDeptsYes.Checked = true;
                                }
                                else if (genInfo.CountyDeptsCompliant == 2)
                                {
                                    rbCountyDeptsNo.Checked = true;
                                }
                            }
                            if (genInfo.IsCityMuni == 1)
                            {
                                rbCityMuni.Checked = true;
                            }
                            else if (genInfo.IsCityMuni == 2)
                            {
                                rbCounty.Checked = true;
                            }
                            if (genInfo.DeptType == 1) { rbCareer.Checked = true; }
                            else if (genInfo.DeptType == 2) { rbVolunteer.Checked = true; }
                            else if (genInfo.DeptType == 3) { rbCombined.Checked = true; }
                            chkAdmin.Checked = genInfo.IsAdminDept;
                            if (genInfo.NumberOfFirefighters != null) { txtFirefighters.Text = genInfo.NumberOfFirefighters.ToString(); }
                            if (genInfo.FFI_Firefighters != null) { txtlblFF1.Text = genInfo.FFI_Firefighters.ToString(); }
                            if (genInfo.FFII_Firefighters != null) { txtFF2.Text = genInfo.FFII_Firefighters.ToString(); }
                            if (genInfo.FireDeptMember == 1)
                            {
                                rbFDMemberYes.Checked = true;
                            }
                            else if (genInfo.FireDeptMember == 2)
                            {
                                rbFDMemberNo.Checked = true;
                            }
                            dvError.InnerHtml = "<div class='alert alert-info'>Some data has been loaded from previous application. Please verify all data is current.</div>";
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        private void LoadGeneralInfoData(FG_App_GeneralInfo model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                }
                if (model.IndividualDept == 1)
                {
                    rbIndividual.Checked = true;
                }
                else if (model.IndividualDept == 2)
                {
                    rbCountyWide.Checked = true;
                    if (model.CountyDeptsCompliant == 1)
                    {
                        rbCountyDeptsYes.Checked = true;
                    }
                    else if (model.CountyDeptsCompliant == 2)
                    {
                        rbCountyDeptsNo.Checked = true;
                    }
                }
                if (model.NERISID != "") { txtFDID.Text = model.NERISID; }
                if (model.DepartmentName != "") { txtDepartment.Text = model.DepartmentName; }
                if (model.FireChiefName != "") { txtFireCheif.Text = model.FireChiefName; }
                if (model.Phone != "") { txtPhone.Text = model.Phone; }
                if (model.EmailAddress != "") { txtEmail.Text = model.EmailAddress; }
                if (model.ISORating != null) { txtISO.Text = model.ISORating.ToString(); }
                if (model.IsCityMuni == 1) {
                    rbCityMuni.Checked = true; 
                }
                else if(model.IsCityMuni == 2)
                {
                    rbCounty.Checked = true; 
                }
                if (model.DeptType == 1) { rbCareer.Checked = true; }
                else if (model.DeptType == 2) { rbVolunteer.Checked = true; }
                else if (model.DeptType == 3) { rbCombined.Checked = true; }
                chkAdmin.Checked = model.IsAdminDept;
                if (model.MainStations != null) { txtMainStations.Text = model.MainStations.ToString(); }
                if (model.SubStations != null) { txtSubStations.Text = model.SubStations.ToString(); }
                if (model.AdminBldgs != null) { txtAdmin.Text = model.AdminBldgs.ToString(); }
                if (model.Community == 1) { chkUrban.Checked = true; }
                else if (model.Community == 2) { chkRural.Checked = true; }
                else if (model.Community == 3) { chkSubUrban.Checked = true; }
                if (model.NumberOfFirefighters != null) { txtFirefighters.Text = model.NumberOfFirefighters.ToString(); }
                if (model.FFI_Firefighters != null) { txtlblFF1.Text = model.FFI_Firefighters.ToString(); }
                if (model.FFII_Firefighters != null) { txtFF2.Text = model.FFII_Firefighters.ToString(); }
                if (model.MailingAddress != "") { txtAddress.Text = model.MailingAddress; }
                if (model.MailingCity != "") { txtCity.Text = model.MailingCity; }
                if (model.MailingState != "") { txtState.Text = model.MailingState; }
                if (model.MailingZip != "") { txtZip.Text = model.MailingZip; }
                if (model.PersonCompleteApp != "") { txtApplicationName.Text = model.PersonCompleteApp; }
                if (model.FireDeptMember == 1)
                {
                    rbFDMemberYes.Checked = true;
                }
                else if (model.FireDeptMember == 2)
                {
                    rbFDMemberNo.Checked = true;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
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
                txtISO.Text = iSO;  //Header Info Agency UDFs

                string MC = await udfService.GetUDFValueAsync(mainGuid, departmentId);
                if (MC == "")
                {
                    MC = "0";
                }
                txtMainStations.Text = MC;  //Header Info Agency UDFs

                string AC = await udfService.GetUDFValueAsync(adminGuid, departmentId);
                if (AC == "")
                {
                    AC = "0";
                }
                txtAdmin.Text = AC;  //Header Info Agency UDFs

                string SC = await udfService.GetUDFValueAsync(subGuid, departmentId);
                if (SC == "")
                {
                    SC = "0";
                }
                txtSubStations.Text = SC;  //Header Info Agency UDFs
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }

        }

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/Instructions", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>General Information Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>General Information Data Saved</div>";
                Response.Redirect("~/Application/GeneralInformation", false);
            }
        }

        protected async void rmStep1_Click(object sender, Telerik.Web.UI.RadMenuEventArgs e)
        {
            if (await SaveForm() == true)
            {
                switch (_rmStep1.SelectedItem.Text)
                {
                    case "Instructions":
                        Response.Redirect("~/Application/Instructions", false);
                        break;
                    case "General Information":
                        //Response.Redirect("~/Application/GeneralInformation", false);
                        break;
                    case "Budget Information":
                        Response.Redirect("~/Application/BudgetInfo", false);
                        break;
                    case "Community Information":
                        Response.Redirect("~/Application/CommunityInfo", false);
                        break;
                    case "Response History":
                        Response.Redirect("~/Application/ResponseHistory", false);
                        break;
                    case "Water Availability":
                        Response.Redirect("~/Application/WaterAvailability", false);
                        break;
                    case "Training":
                        Response.Redirect("~/Application/Training", false);
                        break;
                    case "Apparatus":
                        Response.Redirect("~/Application/Apparatus", false);
                        break;
                    case "Communication Equipment":
                        Response.Redirect("~/Application/CommunicationEquipment", false);
                        break;
                    case "Hazards/Threats":
                        Response.Redirect("~/Application/HazardsThreats", false);
                        break;
                    case "PPE":
                        Response.Redirect("~/Application/PPE", false);
                        break;
                    case "Equipment Needs":
                        Response.Redirect("~/Application/EquipmentNeeds", false);
                        break;
                    case "Grant Funding Justification":
                        Response.Redirect("~/Application/FundingJustification", false);
                        break;
                    case "Project Budget Sheet":
                        Response.Redirect("~/Application/ProjectBudgetSheet", false);
                        break;
                    case "Signatures and Supporting Docs":
                        Response.Redirect("~/Application/SignaturesDocs", false);
                        break;
                    default:
                        Response.Redirect("~/Application/Instructions", false);
                        break;
                }
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/BudgetInfo", false);
            }
        }

        private async Task<bool> SaveForm()
        {
            try
            {
                if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                {
                    return true;
                }
                //ToDo Check Validation
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                int appType = 0;
                if (rbIndividual.Checked) { appType = 1; }
                if (rbCountyWide.Checked) { appType = 2; }
                if (appType == 0)
                {
                    errorMessage += "Grant Source is Required. <br />";
                    isValid = false;
                }

                if (txtFDID.Text.Trim() == "")
                {
                    errorMessage += "Fire Department ID (NERIS ID) is Required. <br />";
                    isValid = false;
                }

                if (txtFireCheif.Text.Trim() == "")
                {
                    errorMessage += "Fire Chief Name is Required. <br />";
                    isValid = false;
                }

                if (txtPhone.Text.Trim() == "")
                {
                    errorMessage += "Phone Number is Required. <br />";
                    isValid = false;
                }

                if (txtEmail.Text.Trim() == "")
                {
                    errorMessage += "Email Address is Required. <br />";
                    isValid = false;
                }

                int cityMuni = 0;
                if (rbCityMuni.Checked) { cityMuni = 1; }
                if (rbCounty.Checked) { cityMuni = 2; }
                if (cityMuni == 0)
                {
                    errorMessage += "Department city/muni or county is Required. <br />";
                    isValid = false;
                }

                int deptType = 0;
                if (rbCareer.Checked) { deptType = 1; }
                if (rbVolunteer.Checked) { deptType = 2; }
                if (rbCombined.Checked) { deptType = 3; }
                if (deptType == 0)
                {
                    errorMessage += "Department Type is Required. <br />";
                    isValid = false;
                }

                int countyDeptsCompliant = 0;
                
                if (rbCountyWide.Checked)
                {
                    if (rbCountyDeptsYes.Checked) { countyDeptsCompliant = 1; }
                    if (rbCountyDeptsNo.Checked) { countyDeptsCompliant = 2; }
                    if (countyDeptsCompliant == 0)
                    {
                        errorMessage += "You must specify if all of the County departments NERIS and Pump Test complient. <br />";
                        isValid = false;
                    }
                }

                int community = 0;
                if (chkUrban.Checked) { community = 1; }
                if (chkRural.Checked) { community = 2; }
                if (chkSubUrban.Checked) { community = 3; }
                if (community == 0)
                {
                    errorMessage += "Community Type is Required. <br />";
                    isValid = false;
                }

                if (txtApplicationName.Text.Trim() == "")
                {
                    errorMessage += "Name of person completing the application is Required. <br />";
                    isValid = false;
                }

                int fdMem = 0;
                if (rbFDMemberYes.Checked) { fdMem = 1; }
                if (rbFDMemberNo.Checked) { fdMem = 2; }
                if (fdMem == 0)
                {
                    errorMessage += "Fire deparment membership response is Required. <br />";
                    isValid = false;
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new FG_App_GeneralInfo();

                model.ApplicationId = new Guid(hfApplicationId.Value);
                model.IndividualDept = appType;
                model.NERISID = txtFDID.Text;
                model.DepartmentName = txtDepartment.Text;
                model.FireChiefName = txtFireCheif.Text;
                model.Phone = txtPhone.Text;
                model.EmailAddress = txtEmail.Text;
                model.ISORating = (txtISO.Text != "") ? Convert.ToInt32(txtISO.Text) : 0;
                model.County = txtCounty.Text;
                model.IsCityMuni = cityMuni;   
                model.DeptType = deptType;
                model.IsAdminDept = chkAdmin.Checked;
                model.CountyDeptsCompliant = countyDeptsCompliant;
                model.MainStations = (txtMainStations.Text != "") ? Convert.ToInt32(txtMainStations.Text) : 0;
                model.SubStations = (txtSubStations.Text != "") ? Convert.ToInt32(txtSubStations.Text) : 0;
                model.AdminBldgs = (txtAdmin.Text != "") ? Convert.ToInt32(txtAdmin.Text) : 0;
                model.Community = community;
                model.NumberOfFirefighters = (txtFirefighters.Text != "") ? Convert.ToInt32(txtFirefighters.Text) : 0;
                model.FFI_Firefighters = (txtlblFF1.Text != "") ? Convert.ToInt32(txtlblFF1.Text) : 0;
                model.FFII_Firefighters = (txtFF2.Text != "") ? Convert.ToInt32(txtFF2.Text) : 0;
                model.MailingAddress = txtAddress.Text;
                model.MailingCity = txtCity.Text;
                model.MailingState = txtState.Text;
                model.MailingZip = txtZip.Text;
                model.PersonCompleteApp = txtApplicationName.Text;
                model.FireDeptMember = fdMem;
                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();

                bool retVal = await fgAppService.SaveGeneralInformationAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
            
        }

    }
}






