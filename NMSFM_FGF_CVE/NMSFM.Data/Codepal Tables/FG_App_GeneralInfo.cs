using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace NMSFM.Data
{
    public partial class FG_App_GeneralInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("newid()")]
        [Key]
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int IndividualDept { get; set; }
        public string NFIRSID { get; set; }
        public string DepartmentName { get; set; }
        public string FireChiefName { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public int? ISORating { get; set; }
        public string County { get; set; }
        public int IsCityMuni { get; set; }
        public int DeptType { get; set; }
        public bool IsAdminDept { get; set; }
        public int CountyDeptsCompliant { get; set; }
        public int? MainStations { get; set; }
        public int? SubStations { get; set; }
        public int? AdminBldgs { get; set; }
        public int Community { get; set; }
        public int? NumberOfFirefighters { get; set; }
        public int? FFI_Firefighters { get; set; }
        public int? FFII_Firefighters { get; set; }
        public string MailingAddress { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZip { get; set; }
        public string PersonCompleteApp { get; set; }
        public int FireDeptMember { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }

        [NotMapped]
        public string NERISID
        {
            get { return NFIRSID; }
            set { NFIRSID = value; }
        }
    }
}
