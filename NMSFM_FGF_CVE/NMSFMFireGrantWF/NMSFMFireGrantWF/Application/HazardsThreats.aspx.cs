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
    public partial class HazardsThreats : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
// private IAccountService accountService; // legacy field, currently unused
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
                _ = ex;

            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Hazards Threats (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Hazards/Threats";
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
                        DetailedFGAppHazardsThreats hazardsThreats = new DetailedFGAppHazardsThreats();
                        hazardsThreats = await fgAppService.GetFGApplicationHazardsThreatsAsync(appIdGuid);
                        if (hazardsThreats != null && hazardsThreats.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadHazadsThreats(hazardsThreats);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                        //added 12/26/23 (vwd) load preexisting info
                        else
                        {
                            Guid addressId = new Guid(Session["Department"].ToString());
                            hazardsThreats = await fgAppService.GetFGApplicationPriorYearHazardsThreatsAsync(addressId, appIdGuid);
                            if (hazardsThreats != null && hazardsThreats.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                            {
                                PrefillChildRowRemap.RemapHazardThreatEvents(
                                    hazardsThreats.HazardsThreats, appIdGuid);
                                LoadHazadsThreats(hazardsThreats, true);
                                if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                                {
                                    dvError.InnerHtml = Session["SaveMessage"].ToString();
                                    Session["SaveMessage"] = "";
                                }
                                else
                                {
                                    dvError.InnerHtml = "Information Loaded from Previous Application";
                                }
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
                btnSave.Visible = true;
            }
        }


        private void LoadHazadsThreats(DetailedFGAppHazardsThreats model, bool listOnly = false)
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
                if (listOnly == false)
                {
                    txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";
                }
                

                rgHazards.DataSource = model.HazardsThreats;
                ViewState["dtHazardsThreats"] = model.HazardsThreats;
            }
            catch (Exception ex)
            {
                _ = ex;
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
                        //Response.Redirect("~/Application/HazardsThreats", false);
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

        private void InitTestSources()
        {
            DataTable cats = new DataTable();
            cats.Columns.Add("Number", typeof(string));
            cats.Columns.Add("HazardType", typeof(string));
            cats.Columns.Add("HazardDetail", typeof(string));
            cats.Columns.Add("HazardId", typeof(string));

            for (int i = 1; i < 5; i++)
            {
                string type = "Hazards/Threats " + i.ToString();
                string detail = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
                string hazardId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), type, detail, hazardId);
            }

            ViewState["dtHazards"] = cats;
            rgHazards.DataSource = cats;
            rgHazards.DataBind();
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
                //    dvError.InnerHtml = "<div class='alert alert-success'>Hazard/Threat Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Hazard/Threat Data Saved</div>";
                Response.Redirect("~/Application/HazardsThreats", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/EquipmentNeeds", false);
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

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                List<FG_App_HazardThreatEvents> hazards = (List<FG_App_HazardThreatEvents>)ViewState["dtHazardsThreats"];

                var model = new DetailedFGAppHazardsThreats();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();

                model.AdminComments = txtComments.Text;
                model.HazardsThreats = hazards;

                bool retVal = await fgAppService.SaveHazardThreatsAsync(model);

                // return retVal;
                return isValid && retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void btnDeleteHazard_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_HazardThreatEvents> hazardThreatEvents = (List<FG_App_HazardThreatEvents>)ViewState["dtHazardsThreats"];
                for (int i = 0; i < hazardThreatEvents.Count; i++)
                {
                    if (hazardThreatEvents[i].HazardId.ToString() == hfHazardnId.Value.ToString())
                    {
                        hazardThreatEvents.RemoveAt(i);
                        break;
                    }
                }
                int num = 1;
                foreach (FG_App_HazardThreatEvents item in hazardThreatEvents)
                {
                    item.Number = num;
                    num += 1;
                }
                hazardThreatEvents = hazardThreatEvents.OrderBy(a => a.Number).ToList();
                ViewState["dtHazardsThreats"] = hazardThreatEvents;
                rgHazards.DataSource = hazardThreatEvents;
                rgHazards.DataBind();
                //txtHazardNumber.Text = "";
                txtHazardType.Text = "";
                txtHazardDetail.Text = "";
                hfHazardnId.Value = "";
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveHazard_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblCommunicationError.Text = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtHazardType.Text == "")
                {
                    errorMessage += "Hazard Type is Required.<br />";
                    isValid = false;
                }
                if (txtHazardDetail.Text == "")
                {
                    errorMessage += "Hazard Detail is Required.<br />";
                    isValid = false;
                }

                //if (txtHazardNumber.Text == "")
                //{
                //    errorMessage += "Hazard Number is Required.<br />";
                //    isValid = false;
                //}
                //else
                //{
                //    if (Convert.ToInt32(txtHazardNumber.Text) < 1)
                //    {
                //        errorMessage += "Hazard Number must be greater than 0.<br />";
                //        isValid = false;
                //    }
                //}

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_HazardThreatEvents> hazardThreatEvents = new List<FG_App_HazardThreatEvents>();
                if (ViewState["dtHazardsThreats"] != null)
                {
                    hazardThreatEvents = (List<FG_App_HazardThreatEvents>)ViewState["dtHazardsThreats"];
                }

                FG_App_HazardThreatEvents hazardThreat = new FG_App_HazardThreatEvents();

                if (hfHazardnId.Value != "")
                {
                    for (int i = 0; i < hazardThreatEvents.Count; i++)
                    {
                        if (hazardThreatEvents[i].HazardId.ToString() == hfHazardnId.Value.ToString())
                        {
                            hazardThreat = hazardThreatEvents[i];
                            hazardThreatEvents.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (hazardThreat.HazardId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    hazardThreat.HazardId = Guid.NewGuid();
                }

                hazardThreat.Number = hazardThreatEvents.Count + 1;
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                hazardThreat.ApplicationId = appId;
                hazardThreat.HazardType = txtHazardType.Text;
                hazardThreat.HazardDetail = txtHazardDetail.Text;
                hazardThreatEvents.Add(hazardThreat);

                hazardThreatEvents = hazardThreatEvents.OrderBy(a => a.Number).ToList();
                ViewState["dtHazardsThreats"] = hazardThreatEvents;
                rgHazards.DataSource = hazardThreatEvents;
                rgHazards.DataBind();
                txtHazardType.Text = "";
                //txtHazardNumber.Text = "";
                txtHazardDetail.Text = "";
                hfHazardnId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + hazardThreat.HazardType + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblCommunicationError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openHazardModal();", true);
            }
        }

        protected void rgHazards_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<FG_App_HazardThreatEvents> hazards = (List<FG_App_HazardThreatEvents>)ViewState["dtHazardsThreats"];
            rgHazards.DataSource = hazards;
        }

        protected void rgHazards_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            List<FG_App_HazardThreatEvents> hazards = (List<FG_App_HazardThreatEvents>)ViewState["dtHazardsThreats"];
            rgHazards.DataSource = hazards;
            rgHazards.DataBind();
        }

        protected void rgHazards_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgHazards_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string type = "";
                    type = dataItem["HazardType"].Text;
                    string number = dataItem["Number"].Text;
                    string details = dataItem["HazardDetail"].Text;
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openHazardModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfHazardnId.Value = pId;
                        txtHazardType.Text = type;
                        txtHazardDetail.Text = details;
                        //txtHazardNumber.Text = number;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }
    }
}






