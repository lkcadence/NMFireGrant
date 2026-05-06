namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("zzHelp")]
    public partial class zzHelp
    {
        [Key]
        public Guid HelpId { get; set; }

        [StringLength(450)]
        public string CPOFormName { get; set; }

        [StringLength(450)]
        public string Topic { get; set; }

        [StringLength(450)]
        public string ID { get; set; }

        public int? ContextID { get; set; }
        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string DisplayForm { get; set; }

        [StringLength(50)]
        public string ReferencedBy { get; set; }

        [StringLength(50)]
        public string ParentObject { get; set; }

        [StringLength(50)]
        public string Sequence { get; set; }

        [StringLength(50)]
        public string DropDownText { get; set; }

        [StringLength(250)]
        public string DocumentLocation { get; set; }

        public string HelpText { get; set; }

        [StringLength(250)]
        public string ToolTipText { get; set; }
        public bool? Inactive { get; set; }
        public string HelpPurpose { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid rowguid { get; set; }


    }
}
