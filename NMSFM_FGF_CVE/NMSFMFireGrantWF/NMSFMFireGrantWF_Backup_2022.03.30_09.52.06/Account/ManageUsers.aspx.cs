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

namespace NMSFMFireGrantWF.Account
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
        private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
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
                this.fpfService = new FPFService(userContext, logger);
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

            }
        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    //InitExistingUsers();
                    HtmlGenericControl helpdiv = new HtmlGenericControl();
                    helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                    FG_App_Help help = await fgService.GetFGHelpByPage("Manage Users (Account)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }

                    if (Session["UserSuccess"] != null)
                    {
                        dvSuccess.InnerHtml = "<div class='alert alert-success'>" + Session["UserSuccess"].ToString() + "</div>";
                        Session["UserSuccess"] = null;
                    }
                    LoadExistingUsers();
                }
                catch (Exception ex)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                }
                
            }
        }

        private async void LoadExistingUsers()
        {
            try
            {
                var existingUsers = (await accountService.GetUserList()).ToList();
                List<NMSFM.ViewModels.ExistingUser> users = new List<NMSFM.ViewModels.ExistingUser>();
                

                foreach (NMSFM.Data.User usr in existingUsers)
                {
                    var user = new NMSFM.ViewModels.ExistingUser();
                    var inspector = await addressService.GetInspectorByIdAsync(usr.CodepalId.Value);
                    if (inspector != null)
                    {
                        user.UserId = usr.UserId;
                        user.Login = usr.Login;
                        user.Role = "Admin";
                        user.Inactive = usr.Inactive;
                        user.Name = inspector.InspectorName;
                        user.Department = "NMSFM";
                        users.Add(user);
                    }
                    else
                    {
                        var party = await addressService.GetPartyWebAccessByIdAsync(usr.CodepalId.Value);
                        //added below 1/10/2021 to create new application (vwd)
                        if (party != null)
                        {
                            var addresses = await fpfService.GetFPFApplicationsAsync(party.PartyID);
                            string depts = "";
                            foreach (v_AddressParties add in addresses)
                            {
                                depts += add.AddressCode + "; ";
                            }
                            if (depts != "") { depts = depts.Remove(depts.Length - 2); }
                            user.UserId = usr.UserId;
                            user.Login = usr.Login;
                            user.Role = "User";
                            user.Inactive = (usr.Inactive != null) ? usr.Inactive : false;
                            user.Name = party.PartyName;
                            user.Department = depts;
                            users.Add(user);
                        }
                    }
                }

                if (txtSearchUser.Text != "")
                {
                    users = users.Where(i => i.Name.ToLower().Contains(txtSearchUser.Text.ToLower().Trim())).ToList();
                }
                if (txtSearchDepartment.Text != "")
                {
                    users = users.Where(i => i.Department.ToLower().Contains(txtSearchDepartment.Text.ToLower().Trim())).ToList();
                }
                ViewState["existingUsers"] = users;
                rgExistingUsers.DataSource = users;
                rgExistingUsers.DataBind();

                //var inspectorList = (await addressService.GetInspectorListAsync()).ToList();
                //var partyList = (await addressService.GetPartyWebAccessListAsync()).ToList();

            }
            catch 
            {
                throw;
            }
        }

        private void InitExistingUsers()
        {
            DataTable existingUsers = new DataTable();
            existingUsers.Columns.Add("Name", typeof(string));
            existingUsers.Columns.Add("Login", typeof(string));
            existingUsers.Columns.Add("Role", typeof(string));
            existingUsers.Columns.Add("Department", typeof(string));
            existingUsers.Columns.Add("Inactive", typeof(bool));
            existingUsers.Columns.Add("UserId", typeof(string));

            //Remove Test row when live
            existingUsers.Rows.Add("Test Name", "Test Login", "User", "Test Department", false, "fd5eaaf8-2c7f-493c-9a5e-d9fad30ea258");
            ViewState["existingUsers"] = existingUsers;

            rgExistingUsers.DataSource = existingUsers;
            rgExistingUsers.DataBind();
        }

        protected void rgExistingUsers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<NMSFM.ViewModels.ExistingUser> users = (List<NMSFM.ViewModels.ExistingUser>)ViewState["existingUsers"];
            rgExistingUsers.DataSource = users;
        }

        protected void rgExistingUsers_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            List<NMSFM.ViewModels.ExistingUser> users = (List<NMSFM.ViewModels.ExistingUser>)ViewState["existingUsers"];
            rgExistingUsers.DataSource = users;
        }

        protected void rgExistingUsers_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["Name"].Text;
                    LinkButton delete = (LinkButton)dataItem["Edit"].Controls[0];
                    delete.Text = "Edit " + name;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void lnkAddCodePalUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Account/AddCodePalUser");
        }

        protected void lnkAddNewUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Account/AddNewUser");
        }

        protected void rgExistingUsers_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var ditem = e.Item as GridDataItem;
                    string itemValue = ditem["UserId"].Text.ToString();
                    if ((e.CommandName == "View"))
                    {
                        Response.RedirectToRoute("EditUser", new { UserId = itemValue });
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadExistingUsers();
            }
            catch (Exception ex)
            {

            }
        }
    }
}