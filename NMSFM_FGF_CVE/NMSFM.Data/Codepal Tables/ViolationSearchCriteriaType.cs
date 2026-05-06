namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ViolationSearchCriteriaType")]
    public partial class ViolationSearchCriteriaType
    {
        [Key]
        public Guid ViolationSearchCriteriaId { get; set; }

        [StringLength(100)]
        public string ViolationSearchCriteria { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
