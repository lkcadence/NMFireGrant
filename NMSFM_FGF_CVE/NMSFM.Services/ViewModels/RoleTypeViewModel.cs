using System;

namespace NMSFM.ViewModels
{
    public class RoleTypeViewModel
    {
        public Guid RoleTypeId { get; set; }

        public string RoleType { get; set; }

        public Guid? AgencyId { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}