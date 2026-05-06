using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_ApparatusEquipment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid ApparatusId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Number { get; set; }
        public string ApparatusName { get; set; }
        public string VehicleType { get; set; }
        public int Year { get; set; }
        public string VIN { get; set; }
        public string License { get; set; }
        public int Capacity { get; set; }
        public int GPM { get; set; }
        public DateTime? TestDate { get; set; }
        public bool? Pass { get; set; }
        public string Comments { get; set; }
    }
}
