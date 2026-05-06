using System;

namespace NMSFM.ViewModels
{
    public class SignatureTypeViewModel
    {
        public Guid SignatureTypeId { get; set; }
        public string SignatureType { get; set; }
        public bool PreserveOnReopen { get; set; }
        public Guid? AgencyId { get; set; }
        public string ModuleId { get; set; }
        public string SignatureLegalText { get; set; }        
    }
}