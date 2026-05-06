using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_Documents
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid DocumentId { get; set; }
        public Guid ApplicationId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public byte[] Document { get; set; }
        public string DocType { get; set; }
    }
}
