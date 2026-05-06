using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_ApplicationEquipment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid EquipmentId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Number { get; set; }
        public string PriorityCategory { get; set; }
        public string EquipmentNeeded { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
