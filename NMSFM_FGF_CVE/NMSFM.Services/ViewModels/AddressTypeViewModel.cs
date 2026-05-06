using System;

namespace NMSFM.ViewModels
{
    public class AddressTypeViewModel
    {
        public Guid AddressTypeId { get; set; }

        public string AddressType { get; set; }

        public Guid? AgencyId { get; set; }

        public bool? AddCodeReadOnly { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}