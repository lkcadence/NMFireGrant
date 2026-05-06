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
    public partial class EquipmentNeeds : System.Web.UI.Page
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
            HtmlGenericControl helpdiv = new HtmlGenericControl();
            helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
            FG_App_Help help = await fgService.GetFGHelpByPage("Equipment Needs (Application)");
            if (help != null)
            {
                helpdiv.InnerHtml = help.HelpText;
            }

            Label lblTheTitle;
            lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
            lblTheTitle.Text = "Equipment Needed";
            _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
            _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);
            try
            {
                if (!Page.IsPostBack)
                {
                    if (await LoadPriorities() == true)
                    {
                        await LoadEquipment(0);
                    }
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
            else if (con is RadGrid)
            {
                RadGrid g = (RadGrid)con;
                g.Columns[0].Visible = false;
            }
            btnSave.Visible = false;
            dvShowModal.Visible = false;
            if (dvAdmin.Visible)
            {
                txtComments.ReadOnly = false;
            }
        }

        private void LoadEquipmentNeeds(DetailedFGAppEquipmentNeeds model)
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
                txtWhatPurchased.Text = model.SpecificNeeds;
                if (model.ISOImpacted == 1) { rbISORatingYes.Checked = true; }
                if (model.ISOImpacted == 2) { rbISORatingNo.Checked = true; }
                txtISOExplanation.Text = model.ISOImpactExplanation;
                txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";

                rgEquipment.DataSource = model.ApplicationEquipment;
                ViewState["dtEquipment"] = model.ApplicationEquipment;
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
                        //Response.Redirect("~/Application/EquipmentNeeds", false);
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

        private async Task<bool> LoadPriorities()
        {
            //ToDo Get Priorities From Database Once connected
            try
            {
                ddlPriorityCategory.Items.Clear();
                ddlPriorityCategory.Items.Add("");
                List<FG_Categories> categories = await fgService.GetFGCategories();
                foreach (FG_Categories category in categories)
                {
                    ListItem li = new ListItem();
                    li.Text = category.CategoryName;
                    li.Value = category.CategoryId.ToString();
                    ddlPriorityCategory.Items.Add(li);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> LoadEquipment(int priorityId = 0)
        {
            //ToDo Get Equipment From Database Once connected
            try
            {
                ddlEquipmentNeeded.Items.Clear();
                ddlEquipmentNeeded.Items.Add("");
                List<FG_Priorities> priorities = await fgService.GetFGPriorities(priorityId);
                foreach (FG_Priorities priority in priorities)
                {
                    ListItem li = new ListItem();
                    li.Text = priority.PriorityName;
                    li.Value = priority.PriorityId.ToString();
                    ddlEquipmentNeeded.Items.Add(li);
                }
                return true;
            }
            catch
            {
                throw;
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
                Response.Redirect("~/Application/PPE", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Equipment Needs Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Equipment Needs Data Saved</div>";
                Response.Redirect("~/Application/EquipmentNeeds", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/FundingJustification", false);
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

                if (txtWhatPurchased.Text.Trim() == "")
                {
                    errorMessage += "Equipment to be purchased is Required.<br />";
                    isValid = false;
                }

                int isoChanged = 0;
                if (rbISORatingYes.Checked) { isoChanged = 1; }
                if (rbISORatingNo.Checked) { isoChanged = 2; }
                if (isoChanged == 0)
                {
                    errorMessage += "ISO changed answer is Required.<br />";
                    isValid = false;
                }

                if (isoChanged == 1)
                {
                    if (txtISOExplanation.Text == "")
                    {
                        errorMessage += "ISO changed explanation is Required.<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                List<FG_App_ApplicationEquipment> equipment = (List<FG_App_ApplicationEquipment>)ViewState["dtEquipment"];

                var model = new DetailedFGAppEquipmentNeeds();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();

                model.SpecificNeeds = txtWhatPurchased.Text;
                model.ISOImpacted = isoChanged;
                model.ISOImpactExplanation = txtISOExplanation.Text;
                model.AdminComments = txtComments.Text;

                model.ApplicationEquipment = equipment;

                bool retVal = await fgAppService.SaveEquipmentNeedsAsync(model);

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

        protected void rgEquipment_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected async void rgEquipment_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string priority = "";
                    priority = dataItem["PriorityCategory"].Text;
                    string priorityNum = ddlPriorityCategory.Items.FindByText(priority).Value;
                    string equipment = dataItem["EquipmentNeeded"].Text;
                    string number = dataItem["Number"].Text;
                    string qty = dataItem["Quantity"].Text;
                    string cost = dataItem["Cost"].Text.Replace("$","").Replace(",","");
                    
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEquipmentModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfEquipmentId.Value = pId;
                        ddlPriorityCategory.SelectedValue = priorityNum;
                        bool isloaded = await LoadEquipment(Convert.ToInt32(priorityNum));
                        string equipmentNum = ddlEquipmentNeeded.Items.FindByText(equipment).Value;
                        ddlEquipmentNeeded.SelectedValue = equipmentNum;
                        txtEquipmentNumber.Text = number;
                        txtEquipmentQty.Text = qty;
                        txtEquipmentCost.Text = cost;
                    }
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnDeleteEquipment_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_ApplicationEquipment> applicationEquipment = (List<FG_App_ApplicationEquipment>)ViewState["dtEquipment"];
                for (int i = 0; i < applicationEquipment.Count; i++)
                {
                    if (applicationEquipment[i].EquipmentId.ToString() == hfEquipmentId.Value.ToString())
                    {
                        applicationEquipment.RemoveAt(i);
                        break;
                    }
                }
                ViewState["dtEquipment"] = applicationEquipment;
                rgEquipment.DataSource = applicationEquipment;
                rgEquipment.DataBind();
                txtEquipmentNumber.Text = "";
                ddlPriorityCategory.SelectedIndex = 0;
                ddlEquipmentNeeded.SelectedIndex = 0;
                txtEquipmentQty.Text = "";
                txtEquipmentCost.Text = "";
                hfEquipmentId.Value = "";
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveEquipment_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblEquipmentError.Text = "";
                string errorMessage = "";
                bool isValid = true;
                if (ddlPriorityCategory.SelectedIndex == 0)
                {
                    errorMessage += "Priority Category is Required.<br />";
                    isValid = false;
                }
                if (ddlEquipmentNeeded.SelectedIndex == 0)
                {
                    errorMessage += "Equipment Needed is Required.<br />";
                    isValid = false;
                }
                if (txtEquipmentNumber.Text == "")
                {
                    errorMessage += "Priority Number is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtEquipmentNumber.Text) < 1)
                    {
                        errorMessage += "Priority Number must be greater than 0.<br />";
                        isValid = false;
                    }
                }
                if (txtEquipmentQty.Text == "")
                {
                    errorMessage += "Priority Quantity is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtEquipmentQty.Text) < 1)
                    {
                        errorMessage += "Priority Quantity must be greater than 0.<br />";
                        isValid = false;
                    }
                }
                if (txtEquipmentCost.Text == "")
                {
                    errorMessage += "Priority Cost is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToDecimal(txtEquipmentCost.Text) < 1)
                    {
                        errorMessage += "Priority Cost must be greater than 0.<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_ApplicationEquipment> applicationEquipment = new List<FG_App_ApplicationEquipment>();
                if (ViewState["dtEquipment"] != null)
                {
                    applicationEquipment = (List<FG_App_ApplicationEquipment>)ViewState["dtEquipment"];
                }

                FG_App_ApplicationEquipment equipment = new FG_App_ApplicationEquipment();

                if (hfEquipmentId.Value != "")
                {
                    for (int i = 0; i < applicationEquipment.Count; i++)
                    {
                        if (applicationEquipment[i].EquipmentId.ToString() == hfEquipmentId.Value.ToString())
                        {
                            equipment = applicationEquipment[i];
                            applicationEquipment.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (equipment.EquipmentId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    equipment.EquipmentId = Guid.NewGuid();
                }

                equipment.Number = Convert.ToInt32(txtEquipmentNumber.Text);
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                equipment.ApplicationId = appId;
                equipment.PriorityCategory = ddlPriorityCategory.SelectedItem.Text;
                equipment.EquipmentNeeded = ddlEquipmentNeeded.SelectedItem.Text;
                equipment.Quantity = Convert.ToInt32(txtEquipmentQty.Text);
                equipment.Cost = Convert.ToDecimal(txtEquipmentCost.Text);
                applicationEquipment.Add(equipment);
                ViewState["dtEquipment"] = applicationEquipment;
                rgEquipment.DataSource = applicationEquipment;
                rgEquipment.DataBind();
                txtEquipmentNumber.Text = "";
                ddlPriorityCategory.SelectedIndex = 0;
                ddlEquipmentNeeded.SelectedIndex = 0;
                txtEquipmentQty.Text = "";
                txtEquipmentCost.Text = "";
                hfEquipmentId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + equipment.EquipmentNeeded + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                lblEquipmentError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEquipmentModal();", true);
            }
        }

        protected async void ddlPriorityCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            int priorityCategory = 0;
            if (ddlPriorityCategory.SelectedValue != "")
            {
                priorityCategory = Convert.ToInt32(ddlPriorityCategory.SelectedValue);
            }
            await LoadEquipment(priorityCategory);            
        }
    }
}