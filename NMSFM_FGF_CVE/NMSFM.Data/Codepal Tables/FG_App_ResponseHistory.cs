using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    public partial class FG_App_ResponseHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        [Column("NFIRSCurrent")]
        public int NERISCurrent { get; set; }
        public int ResponseStructure { get; set; }
        public int ResponseVehicle { get; set; }
        public int ResponseVegitation { get; set; }
        public int ResponseEMS { get; set; }
        public int ResponseRescue { get; set; }
        public int ResponseHazardous { get; set; }
        public int ResponseService { get; set; }
        public int ResponseGoodIntent { get; set; }
        public int ResponseFalse { get; set; }
        public int ResponseOther { get; set; }
        public int ResponseTotal { get; set; }
        public string AdminComments { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
    }
}
