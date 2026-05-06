using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NMSFM.ViewModels
{
    public class SearchParty
    {
        public Guid PartyId { get; set; }
        public string PartyName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Cell { get; set; }
        public string Pager { get; set; }
        public string RoleType { get; set; }
        public string Comment { get; set; }
    }
}