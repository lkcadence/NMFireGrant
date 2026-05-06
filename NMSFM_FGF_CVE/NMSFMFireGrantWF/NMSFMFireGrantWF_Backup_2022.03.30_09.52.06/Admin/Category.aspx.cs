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
    public partial class EditCategory : System.Web.UI.Page
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
                    FG_App_Help help = await fgService.GetFGHelpByPage("Category (Admin)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }
                    

                    if (RouteData.Values["CategoryId"] != null)
                    {
                        short catId = Convert.ToInt16(RouteData.Values["CategoryId"]);
                        LoadCategory(catId);
                        hfCategoryId.Value = catId.ToString();
                        spHeader.InnerText = "Edit Category & Priorities";                    }
                    else
                    {
                        spHeader.InnerText = "Add Category";
                        //btnDelete.Visible = false;
                        hfCategoryId.Value = "0";
                        LoadPriorities(0);
                    }
                }
                catch (Exception ex)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                }
            }
        }

        private async void LoadCategory(short catId)
        {
            try
            {
                FG_Categories cat = new FG_Categories();
                cat = await fgService.GetFGCategory(catId);
                if (cat != null)
                {
                    txtCategoryName.Text = cat.CategoryName;
                    chkInactive.Checked = cat.Inactive;
                    hfCategoryId.Value = catId.ToString();
                    LoadPriorities(catId);
                }
                else
                {
                    throw new Exception("Category Not Found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void LoadPriorities(short catId)
        {
            try
            {
                List<FG_Priorities> priorities = new List<FG_Priorities>();
                priorities = await fgService.GetFGPriorities(catId);
                if (priorities != null)
                {
                    rgPriorities.DataSource = priorities;
                    rgPriorities.DataBind();
                    ViewState["Priorities"] = priorities;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitTestPriorities()
        {
            DataTable cats = new DataTable();
            cats.Columns.Add("Number", typeof(string));
            cats.Columns.Add("Priority", typeof(string));
            cats.Columns.Add("PriorityId", typeof(string));

            for (int i = 1; i < 10; i++)
            {
                string priority = "Test Priority " + i.ToString();
                
                string priorityId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), priority, priorityId);
            }

            ViewState["dtPriorities"] = cats;
            rgPriorities.DataSource = cats;
            rgPriorities.DataBind();
        }

        protected void rgPriorities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable priorities = (DataTable)ViewState["dtPriorities"];
            rgPriorities.DataSource = priorities;
        }

        protected void rgPriorities_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            DataTable priorities = (DataTable)ViewState["dtPriorities"];
            rgPriorities.DataSource = priorities;
            rgPriorities.DataBind();
        }

        protected void rgPriorities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["PriorityName"].Text;
                    LinkButton lb = dataItem.FindControl("btnEdit") as LinkButton;
                    lb.Text = "View/Edit " + name;
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgPriorities_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["PriorityName"].Text;
                    bool inactive = false;
                    if (dataItem["Inactive"].Text == "True")
                    {
                        inactive = true;
                    }
                    //string number = dataItem["Number"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openPriorityModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfPriorityId.Value = pId;
                        txtPriority.Text = name;
                        chkPriorityInactive.Checked = inactive;
                    }
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        //protected void btnDeletePriority_ServerClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        dvError.InnerHtml = "";
        //        List<FG_Priorities> priorities = (List<FG_Priorities>)ViewState["Priorities"];
        //        int index = 0;
        //        foreach (FG_Priorities priority in priorities)
        //        {
        //            if (priority.PriorityId == Convert.ToInt16(hfPriorityId.Value))
        //            {
        //                priorities.RemoveAt(index);
        //                break;
        //            }
        //        }
        //        ViewState["Priorities"] = priorities;
        //        rgPriorities.DataSource = priorities;
        //        rgPriorities.DataBind();
        //        dvError.InnerHtml = "<div class='alert alert-success'>Priority Removed</div>";
        //    }
        //    catch (Exception ex)
        //    {
        //        dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
        //    }
        //}

        protected async void btnSavePriority_ServerClick(object sender, EventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                List<FG_Priorities> priorities = (List<FG_Priorities>)ViewState["Priorities"];
                if (txtPriority.Text == "")
                {
                    throw new Exception("Priority Name cannot be blank");
                }
                FG_Priorities newPriority = new FG_Priorities();
                newPriority.DateCreated = DateTime.Now;
                if (hfPriorityId.Value == "")
                {
                    foreach (FG_Priorities priority in priorities)
                    {
                        if (priority.PriorityName == txtPriority.Text)
                        {
                            throw new Exception("Priority already exists in the list");
                        }
                    }
                    newPriority.PriorityId = 0;
                }
                else
                {
                    foreach (FG_Priorities priority in priorities)
                    {
                        if (priority.PriorityName == txtPriority.Text)
                        {
                            newPriority.PriorityId = priority.PriorityId;
                            newPriority.DateCreated = priority.DateCreated;
                            priorities.Remove(priority);
                            break;
                        }
                    }
                }
                
                newPriority.CategoryId = Convert.ToInt32(hfCategoryId.Value);
                newPriority.PriorityName = txtPriority.Text;
                newPriority.Inactive = chkPriorityInactive.Checked;
                priorities.Add(newPriority);
                //priorities = priorities.OrderBy(a => a.PriorityId);
                rgPriorities.DataSource = priorities.OrderBy(a => a.PriorityId);
                rgPriorities.DataBind();
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Admin/ManageCategories");
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<FG_Priorities> priorities = (List<FG_Priorities>)ViewState["Priorities"];
                if (priorities.Count < 1)
                {
                    throw new Exception("You must add at least one priority");
                }
                if (txtCategoryName.Text.Trim() == "")
                {
                    throw new Exception("You must enter a category name");
                }
                if (spHeader.InnerText == "Add Category")
                {
                    NMSFM.ViewModels.DetailedFGCategory newcat = new NMSFM.ViewModels.DetailedFGCategory();
                    newcat.CategoryName = txtCategoryName.Text;
                    newcat.Priorities = priorities;
                    newcat.Inactive = chkInactive.Checked;
                    bool catCreated = await fgService.SaveCategoryAsync(newcat);
                    if (catCreated)
                    {
                        Response.Redirect("../Admin/ManageCategories");

                    }
                    else
                    {
                        throw new Exception("Category Could Not Be Created");
                    }
                }
                else
                {
                    //ToDo Update Category
                    short catId = Convert.ToInt16(hfCategoryId.Value);
                    NMSFM.ViewModels.DetailedFGCategory newcat = new NMSFM.ViewModels.DetailedFGCategory();
                    newcat.CategoryId = catId;
                    newcat.CategoryName = txtCategoryName.Text;
                    newcat.Priorities = priorities;
                    newcat.Inactive = chkInactive.Checked;
                    bool catCreated = await fgService.UpdateCategoryAsync(newcat);
                    if (catCreated)
                    {
                        Response.Redirect("../Admin/ManageCategories");
                    }
                    else
                    {
                        throw new Exception("Category Could Not Be Updated");
                    }
                }

            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }
    }
}