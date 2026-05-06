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
using System.Configuration;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Spreadsheet;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace NMSFMFireGrantWF.Application
{
    public partial class Training : System.Web.UI.Page
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
                        if (Session["IsWebAdmin"] != null && Convert.ToBoolean(Session["IsWebAdmin"]) == true)
                        {
                            txtTrainingPoints.ReadOnly = true;
                        }
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
                            await LoadTraining(training);
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
            if (dvAdmin.Visible)
            {
                if (Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                {
                    txtTrainingPoints.ReadOnly = false;
                }
                txtComments.ReadOnly = false;
                btnSave.Visible = true;
            }
        }

        private async Task<bool> LoadTraining(DetailedFGAppTraining model)
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

                
                txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";

                rgTrainings.DataSource = model.TrainingOpportunities;
                ViewState["dtTrainings"] = model.TrainingOpportunities;

                short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                if (fiscalYear < 2024)
                {
                    txtTrainingPoints.Text = model.TrainingPoints.ToString();
                }
                else
                {
                    if (Convert.ToBoolean(Session["IsWebAdmin"]) == true)
                    {
                        List<DetailedFGAppScores> appScores = new List<DetailedFGAppScores>();
                        appScores = await fgAppService.GetDetailedFGAppScoresAdminAsync(model.ApplicationId);
                        if (appScores.Count > 0)
                        {
                            int trainingScore = 0;
                            foreach (DetailedFGAppScores score in appScores)
                            {
                                trainingScore += score.TrainingPoints;
                            }
                            lblTrainingPoints.Text = "Average Training Score: ";
                            txtTrainingPoints.Text = trainingScore.ToString();
                        }
                    }
                    else
                    {
                        DetailedFGAppScores appScores = new DetailedFGAppScores();
                        Guid webUserId = new Guid(Session["WebUserId"].ToString());
                        appScores = await fgAppService.GetDetailedFGAppScoresCounselorAsync(model.ApplicationId, webUserId);
                        if (appScores != null)
                        {
                            txtTrainingPoints.Text = appScores.TrainingPoints.ToString();
                        }
                    }
                    
                }
                return true;
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

                List<FG_App_TrainingOpportunityView> trainings = (List<FG_App_TrainingOpportunityView>)ViewState["dtTrainings"];

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

                //Added for 2024 (vwd)
                if (retVal == true && dvAdmin.Visible)
                {
                    if (Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                    {
                        DetailedFGAppScores trainingScores = new DetailedFGAppScores();
                        trainingScores.ApplicationId = model.ApplicationId;
                        trainingScores.WebUserId = new Guid(Session["WebUserId"].ToString());
                        trainingScores.UserName = Session["WebUser"].ToString();
                        trainingScores.TrainingPoints = Convert.ToInt32(txtTrainingPoints.DbValue);
                        await fgAppService.SaveCounselorScores(trainingScores);
                    }
                    
                }

                return retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
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
                    List<FG_App_TrainingOpportunityView> trainingsView = (List<FG_App_TrainingOpportunityView>)ViewState["dtTrainings"];
                    FG_App_TrainingOpportunityView trainingView = trainingsView.FirstOrDefault(a => a.TrainingId == trId);
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "ViewEdit" && trainingView != null)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openTrainingModal();", true);
                        
                        hfTrainingId.Value = pId;
                        txtTrainingDetails.Text = trainingView.TrainingDetail;
                        //txtTrainingNumber.Text = training.Number.ToString();
                        if (trainingView.TrainingDocumentName != null)
                        {
                            lnkTrainingDoc.Text = trainingView.TrainingDocumentName;
                            dvTrainingDocLink.Visible = true;
                        }
                        else
                        {
                            dvTrainingDocLink.Visible = false;
                        }
                    }
                    else if (e.CommandName == "View")
                    {
                        ViewDocument(pId);
                    }
                    else if (e.CommandName == "Download")
                    {
                        DownloadDocument(pId);
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private async void ViewDocument(string docId)
        {
            try
            {
                //string pId = hfTrainingId.Value;
                Guid trId = new Guid(docId);
                FG_App_TrainingOpportunities doc = fgAppService.GetFGApplicationTrainingOpportunity(trId);
                if (doc != null)
                {
                    byte[] bytes;
                    string fileName, fileOnlyName, extention;
                    bytes = doc.TrainingDocument;
                    //contentType = "";
                    fileName = doc.TrainingDocumentName;
                    extention = GetExtension(fileName);
                    fileOnlyName = ExtractFileNameWithoutExtention(fileName);
                    byte[] renderedBytes = bytes;
                    MemoryStream stream = new MemoryStream(bytes);
                    // RadFlow Documents
                    if (Regex.IsMatch(extention, ".docx|.rtf|.html|.txt|.pdf"))
                    {
                        if (Regex.IsMatch(extention, ".docx|.rtf|.html|.txt"))
                        {
                            IFormatProvider<RadFlowDocument> provider = null;
                            RadFlowDocument document = null;
                            switch (extention)
                            {
                                case ".docx": provider = new DocxFormatProvider(); break;
                                case ".rtf": provider = new RtfFormatProvider(); break;
                                case ".html": provider = new HtmlFormatProvider(); break;
                                case ".txt": provider = new TxtFormatProvider(); break;
                                default: provider = null; break;
                            }
                            document = provider.Import(stream);
                            Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider pdfProvider = new
                                Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider();
                            using (MemoryStream ms = new MemoryStream())
                            {
                                pdfProvider.Export(document, ms);
                                renderedBytes = ms.ToArray();
                            }
                        }
                        pdfView.PdfjsProcessingSettings.FileSettings.Data = Convert.ToBase64String(renderedBytes);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDocModal();", true);
                    }
                    else if (Regex.IsMatch(extention, ".jpg|.gif|.png|.jpeg"))
                    {
                        imgDocument.Src = "data:image/png;base64," + Convert.ToBase64String(bytes);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openImgModal();", true);
                    }

                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private static string GetExtension(string path)
        {
            var ret = "";
            for (; ; )
            {
                var ext = Path.GetExtension(path);
                if (String.IsNullOrEmpty(ext))
                    break;
                path = path.Substring(0, path.Length - ext.Length);
                ret = ext + ret;
            }
            return ret;
        }

        public static string ExtractFileNameWithoutExtention(string path)
        {
            string fileName = Path.GetFileName(path);
            int lastIndex = fileName.LastIndexOf(".");
            if (lastIndex != -1)
            {
                fileName = fileName.Substring(0, lastIndex);
            }
            return fileName;
        }

        private async void DownloadDocument(string docId)
        {
            try
            {
                //string pId = hfTrainingId.Value;
                Guid trId = new Guid(docId);
                FG_App_TrainingOpportunities training = fgAppService.GetFGApplicationTrainingOpportunity(trId);
                if (training != null)
                {
                    byte[] bytes;
                    string fileName, extension, contentType;
                    bytes = training.TrainingDocument;
                    extension = GetExtension(training.TrainingDocumentName);
                    contentType = "";
                    switch (extension.ToLower())
                    {
                        case ".doc":
                            contentType = "application/vnd.ms-word";
                            break;
                        case ".docx":
                            contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        case ".xls":
                            contentType = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                        case ".jpg":
                            contentType = "image/jpg";
                            break;
                        case ".png":
                            contentType = "image/png";
                            break;
                        case ".gif":
                            contentType = "image/gif";
                            break;
                        case ".pdf":
                            contentType = "application/pdf";
                            break;
                    }
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
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgTrainings_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
        }

        protected void rgTrainings_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            //List<FG_App_TrainingOpportunityView> trainings = (List<FG_App_TrainingOpportunityView>)ViewState["dtTrainings"];
            //rgTrainings.DataSource = trainings;
            //rgTrainings.DataBind();
        }

        protected void rgTrainings_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<FG_App_TrainingOpportunityView> trainings = (List<FG_App_TrainingOpportunityView>)ViewState["dtTrainings"];
            rgTrainings.DataSource = trainings;
        }

        protected async void btnDeleteTraining_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_TrainingOpportunityView> trainingsVeiw = (List<FG_App_TrainingOpportunityView>)ViewState["dtTrainings"];
                for (int i = 0; i < trainingsVeiw.Count; i++)
                {
                    if (trainingsVeiw[i].TrainingId.ToString() == hfTrainingId.Value.ToString())
                    {
                        if (await fgAppService.DeleteTrainingOpportunityAsync(trainingsVeiw[i].TrainingId) == true)
                        {
                            trainingsVeiw.RemoveAt(i);
                            dvError.InnerHtml = "<div class='alert alert-success'>Training has been deleted</div>";
                            break;
                        }
                    }
                }
                int num = 1;
                foreach (FG_App_TrainingOpportunityView item in trainingsVeiw)
                {
                    item.Number = num;
                    num += 1;
                }
                ViewState["dtTrainings"] = trainingsVeiw;
                rgTrainings.DataSource = trainingsVeiw;
                rgTrainings.DataBind();
                txtTrainingDetails.Text = "";
                //txtTrainingHours.Text = "";
                //txtTrainingNumber.Text = "";
                lnkTrainingDoc.Text = "";
                dvTrainingDocLink.Visible = false;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void btnSaveTraining_ServerClick(object sender, EventArgs e)
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
                //if (txtTrainingNumber.Text == "")
                //{
                //    errorMessage += "Training Number is Required.<br />";
                //    isValid = false;
                //}
                //else
                //{
                //    if (Convert.ToInt32(txtTrainingNumber.Text) < 1)
                //    {
                //        errorMessage += "Training Number must be greater than 0.<br />";
                //        isValid = false;
                //    }
                //}
                if (ruTrainingDoc.UploadedFiles.Count != 0)
                {
                    bool isCorrectFormat = false;
                    if (ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "text/plain" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "image/jpeg" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "image/png" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "image/bmp" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString() == "application/pdf" | ruTrainingDoc.UploadedFiles[0].ContentType.ToString().Contains("word") | ruTrainingDoc.UploadedFiles[0].ContentType.ToString().Contains("spreadsheet") | ruTrainingDoc.UploadedFiles[0].ContentType.ToString().Contains("excel")) {
                        isCorrectFormat = true;
                    }
                    if (isCorrectFormat == false)
                    {
                        throw new Exception("Documents must be in the format of .txt, .docx, .pdf, .jpg, .png or .bmp<br />");
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


                List<FG_App_TrainingOpportunityView> trainingOpportunityViewList = new List<FG_App_TrainingOpportunityView>();
                if (ViewState["dtTrainings"] != null)
                {
                    trainingOpportunityViewList = (List<FG_App_TrainingOpportunityView>)ViewState["dtTrainings"];
                }

                FG_App_TrainingOpportunityView trainingOpportunityView = new FG_App_TrainingOpportunityView();
                FG_App_TrainingOpportunities trainingOpportunity = new FG_App_TrainingOpportunities();

                if (hfTrainingId.Value != "")
                {
                    for (int i = 0; i < trainingOpportunityViewList.Count; i++)
                    {
                        if (trainingOpportunityViewList[i].TrainingId.ToString() == hfTrainingId.Value.ToString())
                        {
                            trainingOpportunityView = trainingOpportunityViewList[i];
                            trainingOpportunityViewList.RemoveAt(i);
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

                trainingOpportunity.Number = trainingOpportunityViewList.Count + 1;
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                trainingOpportunity.ApplicationId = appId;
                trainingOpportunity.TrainingDetail = txtTrainingDetails.Text;
                trainingOpportunity.TrainingDocumentName = filename;
                trainingOpportunity.TrainingDocument = fileData;

                if (await fgAppService.SaveTrainingOpportunity(trainingOpportunity) == true)
                {
                    trainingOpportunityView.Number = trainingOpportunityViewList.Count + 1;
                    trainingOpportunityView.ApplicationId = appId;
                    trainingOpportunityView.TrainingDetail = txtTrainingDetails.Text;
                    trainingOpportunityView.TrainingDocumentName = filename;

                    trainingOpportunityViewList.Add(trainingOpportunityView);
                    ViewState["dtTrainings"] = trainingOpportunityViewList;
                    rgTrainings.DataSource = trainingOpportunityViewList;

                    rgTrainings.DataBind();
                    txtTrainingDetails.Text = "";
                    //txtTrainingNumber.Text = "";
                    ruTrainingDoc.UploadedFiles.Clear();
                    hfTrainingId.Value = "";
                    dvError.InnerHtml = "<div class='alert alert-success'>" + trainingOpportunity.TrainingDetail + " has been added.</div>";
                    dvError.Focus();
                }
                else
                {
                    dvError.InnerHtml = "<div class='alert alert-error'>" + trainingOpportunity.TrainingDetail + " was not added.</div>";
                    dvError.Focus();
                }
               
                
            }
            catch (Exception ex)
            {
                _ = ex;
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
                FG_App_TrainingOpportunities training = fgAppService.GetFGApplicationTrainingOpportunity(trId);
                if (training != null)
                {
                    byte[] bytes;
                    string fileName, extension, contentType;
                    bytes = training.TrainingDocument;
                    extension = GetExtension(training.TrainingDocumentName);
                    contentType = "";
                    switch (extension.ToLower())
                    {
                        case ".doc":
                            contentType = "application/vnd.ms-word";
                            break;
                        case ".docx":
                            contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        case ".xls":
                            contentType = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                        case ".jpg":
                            contentType = "image/jpg";
                            break;
                        case ".png":
                            contentType = "image/png";
                            break;
                        case ".gif":
                            contentType = "image/gif";
                            break;
                        case ".pdf":
                            contentType = "application/pdf";
                            break;
                    }
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
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }
    }
}






