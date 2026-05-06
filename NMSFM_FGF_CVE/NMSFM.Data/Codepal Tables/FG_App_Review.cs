using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    public partial class FG_App_Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
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
        [DefaultValue(false)]
        public string InvalidText { get; set; }
    }
}
