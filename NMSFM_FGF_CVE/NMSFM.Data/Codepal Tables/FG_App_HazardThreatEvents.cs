using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;


namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_HazardThreatEvents
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid HazardId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Number { get; set; }
        public string HazardType { get; set; }
        public string HazardDetail { get; set; }
    }
}
