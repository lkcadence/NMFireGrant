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
using System.Configuration;

namespace NMSFMFireGrantWF.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
        private IPartyService partyService;
        private IFGService fgService;

        private Emailer emailer;
        private static string _RegistrationToken;


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
                this.emailer = new Emailer();
            }
            else
            {
                var userConnection = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationModel"].ToString();
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fpfService = new FPFService(userContext, logger);
                this.partyService = new PartyService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.emailer = new Emailer();
            }
            try
            {
                Session["SessionId"] = Guid.NewGuid(); // Used as the session value for Audit and User records. HttpContext.Session.SessionID is not a GUID.
                Response.Cookies["ASPCookie"].Value = "SomeValue"; // Generating -any- session and cookie value helps most browsers avoid instances where the session/cookie objects aren't loaded yet.
                Session["CodepalUserId"] = "3c15fe68-b359-4c33-b138-90b95d9caea0";
                Session["CodepalUserName"] = "Anonymous Web Registration";
                _RegistrationToken = "334aaeb4-1005-41ba-9f85-54a1b232f70c";
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
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Register (Account)", "");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }
                if (RouteData.Values["UserId"] != null && RouteData.Values["RegistrationToken"] != null)
                {
                    string userId = RouteData.Values["UserId"].ToString();
                    string regToken = RouteData.Values["RegistrationToken"].ToString();
                    try
                    {
                        if (regToken != _RegistrationToken)
                        {
                            Response.Redirect("~/Default");
                        }
                        if (await ApproveRegistration(userId))
                        {
                            Response.Redirect("~/Account/RegistrationApproval", false);
                        }
                        else
                        {
                            Response.Redirect("~/Default");
                        }
                    }
                    catch (Exception ex)
            {
                _ = ex;
                        
                    }
                }
                bool deptsLoaded = await LoadDepartments();
                if (deptsLoaded)
                {
                    InitDepartments();
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

        private async Task<bool> ApproveRegistration(string userId)
        {
            try
            {
                //ToDo Appprove Registration
                Guid gdUserId = new Guid(userId);
                var webUser = await accountService.GetWebUserByIdAsync(gdUserId);
                if (webUser != null)
                {
                    webUser.Inactive = false;
                    bool updated = await accountService.UpdateExistingUser(webUser);
                    if (updated)
                    {
                        string url = (ConfigurationManager.AppSettings["ApplicationUrl"] != null) ? ConfigurationManager.AppSettings["ApplicationUrl"].ToString() : "http://firegranttest.vscomptech.com";
                        string from = (ConfigurationManager.AppSettings["DefaultEmailSender"] != null) ? ConfigurationManager.AppSettings["DefaultEmailSender"].ToString() : "vance@vscomptech.com";
                        string successNotification = "The account registration for user name " + webUser.Login + " has been activated. You may now login to the application at <a href='" + url + "'>" + url + "</a> to submit your Fire Grant Application.";
                        emailer.SendMailMessage(from, webUser.Email, "", "", "Registration for the NMSFM Fire Grant Application Approved", successNotification);
                    }
                    return updated;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private async Task<bool> LoadDepartments(string departmentName = "")
        {
            try
            {
                var addresses = new List<v_Addresses2>();
                addresses = (await fpfService.GetFPFApplicationsAllAsync()).OrderBy(a => a.AddressCode).ToList();
                //if (departmentName != "")
                //{
                //    departmentName = departmentName.ToLower();
                //    addresses = addresses.Where(a => a.AddressCode.ToLower().Contains(departmentName)).ToList();
                //}
                //rgDepartments.DataSource = addresses;
                //rgDepartments.DataBind();
                rcDepartments.DataSource = addresses;
                rcDepartments.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }

        protected void rgDepartments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //await LoadDepartments();
            List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
            rgDepartments.DataSource = assDepts;
        }

        protected void rgDepartments_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            //bool loaded = await LoadDepartments();
            //if (loaded)
            //{
            //    rgDepartments.Rebind();
            //}
            List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
            rgDepartments.DataSource = assDepts;
            rgDepartments.Rebind();
        }

        protected void rgDepartments_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {
            //bool loaded = await LoadDepartments();
            //if (loaded)
            //{
            //    rgDepartments.Rebind();
            //}
            List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
            rgDepartments.DataSource = assDepts;
            rgDepartments.Rebind();
        }

        protected void rgDepartments_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    string error = "";
                    bool bolError = false;
                    if (txtFirstName.Text == "")
                    {
                        error = lblFirstName.Text + " is required";
                        bolError = true;
                    }
                    if (txtLastName.Text == "")
                    {
                        error = lblLastName.Text + " is required";
                        bolError = true;
                    }
                    if (txtUsername.Text.Trim() == "")
                    {
                        error = "Username is required";
                        bolError = true;
                    }
                    else
                    {
                        if (txtUsername.Text.Contains("-"))
                        {
                            error = "Usernames cannot contain hyphens (-)";
                            bolError = true;
                        }
                    }
                    if (txtPassword.Text.Trim() == "")
                    {
                        error = "Password is required";
                        bolError = true;
                    }
                    else
                    {
                        bool passwordValid = true;
                        if (txtPassword.Text.Length < 6)
                        {
                            passwordValid = false;
                        }
                        if (HasUpper(txtPassword.Text.Trim()) == false)
                        {
                            passwordValid = false;
                        }
                        if (HasNumber(txtPassword.Text.Trim()) == false)
                        {
                            passwordValid = false;
                        }
                        if (passwordValid == false)
                        {
                            error = "Password must be a minimum of 6 characters, have at least 1 uppercase letter and at least 1 number.";
                            bolError = true;
                        }
                    }
                    if (txtEmail.Text.Trim() == "")
                    {
                        error = "Email Address is required";
                        bolError = true;
                    }
                    //if (rgDepartments.SelectedItems.Count == 0)
                    //{
                    //    error = "You must select at least one fire department";
                    //    bolError = true;
                    //}
                    List<DetailedFGDepartment> assDepts = (List<DetailedFGDepartment>)ViewState["assDepts"];
                    if (assDepts.Count < 1)
                    {
                        error = "You must select at least one fire department";
                        bolError = true;
                    }
                    if (txtFDID.Text == "")
                    {
                        error = "You must enter the NERIS ID";
                        bolError = true;
                    }
                    else
                    {
                        // Legacy (pre-NERIS 20-char): numeric-only validation via IsFDIDValid(int).
                        // var fdid = await fgService.IsFDIDValid(Convert.ToInt32(txtFDID.Text));
                        string nerisId = txtFDID.Text.Trim().ToUpperInvariant();
                        var fdid = await fgService.GetFDIDByIdAsync(nerisId);
                        if (fdid == null)
                        {
                            error = "You must enter a valid NERIS ID";
                            bolError = true;
                        }
                    }
                    if (bolError == false)
                    {
                        Guid codePalId = Guid.NewGuid();
                        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationModel"].ToString();
                        HttpContext.Current.Session["userConnection"] = connectionString;
                        connectionString = EncryptString(connectionString);
                        string dbName = "Codepal_NMSFM";
                        string organization = "New Mexico State Fire Marshal";
                        NMSFM.Data.User user = new NMSFM.Data.User() { Login = txtUsername.Text.Trim(), Password = EncryptString(txtPassword.Text.Trim()), Email = txtEmail.Text.Trim(), ConnectionString = connectionString, CodepalId = codePalId, DatabaseName = dbName, Organization = organization, NMFGC = false, Readonly = false, NMFGA = true, Inactive = true };
                        var duplicate = await accountService.GetDuplicateWebUserByInfoAsync(user);
                        if (duplicate == null)
                        {
                            var userSlotWasAvailable = await accountService.SaveWebUserAsync(user);
                            if (userSlotWasAvailable)
                            {
                                int numParties = 0;
                                string strDepartments = "";
                                List<DetailedAddressParty> attachedParties = new List<DetailedAddressParty>();
                                //foreach (GridDataItem item in rgDepartments.Items)
                                //{
                                //    if (item.Selected && item.SelectableMode != GridItemSelectableMode.ServerSide)
                                //    {
                                        
                                //        NMSFM.ViewModels.DetailedAddressParty addParty = new NMSFM.ViewModels.DetailedAddressParty();
                                //        addParty.AddressPartyId = Guid.NewGuid();
                                //        addParty.AddressId = new Guid(item["AddressId"].Text.ToString());
                                //        addParty.PartyId = codePalId;
                                //        addParty.RoleTypeId = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");
                                //        attachedParties.Add(addParty);
                                //        strDepartments += item["Department"].Text.ToString() + ",";
                                //        numParties += 1;
                                //    }
                                //}
                                foreach (DetailedFGDepartment assDept in assDepts)
                                {
                                    NMSFM.ViewModels.DetailedAddressParty addParty = new NMSFM.ViewModels.DetailedAddressParty();
                                    addParty.AddressPartyId = Guid.NewGuid();
                                    addParty.AddressId = assDept.addressId;
                                    addParty.PartyId = codePalId;
                                    addParty.RoleTypeId = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");
                                    attachedParties.Add(addParty);
                                    strDepartments += assDept.DepartmentName + ",";
                                    numParties += 1;
                                }

                                //ToDo Add New Party
                                var party = new NMSFM.ViewModels.DetailedAddressParty();
                                party.PartyId = codePalId;
                                party.PartyName = txtFirstName.Text + " " + txtLastName.Text;
                                party.RoleTypeId = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");
                                party.Email = txtEmail.Text;
                                party.Phone = txtPhone.Text;
                                party.AddressId = attachedParties[0].AddressId;
                                party.AddressPartyId = attachedParties[0].AddressPartyId;
                                party.FromWeb = true;
                                Guid partyId = await partyService.CreatePartyAsync(party);

                                if (partyId != null)
                                {
                                    if (numParties > 1)
                                    {
                                        for (int i = 1; i < numParties; i++)
                                        {
                                            await partyService.AttachExistingParty(attachedParties[i]);
                                        }
                                    }
                                }
                                try
                                {
                                    var savedUser = await accountService.GetWebUserByInfoAsync(user.Login, user.Password);
                                    string userId = savedUser.UserId.ToString();
                                    strDepartments = strDepartments.Substring(0, strDepartments.Length - 1);
                                    string from = (ConfigurationManager.AppSettings["DefaultEmailSender"] != null) ? ConfigurationManager.AppSettings["DefaultEmailSender"].ToString() : "vance@vscomptech.com";
                                    string to = (ConfigurationManager.AppSettings["AccountEmailApprovers"] != null) ? ConfigurationManager.AppSettings["AccountEmailApprovers"].ToString() : "vance@vscomptech.com";
                                    string url = (ConfigurationManager.AppSettings["ApplicationUrl"] != null) ? ConfigurationManager.AppSettings["ApplicationUrl"].ToString() : "http://firegranttest.vscomptech.com";
                                    string successAdminEmail = txtFirstName.Text + " " + txtLastName.Text + " (NERIS ID: " + txtFDID.Text + ") has submitted a registration for the Fire Grant Application for the (" + strDepartments + ") departments.";
                                    successAdminEmail += "<br />To approve this registration please click the approval link below or login to the Fire Grant Application and activate the user account.";
                                    successAdminEmail += "<br /><br /><a href=" + url + "/ApproveRegistration/appr/" + userId + "/" + _RegistrationToken + ">Approve Registration</a>";
                                    emailer.SendMailMessage(from, to, "", "", "Fire Grant Registration Submitted", successAdminEmail, "");
                                }
                                catch (Exception ex)
            {
                _ = ex;

                                }
                                
                                
                                //Session["UserSuccess"] = txtUsername.Text + " has been successfully added";
                                Response.Redirect("~/Account/RegistrationConfirmation");
                            }
                            else
                            {
                                error = "The maximum number of Inspector web accounts has already been created for this license.";
                                throw new Exception(error);
                            }
                        }
                        else
                        {
                            error = "The desired username or email address is already in use. Please use a different username or email address.";
                            throw new Exception(error);
                        }
                    }
                    else
                    {
                        throw new Exception(error);
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {

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

        private bool HasUpper(string password)
        {
            bool hasUpper = false;
            foreach (char p in password)
            {
                if (Char.IsUpper(p))
                {
                    hasUpper = true;
                }
            }
            return hasUpper;
        }

        private bool HasNumber(string password)
        {
            bool hasNumber = false;
            foreach (char p in password)
            {
                if (Char.IsNumber(p))
                {
                    hasNumber = true;
                }
            }
            return hasNumber;
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
    }
}





