using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    public partial class FG_App_DocsSigs
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int DocumentNumber { get; set; }
        public int SignaturesCollected { get; set; }
        public int AppCompletenessGrade { get; set; }
        public string AdminComments { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
    }
}
