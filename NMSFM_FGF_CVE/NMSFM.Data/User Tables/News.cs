namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
	
	[Serializable]
    public partial class News
    {
        [Key]
        public Guid NewsKey { get; set; }

        [StringLength(500)]
        public string NewsTitle { get; set; }

        public string NewsText { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateInserted { get; set; }

        public bool? Inactive { get; set; }
    }
}
