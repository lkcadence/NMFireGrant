using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;


namespace NMSFM.ViewModels
{
    public class DetailedFGAppReview
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int NFIRSCompliant { get; set; }
        public int PumpTestCompliant { get; set; }
        public int HoseTestCompliant { get; set; }
        public int AckComSigs { get; set; }
        public int SpecsReceived { get; set; }
        public string Notes { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
        public FG_App_Signatures ReviewerSignature { get; set; }
        public List<FG_App_Signatures> AppSignatures { get; set; }

        public int NERISCompliant
        {
            get { return NFIRSCompliant; }
            set { NFIRSCompliant = value; }
        }
    }
}
