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
    public partial class ProjectBudgetSheet : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
        private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;

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
                else
                {
                    if (Session["Role"].ToString() == "Internal")
                    {
                        dvAdmin.Visible = true;
                    }
                    else if (Session["Role"].ToString() == "External" || Session["Role"].ToString() == "Signator")
                    {
                        dvAdmin.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Project Budget Sheet (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Project Budget Sheet";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);
                //InitTestSources();

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGAppEquipmentNeeds equipmentNeeds = new DetailedFGAppEquipmentNeeds();
                        equipmentNeeds = await fgAppService.GetFGApplicationEquipmentNeedsAsync(appIdGuid);
                        if (equipmentNeeds != null && equipmentNeeds.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadEquipmentNeeds(equipmentNeeds);
                        }
                        else
                        {
                            dvError.InnerHtml = "<div class='alert alert-danger'>Please fill out Equipment Needs before submitting the project budget sheet.</div>";
                        }
                        FG_App_ProjectBudget projectBudget = new FG_App_ProjectBudget();
                        projectBudget = await fgAppService.GetFGApplicationProjectBudgetAsync(appIdGuid);
                        if (projectBudget != null && projectBudget.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadProjectBudget(projectBudget);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                    }
                    if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                    {
                        DisableControls(this);
                    }
                }
            }
            catch (Exception ex)
            {
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
            btnSave.Visible = false;
            if (dvAdmin.Visible)
            {
                txtComments.ReadOnly = false;
            }
        }

        private void LoadEquipmentNeeds(DetailedFGAppEquipmentNeeds model)
        {
            try
            {
                txtWhatPurchased.Text = model.SpecificNeeds;
                if (model.ISOImpacted == 1) { rbISORatingYes.Checked = true; }
                if (model.ISOImpacted == 2) { rbISORatingNo.Checked = true; }
                txtISOExplanation.Text = model.ISOImpactExplanation;
                txtComments.Text = model.AdminComments;

                rgEquipment.DataSource = model.ApplicationEquipment;
                ViewState["dtEquipment"] = model.ApplicationEquipment;

                decimal projectBudgetAmount = 0;
                foreach (FG_App_ApplicationEquipment equip in model.ApplicationEquipment)
                {
                    projectBudgetAmount += equip.Cost;
                }
                hfEquipmentCost.Value = projectBudgetAmount.ToString();
                //txtTotalAmount.Text = projectBudgetAmount.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadProjectBudget(FG_App_ProjectBudget model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    if (model.InvalidText != null)
                    {
                        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                    }
                    else
                    {
                        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                    }
                }
                txtTotalAmount.DbValue = model.TotalProjectCost;
                txtAmountRequested.DbValue = model.AmountRequested;
                //txtTotalDeptResp.DbValue = model.DepartmentResponsibility;
                txtStipendAmountRequested.DbValue = model.StipendAmount;
                txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";
            }
            catch (Exception ex)
            {
                throw ex;
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
                        Response.Redirect("~/Application/GeneralInformation", false);
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
                        //Response.Redirect("~/Application/ProjectBudgetSheet", false);
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

        private void InitTestSources()
        {
            DataTable cats = new DataTable();
            cats.Columns.Add("Number", typeof(string));
            cats.Columns.Add("PriorityCategory", typeof(string));
            cats.Columns.Add("EquipmentNeeded", typeof(string));
            cats.Columns.Add("Quantity", typeof(string));
            cats.Columns.Add("Cost", typeof(string));
            cats.Columns.Add("EquipmentId", typeof(string));

            for (int i = 1; i < 5; i++)
            {
                string priority = "Priority " + i.ToString();
                string equipment = "Equipment " + i.ToString();
                string qty = (i * 2).ToString();
                string cost = (i * 1000).ToString();
                string comId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), priority, equipment, qty, cost, comId);
            }

            ViewState["dtEquipment"] = cats;
            rgEquipment.DataSource = cats;
            rgEquipment.DataBind();
        }

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/FundingJustification", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Project Budget Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Project Budget Data Saved</div>";
                Response.Redirect("~/Application/ProjectBudgetSheet", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/SignaturesDocs", false);
            }
        }

        private async Task<bool> SaveForm()
        {
            try
            {
                if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true && dvAdmin.Visible == false)
                {
                    return true;
                }
                //ToDo Check Validation
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                decimal totalProjectCost = 0;
                try
                {
                    totalProjectCost = Convert.ToDecimal(txtTotalAmount.DbValue);
                    if (totalProjectCost < 1)
                    {
                        errorMessage += "Total Project Amount is Required.<br />";
                        isValid = false;
                    }
                    else
                    {
                        if (totalProjectCost < Convert.ToDecimal(hfEquipmentCost.Value))
                        {
                            errorMessage += "Total Project Amount must be greater than total eqipment cost.<br />";
                            isValid = false;
                        }
                    }
                }
                catch
                {
                    errorMessage += "Total Project Amount is Required.<br />";
                    isValid = false;
                }
                if (txtAmountRequested.Text == "" || Convert.ToDecimal(txtAmountRequested.DbValue) < 1)
                {
                    errorMessage += "Amount Requested is Required.<br />";
                    isValid = false;
                }
                if (txtStipendAmountRequested.Text == "")
                {
                    errorMessage += "Stipend Amount is Required (enter zero if not requested).<br />";
                    isValid = false;
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new FG_App_ProjectBudget();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();
                model.TotalProjectCost = Convert.ToDecimal(txtTotalAmount.DbValue);
                model.AmountRequested = Convert.ToDecimal(txtAmountRequested.DbValue);
                model.DepartmentResponsibility = Convert.ToDecimal(txtTotalDeptResp.DbValue);
                model.StipendAmount = Convert.ToDecimal(txtStipendAmountRequested.DbValue);
                model.AdminComments = txtComments.Text;

                bool retVal = await fgAppService.SaveProjectBudgetAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void rgEquipment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<FG_App_ApplicationEquipment> equipment = (List<FG_App_ApplicationEquipment>)ViewState["dtEquipment"];
            rgEquipment.DataSource = equipment;
        }

        protected void rgEquipment_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            List<FG_App_ApplicationEquipment> equipment = (List<FG_App_ApplicationEquipment>)ViewState["dtEquipment"];
            rgEquipment.DataSource = equipment;
            rgEquipment.DataBind();
        }

    }
}