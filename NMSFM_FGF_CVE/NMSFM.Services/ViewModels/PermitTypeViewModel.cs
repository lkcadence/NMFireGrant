using System;

namespace NMSFM.ViewModels
{
    public class PermitTypeViewModel
    {
        public Guid PermitTypeId { get; set; }

        public string PermitType { get; set; }

        public Guid? AgencyId { get; set; }
        
        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}