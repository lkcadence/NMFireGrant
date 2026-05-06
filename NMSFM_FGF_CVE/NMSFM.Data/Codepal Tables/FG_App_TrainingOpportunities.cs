using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_TrainingOpportunities
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid TrainingId { get; set; }
        public Guid ApplicationId { get; set; }
        public int Number { get; set; }
        public string TrainingDetail { get; set; }
        public string TrainingDocumentName { get; set; }
        public byte[] TrainingDocument { get; set; }
        public string TrainingDocumentType { get; set; }
    }
}
