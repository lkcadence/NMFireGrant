using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

namespace NMSFMFireGrantWF
{
    public partial class _Default : Page
    {
        private ILogging logger;
        private IFGService fgService;

        protected void Page_Init(object sender, EventArgs e)
        {
            logger = new Logging();
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                this.fgService = new FGService(userContext, logger);
            }
            else
            {
                var userConnection = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationModel"].ToString();
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                this.fgService = new FGService(userContext, logger);
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    short fy = Convert.ToInt16(DateTime.Now.Year);
                    if (DateTime.Now.Month >= 4)
                    {
                        fy += 1;
                    }
                    spFY.InnerHtml = fy.ToString();
                    FGApplicationSettings result = null;
                    result = await fgService.GetFireGrantAppSettings(fy);
                    if (result != null)
                    {
                        if (result.DefaultPageContent != null && result.DefaultPageContent != "")
                        {
                            dvApplicationDefaultContent.Visible = true;
                            dvDefaultContent.Visible = false;
                            dvApplicationDefaultContent.InnerHtml = result.DefaultPageContent;
                        }
                        else
                        {
                            dvApplicationDefaultContent.Visible = false;
                            dvDefaultContent.Visible = true;
                        }
                        if (result.DefaultPageHeader != null && result.DefaultPageHeader != "")
                        {
                            dvDefaultHeader.Visible = false;
                            dvDefaultHeaderApplication.Visible = true;
                            dvDefaultHeaderApplication.InnerHtml = result.DefaultPageHeader;
                        }
                        else
                        {
                            dvDefaultHeader.Visible = true;
                            dvDefaultHeaderApplication.Visible = false;
                        }
                    }
                    else
                    {
                        dvApplicationDefaultContent.Visible = false;
                        dvDefaultContent.Visible = true;
                        dvDefaultHeader.Visible = true;
                        dvDefaultHeaderApplication.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }
    }
}


