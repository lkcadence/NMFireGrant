using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_AidDistricts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid AidDistrictId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Number { get; set; }
        public string AidDistrict { get; set; }
    }
}
