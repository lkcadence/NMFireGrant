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
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using NMSFM.Services.FireGrant;
using Telerik.Web.UI;

namespace NMSFMFireGrantWF.Account
{
    public partial class EditUser : System.Web.UI.Page
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

            }
        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Edit User (Account)", "Edit Basic User Information");
                FG_App_Help help2 = await fgService.GetFGHelpByPage("Edit User (Account)", "Associate Departments");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText + "<hr />" + help2.HelpText;
                }

                if (!Page.IsPostBack)
                {
                    string userId = RouteData.Values["UserId"].ToString();
                    LoadUser(userId);
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
            
        }

        private async void LoadUser(string userId)
        {
            try
            {
                Guid gUserId = new Guid(userId);
                var webUser = await accountService.GetWebUserByIdAsync(gUserId);
                if (webUser != null)
                {
                    hfCodepalId.Value = webUser.CodepalId.ToString();
                    hfUserId.Value = webUser.UserId.ToString();
                    chkInactive.Checked = (webUser.Inactive != null) ? Convert.ToBoolean(webUser.Inactive) : false;
                    if (webUser.IsWebAdmin == true)
                    {
                        rbInspectors.Checked = true;
                        lblFirstName.Text = "Admin Name";
                        txtFirstName.Text = "Account Admin";
                        txtFirstName.ReadOnly = true;
                        txtUsername.Text = webUser.Login;
                    }
                    else
                    {
                        var inspector = await addressService.GetInspectorByIdAsync(webUser.CodepalId.Value);
                        if (inspector != null) //Internal User with matching login was found
                        {
                            rbInspectors.Checked = true;
                            txtFirstName.Text = inspector.InspectorName;
                            lblFirstName.Text = "Codepal Username";
                            txtEmail.Text = inspector.Email;
                            txtPhone.Text = inspector.InspectorPhone;
                            txtUsername.Text = webUser.Login;
                            dvDepartments.Visible = false;
                            dvGrantAdmin.Visible = true;
                        }
                        else
                        {
                            var user = await addressService.GetPartyWebAccessByIdAsync(webUser.CodepalId.Value);
                            if (user != null)
                            {
                                rbParties.Checked = true;
                                txtFirstName.Text = user.PartyName;
                                txtEmail.Text = user.Email;
                                //ToDo Get phone number from phone list
                                txtPhone.Text = user.Phone;
                                lblFirstName.Text = "Codepal Party Name";
                                txtUsername.Text = webUser.Login;
                                dvDepartments.Visible = true;
                                chkGrantAdmin.Checked = false;
                                chkReadOnly.Checked = false;
                                dvGrantAdmin.Visible = false;
                                LoadDepartments();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void LoadDepartments()
        {
            try
            {

                if (rbAssociatedDepartments.Checked)
                {
                    var addresses = new List<v_AddressParties>();
                    Guid partyId = new Guid(hfCodepalId.Value.ToString());
                    addresses = (await fpfService.GetFPFApplicationsAsync(partyId)).OrderBy(a => a.AddressCode).ToList();
                    rgDepartments.DataSource = addresses;
                    rgDepartments.DataBind();
                }
                else if (rbAllDepartments.Checked)
                {
                    var addresses = new List<v_Addresses2>();
                    addresses = (await fpfService.GetFPFApplicationsAllAsync()).OrderBy(a => a.AddressCode).ToList();
                    rgDepartments.DataSource = addresses;
                    rgDepartments.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rbAssociatedDepartments_CheckedChanged(object sender, EventArgs e)
        {
            LoadDepartments();
        }


        protected async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string error = "";
                bool bolError = false;
                if (txtFirstName.Text == "")
                {
                    error = lblFirstName.Text + " is required";
                    bolError = true;
                }
                if (txtUsername.Text.Trim() == "")
                {
                    error = "Username is required";
                    bolError = true;
                }
                //if (txtPassword.Text.Trim() == "")
                //{
                //    error = "Password is required";
                //    bolError = true;
                //}
                if (rbParties.Checked)
                {
                    if (rgDepartments.SelectedItems.Count == 0)
                    {
                        error = "You must select at least one fire department";
                        bolError = true;
                    }
                }
                if (bolError == false)
                {
                    Guid userId = new Guid(hfUserId.Value);
                    Guid codePalId = new Guid(hfCodepalId.Value);
                    string connectionString = Session["userConnectionEncrypted"].ToString();
                    string dbName = "Codepal_NMSFM";
                    string organization = "New Mexico State Fire Marshal";
                    NMSFM.Data.User user = new NMSFM.Data.User() {UserId = userId, Login = txtUsername.Text.Trim(), Password = EncryptString(txtPassword.Text.Trim()), ConnectionString = connectionString, CodepalId = codePalId, DatabaseName = dbName, Organization = organization, NMFGC = chkGrantAdmin.Checked, Inactive = chkInactive.Checked, Readonly = chkReadOnly.Checked };
                    var duplicate = await accountService.GetWebUserByIdAsync(user.UserId);
                    if (duplicate != null)
                    {
                        var userUpdated = await accountService.UpdateExistingUser(user);
                        if (userUpdated)
                        {
                            if (rbParties.Checked)
                            {
                                foreach (GridDataItem item in rgDepartments.Items)
                                {
                                    //Update Party??

                                    if (item.Selected && item.SelectableMode != GridItemSelectableMode.ServerSide)
                                    {
                                        NMSFM.ViewModels.DetailedAddressParty addParty = new NMSFM.ViewModels.DetailedAddressParty();
                                        addParty.AddressPartyId = Guid.NewGuid();
                                        addParty.AddressId = new Guid(item["AddressId"].Text.ToString());
                                        addParty.PartyId = codePalId;
                                        addParty.RoleTypeId = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");
                                        await partyService.AttachExistingParty(addParty);
                                    }
                                }
                            }
                            else
                            {
                                //Update Inspector?

                            }
                            Session["UserSuccess"] = txtUsername.Text + " has been successfully updated";
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
                        error = "The User could not be found";
                        throw new Exception(error);
                    }
                }
                else
                {
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
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
            LoadDepartments();
        }

        protected void rgDepartments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            LoadDepartments();
        }

        protected void rgDepartments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    //if (rbAssociatedDepartments.Checked)
                    //{
                    //    CheckBox isSelected = (CheckBox)dataItem["ClientSelect"].Controls[0];
                    //    dataItem.Selected = true;
                    //    isSelected.Checked = true;
                    //    isSelected.Enabled = false;
                    //    dataItem.SelectableMode = GridItemSelectableMode.ServerSide;
                    //}
                    //else
                    //{
                    //    string strAddressId = dataItem["AddressId"].Text.ToString().Replace("{", "").Replace("}", "");
                    //    Guid addressId = new Guid(strAddressId);
                    //    CheckBox isSelected = (CheckBox)dataItem["ClientSelect"].Controls[0];
                    //    isDeptAssociated(isSelected, addressId);
                    //    if (isSelected.Checked == true)
                    //    {
                    //        dataItem.Selected = true;
                    //        if (isSelected.Enabled)
                    //        {
                    //            dataItem.SelectableMode = GridItemSelectableMode.ServerAndClientSide;
                    //        }
                    //        else
                    //        {
                    //            dataItem.SelectableMode = GridItemSelectableMode.ServerSide;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        dataItem.SelectableMode = GridItemSelectableMode.ServerAndClientSide;
                    //    }
                    //}
                    string strAddressId = dataItem["AddressId"].Text.ToString().Replace("{", "").Replace("}", "");
                    Guid addressId = new Guid(strAddressId);
                    CheckBox isSelected = (CheckBox)dataItem["ClientSelect"].Controls[0];
                    isDeptAssociated(isSelected, addressId);
                    if (isSelected.Checked == true)
                    {
                        dataItem.Selected = true;
                        if (isSelected.Enabled)
                        {
                            dataItem.SelectableMode = GridItemSelectableMode.ServerAndClientSide;
                        }
                        else
                        {
                            dataItem.SelectableMode = GridItemSelectableMode.ServerSide;
                        }
                    }
                    else
                    {
                        dataItem.SelectableMode = GridItemSelectableMode.ServerAndClientSide;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void isDeptAssociated(CheckBox address, Guid addressId)
        {
            Guid partyId = new Guid(hfCodepalId.Value.ToString());
            v_AddressParties add = new v_AddressParties();
            add = (fpfService.GetFGFApplicationAddress(partyId, addressId));
            if (add != null)
            {
                address.Checked = true;
                if (add.RoleType == "FPF Responsible Party")
                {
                    address.Enabled = false;
                }
            }
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ManageUsers");
        }
    }
}