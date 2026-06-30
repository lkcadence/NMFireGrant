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
using NMSFM.Services.FireGrant;
using NMSFM.Services.UDF;
using Telerik.Web.UI;
using System.IO;

namespace NMSFMFireGrantWF.Admin
{
    public partial class ManageSettings : System.Web.UI.Page
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
                try
                {
                    HtmlGenericControl helpdiv = new HtmlGenericControl();
                    helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                    FG_App_Help help = await fgService.GetFGHelpByPage("Manage Settings (Admin)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }

                    LoadFiscalYears();
                    short fYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                    LoadSettings(fYear);
                }
                catch (Exception ex)
            {
                _ = ex;
                    dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                    dvError.Focus();
                }
            }
        }

        private void LoadFiscalYears()
        {
            ddlFiscalYear.Items.Clear();
            int fiscalyear = 0;
            fiscalyear = DateTime.Now.Year + 1;
            for (int y = 2021; y <= fiscalyear; y++)
            {
                ListItem li = new ListItem();
                li.Text = y.ToString();
                li.Value = y.ToString();
                ddlFiscalYear.Items.Add(li);
            }
            ddlFiscalYear.SelectedValue = fiscalyear.ToString();
        }
        

        private async void LoadSettings(short fYear)
        {
            try
            {
                FGApplicationSettings result = null;
                result = await fgService.GetFireGrantAppSettings(fYear);
                if (result != null)
                {
                    hfProgramSettings.Value = result.AppSettingsId.ToString();
                    txtStartDate.Text = result.StartDate.ToString("yyyy-MM-dd");
                    txtEndDate.Text = result.EndDate.ToString("yyyy-MM-dd");
                    txtMaxGrant.Text = result.MaxGrantAmount.ToString();
                    txtEsigText.Text = result.eSignatureLegalText;
                    if (result.ApplicationInstructions != null)
                    {
                        rtbPageContent.Content = result.ApplicationInstructions;
                    }
                    else
                    {
                        rtbPageContent.Content = "";
                    }
                    if (result.DefaultPageContent != null)
                    {
                        rtbDefaultPageContent.Content = result.DefaultPageContent;
                    }
                    else
                    {
                        rtbDefaultPageContent.Content = noDefaultPageContent();
                    }
                    if (result.DefaultPageHeader != null)
                    {
                        rtbDefaultPageHeader.Content = result.DefaultPageHeader;
                    }
                    else
                    {
                        rtbDefaultPageHeader.Content = noDefaultPageHeader();
                    }
                    if (result.EligibilityRequirementsText != null)
                    {
                        rtbEligibilityRequirements.Content = result.EligibilityRequirementsText;
                    }
                    else
                    {
                        string requirements = fgService.GetDefaultEligibilityRequirements();
                        rtbEligibilityRequirements.Content = requirements;
                    }
                    if (result.EligibilityDocumentName != null)
                    {
                        string strFileName;
                        string strFilePath = "";
                        string strFolder;
                        strFolder = Server.MapPath("./Documents/" + result.FiscalYear.ToString() + "/");
                        strFileName = result.EligibilityDocumentName;
                        strFileName = Path.GetFileName(strFileName);
                        strFilePath = strFolder + strFileName;
                        if (System.IO.File.Exists(strFilePath))
                        {
                            hlnkDocument.Text = result.EligibilityDocumentName;
                            hlnkDocument.NavigateUrl = "~/Admin/Documents/" + result.FiscalYear.ToString() + "/" + strFileName;
                            hlnkDocument.Text = strFileName;
                            hlnkDocument.Target = "_blank";
                            hlnkDocument.Visible = true;
                        }
                    }
                    else
                    {
                        hlnkDocument.Text = "";
                        hlnkDocument.NavigateUrl = "";
                        hlnkDocument.Text = "";
                        hlnkDocument.Target = "";
                        hlnkDocument.Visible = false;
                        //lnkReqDoc.Text = "";
                        //lnkReqDoc.Visible = false;
                    }
                    if (result.PumpTestStatute != null)
                    {
                        rtbPumpTestStatute.Content = result.PumpTestStatute;
                    }
                    else
                    {
                        rtbPumpTestStatute.Content = defaultPumpTestStatute();
                    }
                    if (result.HoseTestStatute != null)
                    {
                        rtbHoseTestStatute.Content = result.HoseTestStatute;
                    }
                    else
                    {
                        rtbHoseTestStatute.Content = defaulHoseTestStatute();
                    }
                    if (result.faCertifiationText != null)
                    {
                        txtFACertification.Text = result.faCertifiationText;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                FGApplicationSettings result = new FGApplicationSettings();
                result.AppSettingsId = new Guid(hfProgramSettings.Value.ToString());
                result.FiscalYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                result.StartDate = Convert.ToDateTime(txtStartDate.Text);
                result.EndDate = Convert.ToDateTime(txtEndDate.Text);
                result.MaxGrantAmount = Convert.ToDecimal(txtMaxGrant.Text.Replace(",", ""));
                result.eSignatureLegalText = txtEsigText.Text;
                result.faCertifiationText = txtFACertification.Text;

                if (await fgService.UpdateFireGrantMainSettings(result) == true)
                {
                    dvError.InnerHtml = "<div class='alert alert-success'>The Program Settings have been saved</div>";
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected void ddlFiscalYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                short fYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                LoadSettings(fYear);
                ddlFiscalYear.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected async void btnSaveReqDoc_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuReqDoc.PostedFiles.Count != 0)
                {
                    bool isCorrectFormat = false;
                    if (fuReqDoc.PostedFile.ContentType.ToString() == "image/jpeg" | fuReqDoc.PostedFile.ContentType.ToString() == "image/png" | fuReqDoc.PostedFile.ContentType.ToString() == "image/bmp" | fuReqDoc.PostedFile.ContentType.ToString() == "application/pdf" | fuReqDoc.PostedFile.ContentType.ToString().Contains("word"))
                    {
                        isCorrectFormat = true;
                    }
                    if (isCorrectFormat == false)
                    {
                        throw new Exception("Document must be in the format of .doc, .docx, .pdf, .jpg, .png or .bmp<br />");
                    }
                }
                else
                {
                    throw new Exception("You must select a document to upload");
                }
                FGApplicationSettings result = new FGApplicationSettings();
                result.AppSettingsId = new Guid(hfProgramSettings.Value.ToString());
                result.FiscalYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                result.StartDate = Convert.ToDateTime(txtStartDate.Text);
                result.EndDate = Convert.ToDateTime(txtEndDate.Text);
                result.MaxGrantAmount = Convert.ToDecimal(txtMaxGrant.Text.Replace(",", ""));
                result.EligibilityDocumentName = fuReqDoc.PostedFile.FileName;


                //HttpPostedFile file;
                byte[] fileData = null;
                //string theextension = null;
                //if (fuReqDoc.PostedFiles.Count != 0)
                //{
                //    file = fuReqDoc.PostedFiles[0];
                //    fileData = new byte[file.InputStream.Length];
                //    file.InputStream.Read(fileData, 0, Convert.ToInt32(file.InputStream.Length));
                //    theextension = file.ContentType;
                //}
                //else
                //{
                //    fileData = null;
                //    theextension = string.Empty;
                //}
                result.EligibilityDocument = fileData;

                if (await fgService.UpdateFireGrantMainSettings(result) == true)
                {
                    //lnkReqDoc.Text = result.EligibilityDocumentName;
                    //
                    string strFileName;
                    string strFilePath = "";
                    string strFolder;
                    bool uploaded = true;
                    strFolder = Server.MapPath("./Documents/" + result.FiscalYear.ToString() + "/");
                    // Retrieve the name of the file that is posted.
                    strFileName = fuReqDoc.PostedFile.FileName;
                    strFileName = Path.GetFileName(strFileName);
                    if (fuReqDoc.PostedFile != null)
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
                            fuReqDoc.PostedFile.SaveAs(strFilePath);
                            //lblReqDoc.Text = strFileName + " has been successfully uploaded.";
                        }
                    }
                    else
                    {
                        throw new Exception("Please select a document to upload.");
                    }
                    if (uploaded)
                    {
                        hlnkDocument.NavigateUrl = "~/Admin/Documents/" + result.FiscalYear.ToString() + "/" + strFileName;
                        hlnkDocument.Text = strFileName;
                        hlnkDocument.Target = "_blank";
                        hlnkDocument.Visible = true;
                        //lblReqDoc.Text = "<a href='" + strFilePath.ToString() + "' target='_blank'>" + strFileName + "</a>";
                        dvError.InnerHtml = "<div class='alert alert-success'>The Requirements Document has been saved</div>";
                    }

                    // Display the result of the upload.'
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected async void btnSavePageContent_Click(object sender, EventArgs e)
        {
            try
            {
                FGApplicationSettings result = new FGApplicationSettings();
                result.AppSettingsId = new Guid(hfProgramSettings.Value.ToString());
                result.FiscalYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                result.StartDate = Convert.ToDateTime(txtStartDate.Text);
                result.EndDate = Convert.ToDateTime(txtEndDate.Text);
                result.MaxGrantAmount = Convert.ToDecimal(txtMaxGrant.Text.Replace(",", ""));
                result.ApplicationInstructions = rtbPageContent.Content;

                if (await fgService.UpdateFireGrantMainSettings(result) == true)
                {
                    dvError.InnerHtml = "<div class='alert alert-success'>The Initial Page Content has been saved</div>";
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected async void lnkReqDoc_Click(object sender, EventArgs e)
        {
            try
            {
                FGApplicationSettings result = null;
                short fYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                result = await fgService.GetFireGrantAppSettings(fYear);
                if (result != null)
                {
                    //byte[] bytes;
                    //string fileName, contentType;
                    //bytes = (byte[])result.EligibilityDocument;
                    ////contentType = docs.Rows[0]["ContentType"].ToString();
                    //fileName = result.EligibilityDocumentName;
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    ////Response.ContentType = contentType;
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    //Response.BinaryWrite(bytes);
                    //Response.Flush();
                    //Response.End();

                    
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected async void btnSaveDefaultPageContent_Click(object sender, EventArgs e)
        {
            try
            {
                FGApplicationSettings result = new FGApplicationSettings();
                result.AppSettingsId = new Guid(hfProgramSettings.Value.ToString());
                result.FiscalYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                result.StartDate = Convert.ToDateTime(txtStartDate.Text);
                result.EndDate = Convert.ToDateTime(txtEndDate.Text);
                result.MaxGrantAmount = Convert.ToDecimal(txtMaxGrant.Text.Replace(",", ""));
                result.DefaultPageContent = rtbDefaultPageContent.Content;
                result.DefaultPageHeader = rtbDefaultPageHeader.Content;

                if (await fgService.UpdateFireGrantMainSettings(result) == true)
                {
                    dvError.InnerHtml = "<div class='alert alert-success'>The Default Page Content has been saved</div>";
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        private string noDefaultPageHeader()
        {
            string defaultPageHeader = "<h1>FY - Online Fire Protection Grant Application ( OFPGA )</h1><p class='lead'>*For questions and technical support regarding the application, departments should initially contact their SFMO Fire Support Inspector.</p>";
            return defaultPageHeader;

        }
        private string noDefaultPageContent()
        {
            string defaultPageContent = "";
            defaultPageContent += "<h2>Welcome to the Online New Mexico Fire Protection Grant Application</h2>";
            defaultPageContent += "<p>This web-app is in response to feedback from many New Mexico departments for a more user - friendly process.Your continued patience and understanding is appreciated as we work to improve the process and serve you better.</p>";
            defaultPageContent += "<p>To begin, enter your department’s five digit NERIS ID number in both the NERIS ID number field and the Password field. Upon logon, in the General Information page, you will be prompted to provide an email address and may change the password. Please note: Only one application per department will be accepted; therefore only one email address and password per department will be recognized.</p>";
            defaultPageContent += "<p style='text-decoration:underline'>Please read the eligibility requirements on the Welcome Page carefully before completing the application.</p>";
            defaultPageContent += "<p>To assist in tracking completion of the application, the status is shown in then gray shaded area to the right of the application. A green checkmark <img src='Content/images/tick.png' /> tick indicates the section has been opened and started. Due to the varied responses, however, It does not necessarily indicate that the section is complete. A red cross <img src='Content/images/cross.png' /> indicates that there is required information that has not been completed. A circle <img src='Content/images/round.png' /> with an empty in the center indicates the section has not yet been started.</p>";
            defaultPageContent += "<p>Should you have technical questions or experience problems navigating through the application, click on the Technical Support link located in the gray shaded banner at the top of each page, describe the problem, and click SEND. Your question will be answered within 2 business days.</p>";
            defaultPageContent += "<p>Should you have questions specific to the content requirements of the application, click on the SFMO Fire Services Support Team link located in the gray shaded banner at the top of each page, describe the question, and click SEND. Your question will be answered within 2 business days.</p>";
            defaultPageContent += "<p>You are now ready to begin entering the NM Fire Protection Grant application.</p>";
            defaultPageContent += "<p><a class='btn btn-default' href='https://www.nmdhsem.org/state-firemarshal/fire-grant-council/#grant' target='_blank'>New Mexico State Fire Marshal Fire Grant Council &raquo;</a></p";
            return defaultPageContent;
        }

        private string defaultPumpTestStatute()
        {
            string pumpTestStatute = "<p style='font-weight: bold'>Please update the pump test requirements in admin</p>";
            return pumpTestStatute;
        }

        private string defaulHoseTestStatute()
        {
            string hoseTestStatute = "<p style='font-weight: bold'>Please update the hose test requirements in admin</p>";
            return hoseTestStatute;
        }


        protected async void btnSaveTestStatutes_Click(object sender, EventArgs e)
        {
            try
            {
                FGApplicationSettings result = new FGApplicationSettings();
                result.AppSettingsId = new Guid(hfProgramSettings.Value.ToString());
                result.FiscalYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                result.StartDate = Convert.ToDateTime(txtStartDate.Text);
                result.EndDate = Convert.ToDateTime(txtEndDate.Text);
                result.MaxGrantAmount = Convert.ToDecimal(txtMaxGrant.Text.Replace(",", ""));
                result.PumpTestStatute = rtbPumpTestStatute.Content;
                result.HoseTestStatute = rtbHoseTestStatute.Content;

                if (await fgService.UpdateFireGrantMainSettings(result) == true)
                {
                    dvError.InnerHtml = "<div class='alert alert-success'>The Pump/Hose Test statutes have been saved</div>";
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected async void btnSaveEligibilityRequirements_Click(object sender, EventArgs e)
        {
            try
            {
                FGApplicationSettings result = new FGApplicationSettings();
                result.AppSettingsId = new Guid(hfProgramSettings.Value.ToString());
                result.FiscalYear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                result.StartDate = Convert.ToDateTime(txtStartDate.Text);
                result.EndDate = Convert.ToDateTime(txtEndDate.Text);
                result.MaxGrantAmount = Convert.ToDecimal(txtMaxGrant.Text.Replace(",", ""));
                result.EligibilityRequirementsText = rtbEligibilityRequirements.Content;

                if (await fgService.UpdateFireGrantMainSettings(result) == true)
                {
                    dvError.InnerHtml = "<div class='alert alert-success'>The Eligibility Requirements have been saved</div>";
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-error'>" + ex.Message.ToString() + "</div>";
                dvError.Focus();
            }
        }

        protected void btnSaveCertificationText_Click(object sender, EventArgs e)
        {

        }
    }
}

