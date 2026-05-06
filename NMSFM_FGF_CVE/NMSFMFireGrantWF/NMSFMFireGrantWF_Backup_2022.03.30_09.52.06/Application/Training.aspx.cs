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
    public partial class Training : System.Web.UI.Page
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
                FG_App_Help help = await fgService.GetFGHelpByPage("Training (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Training";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGAppTraining training = new DetailedFGAppTraining();
                        training = await fgAppService.GetFGApplicationTrainingAsync(appIdGuid);
                        if (training != null && training.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadTraining(training);
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
            if (dvAdmin.Visible)
            {
                txtComments.ReadOnly = false;
                txtTrainingPoints.ReadOnly = false;
            }
        }

        private void LoadTraining(DetailedFGAppTraining model)
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
                txtTrainingHours.Text = model.YearlyTrainingHours.ToString();

                txtTrainingPoints.Text = model.TrainingPoints.ToString();
                txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";

                rgTrainings.DataSource = model.TrainingOpportunities;
                ViewState["dtTrainings"] = model.TrainingOpportunities;
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
                        //Response.Redirect("~/Application/Training", false);
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
            cats.Columns.Add("TrainingDetail", typeof(string));
            cats.Columns.Add("SupportingDocument", typeof(string));
            cats.Columns.Add("TrainingId", typeof(string));

            for (int i = 1; i < 5; i++)
            {
                string source = "Training " + i.ToString();
                string supportingdoc = "Supporting Doc " + i.ToString();
                string trainingId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), source, supportingdoc, trainingId);
            }

            ViewState["dtTrainings"] = cats;
            rgTrainings.DataSource = cats;
            rgTrainings.DataBind();
        }

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/WaterAvailability", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Training Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Training Data Saved</div>";
                Response.Redirect("~/Application/Training", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/Apparatus", false);
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

                List<FG_App_TrainingOpportunities> trainings = (List<FG_App_TrainingOpportunities>)ViewState["dtTrainings"];

                if (txtTrainingHours.Text == "" || Convert.ToInt32(txtTrainingHours.DbValue) < 1)
                {
                    errorMessage += "Training Hours Required.<br />";
                    isValid = false;
                }

                if (trainings != null && trainings.Count < 1)
                {
                    errorMessage += "Training opportunities and documenation Required.<br />";
                    isValid = false;
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                } 

                var model = new DetailedFGAppTraining();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();
                model.YearlyTrainingHours = Convert.ToInt32(txtTrainingHours.DbValue);
                model.TrainingOpportunities = trainings;

                model.TrainingPoints = Convert.ToInt32(txtTrainingPoints.DbValue);
                model.AdminComments = txtComments.Text;

                bool retVal = await fgAppService.SaveTrainingAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void rgTrainings_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string pId = e.CommandArgument.ToString();
                    Guid trId = new Guid(pId);
                    List<FG_App_TrainingOpportunities> trainings = (List<FG_App_TrainingOpportunities>)ViewState["dtTrainings"];
                    FG_App_TrainingOpportunities training = trainings.FirstOrDefault(a => a.TrainingId == trId);
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "View" && training != null)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openTrainingModal();", true);
                        
                        hfTrainingId.Value = pId;
                        txtTrainingDetails.Text = training.TrainingDetail;
                        txtTrainingNumber.Text = training.Number.ToString();
                        if (training.TrainingDocument != null)
                        {
                            lnkTrainingDoc.Text = training.TrainingDocumentName;
                            dvTrainingDocLink.Visible = true;
                        }
                        else
                        {
                            dvTrainingDocLink.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgTrainings_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
        }

        protected void rgTrainings_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            List<FG_App_TrainingOpportunities> trainings = (List<FG_App_TrainingOpportunities>)ViewState["dtTrainings"];
            rgTrainings.DataSource = trainings;
            rgTrainings.DataBind();
        }

        protected void rgTrainings_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<FG_App_TrainingOpportunities> trainings = (List<FG_App_TrainingOpportunities>)ViewState["dtTrainings"];
            rgTrainings.DataSource = trainings;
        }

        protected void btnDeleteTraining_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_TrainingOpportunities> trainings = (List<FG_App_TrainingOpportunities>)ViewState["dtTrainings"];
                for (int i = 0; i < trainings.Count; i++)
                {
                    if (trainings[i].TrainingId.ToString() == hfTrainingId.Value.ToString())
                    {
                        trainings.RemoveAt(i);
                        break;
                    }
                }
                ViewState["dtTrainings"] = trainings;
                rgTrainings.DataSource = trainings;
                rgTrainings.DataBind();
                txtTrainingDetails.Text = "";
                //txtTrainingHours.Text = "";
                txtTrainingNumber.Text = "";
                lnkTrainingDoc.Text = "";
                dvTrainingDocLink.Visible = false;
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveTraining_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblTrainingError.Text = "";
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtTrainingDetails.Text == "")
                {
                    errorMessage += "Training Details are Required.<br />";
                    isValid = false;
                }
                if (txtTrainingNumber.Text == "")
                {
                    errorMessage += "Training Number is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtTrainingNumber.Text) < 1)
                    {
                        errorMessage += "Training Number must be greater than 0.<br />";
                        isValid = false;
                    }
                }
                if (ruTrainingDoc.UploadedFiles.Count != 0)
                {
                    bool isCorrectFormat = false;
                    if (ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "text/plain" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "image/jpeg" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "image/png" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "image/bmp" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "application/pdf" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString().Contains("word") | ruTrainingDoc.UploadedFiles[0].ContentType.ToString().Contains("spreadsheet"))                   {
                        isCorrectFormat = true;
                    }
                    if (isCorrectFormat == false)
                    {
                        throw new Exception("Images must be in the format of .txt, .doc, .docx, .xls, .xlsx, .pdf, .jpg, .png or .bmp<br />");
                    }
                }
                else
                {
                    errorMessage += "Training Documentation is required<br />";
                    isValid = false;
                }
                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_TrainingOpportunities> trainingOpportunities = new List<FG_App_TrainingOpportunities>();
                if (ViewState["dtTrainings"] != null)
                {
                    trainingOpportunities = (List<FG_App_TrainingOpportunities>)ViewState["dtTrainings"];
                }

                FG_App_TrainingOpportunities trainingOpportunity = new FG_App_TrainingOpportunities();

                if (hfTrainingId.Value != "")
                {
                    for (int i = 0; i < trainingOpportunities.Count; i++)
                    {
                        if (trainingOpportunities[i].TrainingId.ToString() == hfTrainingId.Value.ToString())
                        {
                            trainingOpportunity = trainingOpportunities[i];
                            trainingOpportunities.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (trainingOpportunity.TrainingId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    trainingOpportunity.TrainingId = Guid.NewGuid();
                }

                UploadedFile file;
                byte[] fileData = null;
                string theextension = null;
                string filename = null;
                if (ruTrainingDoc.UploadedFiles.Count != 0)
                {
                    file = ruTrainingDoc.UploadedFiles[0];
                    fileData = new byte[file.InputStream.Length];
                    file.InputStream.Read(fileData, 0, Convert.ToInt32(file.InputStream.Length));
                    theextension = file.GetExtension();
                    filename = file.FileName;
                }
                else
                {
                    fileData = null;
                    theextension = string.Empty;
                    filename = null;
                }

                trainingOpportunity.Number = Convert.ToInt32(txtTrainingNumber.Text);
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                trainingOpportunity.ApplicationId = appId;
                trainingOpportunity.TrainingDetail = txtTrainingDetails.Text;
                trainingOpportunity.TrainingDocumentName = filename;
                trainingOpportunity.TrainingDocument = fileData;
                trainingOpportunities.Add(trainingOpportunity);
                ViewState["dtTrainings"] = trainingOpportunities;
                rgTrainings.DataSource = trainingOpportunities;
                rgTrainings.DataBind();
                txtTrainingDetails.Text = "";
                txtTrainingNumber.Text = "";
                ruTrainingDoc.UploadedFiles.Clear();
                hfTrainingId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + trainingOpportunity.TrainingDetail + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                lblTrainingError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openTrainingModal();", true);

            }
        }

        protected void lnkTrainingDoc_Click(object sender, EventArgs e)
        {
            try
            {
                string pId = hfTrainingId.Value;
                Guid trId = new Guid(pId);
                List<FG_App_TrainingOpportunities> trainings = (List<FG_App_TrainingOpportunities>)ViewState["dtTrainings"];
                FG_App_TrainingOpportunities training = trainings.FirstOrDefault(a => a.TrainingId == trId);
                if (training != null)
                {
                    byte[] bytes;
                    string fileName, contentType;
                    bytes = training.TrainingDocument;
                    contentType = training.TrainingDocumentType;
                    fileName = training.TrainingDocumentName;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = contentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }
    }
}