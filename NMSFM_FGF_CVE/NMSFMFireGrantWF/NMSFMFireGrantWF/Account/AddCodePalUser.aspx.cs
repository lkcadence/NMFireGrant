using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
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
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using NMSFM.Services.FireGrant;
using Telerik.Web.UI;

namespace NMSFMFireGrantWF.Account
{
    public partial class AddCodePalUser : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
        private IPartyService partyService;
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
                this.partyService = new PartyService(userContext, logger);
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
                if (Session["IsWebAdmin"] == null || Convert.ToBoolean(Session["IsWebAdmin"]) != true)
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
                try
                {
                    HtmlGenericControl helpdiv = new HtmlGenericControl();
                    helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                    FG_App_Help help = await fgService.GetFGHelpByPage("Add Codepal User (Account)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }
                    InitDepartments();
                    LoadAllDepartments();
                    LoadUsers();
                }
                catch (Exception ex)
            {
                _ = ex;
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                }               
            }
        }

        private void InitDepartments()
        {
            try
            {
                var depts = new List<DetailedFGDepartment>();
                ViewState["assDepts"] = depts;
            }
            catch
            {
                throw;
            }
        }

        private async void LoadAllDepartments()
        {
            try
            {
                if (rbInspectors.Checked)
                {
                    dvDepartments.Visible = false;
                }
                else
                {
                    dvDepartments.Visible = true;
                    var addresses = new List<v_Addresses2>();
                    addresses = (await fpfService.GetFPFApplicationsAllAsync()).OrderBy(a => a.AddressCode).ToList();
                    rcDepartments.DataSource = addresses;
                    rcDepartments.DataBind();
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private async void LoadUsers()
        {
            try
            {
                ddlUsers.Items.Clear();
                var userList = new UserList();
                var existingUserList = (await accountService.GetUserList()).ToList();
                var inspectorList = (await addressService.GetInspectorListAsync()).OrderBy(a => a.InspectorName).ToList();
                var partyList = (await addressService.GetPartyWebAccessListAsync()).OrderBy(a => a.PartyName).ToList();
                if (rbInspectors.Checked)
                {
                    lblSelectUser.Text = "Select Inspector";
                    dvDepartments.Visible = false;
                    dvGrantAdmin.Visible = true;
                    foreach (Inspector insp in inspectorList)
                    {
                        if (existingUserList.Find(a => a.CodepalId == insp.InspectorId) == null)
                        {
                            ListItem li = new ListItem();
                            li.Text = insp.InspectorName;
                            li.Value = insp.InspectorId.ToString();
                            ddlUsers.Items.Add(li);
                        }  
                    }
                    Guid inspectorId = new Guid(ddlUsers.SelectedValue);
                    var inspector = await addressService.GetInspectorByIdAsync(inspectorId);
                    if (inspector != null)
                    {
                        txtEmail.Text = inspector.Email;
                    }
                }
                if (rbParties.Checked)
                {
                    lblSelectUser.Text = "Select Party";
                    dvDepartments.Visible = true;
                    chkGrantAdmin.Checked = false;
                    chkReadOnly.Checked = false;
                    dvGrantAdmin.Visible = false;
                    foreach (v_Parties party in partyList)
                    {
                        if (existingUserList.Find(a => a.CodepalId == party.PartyID) == null)
                        {
                            ListItem li = new ListItem();
                            li.Text = (party.PartyName != null) ? party.PartyName : "Unknown";
                            li.Value = party.PartyID.ToString();
                            ddlUsers.Items.Add(li);
                        }
                    }
                    Guid partyId = new Guid(ddlUsers.SelectedValue);
                    var thisParty = await addressService.GetPartyWebAccessByIdAsync(partyId);
                    if (thisParty != null)
                    {
                        txtEmail.Text = thisParty.Email;
                    }
                    LoadAssociatedDepartments();
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private async void LoadAssociatedDepartments()
        {
            try
            {
                List<DetailedFGDepartment> assDepts = new List<DetailedFGDepartment>();
                var addresses = new List<v_AddressParties>();
                Guid partyId = new Guid(ddlUsers.SelectedValue);
                addresses = (await fpfService.GetFPFApplicationsAsync(partyId)).OrderBy(a => a.AddressCode).ToList();
                foreach (v_AddressParties party in addresses)
                {
                    DetailedFGDepartment thisDept = new DetailedFGDepartment();
                    thisDept.addressId = new Guid(party.AddressId.ToString());
                    thisDept.DepartmentName = party.AddressCode;
                    assDepts.Add(thisDept);
                }
                ViewState["assDepts"] = assDepts;
                rgDepartments.DataSource = assDepts;
                rgDepartments.DataBind();

            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        protected void rbInspectors_CheckedChanged(object sender, EventArgs e)
        {
            LoadAllDepartments();
            LoadUsers();
            ddlUsers.Focus();
        }


        protected async void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            try
            {
                if (rbInspectors.Checked)
                {
                    Guid inspectorId = new Guid(ddlUsers.SelectedValue);
                    var inspector = await addressService.GetInspectorByIdAsync(inspectorId);
                    if (inspector != null)
                    {
                        txtEmail.Text = inspector.Email;
                    }
                }
                else if (rbParties.Checked)
                {
                    Guid partyId = new Guid(ddlUsers.SelectedValue);
                    var party = await addressService.GetPartyWebAccessByIdAsync(partyId);
                    if (party != null)
                    {
                        txtEmail.Text = party.Email;
                    }
                }
                LoadAssociatedDepartments();
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string error = "";
                if (ddlUsers.SelectedItem != null)
                {
                    if (txtUsername.Text.Trim() == "")
                    {
                        throw new Exception("Username is required");
                    }
                    else
                    {
                        if (txtUsername.Text.Contains("-"))
                        {
                            throw new Exception("Usernames cannot contain hyphens (-)");
                        }
                    }
                    List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
                    if (rbParties.Checked && assDepts.Count < 1)
                    {
                        throw new Exception("You must select at least 1 department");
                    }
                    if (txtPassword.Text.Trim() == "")
                    {
                        throw new Exception("Password is required");
                    }
                    Guid codePalId = new Guid(ddlUsers.SelectedValue);
                    string connectionString = Session["userConnectionEncrypted"].ToString();
                    string dbName = "Codepal_NMSFM";
                    string organization = "New Mexico State Fire Marshal";
                    NMSFM.Data.User user = new NMSFM.Data.User() { Login = txtUsername.Text.Trim(), Password = EncryptString(txtPassword.Text.Trim()), Email = txtEmail.Text.Trim(), ConnectionString = connectionString, CodepalId = codePalId, DatabaseName = dbName, Organization = organization, NMFGC = chkGrantAdmin.Checked, Readonly = chkReadOnly.Checked, NMFGA = true };
                    var duplicate = await accountService.GetDuplicateWebUserByInfoAsync(user);
                    if (duplicate == null)
                    {
                        var userSlotWasAvailable = await accountService.SaveWebUserAsync(user);
                        if (userSlotWasAvailable)
                        {
                            if (rbParties.Checked)
                            {
                                foreach (DetailedFGDepartment dept in assDepts)
                                {
                                    NMSFM.ViewModels.DetailedAddressParty addParty = new NMSFM.ViewModels.DetailedAddressParty();
                                    addParty.AddressPartyId = Guid.NewGuid();
                                    addParty.AddressId = new Guid(dept.addressId.ToString());
                                    addParty.PartyId = codePalId;
                                    addParty.RoleTypeId = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");
                                    await partyService.AttachExistingParty(addParty);
                                }
                            }
                            Session["UserSuccess"] = txtUsername.Text + " has been successfully added";
                            Response.Redirect("~/Account/ManageUsers");
                        }
                        else
                        {
                            error = "The maximum number of Inspector web accounts has already been created for this license.";
                            throw new Exception(error);
                        }
                    }
                    else
                    {
                        error = "The desired username is already in use. Please use a different username.";
                        throw new Exception(error);
                    }
                }
                else
                {
                    error = "Please select a Codepal User.";
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private string EncryptString(string baseString)
        {
            byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
            byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            byte[] inputByteArray = System.Text.UTF8Encoding.UTF8.GetBytes(baseString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var encryptString = Convert.ToBase64String(ms.ToArray());

            return encryptString;
        }

        private async Task<string> DecryptString(string encryptedString)
        {
            string baseString = "";
            encryptedString = encryptedString.Replace(" ", "+");
            byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
            byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(encryptedString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            baseString = System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray());
            return baseString;
        }

        protected void rgDepartments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
            rgDepartments.DataSource = assDepts;
        }

        protected void rgDepartments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
            rgDepartments.DataSource = assDepts;
            rgDepartments.Rebind();
        }

        protected void rgDepartments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //try
            //{

            //    if (e.Item is GridDataItem)
            //    {
            //        GridDataItem dataItem = e.Item as GridDataItem;
            //        if (rbAssociatedDepartments.Checked)
            //        {
            //            CheckBox isSelected = (CheckBox)dataItem["ClientSelect"].Controls[0];
            //            dataItem.Selected = true;
            //            isSelected.Checked = true;
            //            isSelected.Enabled = false;
            //        }
            //        else
            //        {
            //            string strAddressId = dataItem["AddressId"].Text.ToString().Replace("{", "").Replace("}", "");
            //            Guid addressId = new Guid(strAddressId);
            //            CheckBox isSelected = (CheckBox)dataItem["ClientSelect"].Controls[0];
            //            isDeptAssociated(isSelected, addressId);
            //            if (isSelected.Checked == true)
            //            {
            //                dataItem.Selected = true;
            //                dataItem.SelectableMode = GridItemSelectableMode.ServerSide;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        protected async void lnkSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //string searchTerm = txtDepartmentName.Text.Trim();
                //await LoadDepartments(searchTerm);
                //rgDepartments.Rebind();
                if (rcDepartments.SelectedValue != null)
                {
                    string addressId = rcDepartments.SelectedValue;
                    string DepartmentName = rcDepartments.SelectedItem.Text;

                    List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
                    bool alreadyadded = false;
                    foreach (DetailedFGDepartment dept in assDepts)
                    {
                        if (dept.addressId.ToString() == addressId)
                        {
                            alreadyadded = true;
                        }
                    }
                    if (alreadyadded == false)
                    {
                        Guid gAddId = new Guid(addressId);
                        DetailedFGDepartment newDept = new DetailedFGDepartment();
                        newDept.addressId = gAddId;
                        newDept.DepartmentName = DepartmentName;
                        if (newDept != null)
                        {
                            assDepts.Add(newDept);
                        }
                        ViewState["assDepts"] = assDepts;
                        rgDepartments.DataSource = assDepts;
                        rgDepartments.DataBind();
                    }
                    else
                    {
                        throw new Exception("Department is already in the list");
                    }
                    rcDepartments.SelectedIndex = 0;
                    rcDepartments.Focus();
                }
                else
                {
                    throw new Exception("Please select a department from the list to add");
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgDepartments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string id = "";
                    id = dataItem["addressId"].Text;
                    if (e.CommandName == "Delete")
                    {
                        List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
                        for (int i = 0; i < assDepts.Count; i++)
                        {
                            if (assDepts[i].addressId.ToString() == id)
                            {
                                assDepts.RemoveAt(i);
                                break;
                            }
                        }
                        ViewState["assDepts"] = assDepts;
                        rgDepartments.DataSource = assDepts;
                        rgDepartments.Rebind();
                        rgDepartments.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private void isDeptAssociated(CheckBox address, Guid addressId)
        {
            Guid partyId = new Guid(ddlUsers.SelectedValue);
            v_AddressParties add = new v_AddressParties();
            add = (fpfService.GetFGFApplicationAddress(partyId, addressId));
            if (add != null)
            {
                address.Checked = true;
                address.Enabled = false;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ManageUsers");
        }
    }
}





