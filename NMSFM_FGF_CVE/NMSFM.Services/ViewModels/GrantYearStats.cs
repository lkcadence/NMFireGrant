using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class GrantYearStats
    {
        public int FiscalYear { get; set; }
        public int NumApps { get; set; }
        public decimal FundingRequested { get; set; }
        public decimal GrantsAwarded { get; set; }
    }
}
