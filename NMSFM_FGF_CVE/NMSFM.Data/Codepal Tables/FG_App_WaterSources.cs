using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_WaterSources
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid WaterSourceId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Number { get; set; }
        public string WaterSource { get; set; }
        public int Capacity { get; set; }
    }
}
