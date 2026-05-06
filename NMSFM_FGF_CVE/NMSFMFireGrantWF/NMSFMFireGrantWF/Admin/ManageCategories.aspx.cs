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
using NMSFM.Services.Address;
using NMSFM.Services.Account;
using NMSFM.Services.Menu;
using NMSFM.Services.FYDist;
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using NMSFM.Services.FireGrant;
using Telerik.Web.UI;

namespace NMSFMFireGrantWF.Admin
{
    public partial class ManageCategories : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private ISystemService systemService;
        private IFGService fgService;

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
                //InitTestCategories();
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Manage Categories (Admin)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }
                LoadCategories();
            }
        }

        private async void LoadCategories()
        {
            try
            {
                List<FG_Categories> cats = await fgService.GetFGCategories();
                ViewState["dtCategories"] = cats;
                rgCategories.DataSource = cats;
                rgCategories.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private void InitTestCategories()
        {
            DataTable cats = new DataTable();
            cats.Columns.Add("Number", typeof(string));
            cats.Columns.Add("CategoryName", typeof(string));
            cats.Columns.Add("CategoryId", typeof(string));

            for (int i = 1; i < 10; i++)
            {
                string catname = "";
                switch (i)
                {
                    case 1:
                        catname = "Apparatus";
                        break;
                    case 2:
                        catname = "Communication";
                        break;
                    case 3:
                        catname = "Dedicated Fire Supression Water Supply";
                        break;
                    case 4:
                        catname = "Facility Improvement";
                        break;
                    case 5:
                        catname = "General Firefighting Equipment";
                        break;
                    case 6:
                        catname = "Others";
                        break;
                    case 7:
                        catname = "PPE";
                        break;
                    case 8:
                        catname = "Rescue";
                        break;
                    case 9:
                        catname = "SCBA and/or Cylinders";
                        break;
                    case 10:
                        catname = "Training";
                        break;
                }
                string catId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), catname, catId);
            }

            ViewState["dtCategories"] = cats;
            rgCategories.DataSource = cats;
            rgCategories.DataBind();
        }

        protected void rgCategories_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<FG_App_Help> cats = (List<FG_App_Help>)ViewState["dtCategories"];
            rgCategories.DataSource = cats;
        }

        protected void rgCategories_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            List<FG_App_Help> cats = (List<FG_App_Help>)ViewState["dtCategories"];
            rgCategories.DataSource = cats;
            rgCategories.DataBind();
        }

        protected void rgCategories_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["CategoryName"].Text;
                    LinkButton delete = (LinkButton)dataItem["Edit"].Controls[0];
                    delete.Text = "View/Edit " + name;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgCategories_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var ditem = e.Item as GridDataItem;
                    string itemValue = ditem["CategoryId"].Text.ToString();
                    if ((e.CommandName == "View"))
                    {
                        Response.RedirectToRoute("Category", new { CategoryId = itemValue });
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Admin/Category");
        }
    }
}





