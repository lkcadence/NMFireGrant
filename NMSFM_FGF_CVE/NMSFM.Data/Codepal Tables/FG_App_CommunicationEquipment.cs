using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_CommunicationEquipment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid CommunicationEquipmentId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Number { get; set; }
        public string CommunicationEquipment { get; set; }
        public int CommunicationQty { get; set; }
    }
}
