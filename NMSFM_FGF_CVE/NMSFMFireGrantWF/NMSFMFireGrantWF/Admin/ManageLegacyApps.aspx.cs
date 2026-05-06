using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NMSFM.Data;
using NMSFM.Services.Logging;
using NMSFM.Services.Images;
using NMSFM.Services.Party;
using System.Threading.Tasks;
using NMSFM.Services.Address;
using NMSFM.Services.Account;
using NMSFM.Services.Menu;
using NMSFM.Services.FYDist;
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using NMSFM.Services.FireGrant;
using NMSFM.ViewModels;
using Telerik.Web.UI;
using System.IO;


namespace NMSFMFireGrantWF.Admin
{
    public partial class ManageLegacyApps : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private ISystemService systemService;
        private IFGService fgService;
        private IFPFService fpfService;

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
                this.fpfService = new FPFService(userContext, logger);
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
                if (Session["IsWebAdmin"] == null || Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                {
                    Response.Redirect("~/Unauthorized");
                }
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
                    HtmlGenericControl helpdiv = new HtmlGenericControl();
                    helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                    FG_App_Help help = await fgService.GetFGHelpByPage("Manage Legacy Apps (Admin)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }
                    await LoadDepartments();
                    LoadApplications();
                }
                catch (Exception ex)
            {
                _ = ex;
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                }
            }
        }

        private async Task<bool> LoadDepartments()
        {
            try
            {
                var addresses = new List<v_Addresses2>();
                addresses = (await fpfService.GetFPFApplicationsAllAsync()).OrderBy(a => a.AddressCode).ToList();
                rcbDepartments.DataSource = addresses;
                rcbDepartments.DataTextField = "AddressCode";
                rcbDepartments.DataValueField = "AddressId";
                rcbDepartments.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        protected void rcbDepartments_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                LoadApplications();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private void LoadApplications()
        {
            try
            {
                List<DetailedFGLegacyApps> legacyApps = new List<DetailedFGLegacyApps>();
                dlApplications.DataSource = legacyApps;
                dlApplications.DataBind();
                string addressId = rcbDepartments.SelectedValue.ToString();
                string strFolder;
                strFolder = Server.MapPath("./LegacyApps/" + addressId);
                System.IO.FileInfo[] files = null;

                if (Directory.Exists(strFolder))
                {
                    System.IO.DirectoryInfo[] subDirs = null;
                    DirectoryInfo dir = new DirectoryInfo(strFolder);
                    subDirs = dir.GetDirectories();
                    foreach (DirectoryInfo yearDir in subDirs)
                    {
                        files = yearDir.GetFiles("*.*");
                        if (files != null)
                        {
                            foreach (FileInfo appFile in files)
                            {
                                DetailedFGLegacyApps application = new DetailedFGLegacyApps();
                                application.AddressId = new Guid(addressId);
                                application.FiscalYear = yearDir.Name;
                                application.FileName = appFile.Name;
                                application.FilePath = "/Admin/LegacyApps/" + addressId + "/" + yearDir.Name + "/" + appFile.Name;
                                legacyApps.Add(application);
                            }
                        }
                    }
                    dlApplications.DataSource = legacyApps;
                    dlApplications.DataBind();
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        protected void btnSaveHelp_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblModalError.Text = "";
                string errorMessage = "";
                bool isValid = true;

                if (ruAppUpload.UploadedFiles.Count != 0)
                {
                    bool isCorrectFormat = false;
                    if (ruAppUpload.UploadedFiles[0].ContentType.ToString() == "application/pdf" | ruAppUpload.UploadedFiles[0].ContentType.ToString().Contains("word"))
                    {
                        isCorrectFormat = true;
                    }
                    if (isCorrectFormat == false)
                    {
                        throw new Exception("Images must be in the format of .txt, .doc, .docx, .xls, .xlsx, .pdf, .jpg, .png or .bmp<br />");
                    }
                }
                else
                {
                    errorMessage += "Appliction is required<br />";
                    isValid = false;
                }
                if (txtFiscalYear.Text == "")
                {
                    errorMessage += "Fiscal Year is required<br />";
                    isValid = false;
                }
                else
                {
                    try
                    {
                        if (Convert.ToInt32(txtFiscalYear.Text) < 2000 || Convert.ToInt32(txtFiscalYear.Text) > 2023)
                        {
                            errorMessage += "Fiscal Year cannot be less than 2000 or greater than 2023<br />";
                            isValid = false;
                        }
                    }
                    catch
                    {
                        errorMessage += "Fiscal Year must be numeric<br />";
                        isValid = false;
                    }
                }
                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }

                string strFileName;
                string strFilePath = "";
                string strFolder;
                bool uploaded = true;
                strFolder = Server.MapPath("./LegacyApps/" + rcbDepartments.SelectedValue.ToString() + "/" + txtFiscalYear.Text + "/");
                // Retrieve the name of the file that is posted.
                strFileName = ruAppUpload.UploadedFiles[0].FileName;
                strFileName = Path.GetFileName(strFileName);
                if (ruAppUpload.UploadedFiles[0] != null)
                {
                    // Create the folder if it does not exist.
                    if (!Directory.Exists(strFolder))
                    {
                        Directory.CreateDirectory(strFolder);
                    }
                    // Save the uploaded file to the server.
                    strFilePath = strFolder + strFileName;
                    if (System.IO.File.Exists(strFilePath))
                    {
                        throw new Exception("File already exists.");
                    }
                    else
                    {
                        System.IO.DirectoryInfo di = new DirectoryInfo(strFolder);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        ruAppUpload.UploadedFiles[0].SaveAs(strFilePath);
                        //lblReqDoc.Text = strFileName + " has been successfully uploaded.";
                    }
                }
                else
                {
                    throw new Exception("Please select a document to upload.");
                }
                if (uploaded)
                {
                    txtFiscalYear.Text = "";
                    btnSaveHelp.InnerText = "Save Appllication";
                    LoadApplications();
                    dvError.InnerHtml = "<div class='alert alert-success'>The Application has been uploaded</div>";
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                lblModalError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openHelpModal();", true);
            }
        }

        protected void lnkViewEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // Legacy placeholder action intentionally empty.
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void dlApplications_ItemCommand(object source, DataListCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    txtFiscalYear.Text = e.CommandArgument.ToString();
                    btnSaveHelp.InnerText = "Change Application File";
                    lblModalError.Text = "<div class='alert alert-info'>Note: Uploading a new document will overwrite the old document.</div>";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openHelpModal();", true);
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnDeleteApp_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strFolder;
                bool uploaded = true;
                strFolder = Server.MapPath("./LegacyApps/" + rcbDepartments.SelectedValue.ToString() + "/" + txtFiscalYear.Text + "/");
                // Retrieve the name of the file that is posted.
                System.IO.DirectoryInfo di = new DirectoryInfo(strFolder);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                if (uploaded)
                {
                    txtFiscalYear.Text = "";
                    btnSaveHelp.InnerText = "Save Appllication";
                    LoadApplications();
                    dvError.InnerHtml = "<div class='alert alert-success'>The Application has been deleted</div>";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeHelpModal();", true);
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





