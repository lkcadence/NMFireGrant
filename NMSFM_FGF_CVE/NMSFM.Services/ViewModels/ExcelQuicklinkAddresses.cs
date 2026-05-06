using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class ExcelQuicklinkAddresses
    {
        public Guid AddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string AddressType { get; set; }
        public string Region { get; set; }
        public string Quicklink { get; set; }
    }
}