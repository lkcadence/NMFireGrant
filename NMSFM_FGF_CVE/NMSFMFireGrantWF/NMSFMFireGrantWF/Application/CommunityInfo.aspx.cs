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
    public partial class CommunityInfo : System.Web.UI.Page
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
                FG_App_Help help = await fgService.GetFGHelpByPage("Community Info (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Community Information";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGAppCommunityInfo communityInfo = new DetailedFGAppCommunityInfo();
                        communityInfo = await fgAppService.GetFGApplicationCommunityInfoAsync(appIdGuid);
                        //Added 6/1/2022
                        //txtDistrictNumber.Text = hfAidDistrictCount.Value.ToString();
                        //End Add
                        if (communityInfo != null && communityInfo.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadCommunityInfoData(communityInfo);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                        //added 12/26/23 (vwd) load preexisting apparatus
                        else
                        {
                            Guid addressId = new Guid(Session["Department"].ToString());
                            communityInfo = await fgAppService.GetFGApplicationPriorYearCommunityInfoAsync(addressId, appIdGuid);
                            if (communityInfo != null && communityInfo.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                            {
                                LoadCommunityInfoData(communityInfo, true);
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
            //InitTestDistricts();
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

        private void LoadCommunityInfoData(DetailedFGAppCommunityInfo model, bool communityListOnly = false)
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
                if (communityListOnly == false)
                {
                    txtCommunityProtected.Text = model.CommunityName;
                    txtHomesProtected.Text = model.NumberOfHomes.ToString();
                    txtCommercial.Text = model.NumberOfComm.ToString();
                    txtPopulation.Text = model.ResidentPopulation.ToString();
                    if (model.AidAgreements == 1)
                    {
                        rbAidAgreementsYes.Checked = true;
                    }
                    else if (model.AidAgreements == 2)
                    {
                        rbAidAgreementsNo.Checked = true;
                    }
                }
                
                rgAidDistricts.DataSource = model.AidDistricts;
                ViewState["dtAidDistricts"] = model.AidDistricts;
                //Added 6/1/2022
                //hfAidDistrictCount.Value = model.AidDistricts.Count.ToString();
                //txtDistrictNumber.Text = (Convert.ToInt32(hfAidDistrictCount.Value) + 1).ToString();
                //End Add
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
                        //Response.Redirect("~/Application/CommunityInfo", false);
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

        private void InitTestDistricts()
        {
            DataTable cats = new DataTable();
            cats.Columns.Add("Number", typeof(string));
            cats.Columns.Add("AidDistrict", typeof(string));
            cats.Columns.Add("AidDistrictId", typeof(string));

            for (int i = 1; i < 5; i++)
            {
                string district = "Test District " + i.ToString();

                string districtId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), district, districtId);
            }

            ViewState["dtAidDistricts"] = cats;
            rgAidDistricts.DataSource = cats;
            rgAidDistricts.DataBind();
        }

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/BudgetInfo", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Community Information Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Community Information Data Saved</div>";
                Response.Redirect("~/Application/CommunityInfo", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/ResponseHistory", false);
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
                List<FG_App_AidDistricts> districts = (List<FG_App_AidDistricts>)ViewState["dtAidDistricts"];
                //ToDo Check Validation
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtCommunityProtected.Text == "")
                {
                    errorMessage += "Community Proteted is Required.<br />";
                    isValid = false;
                }
                if (txtHomesProtected.Text == "" || Convert.ToInt32(txtHomesProtected.Text) < 1)
                {
                    errorMessage += "Number of homes protected is Required.<br />";
                    isValid = false;
                }
                if (txtCommercial.Text == "" || Convert.ToInt32(txtCommercial.Text) < 1)
                {
                    errorMessage += "Number of commercial properties protected is Required.<br />";
                    isValid = false;
                }
                if (txtPopulation.Text == "" || Convert.ToInt32(txtPopulation.Text) < 1)
                {
                    errorMessage += "Permanent resident population is Required.<br />";
                    isValid = false;
                }

                int aidAgreements = 0;
                if (rbAidAgreementsYes.Checked) { aidAgreements = 1; }
                if (rbAidAgreementsNo.Checked) { aidAgreements = 2; }
                if (aidAgreements == 0)
                {
                    errorMessage += "Aid Agreements answer is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (aidAgreements == 1)
                    {
                        if (districts == null || districts.Count < 1)
                        {
                            errorMessage += "Aid fire districts required if Aid Agreements answer is 'Yes'<br />";
                            isValid = false;
                        }
                    }
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                
                if (ViewState["dtAidDistricts"] != null)
                {
                    districts = (List<FG_App_AidDistricts>)ViewState["dtAidDistricts"];
                    rgAidDistricts.DataSource = districts;
                }

                var model = new DetailedFGAppCommunityInfo();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();
                model.CommunityName = txtCommunityProtected.Text;
                model.NumberOfHomes = Convert.ToInt32(txtHomesProtected.DbValue);
                model.NumberOfComm = Convert.ToInt32(txtCommercial.DbValue);
                model.ResidentPopulation = Convert.ToInt32(txtPopulation.DbValue);
                model.AidAgreements = aidAgreements;
                model.AidDistricts = districts;

                bool retVal = await fgAppService.SaveCommunityInformationAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void rgAidDistricts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (ViewState["dtAidDistricts"] != null)
            {
                List<FG_App_AidDistricts> districts = (List<FG_App_AidDistricts>)ViewState["dtAidDistricts"];
                rgAidDistricts.DataSource = districts;
            }
        }

        protected void rgAidDistricts_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (ViewState["dtAidDistricts"] != null)
            {
                List<FG_App_AidDistricts> districts = (List<FG_App_AidDistricts>)ViewState["dtAidDistricts"];
                rgAidDistricts.DataSource = districts;
                rgAidDistricts.DataBind();
            }
        }

        protected void rgAidDistricts_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
        }

        protected void rgAidDistricts_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["AidDistrict"].Text;
                    string number = dataItem["Number"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDistrictModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfDistrictId.Value = pId;
                        txtDistrict.Text = name;
                        //txtDistrictNumber.Text = number;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnDeleteDistrict_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_AidDistricts> districts = (List<FG_App_AidDistricts>)ViewState["dtAidDistricts"];
                for (int i = 0; i < districts.Count; i++)
                {
                    if (districts[i].AidDistrictId.ToString() == hfDistrictId.Value.ToString())
                    {
                        districts.RemoveAt(i);
                        break;
                    }
                }
                int distNumber = 1;
                foreach (FG_App_AidDistricts dist in districts)
                {
                    dist.Number = distNumber;
                    distNumber += 1;
                }
                ViewState["dtAidDistricts"] = districts;
                rgAidDistricts.DataSource = districts;
                rgAidDistricts.DataBind();
                txtDistrict.Text = "";
                //Added 6/1/2022
                //hfDistrictId.Value = "";
                //hfAidDistrictCount.Value = districts.Count().ToString();
                //txtDistrictNumber.Text = "";
                //End Add
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveDistrict_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblDistrictError.Text = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtDistrict.Text == "")
                {
                    errorMessage += "Fire Aid District is Required.<br />";
                    isValid = false;
                }
                //if (txtDistrictNumber.Text == "")
                //{
                //    errorMessage += "Water Source Number is Required.<br />";
                //    isValid = false;
                //}
                //else
                //{
                //    if (Convert.ToInt32(txtDistrictNumber.Text) < 1)
                //    {
                //        errorMessage += "District Number must be greater than 0.<br />";
                //        isValid = false;
                //    }
                //}

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_AidDistricts> districts = new List<FG_App_AidDistricts>();
                if (ViewState["dtAidDistricts"] != null)
                {
                    districts = (List<FG_App_AidDistricts>)ViewState["dtAidDistricts"];
                }

                FG_App_AidDistricts dist = new FG_App_AidDistricts();

                if (hfDistrictId.Value != "")
                {
                    for (int i = 0; i < districts.Count; i++)
                    {
                        if (districts[i].AidDistrictId.ToString() == hfDistrictId.Value.ToString())
                        {
                            dist = districts[i];
                            districts.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (dist.AidDistrictId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    dist.AidDistrictId = Guid.NewGuid();
                }
                //Added 6/1/2022
                //hfAidDistrictCount.Value = districts.Count().ToString();
                //End Add

                dist.Number = districts.Count + 1;
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                dist.ApplicationId = appId;
                dist.AidDistrict = txtDistrict.Text;
                districts.Add(dist);
                ViewState["dtAidDistricts"] = districts;
                rgAidDistricts.DataSource = districts;
                rgAidDistricts.DataBind();
                txtDistrict.Text = "";
                //Added 6/1/2022
                //txtDistrictNumber.Text = (districts.Count + 1).ToString();
                //End Add
                hfDistrictId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + dist.AidDistrict + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblDistrictError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDistrictModal();", true);
            }
        }
    }
}






