using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
    public class DetailedFGAppCommunityInfo
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string CommunityName { get; set; }
        public int NumberOfHomes { get; set; }
        public int NumberOfComm { get; set; }
        public int ResidentPopulation { get; set; }
        public int AidAgreements { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
        public List<FG_App_AidDistricts> AidDistricts { get; set; }
    }
}
