namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectSetting
    {
        [Key]
        public Guid ProjectTypeId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(50)]
        public string ProjectNameLabel { get; set; }

        [StringLength(50)]
        public string ProjectTypeLabel { get; set; }

        [StringLength(50)]
        public string ProjectNumberLabel { get; set; }

        [StringLength(50)]
        public string StatusLabel { get; set; }

        [StringLength(50)]
        public string CompleteLabel { get; set; }

        [StringLength(50)]
        public string TabFiles { get; set; }

        [StringLength(50)]
        public string DetailHides { get; set; }

        [StringLength(50)]
        public string TabHides { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
