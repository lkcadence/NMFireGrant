using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    public partial class FG_App_Communication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int CommunicationProject { get; set; }
        public int HandheldRadios { get; set; }
        public int BaseStations { get; set; }
        public int MobileRadios { get; set; }
        public int ApparatusWoRadio { get; set; }
        public int LawEnforcement { get; set; }
        public int EmergencyMedical { get; set; }
        public int OtherFireDepts { get; set; }
        public int Other { get; set; }
        public string OtherDescription { get; set; }
        public int AreasNotCovered { get; set; }
        public string DescribeAreasNotCovered { get; set; }
        public string AdminComments { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
    }
}
