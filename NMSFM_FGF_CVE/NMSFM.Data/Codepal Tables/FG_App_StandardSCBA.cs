using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_StandardSCBA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid StandardComplientSCBAId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
        public int Age { get; set; }
        public string Condition { get; set; }
        public string SCBAType { get; set; }
    }
}
