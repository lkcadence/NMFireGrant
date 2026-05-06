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
    public partial class ManageFDIDs : System.Web.UI.Page
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
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //InitTestPriorities();
                //ToDo Load Category (or add new category)
                try
                {
                    HtmlGenericControl helpdiv = new HtmlGenericControl();
                    helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                    FG_App_Help help = await fgService.GetFGHelpByPage("FDIDs (Admin)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }


                    LoadFDIDs();
                }
                catch (Exception ex)
            {
                _ = ex;
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                }
            }
        }

        private async void LoadFDIDs()
        {
            try
            {
                List<FG_FDIDs> fdids = new List<FG_FDIDs>();
                fdids = await fgService.GetFG_FDIDs();
                fdids.OrderBy(a => a.FDID);
                if (fdids != null)
                {
                    rgFDIDs.DataSource = fdids;
                    rgFDIDs.DataBind();
                    ViewState["dtFDIDs"] = fdids;
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        protected void rgFDIDs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<FG_FDIDs> fdids = (List<FG_FDIDs>)ViewState["dtFDIDs"];
            rgFDIDs.DataSource = fdids;
        }

        protected void rgFDIDs_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            List<FG_FDIDs> fdids = (List<FG_FDIDs>)ViewState["dtFDIDs"];
            rgFDIDs.DataSource = fdids;
            rgFDIDs.DataBind();
        }

        protected void rgFDIDs_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["FDID"].Text;
                    LinkButton lb = dataItem.FindControl("btnEdit") as LinkButton;
                    lb.Text = "View/Edit " + name;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgFDIDs_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["FireDepartment"].Text;
                    bool inactive = false;
                    if (dataItem["Inactive"].Text == "True")
                    {
                        inactive = true;
                    }
                    //string number = dataItem["Number"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openFDIDModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfFDID.Value = pId;
                        txtFDID.Text = pId;
                        txtFDID.ReadOnly = true;
                        txtDepartmentName.Text = name;
                        chkFDIDInactive.Checked = inactive;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void btnSaveFDID_ServerClick(object sender, EventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                List<FG_FDIDs> fdidlist = (List<FG_FDIDs>)ViewState["dtFDIDs"];
                if (txtFDID.Text == "")
                {
                    throw new Exception("NERIS ID cannot be blank");
                }
                if (txtDepartmentName.Text == "")
                {
                    throw new Exception("Department Name cannot be blank");
                }
                FG_FDIDs newFDID = new FG_FDIDs();
                if (hfFDID.Value == "")
                {
                    foreach (FG_FDIDs fdid in fdidlist)
                    {
                        if (fdid.FDID == txtFDID.Text)
                        {
                            throw new Exception("NERIS ID exists in the list");
                        }
                    }
                    newFDID.FDID = txtFDID.Text;
                    newFDID.FireDepartment = txtDepartmentName.Text;
                    newFDID.Inactive = chkFDIDInactive.Checked;
                    bool isadded = await fgService.SaveFDIDAsync(newFDID);
                }
                else
                {

                    newFDID.FDID = hfFDID.Value;
                    newFDID.FireDepartment = txtDepartmentName.Text;
                    newFDID.Inactive = chkFDIDInactive.Checked;
                    bool isadded = await fgService.UpdateFDIDAsync(newFDID);
                }

                fdidlist = await fgService.GetFG_FDIDs();
                //priorities = priorities.OrderBy(a => a.PriorityId);
                rgFDIDs.DataSource = fdidlist;
                rgFDIDs.Rebind();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}





