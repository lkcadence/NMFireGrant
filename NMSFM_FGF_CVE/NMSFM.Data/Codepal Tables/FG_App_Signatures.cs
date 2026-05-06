using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    [Serializable]
    public partial class FG_App_Signatures
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid SignatureId { get; set; }
        public Guid ApplicationId { get; set; }
        public string SignatureRole { get; set; }
        public string PrintedName { get; set;  }
        public string EmailAddress { get; set; }
        public string Signature { get; set; }
        public byte[] SignatureImage { get; set; }
        public string SignatureImageType { get; set; }
        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public Guid? WebUserId { get; set; }
        public DateTime? DateSigned { get; set; }
        public string SignedBy { get; set; }
        public string LoginToken { get; set; }
        public bool FromReview { get; set; }
        public bool FromStatus { get; set; }
    }
}
