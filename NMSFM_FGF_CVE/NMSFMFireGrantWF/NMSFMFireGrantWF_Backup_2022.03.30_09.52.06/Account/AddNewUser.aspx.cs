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
    public partial class AddNewUser : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
        private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
        private IPartyService partyService;
        private IFGService fgService;

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

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
            if (!Page.IsPostBack)
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Add New User (Account)", "");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                LoadDepartments();
            }
        }

        private async void LoadDepartments()
        {
            try
            {
                if (rbInspectors.Checked)
                {
                    dvDepartments.Visible = false;
                    lblFirstName.Text = "Admin Name";
                    dvGrantAdmin.Visible = true;
                }
                else
                {
                    dvDepartments.Visible = true;
                    lblFirstName.Text = "Party Name";
                    var addresses = new List<v_Addresses2>();
                    addresses = (await fpfService.GetFPFApplicationsAllAsync()).OrderBy(a => a.AddressCode).ToList();
                    rgDepartments.DataSource = addresses;
                    rgDepartments.DataBind();
                    chkGrantAdmin.Checked = false;
                    chkReadOnly.Checked = false;
                    dvGrantAdmin.Visible = false;
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        protected void rbInspectors_CheckedChanged(object sender, EventArgs e)
        {
            LoadDepartments();
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
                    if (txtUsername.Text.Trim() == "")
                    {
                        error = "Username is required";
                        bolError = true;
                    }
                    if (txtPassword.Text.Trim() == "")
                    {
                        error = "Password is required";
                        bolError = true;
                    }
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
                        Guid codePalId = Guid.NewGuid();
                        string connectionString = Session["userConnectionEncrypted"].ToString();
                        string dbName = "Codepal_NMSFM";
                        string organization = "New Mexico State Fire Marshal";
                        NMSFM.Data.User user = new NMSFM.Data.User() { Login = txtUsername.Text.Trim(), Password = EncryptString(txtPassword.Text.Trim()), ConnectionString = connectionString, CodepalId = codePalId, DatabaseName = dbName, Organization = organization, NMFGC = chkGrantAdmin.Checked, Readonly = chkReadOnly.Checked };
                        var duplicate = await accountService.GetDuplicateWebUserByInfoAsync(user);
                        if (duplicate == null)
                        {
                            var userSlotWasAvailable = await accountService.SaveWebUserAsync(user);
                            if (userSlotWasAvailable)
                            {
                                if (rbParties.Checked)
                                {
                                    int numParties = 0;
                                    List<DetailedAddressParty> attachedParties = new List<DetailedAddressParty>();
                                    foreach (GridDataItem item in rgDepartments.Items)
                                    {
                                        if (item.Selected && item.SelectableMode != GridItemSelectableMode.ServerSide)
                                        {
                                            numParties += 1;
                                            NMSFM.ViewModels.DetailedAddressParty addParty = new NMSFM.ViewModels.DetailedAddressParty();
                                            addParty.AddressPartyId = Guid.NewGuid();
                                            addParty.AddressId = new Guid(item["AddressId"].Text.ToString());
                                            addParty.PartyId = codePalId;
                                            addParty.RoleTypeId = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");
                                            attachedParties.Add(addParty);
                                        }
                                    }

                                    //ToDo Add New Party
                                    var party = new NMSFM.ViewModels.DetailedAddressParty();
                                    party.PartyId = codePalId;
                                    party.PartyName = txtFirstName.Text;
                                    party.RoleTypeId = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");
                                    party.Email = txtEmail.Text;
                                    party.Phone = txtPhone.Text;
                                    party.AddressId = attachedParties[0].AddressId;
                                    party.AddressPartyId = attachedParties[0].AddressPartyId;
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


                                }
                                else
                                {
                                    //ToDo add new Inspector?
                                    var inspector = new NMSFM.ViewModels.DetailedInspector();
                                    inspector.InspectorId = codePalId;
                                    inspector.InspectorName = txtFirstName.Text;
                                    inspector.Email = txtEmail.Text;
                                    inspector.InspectorPhone = txtPhone.Text;
                                    inspector.Login = txtUsername.Text;
                                    inspector.Password = EncryptString(txtPassword.Text);
                                    inspector.AgencyId = new Guid(Session["AgencyId"].ToString());
                                    await systemService.CreateInspector(inspector);


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
                        throw new Exception(error);
                    }
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

        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ManageUsers");
        }
    }
}