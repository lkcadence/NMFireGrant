using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NMSFM.Data;
using System.Threading.Tasks;
using NMSFM.Services.Logging;
using NMSFM.Services.Images;
using NMSFM.Services.Party;
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
    public partial class ViewLegacyApps : System.Web.UI.Page
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
    }
}





