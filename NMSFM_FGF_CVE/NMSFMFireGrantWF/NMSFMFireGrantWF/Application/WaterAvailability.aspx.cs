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
    public partial class WaterAvailability : System.Web.UI.Page
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
                FG_App_Help help = await fgService.GetFGHelpByPage("Water Availability (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }
                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Water Availability";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGWaterAvailability waterAvailability = new DetailedFGWaterAvailability();
                        waterAvailability = await fgAppService.GetFGApplicationWaterAvailabilityAsync(appIdGuid);
                        if (waterAvailability != null && waterAvailability.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadWaterAvailability(waterAvailability);
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
                            waterAvailability = await fgAppService.GetFGApplicationPriorYearWaterAvailabilityAsync(addressId, appIdGuid);
                            if (waterAvailability != null && waterAvailability.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                            {
                                PrefillChildRowRemap.RemapWaterSources(
                                    waterAvailability.WaterSources, appIdGuid);
                                LoadWaterAvailability(waterAvailability);
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
            

            //InitTestSources();
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
        }

        private void LoadWaterAvailability(DetailedFGWaterAvailability model)
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
                if (model.ComHydrantSys == 1) { rbCommunityHydrantsYes.Checked = true; }
                if (model.ComHydrantSys == 2) { rbCommunityHydrantsNo.Checked = true; }
                txtTotalWaterCapacity.Text = model.AvailableWaterCapacity.ToString();
                txtWaterCapacityWheels.Text = model.WaterOnWheelsCapacity.ToString();
                txtWaterCapacityStation.Text = model.StationWaterCapacity.ToString();
                if (model.TankAtStation == 1) { rbWaterStorageTankYes.Checked = true; }
                if (model.TankAtStation == 2) { rbWaterStorageTankNo.Checked = true; }

                rgAdditionalWater.DataSource = model.WaterSources;
                ViewState["dtWaterSources"] = model.WaterSources;
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
                        //Response.Redirect("~/Application/WaterAvailability", false);
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

        private void InitTestSources()
        {
            DataTable cats = new DataTable();
            cats.Columns.Add("Number", typeof(string));
            cats.Columns.Add("WaterSource", typeof(string));
            cats.Columns.Add("Capacity", typeof(string));
            cats.Columns.Add("WaterSourceId", typeof(string));

            for (int i = 1; i < 5; i++)
            {
                string source = "Water Source " + i.ToString();
                string capacity = (i * 1000).ToString();
                string sourceId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), source, capacity, sourceId);
            }

            ViewState["dtWaterSources"] = cats;
            rgAdditionalWater.DataSource = cats;
            rgAdditionalWater.DataBind();
        }

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/ResponseHistory", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Water Availability Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Water Availability Saved</div>";
                Response.Redirect("~/Application/WaterAvailability", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/Training", false);
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
                int communityHydrant = 0;
                if (rbCommunityHydrantsYes.Checked) { communityHydrant = 1; }
                if (rbCommunityHydrantsNo.Checked) { communityHydrant = 2; }
                if (communityHydrant == 0)
                {
                    errorMessage += "Community Hydrant System answer is Required.<br />";
                    isValid = false;
                }
                if (txtTotalWaterCapacity.Text == "" || Convert.ToDecimal(txtTotalWaterCapacity.DbValue) < 0 )
                {
                    errorMessage += "Total Water Capacity is Required.<br />";
                    isValid = false;
                }
                if (txtWaterCapacityWheels.Text == "" || Convert.ToDecimal(txtWaterCapacityWheels.DbValue) < 0)
                {
                    errorMessage += "Total Capacity on Wheels is Required.<br />";
                    isValid = false;
                }
                if (txtWaterCapacityStation.Text == "" || Convert.ToDecimal(txtWaterCapacityStation.DbValue) < 0)
                {
                    errorMessage += "Total Capacity at Station is Required.<br />";
                    isValid = false;
                }
                
                int waterStorageTank = 0;
                if (rbWaterStorageTankYes.Checked) { waterStorageTank = 1; }
                if (rbWaterStorageTankNo.Checked) { waterStorageTank = 2; }
                if (waterStorageTank == 0)
                {
                    errorMessage += "Water Storage Tank answer is Required.<br />";
                    isValid = false;
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                List<FG_App_WaterSources> waterSources = (List<FG_App_WaterSources>)ViewState["dtWaterSources"];

                var model = new DetailedFGWaterAvailability();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();
                model.ComHydrantSys = communityHydrant;
                model.AvailableWaterCapacity = Convert.ToInt32(txtTotalWaterCapacity.DbValue);
                model.WaterOnWheelsCapacity = Convert.ToInt32(txtWaterCapacityWheels.DbValue);
                model.StationWaterCapacity = Convert.ToInt32(txtWaterCapacityStation.DbValue);
                model.TankAtStation = waterStorageTank;
                model.WaterSources = waterSources;

                bool retVal = await fgAppService.SaveWaterAvailabilityAsync(model);

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

        protected void rgAdditionalWater_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<FG_App_WaterSources> watersources = (List<FG_App_WaterSources>)ViewState["dtWaterSources"];
            rgAdditionalWater.DataSource = watersources;
        }

        protected void rgAdditionalWater_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            List<FG_App_WaterSources> watersources = (List<FG_App_WaterSources>)ViewState["dtWaterSources"];
            rgAdditionalWater.DataSource = watersources;
            rgAdditionalWater.DataBind();
        }

        protected void rgAdditionalWater_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgAdditionalWater_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["WaterSource"].Text;
                    string number = dataItem["Number"].Text;
                    string capacity = dataItem["Capacity"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openWaterSourceModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfWaterSourceId.Value = pId;
                        txtWaterSource.Text = name;
                        txtCapacity.Text = capacity;
                        //txtWaterSourceNumber.Text = number;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnDeleteWaterSource_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_WaterSources> waterSources = (List<FG_App_WaterSources>)ViewState["dtWaterSources"];
                for (int i = 0; i < waterSources.Count; i++)
                {
                    if (waterSources[i].WaterSourceId.ToString() == hfWaterSourceId.Value.ToString())
                    {
                        waterSources.RemoveAt(i);
                        break;
                    }
                }
                int num = 1;
                foreach (FG_App_WaterSources item in waterSources)
                {
                    item.Number = num;
                    num += 1;
                }
                ViewState["dtWaterSources"] = waterSources;
                rgAdditionalWater.DataSource = waterSources;
                rgAdditionalWater.DataBind();
                txtWaterSource.Text = "";
                //txtWaterSourceNumber.Text = "";
                txtCapacity.Text = "";
                hfWaterSourceId.Value = "";
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveWaterSource_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblWaterSourceError.Text = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtWaterSource.Text == "")
                {
                    errorMessage += "Water Source is Required.<br />";
                    isValid = false;
                }
                if (txtCapacity.Text == "")
                {
                    errorMessage += "Water Source Capacity is Required.<br />";
                    isValid = false;
                }
                else
                {
                    try
                    {
                        txtCapacity.Text = Convert.ToInt32(txtCapacity.Text).ToString();
                        if (Convert.ToInt32(txtCapacity.Text) < 0)
                        {
                            errorMessage += "Water Source Capacity must be greater than 0.<br />";
                            isValid = false;
                        }
                    }
                    catch
                    {
                        errorMessage += "Water Source Capacity must be numeric.<br />";
                        isValid = false;
                    }
                }
                //if (txtWaterSourceNumber.Text == "")
                //{
                //    errorMessage += "Water Source Number is Required.<br />";
                //    isValid = false;
                //}
                //else
                //{
                //    if (Convert.ToInt32(txtWaterSourceNumber.Text) < 1)
                //    {
                //        errorMessage += "Water Source Number must be greater than 0.<br />";
                //        isValid = false;
                //    }
                //}

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_WaterSources> waterSources = new List<FG_App_WaterSources>();
                if (ViewState["dtWaterSources"] != null)
                {
                    waterSources = (List<FG_App_WaterSources>)ViewState["dtWaterSources"];
                }

                FG_App_WaterSources waterSource = new FG_App_WaterSources();

                if (hfWaterSourceId.Value != "")
                {
                    for (int i = 0; i < waterSources.Count; i++)
                    {
                        if (waterSources[i].WaterSourceId.ToString() == hfWaterSourceId.Value.ToString())
                        {
                            waterSource = waterSources[i];
                            waterSources.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (waterSource.WaterSourceId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    waterSource.WaterSourceId = Guid.NewGuid();
                }

                waterSource.Number = waterSources.Count + 1;
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                waterSource.ApplicationId = appId;
                waterSource.WaterSource = txtWaterSource.Text;
                waterSource.Capacity = Convert.ToInt32(txtCapacity.Text);
                waterSources.Add(waterSource);
                ViewState["dtWaterSources"] = waterSources;
                rgAdditionalWater.DataSource = waterSources;
                rgAdditionalWater.DataBind();
                txtWaterSource.Text = "";
                //txtWaterSourceNumber.Text = "";
                txtCapacity.Text = "";
                hfWaterSourceId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + waterSource.WaterSource + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblWaterSourceError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openWaterSourceModal();", true);
            }
        }
    }
}






