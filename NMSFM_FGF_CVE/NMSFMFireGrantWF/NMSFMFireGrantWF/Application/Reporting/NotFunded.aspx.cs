using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
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
using NMSFM.Services.FireGrant;
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Flow.Model;

namespace NMSFMFireGrantWF.Application.Reporting
{
    public partial class NotFunded : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
// private IAccountService accountService; // legacy field, currently unused
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;
        private IUDFService udfService;

        protected void Page_Init(object sender, EventArgs e)
        {
            logger = new Logging();
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fpfService = new FPFService(userContext, logger);
                this.fgAppService = new FGApplicationService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.udfService = new UDFService(userContext, logger);

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
                if (Session["Role"] == null)
                {
                    Response.Redirect("~/Unauthorized");
                }
                if (Session["Department"] == null)
                {
                    if (Session["Role"].ToString() == "Internal")
                    {
                        Response.Redirect("~/Admin/Home");
                    }
                    else if (Session["Role"].ToString() == "External")
                    {
                        Response.Redirect("~/User/Home");
                    }
                    else
                    {
                        Response.Redirect("~/Unauthorized");
                    }
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        lblInvoiceNumber.Text = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString();
                        await GetAgencyUDFS();
                        await LoadDepartment();
                        await GetAppInfo(appId);
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                //dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private async Task<bool> GetAgencyUDFS()
        {
            Guid agencyId = new Guid(Session["AgencyId"].ToString());
            Guid fieldId;

            fieldId = new Guid("553660a1-0724-4112-9acd-cb6b4ac1c621");
            string strGovernor = (await udfService.GetUDFValueAsync(fieldId, agencyId)).ToString();  //Header Info Agency UDFs
            lblGovernor.Text = strGovernor;
            fieldId = new Guid("08356792-0743-49c4-88c3-e649321deb32");
            string strCabinetSec = (await udfService.GetUDFValueAsync(fieldId, agencyId)).ToString();  //Header Info Agency UDFs
            lblCabinetSec.Text = strCabinetSec;
            fieldId = new Guid("40014969-f84e-4a26-a63a-8f50365c7df4");
            string strDeputyCabinetSec = (await udfService.GetUDFValueAsync(fieldId, agencyId)).ToString(); //Header Info Agency UDFs
            if (strDeputyCabinetSec != "")
            {
                dvDeputyCabinetSec.Visible = true;
                lblDeputyCabinetSec.Text = strDeputyCabinetSec;
            }
            else
            {
                dvDeputyCabinetSec.Visible = false;
            }
            fieldId = new Guid("efd387ef-26f4-4487-9194-9a7e656000c3");
            string strDeputyCabinetSec2 = (await udfService.GetUDFValueAsync(fieldId, agencyId)).ToString(); //Header Info Agency UDFs
            if (strDeputyCabinetSec2 != "")
            {
                dvDeputyCabinetSec2.Visible = true;
                lblDeputyCabinetSec2.Text = strDeputyCabinetSec2;
            }
            else
            {
                dvDeputyCabinetSec2.Visible = false;
            }
            fieldId = new Guid("ed078cdf-7ae7-46d0-93e1-69e14caeec82");
            string strFireMarshalName = (await udfService.GetUDFValueAsync(fieldId, agencyId)).ToString();  //Header Info Agency UDFs
            lblFireMarshal.Text = strFireMarshalName;
            lblFireMarshal2.Text = strFireMarshalName;
            return true;
        }

        private async Task<bool> LoadDepartment()
        {
            try
            {
                var department = new v_AddressParties();
                Guid deptId = new Guid(Session["Department"].ToString());
                department = await fgService.GetFGDepartmentByIdAsync(deptId);

                if (department != null)
                {
                    //tdDepartmentName.InnerHtml = department.AddressCode;
                    //spDepartmentName2.InnerHtml = department.AddressCode;
                    string addressDesc = "";
                    string addressCityStateZip = "";
                    if (department.AddressNumber != null && department.AddressNumber != "")
                    {
                        addressDesc += department.AddressNumber;
                    }
                    if (department.Direction != null)
                    {
                        addressDesc += " " + department.Direction;
                    }
                    if (department.Address != null)
                    {
                        addressDesc += " " + department.Address;
                    }
                    if (department.Suffix != null)
                    {
                        addressDesc += " " + department.Suffix;
                    }
                    addressCityStateZip = department.City + ", " + department.StateAbbr + " " + department.Zip;
                    string County = (await addressService.GetCountyListAsync()).First(c => c.CountyId == department.CountyId).County1;

                    lblDate.Text = DateTime.Now.ToLongDateString();
                    spDepartment.InnerHtml = department.AddressCode;
                    spAddressDesc.InnerHtml = addressDesc;
                    spCityStateZip.InnerHtml = addressCityStateZip;

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }

        private async Task<bool> GetAppInfo(string appId)
        {
            try
            {
                Guid appIdGuid = new Guid(appId);
                FGApplications app = new FGApplications();
                app = await fgAppService.GetFGApplicationById(appIdGuid);
                short fiscalYear = app.FiscalYear;
                FGApplicationSettings appSettings = new FGApplicationSettings();
                appSettings = await fgService.GetFireGrantAppSettings(fiscalYear);
                GrantYearStats stats = new GrantYearStats();
                stats = await fgAppService.GetGrantYearStats(fiscalYear);
                spFY.InnerHtml = fiscalYear.ToString();
                spGrantApps.InnerHtml = stats.NumApps.ToString();
                spGrantAmounts.InnerHtml = "$" + Math.Round(stats.FundingRequested / 1000000, 1).ToString();
                spGrantsAwarded.InnerHtml = "$" + Math.Round(stats.GrantsAwarded / 1000000, 1).ToString();
                //dvDenialReason.InnerHtml = app.ApplicationNotes;
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Application/Instructions");
        }

        protected void btnSavePDF_Click(object sender, EventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                var sbHead = new StringBuilder();
                dvLetter.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                repHead.RenderControl(new HtmlTextWriter(new StringWriter(sbHead)));

                string head = "<style>body {max-width: 800px;margin:auto;}h1 {font-size:1.6em;font-weight:bold;}.toolbar ul {list-style-type: none;margin: 0;padding: 0;overflow: hidden;background-color: #333;}.toolbar li {float: left;display: inline;text-decoration: none;}.toolbar li a {display: block;color: white;text-align: center;padding: 14px 16px;text-decoration: none;}.toolbar li a:hover {background-color: #111;}@media print {.toolbar {display:none !important;}}.row{width:100%;margin-right:0px;margin-left:0px; clear:both}.col-md-12,.col-md-3,.col-md-6{position:relative;min-height:1px;padding-right:0px;padding-left:0px}.col-md-12,.col-md-3,.col-md-6{float:left}.col-md-12{width:100%}.col-md-6{width:50%}.col-md-3{width:33%}</style>";
                string body = sb.ToString();
                string htmlContent = "<!DOCTYPE html><html><head>" + head + "</head><body>" + body + "</body></html>";

                htmlContent = htmlContent.Replace("<div style='page-break-before:always'></div>", "<b>[PAGEBREAK]</b>");

                Telerik.Windows.Documents.Flow.FormatProviders.Html.HtmlFormatProvider htmlProvider = new Telerik.Windows.Documents.Flow.FormatProviders.Html.HtmlFormatProvider();
                // Create a document instance from the content. 
                RadFlowDocument document = htmlProvider.Import(htmlContent);

                foreach (var section in document.Sections)
                {
                    //section.PageSize = new System.Windows.Size(1600, 2000);
                    section.PageMargins = new Telerik.Windows.Documents.Primitives.Padding(25, 25, 25, 25);
                }

                InsertPageBreak(document);

                Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider pdfProvider = new Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider();

                // Export the document. The different overloads enables you to export to a byte[] or to a Stream. 
                byte[] pdfBytes = pdfProvider.Export(document);

                string contentType = "pdf";
                //ToDo Update this string
                //string fileName = "NMSFM Fire Grant Application (" + tdDepartmentName.InnerText + "_" + spFiscalYear.InnerText + ").pdf";
                string fileName = "NMSFM Fire Grant Not Funded Letter (" + spDepartment.InnerText + ").pdf";
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = contentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        private void InsertPageBreak(RadFlowDocument document)
        {
            foreach (var fieldCharacter in document.EnumerateChildrenOfType<Paragraph>().ToArray())
            {
                foreach (var inline in fieldCharacter.Inlines.ToList())
                {
                    if (((inline is InlineBase)) && ((inline as Run) != null) && (((Run)inline).Text == "[PAGEBREAK]"))
                    {
                        var index = fieldCharacter.Inlines.IndexOf(inline);
                        var breakPage = new Break(document);
                        breakPage.BreakType = BreakType.PageBreak;

                        fieldCharacter.Inlines.Insert(index, breakPage);
                        fieldCharacter.Inlines.RemoveAt(index + 1);
                    }
                }
            }
        }
    }
}






