namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CheckList
    {
        public Guid CheckListId { get; set; }

        [Required]
        [StringLength(50)]
        public string CheckListName { get; set; }

        public short? CheckListOrder { get; set; }

        public Guid? InspectionTypeId { get; set; }

        public Guid rowguid { get; set; }

        public bool? NotPrinted { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool Inactive { get; set; }

        [StringLength(50)]
        public string NFPAReport { get; set; }

        public bool DefaultValues { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public Guid? CheckListTypeId { get; set; }

        public bool WebViewable { get; set; }
    }
}
