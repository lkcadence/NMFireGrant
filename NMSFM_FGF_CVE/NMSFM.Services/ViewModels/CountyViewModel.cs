using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class CountyViewModel
    {
        public Guid CountyId { get; set; }

        public string County { get; set; }

        public string CountyCode { get; set; }

        public Guid StateId { get; set; }
    }
}