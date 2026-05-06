using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NMSFM.Data
{
    public partial class FGApplicationSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid AppSettingsId { get; set; }
        public short FiscalYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MaxGrantAmount { get; set; }
        public string ApplicationInstructions { get; set; }
        public string DefaultPageContent { get; set; }
        public string DefaultPageHeader { get; set; }
        public byte[] EligibilityDocument { get; set; }
        public string EligibilityDocumentName { get; set; }
        public string EligibilityRequirementsText { get; set; }
        public string PumpTestStatute { get; set; }
        public string HoseTestStatute { get; set; }
        public string eSignatureLegalText { get; set; }
        public string faCertifiationText { get; set; }
    }
}
