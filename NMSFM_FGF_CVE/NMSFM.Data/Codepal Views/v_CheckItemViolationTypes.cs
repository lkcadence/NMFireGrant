namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_CheckItemViolationTypes
    {
        [Key]
        public Guid CheckItemId { get; set; }

        public Guid? ViolationTypeId { get; set; }

        [StringLength(100)]
        public string CodeVersion { get; set; }

        [StringLength(300)]
        public string CategoryType { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        public Guid? CategoryTypeId { get; set; }

        public Guid? CodeVersionId { get; set; }
    }
}
