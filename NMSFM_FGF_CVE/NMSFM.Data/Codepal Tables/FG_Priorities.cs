namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class FG_Priorities
    {
        [Key]
        public int PriorityId { get; set; }
        public int CategoryId { get; set; }

        [StringLength(50)]
        public string PriorityName { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Inactive { get; set; }
    }
}
