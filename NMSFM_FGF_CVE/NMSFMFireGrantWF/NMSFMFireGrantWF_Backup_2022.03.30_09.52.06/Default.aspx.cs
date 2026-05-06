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
                    short fYear = Convert.ToInt16(DateTime.Now.Year);
                    FGApplicationSettings result = null;
                    result = await fgService.GetFireGrantAppSettings(fYear);
                    if (result != null && (result.DefaultPageContent != null && result.DefaultPageContent != ""))
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
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}