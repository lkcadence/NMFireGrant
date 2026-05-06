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
    public partial class GrantAwarded : System.Web.UI.Page
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
                        lblInvoiceNumber2.Text = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString();
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
                    spDepartment2.InnerHtml = department.AddressCode;
                    spAddressDesc.InnerHtml = addressDesc;
                    spCityStateZip.InnerHtml = addressCityStateZip;
                    spCounty.InnerHtml = County;

                    var trAddress = new v_Addresses2();
                    if (department.SubAddress.ToUpper() == "CITY")
                    {
                        trAddress = await fgAppService.GetRemittanceAddress(true, department.AddressCode);
                        spRemit1.InnerHtml = "Remit To: " + department.AddressCode + "<br />";
                        if (trAddress != null)
                        {
                            if (trAddress.Comment != null && trAddress.Comment != "")
                            {
                                spRemit2.InnerHtml = trAddress.Comment + "<br />";
                            }
                            else
                            {
                                spRemit2.Visible = false;
                            }
                            if (trAddress.AddressNumber != null && trAddress.AddressNumber != "")
                            {
                                string remitAdd = "";
                                remitAdd = trAddress.AddressNumber + "&nbsp;";
                                if (trAddress.Direction != null && trAddress.Direction != "")
                                {
                                    remitAdd += trAddress.Direction + "&nbsp;";
                                }
                                if (trAddress.Address != null && trAddress.Address != "")
                                {
                                    remitAdd += trAddress.Address + "&nbsp;";
                                }
                                if (trAddress.Suffix != null && trAddress.Suffix != "")
                                {
                                    remitAdd += trAddress.Direction + "&nbsp;";
                                }
                                spRemit3.InnerHtml = remitAdd + "<br />";
                            }
                            else if (trAddress.SubAddress != null && trAddress.SubAddress != "")
                            {
                                spRemit3.InnerHtml = trAddress.SubAddress + "<br />";
                            }
                            string cityStateZip = "";
                            if (trAddress.City != null && trAddress.City != "")
                            {
                                cityStateZip += trAddress.City + ",&nbsp;";
                            }
                            if (trAddress.StateAbbr != null && trAddress.StateAbbr != "")
                            {
                                cityStateZip += trAddress.StateAbbr + "&nbsp;";
                            }
                            if (trAddress.Zip != null && trAddress.Zip != "")
                            {
                                cityStateZip += trAddress.Zip + "&nbsp;";
                            }
                            spRemit4.InnerHtml = cityStateZip;
                        }
                        
                    }
                    else
                    {
                        trAddress = await fgAppService.GetRemittanceAddress(false, County);
                        spRemit1.InnerHtml = "Remit To: " + County + "<br />";
                        if (trAddress != null)
                        {
                            if (trAddress.Comment != null && trAddress.Comment != "")
                            {
                                spRemit2.InnerHtml = trAddress.Comment + "<br />";
                            }
                            else
                            {
                                spRemit2.Visible = false;
                            }
                            if (trAddress.AddressNumber != null && trAddress.AddressNumber != "")
                            {
                                string remitAdd = "";
                                remitAdd = trAddress.AddressNumber + "&nbsp;";
                                if (trAddress.Direction != null && trAddress.Direction != "")
                                {
                                    remitAdd += trAddress.Direction + "&nbsp;";
                                }
                                if (trAddress.Address != null && trAddress.Address != "")
                                {
                                    remitAdd += trAddress.Address + "&nbsp;";
                                }
                                if (trAddress.Suffix != null && trAddress.Suffix != "")
                                {
                                    remitAdd += trAddress.Direction + "&nbsp;";
                                }
                                spRemit3.InnerHtml = remitAdd + "<br />";
                            }
                            else if (trAddress.SubAddress != null && trAddress.SubAddress != "")
                            {
                                spRemit3.InnerHtml = trAddress.SubAddress + "<br />";
                            }
                            string cityStateZip = "";
                            if (trAddress.City != null && trAddress.City != "")
                            {
                                cityStateZip += trAddress.City + ",&nbsp;";
                            }
                            if (trAddress.StateAbbr != null && trAddress.StateAbbr != "")
                            {
                                cityStateZip += trAddress.StateAbbr + "&nbsp;";
                            }
                            if (trAddress.Zip != null && trAddress.Zip != "")
                            {
                                cityStateZip += trAddress.Zip + "&nbsp;";
                            }
                            spRemit4.InnerHtml = cityStateZip;

                        }

                    }

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
                FG_App_ProjectBudget projectBudget = new FG_App_ProjectBudget();
                projectBudget = await fgAppService.GetFGApplicationProjectBudgetAsync(appIdGuid);
                DetailedFGAppEquipmentNeeds equipmentNeeded = new DetailedFGAppEquipmentNeeds();
                equipmentNeeded = await fgAppService.GetFGApplicationEquipmentNeedsAsync(appIdGuid);
                FG_App_GeneralInfo generalInfo = new FG_App_GeneralInfo();
                generalInfo = await fgAppService.GetFGApplicationGeneralInfoAsync(appIdGuid);

                decimal grantedAmount = app.GrantedAmount;
                decimal stipendAmount = app.StipendAmount;
                spFY.InnerHtml = fiscalYear.ToString();
                if (grantedAmount == 0)
                {
                    spDepartment2.InnerHtml = spDepartment2.InnerHtml + " for Stipends";
                    dvGrantApps.Visible = false;
                    dvGrantAmount.InnerHtml = "A voucher or ACH deposit, in the amount " + String.Format("{0:C}", projectBudget.StipendAmount) + " for Stipends after approval by this office will be sent to your local governing body Treasurer.";
                    dvDeadline.Visible = false;
                    dvDeadline2.Visible = false;
                    dvPage2.Visible = false;
                }
                else
                {
                    spGrantApps.InnerHtml = stats.NumApps.ToString();
                    spGrantAmounts.InnerHtml = Math.Round(stats.FundingRequested / 1000000, 1).ToString();
                    spGrantAmount.InnerHtml = String.Format("{0:C}", grantedAmount);
                    spGrantAmount2.InnerHtml = String.Format("{0:C}", grantedAmount);
                    spStipendAmount.InnerHtml = String.Format("{0:C}", stipendAmount);
                    spEncumberYear.InnerHtml = (DateTime.Now.Year + 1).ToString();
                    //Removed 10/13/2023 (vwd)
                    //spProject.InnerHtml = equipmentNeeded.SpecificNeeds;
                    //spCklFiscalYear1.InnerHtml = fiscalYear.ToString();
                    //spCklFiscalYear2.InnerHtml = fiscalYear.ToString();
                    spCklFiscalYear3.InnerHtml = fiscalYear.ToString();
                    //spAwardDate.InnerHtml = "November 15, " + (fiscalYear - 1).ToString();
                }
                
                if (generalInfo.IndividualDept == 2)
                {
                    dvCountyTable.Visible = true;
                    await LoadCountyAppInfo(generalInfo.County, fiscalYear);
                }
                //Added 10/12/2023 to fill equipment needs
                DetailedFGAppEquipmentNeeds equipmentNeeds = new DetailedFGAppEquipmentNeeds();
                equipmentNeeds = await fgAppService.GetFGApplicationEquipmentNeedsAsync(appIdGuid);
                if (equipmentNeeds != null && equipmentNeeds.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    string projectApparatus = "";
                    string projectEquipment = "";
                    foreach (FG_App_ApplicationEquipment equip in equipmentNeeds.ApplicationEquipment)
                    {
                        if (!projectApparatus.Contains(equip.PriorityCategory))
                        {
                            projectApparatus = projectApparatus + equip.PriorityCategory + "; ";
                        }
                        if (!projectEquipment.Contains(equip.EquipmentNeeded))
                        {
                            projectEquipment = projectEquipment + equip.EquipmentNeeded + "; ";
                        }
                    }
                    if (projectEquipment.Length > 2)
                    { 
                        projectEquipment = projectEquipment.Substring(0, projectEquipment.Length - 2);
                    }
                    if (projectApparatus.Length > 2)
                    {
                        projectApparatus = projectApparatus.Substring(0, projectApparatus.Length - 2);
                    }
                    spProjectApparatus.InnerHtml = projectApparatus;
                    spProjectEquipment.InnerHtml = projectEquipment;
                }

                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                return false;
            }
        }

        private async Task<bool> LoadCountyAppInfo(string county, int fiscalYear)
        {
            try
            {
                List<nm_FGApplication> countyApps = new List<nm_FGApplication>();
                countyApps = await fgAppService.GetFGApplicationByCounty(county, fiscalYear);
                decimal totalGrant = 0;
                decimal totalStipends = 0;
                if (countyApps.Count > 0)
                {
                    string tableRows = "";
                    foreach (nm_FGApplication app in countyApps)
                    {
                        decimal grantedAmount = 0;
                        decimal stipendAmount = 0;
                        
                        
                        
                        tableRows += "<tr>";
                        tableRows += "<td>" + app.AddressCode + "</td>";
                        if (app.SpecificNeeds.Length > 50)
                        {
                            tableRows += "<td>" + app.SpecificNeeds.Substring(0, 50) + "...</td>";
                        }
                        else
                        {
                            tableRows += "<td>" + app.SpecificNeeds + "</td>";
                        }

                        if (app.GrantedAmount.HasValue)
                        {
                            totalGrant += app.GrantedAmount ?? 0;
                            grantedAmount = app.GrantedAmount ?? 0;
                        }
                        if (app.StipendAmount.HasValue)
                        {
                            totalStipends += app.StipendAmount ?? 0;
                            stipendAmount = app.StipendAmount ?? 0;
                        }
                        tableRows += "<td>" + String.Format("{0:C}", grantedAmount) + "</td>";
                        tableRows += "<td>" + String.Format("{0:C}", stipendAmount) + "</td>";
                        tableRows += "</tr>";
                    }
                    tableRows += "<tr style='font-weight:bold'><td>Total:</td><td></td><td>" + String.Format("{0:C}", totalGrant) + "</td><td>" + String.Format("{0:C}", totalStipends) + "</td></tr>";
                    CountyTableBody.InnerHtml = tableRows;
                }

                spGrantAmount.InnerHtml = String.Format("{0:C}", totalGrant);
                spGrantAmount2.InnerHtml = String.Format("{0:C}", totalGrant);
                spStipendAmount.InnerHtml = String.Format("{0:C}", totalStipends);

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

                string head = "<style>body {max-width: 800px;margin:auto;font-size:1em;}h1 {font-size:1.6em;font-weight:bold;}.toolbar ul {list-style-type: none;margin: 0;padding: 0;overflow: hidden;background-color: #333;}.toolbar li {float: left;display: inline;text-decoration: none;}.toolbar li a {display: block;color: white;text-align: center;padding: 14px 16px;text-decoration: none;}.toolbar li a:hover {background-color: #111;}@media print {.toolbar {display:none !important;}}.row{width:100%;margin-right:0px;margin-left:0px; clear:both}.col-md-12,.col-md-3,.col-md-6{position:relative;min-height:1px;padding-right:0px;padding-left:0px}.col-md-12,.col-md-3,.col-md-6{float:left}.col-md-12{width:100%}.col-md-6{width:50%}.col-md-3{width:33%}table, th, td {border: 1px solid;border-collapse:collapse;padding:3px 3px 3px 3px;}</style>";
                string body = sb.ToString();
                string htmlContent = "<!DOCTYPE html><html><head>" + head + "</head><body>" + body + "</body></html>";

                htmlContent = htmlContent.Replace("<div style='page-break-before:always'></div>", "<b>[PAGEBREAK]</b>");
                htmlContent = htmlContent.Replace("<div style='page-break-after:always'></div>", "<b>[PAGEBREAK]</b>");

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
                string fileName = "NMSFM Fire Grant Award Letter (" + spDepartment.InnerText + ").pdf";
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






